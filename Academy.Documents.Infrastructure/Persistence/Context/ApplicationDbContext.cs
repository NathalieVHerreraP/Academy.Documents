using Academy.Documents.Domain.Entities.DocumentsEntity;
using Microsoft.EntityFrameworkCore;

namespace Academy.Documents.Infrastructure.Persistence.Context
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        { }

        public virtual DbSet<Document> Documents { get; set; }
        public virtual DbSet<DocumentLog> DocumentsLog { get; set; }


    }
}
