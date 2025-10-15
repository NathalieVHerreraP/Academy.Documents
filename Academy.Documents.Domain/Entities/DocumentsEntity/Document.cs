using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Academy.Documents.Domain.Entities.DocumentsEntity;

public class Document
{
    [Key]
    public Guid Id { get; set; }
    public int UserId { get; set; }
    public string Path { get; set; } = string.Empty;
    public DateTime UploadDate { get; set; }
}
