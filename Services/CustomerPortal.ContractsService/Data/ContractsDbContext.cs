using Microsoft.EntityFrameworkCore;
using CustomerPortal.ContractsService.Entities;

namespace CustomerPortal.ContractsService.Data;

public class ContractsDbContext : DbContext
{
    public ContractsDbContext(DbContextOptions<ContractsDbContext> options) : base(options)
    {
    }

    public DbSet<Contract> Contracts { get; set; }
    public DbSet<Company> Companies { get; set; }
    public DbSet<Service> Services { get; set; }
    public DbSet<Site> Sites { get; set; }
    public DbSet<ContractService> ContractServices { get; set; }
    public DbSet<ContractSite> ContractSites { get; set; }
    public DbSet<ContractTerm> ContractTerms { get; set; }
    public DbSet<ContractAmendment> ContractAmendments { get; set; }
    public DbSet<ContractRenewal> ContractRenewals { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure Contract entity
        modelBuilder.Entity<Contract>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.ContractNumber).IsRequired().HasMaxLength(50);
            entity.Property(e => e.ContractType).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.Value).HasColumnType("decimal(18,2)");
            entity.Property(e => e.Currency).HasMaxLength(10).HasDefaultValue("USD");
            entity.Property(e => e.Status).HasMaxLength(30).HasDefaultValue("DRAFT");
            entity.Property(e => e.PaymentTerms).HasMaxLength(50).HasDefaultValue("NET_30");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("GETUTCDATE()");
            entity.Property(e => e.ModifiedDate).HasDefaultValueSql("GETUTCDATE()");

            entity.HasIndex(e => e.ContractNumber).IsUnique();
            entity.HasIndex(e => e.CompanyId);
            entity.HasIndex(e => e.Status);
            entity.HasIndex(e => e.EndDate);

            entity.HasOne(e => e.Company)
                .WithMany(c => c.Contracts)
                .HasForeignKey(e => e.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Configure Company entity
        modelBuilder.Entity<Company>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.CompanyName).IsRequired().HasMaxLength(200);
            entity.Property(e => e.CompanyCode).IsRequired().HasMaxLength(20);
            entity.Property(e => e.ContactPerson).HasMaxLength(100);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Phone).HasMaxLength(20);
            entity.Property(e => e.Address).HasMaxLength(500);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("GETUTCDATE()");

            entity.HasIndex(e => e.CompanyCode).IsUnique();
            entity.HasIndex(e => e.CompanyName);
        });

        // Configure Service entity
        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.ServiceName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.ServiceCode).IsRequired().HasMaxLength(20);
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Category).HasMaxLength(50);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("GETUTCDATE()");

            entity.HasIndex(e => e.ServiceCode).IsUnique();
            entity.HasIndex(e => e.ServiceName);
            entity.HasIndex(e => e.Category);
        });

        // Configure Site entity
        modelBuilder.Entity<Site>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.SiteName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.SiteCode).IsRequired().HasMaxLength(20);
            entity.Property(e => e.Address).HasMaxLength(500);
            entity.Property(e => e.City).HasMaxLength(100);
            entity.Property(e => e.Country).HasMaxLength(100);
            entity.Property(e => e.PostalCode).HasMaxLength(20);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("GETUTCDATE()");

            entity.HasIndex(e => e.SiteCode).IsUnique();
            entity.HasIndex(e => e.CompanyId);

            entity.HasOne(e => e.Company)
                .WithMany(c => c.Sites)
                .HasForeignKey(e => e.CompanyId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Configure ContractService entity
        modelBuilder.Entity<ContractService>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.UnitPrice).HasColumnType("decimal(18,2)");
            entity.Property(e => e.TotalPrice).HasColumnType("decimal(18,2)");
            entity.Property(e => e.Quantity).HasDefaultValue(1);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("GETUTCDATE()");

            entity.HasIndex(e => new { e.ContractId, e.ServiceId }).IsUnique();

            entity.HasOne(e => e.Contract)
                .WithMany(c => c.Services)
                .HasForeignKey(e => e.ContractId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Service)
                .WithMany(s => s.ContractServices)
                .HasForeignKey(e => e.ServiceId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Configure ContractSite entity
        modelBuilder.Entity<ContractSite>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("GETUTCDATE()");

            entity.HasIndex(e => new { e.ContractId, e.SiteId }).IsUnique();

            entity.HasOne(e => e.Contract)
                .WithMany(c => c.Sites)
                .HasForeignKey(e => e.ContractId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Site)
                .WithMany(s => s.ContractSites)
                .HasForeignKey(e => e.SiteId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Configure ContractTerm entity
        modelBuilder.Entity<ContractTerm>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.TermType).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Description).IsRequired().HasMaxLength(1000);
            entity.Property(e => e.Value).HasMaxLength(200);
            entity.Property(e => e.Unit).HasMaxLength(50);
            entity.Property(e => e.IsRequired).HasDefaultValue(false);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("GETUTCDATE()");

            entity.HasIndex(e => e.ContractId);
            entity.HasIndex(e => e.TermType);

            entity.HasOne(e => e.Contract)
                .WithMany(c => c.Terms)
                .HasForeignKey(e => e.ContractId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Configure ContractAmendment entity
        modelBuilder.Entity<ContractAmendment>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.AmendmentNumber).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Description).IsRequired().HasMaxLength(1000);
            entity.Property(e => e.AmendmentType).IsRequired().HasMaxLength(50);
            entity.Property(e => e.ValueChange).HasColumnType("decimal(18,2)");
            entity.Property(e => e.Status).HasMaxLength(30).HasDefaultValue("PENDING");
            entity.Property(e => e.ApprovedBy).HasMaxLength(100);
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("GETUTCDATE()");

            entity.HasIndex(e => e.AmendmentNumber).IsUnique();
            entity.HasIndex(e => e.ContractId);
            entity.HasIndex(e => e.Status);

            entity.HasOne(e => e.Contract)
                .WithMany(c => c.Amendments)
                .HasForeignKey(e => e.ContractId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Configure ContractRenewal entity
        modelBuilder.Entity<ContractRenewal>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.RenewalNumber).IsRequired().HasMaxLength(50);
            entity.Property(e => e.ProposedValue).HasColumnType("decimal(18,2)");
            entity.Property(e => e.Status).HasMaxLength(30).HasDefaultValue("INITIATED");
            entity.Property(e => e.AutoRenewal).HasDefaultValue(false);
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("GETUTCDATE()");
            entity.Property(e => e.ProcessedBy).HasMaxLength(100);

            entity.HasIndex(e => e.RenewalNumber).IsUnique();
            entity.HasIndex(e => e.ContractId);
            entity.HasIndex(e => e.Status);

            entity.HasOne(e => e.Contract)
                .WithMany(c => c.Renewals)
                .HasForeignKey(e => e.ContractId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Seed data
        SeedData(modelBuilder);
    }

    private static void SeedData(ModelBuilder modelBuilder)
    {
        // Seed Companies
        modelBuilder.Entity<Company>().HasData(
            new Company
            {
                Id = 1,
                CompanyName = "Acme Corporation",
                CompanyCode = "ACME001",
                ContactPerson = "John Doe",
                Email = "john.doe@acme.com",
                Phone = "+1-555-0100",
                Address = "123 Business Ave, Business City, BC 12345",
                IsActive = true,
                CreatedDate = DateTime.UtcNow
            },
            new Company
            {
                Id = 2,
                CompanyName = "Global Industries",
                CompanyCode = "GLOB001",
                ContactPerson = "Jane Smith",
                Email = "jane.smith@global.com",
                Phone = "+1-555-0200",
                Address = "456 Corporate Blvd, Commerce City, CC 67890",
                IsActive = true,
                CreatedDate = DateTime.UtcNow
            }
        );

        // Seed Services
        modelBuilder.Entity<Service>().HasData(
            new Service
            {
                Id = 1,
                ServiceName = "ISO 9001 Certification",
                ServiceCode = "ISO9001",
                Description = "Quality Management System Certification",
                Category = "CERTIFICATION",
                IsActive = true,
                CreatedDate = DateTime.UtcNow
            },
            new Service
            {
                Id = 2,
                ServiceName = "ISO 14001 Certification",
                ServiceCode = "ISO14001",
                Description = "Environmental Management System Certification",
                Category = "CERTIFICATION",
                IsActive = true,
                CreatedDate = DateTime.UtcNow
            },
            new Service
            {
                Id = 3,
                ServiceName = "Quality Management Training",
                ServiceCode = "QMT001",
                Description = "Comprehensive quality management training program",
                Category = "TRAINING",
                IsActive = true,
                CreatedDate = DateTime.UtcNow
            }
        );

        // Seed Sites
        modelBuilder.Entity<Site>().HasData(
            new Site
            {
                Id = 1,
                CompanyId = 1,
                SiteName = "Main Production Facility",
                SiteCode = "ACME-PROD-01",
                Address = "456 Factory Rd",
                City = "Industrial City",
                Country = "USA",
                PostalCode = "54321",
                IsActive = true,
                CreatedDate = DateTime.UtcNow
            },
            new Site
            {
                Id = 2,
                CompanyId = 1,
                SiteName = "Warehouse Distribution Center",
                SiteCode = "ACME-DIST-01",
                Address = "789 Warehouse St",
                City = "Distribution City",
                Country = "USA",
                PostalCode = "98765",
                IsActive = true,
                CreatedDate = DateTime.UtcNow
            },
            new Site
            {
                Id = 3,
                CompanyId = 2,
                SiteName = "Global Manufacturing Plant",
                SiteCode = "GLOB-MFG-01",
                Address = "321 Manufacturing Way",
                City = "Manufacturing City",
                Country = "USA",
                PostalCode = "13579",
                IsActive = true,
                CreatedDate = DateTime.UtcNow
            }
        );

        // Seed Contracts
        modelBuilder.Entity<Contract>().HasData(
            new Contract
            {
                Id = 1,
                ContractNumber = "CNT-2024-001",
                CompanyId = 1,
                ContractType = "CERTIFICATION_SERVICES",
                Title = "ISO Management System Certification Contract",
                Description = "Multi-year certification contract for ISO 9001, 14001, and 45001",
                StartDate = new DateTime(2024, 1, 1),
                EndDate = new DateTime(2026, 12, 31),
                RenewalDate = new DateTime(2026, 9, 30),
                Value = 50000.00m,
                Currency = "USD",
                Status = "ACTIVE",
                PaymentTerms = "NET_30",
                IsActive = true,
                CreatedDate = new DateTime(2023, 12, 15),
                ModifiedDate = new DateTime(2024, 6, 15)
            },
            new Contract
            {
                Id = 2,
                ContractNumber = "CNT-2024-002",
                CompanyId = 2,
                ContractType = "TRAINING_SERVICES",
                Title = "Quality Management Training Contract",
                Description = "Annual training program for quality management",
                StartDate = new DateTime(2024, 10, 1),
                EndDate = new DateTime(2025, 9, 30),
                RenewalDate = new DateTime(2025, 7, 1),
                Value = 25000.00m,
                Currency = "USD",
                Status = "ACTIVE",
                PaymentTerms = "NET_30",
                IsActive = true,
                CreatedDate = new DateTime(2024, 9, 1),
                ModifiedDate = new DateTime(2024, 9, 1)
            }
        );

        // Seed Contract Services
        modelBuilder.Entity<ContractService>().HasData(
            new ContractService
            {
                Id = 1,
                ContractId = 1,
                ServiceId = 1,
                UnitPrice = 15000.00m,
                Quantity = 1,
                TotalPrice = 15000.00m,
                IsActive = true,
                CreatedDate = new DateTime(2023, 12, 15)
            },
            new ContractService
            {
                Id = 2,
                ContractId = 1,
                ServiceId = 2,
                UnitPrice = 12000.00m,
                Quantity = 1,
                TotalPrice = 12000.00m,
                IsActive = true,
                CreatedDate = new DateTime(2023, 12, 15)
            },
            new ContractService
            {
                Id = 3,
                ContractId = 2,
                ServiceId = 3,
                UnitPrice = 5000.00m,
                Quantity = 5,
                TotalPrice = 25000.00m,
                IsActive = true,
                CreatedDate = new DateTime(2024, 9, 1)
            }
        );

        // Seed Contract Sites
        modelBuilder.Entity<ContractSite>().HasData(
            new ContractSite
            {
                Id = 1,
                ContractId = 1,
                SiteId = 1,
                IsActive = true,
                CreatedDate = new DateTime(2023, 12, 15)
            },
            new ContractSite
            {
                Id = 2,
                ContractId = 1,
                SiteId = 2,
                IsActive = true,
                CreatedDate = new DateTime(2023, 12, 15)
            },
            new ContractSite
            {
                Id = 3,
                ContractId = 2,
                SiteId = 3,
                IsActive = true,
                CreatedDate = new DateTime(2024, 9, 1)
            }
        );

        // Seed Contract Terms
        modelBuilder.Entity<ContractTerm>().HasData(
            new ContractTerm
            {
                Id = 1,
                ContractId = 1,
                TermType = "PAYMENT_SCHEDULE",
                Description = "Annual payment in advance",
                Value = "ANNUAL",
                Unit = "PAYMENT",
                IsRequired = true,
                EffectiveDate = new DateTime(2024, 1, 1),
                IsActive = true,
                CreatedDate = new DateTime(2023, 12, 15)
            },
            new ContractTerm
            {
                Id = 2,
                ContractId = 2,
                TermType = "CANCELLATION",
                Description = "30 days written notice required",
                Value = "30",
                Unit = "DAYS",
                IsRequired = true,
                EffectiveDate = new DateTime(2024, 10, 1),
                IsActive = true,
                CreatedDate = new DateTime(2024, 9, 1)
            }
        );

        // Seed Contract Amendments
        modelBuilder.Entity<ContractAmendment>().HasData(
            new ContractAmendment
            {
                Id = 1,
                ContractId = 1,
                AmendmentNumber = "AMD-001",
                Description = "Added ISO 45001 certification service",
                AmendmentType = "SERVICE_ADDITION",
                EffectiveDate = new DateTime(2024, 6, 1),
                ValueChange = 23000.00m,
                Status = "APPROVED",
                ApprovedBy = "Contract Manager",
                ApprovedDate = new DateTime(2024, 5, 15),
                CreatedDate = new DateTime(2024, 5, 1)
            }
        );
    }
}