namespace Academy.Documents.Application.Documents.Commands.SaveDocument;

public class SaveDocumentCommandRequest
{
    public int UserId { get; set; }
    public string FileName { get; set; } = String.Empty;
    public string ContentType { get; set; } = String.Empty;
    public byte[] Content { get; set; } = [];
}
