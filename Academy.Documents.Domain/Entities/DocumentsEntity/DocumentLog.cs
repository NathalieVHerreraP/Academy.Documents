using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Academy.Documents.Domain.Entities.DocumentsEntity;

public class DocumentLog
{
    [Key]
    public Guid Id { get; set; }
    public int UserId { get; set; }
    public string FileName { get; set; } = string.Empty;
    public bool Status { get; set; } 
    public string Description { get; set; } = string.Empty;
    public DateTime UploadDate { get; set; }

}

