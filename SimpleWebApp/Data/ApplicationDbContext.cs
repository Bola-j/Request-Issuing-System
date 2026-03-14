using Microsoft.EntityFrameworkCore;
using SimpleWebApp.Models;

namespace SimpleWebApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Request> Requests => Set<Request>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Request>(entity =>
            {
                entity.Property(r => r.FullName).HasMaxLength(120).IsRequired();
                entity.Property(r => r.Email).HasMaxLength(150).IsRequired();
                entity.Property(r => r.RequestType).HasMaxLength(80).IsRequired();
                entity.Property(r => r.Description).HasMaxLength(1000).IsRequired();
                entity.Property(r => r.Status).HasConversion<int>();
                entity.Property(r => r.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            });
        }
    }
}
