using Academy.Documents.Domain.Entities.DocumentsEntity;
using Academy.Documents.Domain.Entities.DocumentsEntity.Repositories;
using Academy.Documents.Domain.Shared;
using MediatR;

namespace Academy.Documents.Application.Documents.Commands.SaveDocument;

public class SaveDocumentCommandHandler(IDocumentsRepository repository, IVirusTotalApi virusTotal) : IRequestHandler<SaveDocumentCommand, Result>
{
    private readonly IDocumentsRepository _repository = repository;
    private readonly IVirusTotalApi _virusTotal = virusTotal;
    public async Task<Result> Handle(SaveDocumentCommand request, CancellationToken cancellationToken)
    {
        if (
            request is null || 
            request.request.UserId <= 0 ||
            string.IsNullOrWhiteSpace(request.request.FileName) ||
            string.IsNullOrEmpty(request.request.FileName) ||
            string.IsNullOrWhiteSpace(request.request.ContentType) ||
            string.IsNullOrEmpty(request.request.ContentType) ||
            request.request.Content.Length == 0||
            string.Equals(request.request.ContentType, "application/vnd.microsoft.portable-executable")
            )
            return Result.Failure(400,"InvalidData", "Invalid Data");
        bool virusTotalResponse;

        // Scan the document with VirusTotal
        try
        {
            virusTotalResponse = await _virusTotal.ScanDocument(request.request.Content, request.request.FileName);
        }
        catch(TaskCanceledException)
        {
            return Result.Failure(408, "TaskCancelled", "Task Cancelled");
        }
        catch(Exception ex)
        {
            return Result.Failure(500, "Exception", ex.Message);
        }

        if (!virusTotalResponse)
        {
            try
            {
                await _repository.RegisterdocumentLog(new DocumentLog
                {
                    UserId = request.request.UserId,
                    FileName = request.request.FileName,
                    Status = false,
                    Description = "The document is infected",
                    UploadDate = DateTime.UtcNow,
                }, cancellationToken);
            }
            catch (TaskCanceledException)
            {
                return Result.Failure(408, "TaskCancelled", "Task Cancelled");
            }
            catch (Exception ex)
            {
                return Result.Failure(500, "UnhandledException", ex.Message);
            }
            return Result.Failure(400,"DocumentInfected", "The document can not be saved because it is infected with virus");
        }

        // Save File in Archive System
        string docPath = Path.Combine(Directory.GetCurrentDirectory(), "DocumentsUploads");
        Directory.CreateDirectory(docPath);

        Guid docId = Guid.NewGuid();
        string newFileName = $"{docId}_{request.request.FileName}";
        string filePath = Path.Combine(docPath, newFileName);

        try
        {
            await File.WriteAllBytesAsync(filePath, request.request.Content, cancellationToken);
        } catch (TaskCanceledException)
        {
            return Result.Failure(408, "TaskCancelled", "Task Cancelled");
        }
        catch (Exception ex)
        {
            return Result.Failure(500, "UnhandledException", ex.Message);
        }

        // Register Document and Log in Database

        Document document = new()
        {
            Id = docId,
            UserId = request.request.UserId,
            Path = filePath,
            UploadDate = DateTime.UtcNow,
        };
        DocumentLog documentLog = new()
        {
            UserId = request.request.UserId,
            FileName = request.request.FileName,
            Status = true,
            Description = "File uploaded successfully",
            UploadDate = DateTime.UtcNow,
        };

        try
        {
            await _repository.SaveDocument(document, cancellationToken);
            await _repository.RegisterdocumentLog(documentLog, cancellationToken);
        }
        catch (TaskCanceledException)
        {
            return Result.Failure(408, "TaskCancelled", "Task Cancelled");
        }
        catch (Exception ex)
        {
            return Result.Failure(500, "UnhandledException", ex.Message);
        }


        return Result.Success();
    }
}
