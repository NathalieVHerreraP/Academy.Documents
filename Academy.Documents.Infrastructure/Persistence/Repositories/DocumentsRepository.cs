using Academy.Documents.Domain.Entities.DocumentsEntity;
using Academy.Documents.Domain.Entities.DocumentsEntity.Repositories;
using Academy.Documents.Infrastructure.Persistence.Context;

namespace Academy.Documents.Infrastructure.Persistence.Repositories;

public class DocumentsRepository(ApplicationDbContext context) : IDocumentsRepository
{
    public async Task RegisterdocumentLog(DocumentLog log, CancellationToken cancellationToken)
    {
        await context.DocumentsLog.AddAsync(log);
        try
        {
            await context.SaveChangesAsync(cancellationToken);
        }
        catch (TaskCanceledException)
        {
            throw;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task SaveDocument(Document document, CancellationToken cancellationToken)
    {
        await context.Documents.AddAsync(document);
        try
        {
            await context.SaveChangesAsync(cancellationToken);
        }
        catch (TaskCanceledException)
        {
            throw;
        }
        catch (Exception)
        {
            throw;
        }
    }
}
