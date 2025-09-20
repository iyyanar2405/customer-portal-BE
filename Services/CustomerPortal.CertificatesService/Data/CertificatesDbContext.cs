using CustomerPortal.CertificatesService.Entities;
using Microsoft.EntityFrameworkCore;

namespace CustomerPortal.CertificatesService.Data
{
    public class CertificatesDbContext : DbContext
    {
        public CertificatesDbContext(DbContextOptions<CertificatesDbContext> options) : base(options)
        {
        }

        // DbSets for all entities
        public DbSet<Certificate> Certificates { get; set; }
        public DbSet<CertificateType> CertificateTypes { get; set; }
        public DbSet<CertificateSite> CertificateSites { get; set; }
        public DbSet<CertificateService> CertificateServices { get; set; }
        public DbSet<CertificateAdditionalScope> CertificateAdditionalScopes { get; set; }
        public DbSet<CertificateRenewal> CertificateRenewals { get; set; }
        public DbSet<CertificateValidation> CertificateValidations { get; set; }
        public DbSet<CertificateDocument> CertificateDocuments { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Audit> Audits { get; set; }
        public DbSet<Site> Sites { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            ConfigureEntities(modelBuilder);
            ConfigureRelationships(modelBuilder);
            ConfigureIndexes(modelBuilder);
            SeedData(modelBuilder);
        }

        private void ConfigureEntities(ModelBuilder modelBuilder)
        {
            // Certificate configuration
            modelBuilder.Entity<Certificate>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.CertificateNumber).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Scope).IsRequired().HasMaxLength(2000);
                entity.Property(e => e.Status).IsRequired().HasMaxLength(20).HasDefaultValue("ACTIVE");
                entity.Property(e => e.IssueDate).IsRequired();
                entity.Property(e => e.ExpiryDate).IsRequired();

                entity.HasIndex(e => e.CertificateNumber).IsUnique();
                entity.HasIndex(e => e.CompanyId);
                entity.HasIndex(e => e.Status);
                entity.HasIndex(e => e.ExpiryDate);
            });

            // CertificateType configuration
            modelBuilder.Entity<CertificateType>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.TypeName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Standard).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Category).IsRequired().HasMaxLength(50).HasDefaultValue("MANAGEMENT_SYSTEMS");
                entity.Property(e => e.ValidityPeriodMonths).IsRequired().HasDefaultValue(36);
                entity.Property(e => e.RenewalNoticeDays).IsRequired().HasDefaultValue(90);
                entity.Property(e => e.IsAccredited).HasDefaultValue(true);
                entity.Property(e => e.RequiresAnnualSurveillance).HasDefaultValue(true);

                entity.HasIndex(e => e.TypeName);
                entity.HasIndex(e => e.Standard);
            });

            // Company configuration
            modelBuilder.Entity<Company>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.CompanyName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.CompanyCode).IsRequired().HasMaxLength(20);

                entity.HasIndex(e => e.CompanyCode).IsUnique();
                entity.HasIndex(e => e.CompanyName);
            });

            // User configuration
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.FirstName).IsRequired().HasMaxLength(50);
                entity.Property(e => e.LastName).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Role).IsRequired().HasMaxLength(20).HasDefaultValue("USER");

                entity.HasIndex(e => e.Email).IsUnique();
            });

            // Site configuration
            modelBuilder.Entity<Site>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.SiteName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.SiteCode).IsRequired().HasMaxLength(20);

                entity.HasIndex(e => e.SiteCode).IsUnique();
                entity.HasIndex(e => e.CompanyId);
            });

            // Service configuration
            modelBuilder.Entity<Service>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.ServiceName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.ServiceCode).IsRequired().HasMaxLength(20);
                entity.Property(e => e.Category).HasMaxLength(50).HasDefaultValue("GENERAL");

                entity.HasIndex(e => e.ServiceCode).IsUnique();
            });

            // Audit configuration
            modelBuilder.Entity<Audit>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.AuditNumber).IsRequired().HasMaxLength(50);
                entity.Property(e => e.AuditTitle).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Status).IsRequired().HasMaxLength(20).HasDefaultValue("PLANNED");

                entity.HasIndex(e => e.AuditNumber).IsUnique();
                entity.HasIndex(e => e.AuditDate);
            });

            // City configuration
            modelBuilder.Entity<City>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.CityName).IsRequired().HasMaxLength(100);
            });

            // Country configuration
            modelBuilder.Entity<Country>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.CountryName).IsRequired().HasMaxLength(100);
            });

            // CertificateRenewal configuration
            modelBuilder.Entity<CertificateRenewal>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.RenewalNumber).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Status).IsRequired().HasMaxLength(20).HasDefaultValue("INITIATED");

                entity.HasIndex(e => e.RenewalNumber).IsUnique();
            });

            // CertificateValidation configuration
            modelBuilder.Entity<CertificateValidation>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.VerificationCode).IsRequired().HasMaxLength(100);
                entity.Property(e => e.VerificationMethod).IsRequired().HasMaxLength(50).HasDefaultValue("MANUAL");
                entity.Property(e => e.Result).IsRequired().HasMaxLength(20).HasDefaultValue("VALID");
                entity.Property(e => e.ValidationDate).HasDefaultValueSql("GETUTCDATE()");

                entity.HasIndex(e => e.VerificationCode).IsUnique();
            });

            // CertificateDocument configuration
            modelBuilder.Entity<CertificateDocument>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.DocumentType).IsRequired().HasMaxLength(20).HasDefaultValue("CERTIFICATE");
                entity.Property(e => e.FileName).IsRequired().HasMaxLength(255);
                entity.Property(e => e.FileType).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Version).HasMaxLength(20).HasDefaultValue("1.0");
                entity.Property(e => e.IsLatest).HasDefaultValue(true);
                entity.Property(e => e.UploadDate).HasDefaultValueSql("GETUTCDATE()");
            });
        }

        private void ConfigureRelationships(ModelBuilder modelBuilder)
        {
            // Certificate relationships
            modelBuilder.Entity<Certificate>()
                .HasOne(c => c.Company)
                .WithMany(co => co.Certificates)
                .HasForeignKey(c => c.CompanyId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Certificate>()
                .HasOne(c => c.Audit)
                .WithMany(a => a.Certificates)
                .HasForeignKey(c => c.AuditId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Certificate>()
                .HasOne(c => c.CertificateType)
                .WithMany(ct => ct.Certificates)
                .HasForeignKey(c => c.CertificateTypeId)
                .OnDelete(DeleteBehavior.NoAction);

            // CertificateSite relationships
            modelBuilder.Entity<CertificateSite>()
                .HasOne(cs => cs.Certificate)
                .WithMany(c => c.Sites)
                .HasForeignKey(cs => cs.CertificateId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CertificateSite>()
                .HasOne(cs => cs.Site)
                .WithMany(s => s.CertificateSites)
                .HasForeignKey(cs => cs.SiteId)
                .OnDelete(DeleteBehavior.NoAction);

            // CertificateService relationships
            modelBuilder.Entity<CertificateService>()
                .HasOne(cs => cs.Certificate)
                .WithMany(c => c.Services)
                .HasForeignKey(cs => cs.CertificateId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CertificateService>()
                .HasOne(cs => cs.Service)
                .WithMany(s => s.CertificateServices)
                .HasForeignKey(cs => cs.ServiceId)
                .OnDelete(DeleteBehavior.NoAction);

            // CertificateAdditionalScope relationships
            modelBuilder.Entity<CertificateAdditionalScope>()
                .HasOne(cas => cas.Certificate)
                .WithMany(c => c.AdditionalScopes)
                .HasForeignKey(cas => cas.CertificateId)
                .OnDelete(DeleteBehavior.Cascade);

            // CertificateRenewal relationships
            modelBuilder.Entity<CertificateRenewal>()
                .HasOne(cr => cr.Certificate)
                .WithMany(c => c.Renewals)
                .HasForeignKey(cr => cr.CertificateId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CertificateRenewal>()
                .HasOne(cr => cr.AssignedAuditor)
                .WithMany(u => u.RenewalsAsAuditor)
                .HasForeignKey(cr => cr.AssignedAuditorId)
                .OnDelete(DeleteBehavior.SetNull);

            // CertificateValidation relationships
            modelBuilder.Entity<CertificateValidation>()
                .HasOne(cv => cv.Certificate)
                .WithMany(c => c.Validations)
                .HasForeignKey(cv => cv.CertificateId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CertificateValidation>()
                .HasOne(cv => cv.ValidatedBy)
                .WithMany(u => u.Validations)
                .HasForeignKey(cv => cv.ValidatedById)
                .OnDelete(DeleteBehavior.SetNull);

            // CertificateDocument relationships
            modelBuilder.Entity<CertificateDocument>()
                .HasOne(cd => cd.Certificate)
                .WithMany(c => c.Documents)
                .HasForeignKey(cd => cd.CertificateId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CertificateDocument>()
                .HasOne(cd => cd.UploadedBy)
                .WithMany(u => u.UploadedDocuments)
                .HasForeignKey(cd => cd.UploadedById)
                .OnDelete(DeleteBehavior.SetNull);

            // Site relationships
            modelBuilder.Entity<Site>()
                .HasOne(s => s.Company)
                .WithMany(c => c.Sites)
                .HasForeignKey(s => s.CompanyId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Site>()
                .HasOne(s => s.City)
                .WithMany(c => c.Sites)
                .HasForeignKey(s => s.CityId)
                .OnDelete(DeleteBehavior.SetNull);

            // Audit relationships
            modelBuilder.Entity<Audit>()
                .HasOne(a => a.LeadAuditor)
                .WithMany(u => u.AuditsAsLeadAuditor)
                .HasForeignKey(a => a.LeadAuditorId)
                .OnDelete(DeleteBehavior.SetNull);

            // City relationships
            modelBuilder.Entity<City>()
                .HasOne(c => c.Country)
                .WithMany(co => co.Cities)
                .HasForeignKey(c => c.CountryId)
                .OnDelete(DeleteBehavior.NoAction);
        }

        private void ConfigureIndexes(ModelBuilder modelBuilder)
        {
            // Additional indexes for performance
            modelBuilder.Entity<Certificate>()
                .HasIndex(c => c.IssueDate);

            modelBuilder.Entity<Certificate>()
                .HasIndex(c => c.RenewalDate);

            modelBuilder.Entity<CertificateSite>()
                .HasIndex(cs => new { cs.CertificateId, cs.SiteId })
                .IsUnique();

            modelBuilder.Entity<CertificateService>()
                .HasIndex(cs => new { cs.CertificateId, cs.ServiceId })
                .IsUnique();
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            var now = DateTime.UtcNow;

            // Seed Countries
            modelBuilder.Entity<Country>().HasData(
                new Country { Id = 1, CountryName = "United States", CountryCode = "US", CreatedDate = now, ModifiedDate = now, IsActive = true },
                new Country { Id = 2, CountryName = "United Kingdom", CountryCode = "UK", CreatedDate = now, ModifiedDate = now, IsActive = true },
                new Country { Id = 3, CountryName = "Germany", CountryCode = "DE", CreatedDate = now, ModifiedDate = now, IsActive = true }
            );

            // Seed Cities
            modelBuilder.Entity<City>().HasData(
                new City { Id = 1, CityName = "New York", CityCode = "NYC", CountryId = 1, CreatedDate = now, ModifiedDate = now, IsActive = true },
                new City { Id = 2, CityName = "London", CityCode = "LON", CountryId = 2, CreatedDate = now, ModifiedDate = now, IsActive = true },
                new City { Id = 3, CityName = "Berlin", CityCode = "BER", CountryId = 3, CreatedDate = now, ModifiedDate = now, IsActive = true }
            );

            // Seed Users
            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, FirstName = "Jane", LastName = "Auditor", Email = "jane.auditor@company.com", Role = "AUDITOR", Department = "Quality", JobTitle = "Lead Auditor", CreatedDate = now, ModifiedDate = now, IsActive = true },
                new User { Id = 2, FirstName = "John", LastName = "Manager", Email = "john.manager@company.com", Role = "MANAGER", Department = "Quality", JobTitle = "QA Manager", CreatedDate = now, ModifiedDate = now, IsActive = true },
                new User { Id = 3, FirstName = "Bob", LastName = "Inspector", Email = "bob.inspector@company.com", Role = "AUDITOR", Department = "Quality", JobTitle = "Inspector", CreatedDate = now, ModifiedDate = now, IsActive = true }
            );

            // Seed Companies
            modelBuilder.Entity<Company>().HasData(
                new Company { Id = 1, CompanyName = "Acme Corporation", CompanyCode = "ACME001", Address = "123 Business St, Business City", ContactPerson = "John Doe", Email = "contact@acme.com", Phone = "+1-555-0100", CreatedDate = now, ModifiedDate = now, IsActive = true },
                new Company { Id = 2, CompanyName = "Global Industries", CompanyCode = "GLOB001", Address = "456 Industry Ave, Industrial Park", ContactPerson = "Jane Smith", Email = "info@global.com", Phone = "+1-555-0200", CreatedDate = now, ModifiedDate = now, IsActive = true }
            );

            // Seed Sites
            modelBuilder.Entity<Site>().HasData(
                new Site { Id = 1, SiteName = "Main Production Facility", SiteCode = "ACME-PROD-01", Address = "456 Factory Rd", CompanyId = 1, CityId = 1, CreatedDate = now, ModifiedDate = now, IsActive = true },
                new Site { Id = 2, SiteName = "Quality Control Lab", SiteCode = "ACME-QC-01", Address = "789 Lab Street", CompanyId = 1, CityId = 1, CreatedDate = now, ModifiedDate = now, IsActive = true },
                new Site { Id = 3, SiteName = "Global Manufacturing Hub", SiteCode = "GLOB-MFG-01", Address = "321 Production Way", CompanyId = 2, CityId = 2, CreatedDate = now, ModifiedDate = now, IsActive = true }
            );

            // Seed Services
            modelBuilder.Entity<Service>().HasData(
                new Service { Id = 1, ServiceName = "Manufacturing", ServiceCode = "MFG001", Description = "Manufacturing services", Category = "PRODUCTION", CreatedDate = now, ModifiedDate = now, IsActive = true },
                new Service { Id = 2, ServiceName = "Quality Control", ServiceCode = "QC001", Description = "Quality control and testing", Category = "QUALITY", CreatedDate = now, ModifiedDate = now, IsActive = true },
                new Service { Id = 3, ServiceName = "Installation", ServiceCode = "INST001", Description = "Installation and setup services", Category = "INSTALLATION", CreatedDate = now, ModifiedDate = now, IsActive = true }
            );

            // Seed CertificateTypes
            modelBuilder.Entity<CertificateType>().HasData(
                new CertificateType { Id = 1, TypeName = "ISO 9001:2015", Standard = "ISO 9001", Description = "Quality Management System", Category = "MANAGEMENT_SYSTEMS", ValidityPeriodMonths = 36, RenewalNoticeDays = 90, IsAccredited = true, AccreditationBody = "UKAS", RequiresAnnualSurveillance = true, CreatedDate = now, ModifiedDate = now, IsActive = true },
                new CertificateType { Id = 2, TypeName = "ISO 14001:2015", Standard = "ISO 14001", Description = "Environmental Management System", Category = "MANAGEMENT_SYSTEMS", ValidityPeriodMonths = 36, RenewalNoticeDays = 90, IsAccredited = true, AccreditationBody = "UKAS", RequiresAnnualSurveillance = true, CreatedDate = now, ModifiedDate = now, IsActive = true },
                new CertificateType { Id = 3, TypeName = "ISO 45001:2018", Standard = "ISO 45001", Description = "Occupational Health and Safety Management System", Category = "MANAGEMENT_SYSTEMS", ValidityPeriodMonths = 36, RenewalNoticeDays = 90, IsAccredited = true, AccreditationBody = "UKAS", RequiresAnnualSurveillance = true, CreatedDate = now, ModifiedDate = now, IsActive = true }
            );

            // Seed Audits
            modelBuilder.Entity<Audit>().HasData(
                new Audit { Id = 1, AuditNumber = "AUD-2024-001", AuditTitle = "ISO 9001:2015 Initial Certification Audit", Description = "Initial certification audit for quality management system", AuditDate = new DateTime(2024, 1, 10), LeadAuditorId = 1, Status = "COMPLETED", CreatedDate = now, ModifiedDate = now, IsActive = true },
                new Audit { Id = 2, AuditNumber = "AUD-2024-002", AuditTitle = "ISO 14001:2015 Initial Certification Audit", Description = "Initial certification audit for environmental management system", AuditDate = new DateTime(2024, 3, 1), LeadAuditorId = 1, Status = "COMPLETED", CreatedDate = now, ModifiedDate = now, IsActive = true }
            );

            // Seed Certificates
            modelBuilder.Entity<Certificate>().HasData(
                new Certificate { Id = 1, CertificateNumber = "CERT-ISO9001-2024-001", CompanyId = 1, AuditId = 1, CertificateTypeId = 1, Scope = "Design, manufacture and supply of industrial equipment", IssueDate = new DateTime(2024, 1, 15), ExpiryDate = new DateTime(2027, 1, 14), RenewalDate = new DateTime(2026, 10, 15), Status = "ACTIVE", CreatedDate = now, ModifiedDate = now, IsActive = true },
                new Certificate { Id = 2, CertificateNumber = "CERT-ISO14001-2024-001", CompanyId = 1, AuditId = 2, CertificateTypeId = 2, Scope = "Environmental management for manufacturing operations", IssueDate = new DateTime(2024, 3, 1), ExpiryDate = new DateTime(2027, 2, 28), RenewalDate = new DateTime(2026, 12, 1), Status = "ACTIVE", CreatedDate = now, ModifiedDate = now, IsActive = true }
            );
        }
    }
}