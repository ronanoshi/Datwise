using Microsoft.EntityFrameworkCore;
using Datwise.Models;

namespace Datwise.Data
{
    public class DatwiseDbContext : DbContext
    {
        public DatwiseDbContext(DbContextOptions<DatwiseDbContext> options) : base(options) { }

        public DbSet<Issue> Issues { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Issue entity
            modelBuilder.Entity<Issue>(entity =>
            {
                entity.HasKey(i => i.Id);

                entity.Property(i => i.Title)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(i => i.Description)
                    .IsRequired()
                    .HasMaxLength(2000);

                entity.Property(i => i.Severity)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasDefaultValue("Medium");

                entity.Property(i => i.Status)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasDefaultValue("Open");

                entity.Property(i => i.ReportedDate)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(i => i.ReportedBy)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(i => i.Department)
                    .HasMaxLength(100);

                entity.Property(i => i.Location)
                    .HasMaxLength(255);

                entity.Property(i => i.ResolutionNotes)
                    .HasMaxLength(2000);

                // Create indexes for better query performance
                entity.HasIndex(i => i.Status);
                entity.HasIndex(i => i.Severity);
                entity.HasIndex(i => i.ReportedDate);
            });
        }
    }
}
