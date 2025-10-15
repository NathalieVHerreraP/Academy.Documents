namespace Academy.Documents.Domain.Entities.DocumentsEntity.Repositories;

public interface IDocumentsRepository
{
    Task SaveDocument(Document document, CancellationToken cancellationToken);
    Task RegisterdocumentLog(DocumentLog log, CancellationToken cancellationToken);
}
