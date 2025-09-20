using CustomerPortal.FindingsService.Entities;
using Microsoft.EntityFrameworkCore;

namespace CustomerPortal.FindingsService.Data;

public class FindingsDbContext : DbContext
{
    public FindingsDbContext(DbContextOptions<FindingsDbContext> options) : base(options) { }

    public DbSet<Finding> Findings { get; set; }
    public DbSet<FindingCategory> FindingCategories { get; set; }
    public DbSet<FindingStatus> FindingStatuses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure Finding entity
        modelBuilder.Entity<Finding>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            
            entity.HasIndex(e => e.ReferenceNumber).IsUnique();
            entity.HasIndex(e => new { e.CategoryId, e.StatusId });
            entity.HasIndex(e => e.IdentifiedDate);
            entity.HasIndex(e => e.RequiredCompletionDate);
            entity.HasIndex(e => e.AssignedTo);
            
            entity.HasOne(d => d.Category)
                .WithMany(p => p.Findings)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(d => d.Status)
                .WithMany(p => p.Findings)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Configure FindingCategory entity
        modelBuilder.Entity<FindingCategory>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            
            entity.HasIndex(e => e.Code).IsUnique();
            entity.HasIndex(e => e.Name);
        });

        // Configure FindingStatus entity
        modelBuilder.Entity<FindingStatus>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            
            entity.HasIndex(e => e.Code).IsUnique();
            entity.HasIndex(e => e.Name);
            entity.HasIndex(e => e.DisplayOrder);
        });
    }
}