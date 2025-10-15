namespace Academy.Documents.Domain.Entities.DocumentsEntity.Repositories;

public interface IVirusTotalApi
{
    Task<bool> ScanDocument(byte[] conten, string fileName);
}
