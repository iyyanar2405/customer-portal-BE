using CustomerPortal.AuditsService.Entities;
using Microsoft.EntityFrameworkCore;

namespace CustomerPortal.AuditsService.Data
{
    public class AuditsDbContext : DbContext
    {
        public AuditsDbContext(DbContextOptions<AuditsDbContext> options) : base(options)
        {
        }

        // DbSets
        public DbSet<Audit> Audits { get; set; }
        public DbSet<AuditType> AuditTypes { get; set; }
        public DbSet<AuditSite> AuditSites { get; set; }
        public DbSet<AuditService> AuditServices { get; set; }
        public DbSet<AuditTeamMember> AuditTeamMembers { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<AuditSiteAudit> AuditSiteAudits { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Site> Sites { get; set; }
        public DbSet<Service> Services { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Audit entity
            modelBuilder.Entity<Audit>(entity =>
            {
                entity.ToTable("Audits");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.AuditNumber).IsRequired().HasMaxLength(50);
                entity.Property(e => e.AuditTitle).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Status).IsRequired().HasMaxLength(20).HasDefaultValue("PLANNED");
                entity.Property(e => e.StartDate).IsRequired();
                entity.Property(e => e.EndDate).IsRequired();
                
                // Relationships
                entity.HasOne(e => e.Company)
                    .WithMany(c => c.Audits)
                    .HasForeignKey(e => e.CompanyId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.AuditType)
                    .WithMany(at => at.Audits)
                    .HasForeignKey(e => e.AuditTypeId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.LeadAuditor)
                    .WithMany(u => u.LeadAudits)
                    .HasForeignKey(e => e.LeadAuditorId)
                    .OnDelete(DeleteBehavior.SetNull);

                // Index for audit number (should be unique)
                entity.HasIndex(e => e.AuditNumber).IsUnique();
            });

            // Configure Company entity
            modelBuilder.Entity<Company>(entity =>
            {
                entity.ToTable("Companies");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.CompanyName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.CompanyCode).IsRequired().HasMaxLength(20);
                entity.Property(e => e.Address).HasMaxLength(500);
                entity.Property(e => e.ContactPerson).HasMaxLength(100);
                entity.Property(e => e.ContactEmail).HasMaxLength(50);
                entity.Property(e => e.ContactPhone).HasMaxLength(20);

                // Index for company code (should be unique)
                entity.HasIndex(e => e.CompanyCode).IsUnique();
            });

            // Configure User entity
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.FirstName).IsRequired().HasMaxLength(50);
                entity.Property(e => e.LastName).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Phone).HasMaxLength(20);
                entity.Property(e => e.Qualifications).HasMaxLength(500);
                entity.Property(e => e.Role).HasMaxLength(20).HasDefaultValue("USER");

                // Index for email (should be unique)
                entity.HasIndex(e => e.Email).IsUnique();
            });

            // Configure Site entity
            modelBuilder.Entity<Site>(entity =>
            {
                entity.ToTable("Sites");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.SiteName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.SiteCode).IsRequired().HasMaxLength(20);
                entity.Property(e => e.Address).HasMaxLength(500);
                entity.Property(e => e.ContactPerson).HasMaxLength(100);
                entity.Property(e => e.ContactEmail).HasMaxLength(50);
                entity.Property(e => e.ContactPhone).HasMaxLength(20);

                // Relationship with Company
                entity.HasOne(e => e.Company)
                    .WithMany()
                    .HasForeignKey(e => e.CompanyId)
                    .OnDelete(DeleteBehavior.Restrict);

                // Index for site code (should be unique)
                entity.HasIndex(e => e.SiteCode).IsUnique();
            });

            // Configure Service entity
            modelBuilder.Entity<Service>(entity =>
            {
                entity.ToTable("Services");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.ServiceName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.ServiceCode).HasMaxLength(20);
                entity.Property(e => e.Description).HasMaxLength(500);

                // Index for service code (if provided, should be unique)
                entity.HasIndex(e => e.ServiceCode).IsUnique();
            });

            // Configure AuditType entity
            modelBuilder.Entity<AuditType>(entity =>
            {
                entity.ToTable("AuditTypes");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.AuditTypeName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.Property(e => e.EstimatedDurationDays).HasDefaultValue(1);
            });

            // Configure AuditSite entity
            modelBuilder.Entity<AuditSite>(entity =>
            {
                entity.ToTable("AuditSites");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Status).HasMaxLength(20).HasDefaultValue("SCHEDULED");
                entity.Property(e => e.Notes).HasMaxLength(1000);

                // Composite unique constraint
                entity.HasIndex(e => new { e.AuditId, e.SiteId }).IsUnique();
            });

            // Configure AuditService entity
            modelBuilder.Entity<AuditService>(entity =>
            {
                entity.ToTable("AuditServices");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Status).HasMaxLength(20).HasDefaultValue("ACTIVE");
                entity.Property(e => e.Notes).HasMaxLength(1000);

                // Composite unique constraint
                entity.HasIndex(e => new { e.AuditId, e.ServiceId }).IsUnique();
            });

            // Configure AuditTeamMember entity
            modelBuilder.Entity<AuditTeamMember>(entity =>
            {
                entity.ToTable("AuditTeamMembers");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Role).IsRequired().HasMaxLength(30).HasDefaultValue("AUDITOR");
                entity.Property(e => e.AssignedDate).HasDefaultValueSql("GETUTCDATE()");
                entity.Property(e => e.Notes).HasMaxLength(1000);

                // Composite unique constraint
                entity.HasIndex(e => new { e.AuditId, e.UserId }).IsUnique();
            });

            // Configure AuditLog entity
            modelBuilder.Entity<AuditLog>(entity =>
            {
                entity.ToTable("AuditLogs");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Action).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Description).HasMaxLength(1000);
                entity.Property(e => e.ActionDate).HasDefaultValueSql("GETUTCDATE()");
                entity.Property(e => e.IpAddress).HasMaxLength(50);
            });

            // Configure AuditSiteAudit entity
            modelBuilder.Entity<AuditSiteAudit>(entity =>
            {
                entity.ToTable("AuditSiteAudits");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Status).HasMaxLength(20).HasDefaultValue("PENDING");
                entity.Property(e => e.Notes).HasMaxLength(1000);
                entity.Property(e => e.ComplianceScore).HasColumnType("decimal(5,2)");

                // Composite unique constraint
                entity.HasIndex(e => new { e.AuditId, e.SiteId }).IsUnique();

                // Additional relationship with LeadAuditor
                entity.HasOne(e => e.LeadAuditor)
                    .WithMany()
                    .HasForeignKey(e => e.LeadAuditorId)
                    .OnDelete(DeleteBehavior.SetNull);
            });
        }

        public async Task SeedDataAsync()
        {
            // Seed Companies
            if (!Companies.Any())
            {
                var companies = new List<Company>
                {
                    new Company { CompanyName = "Acme Corporation", CompanyCode = "ACME001", Address = "123 Business St, City, State", ContactPerson = "John Smith", ContactEmail = "john.smith@acme.com", ContactPhone = "+1-555-0101" },
                    new Company { CompanyName = "Tech Industries Ltd", CompanyCode = "TECH002", Address = "456 Innovation Ave, Tech City, State", ContactPerson = "Jane Doe", ContactEmail = "jane.doe@techindustries.com", ContactPhone = "+1-555-0102" }
                };
                Companies.AddRange(companies);
                await SaveChangesAsync();
            }

            // Seed Users
            if (!Users.Any())
            {
                var users = new List<User>
                {
                    new User { FirstName = "John", LastName = "Auditor", Email = "john.auditor@certbody.com", Role = "LEAD_AUDITOR", Qualifications = "ISO 9001, ISO 14001 Lead Auditor" },
                    new User { FirstName = "Sarah", LastName = "Inspector", Email = "sarah.inspector@certbody.com", Role = "AUDITOR", Qualifications = "ISO 9001 Auditor" },
                    new User { FirstName = "Michael", LastName = "Expert", Email = "michael.expert@certbody.com", Role = "TECHNICAL_EXPERT", Qualifications = "Environmental Management Expert" }
                };
                Users.AddRange(users);
                await SaveChangesAsync();
            }

            // Seed Sites
            if (!Sites.Any())
            {
                var sites = new List<Site>
                {
                    new Site { SiteName = "Main Production Facility", SiteCode = "ACME-PROD-01", CompanyId = 1, Address = "789 Manufacturing Dr, Industrial Park", ContactPerson = "Bob Wilson", ContactEmail = "bob.wilson@acme.com" },
                    new Site { SiteName = "R&D Laboratory", SiteCode = "TECH-LAB-01", CompanyId = 2, Address = "321 Research Blvd, Science Park", ContactPerson = "Alice Brown", ContactEmail = "alice.brown@techindustries.com" }
                };
                Sites.AddRange(sites);
                await SaveChangesAsync();
            }

            // Seed Services
            if (!Services.Any())
            {
                var services = new List<Service>
                {
                    new Service { ServiceName = "Quality Management System", ServiceCode = "QMS", Description = "ISO 9001:2015 Quality Management System" },
                    new Service { ServiceName = "Environmental Management System", ServiceCode = "EMS", Description = "ISO 14001:2015 Environmental Management System" },
                    new Service { ServiceName = "Occupational Health & Safety", ServiceCode = "OHSMS", Description = "ISO 45001:2018 OH&S Management System" }
                };
                Services.AddRange(services);
                await SaveChangesAsync();
            }

            // Seed Audit Types
            if (!AuditTypes.Any())
            {
                var auditTypes = new List<AuditType>
                {
                    new AuditType { AuditTypeName = "Surveillance Audit", Description = "Regular surveillance audit for maintaining certification", EstimatedDurationDays = 2 },
                    new AuditType { AuditTypeName = "Initial Certification Audit", Description = "Initial certification audit for new certificates", EstimatedDurationDays = 5 },
                    new AuditType { AuditTypeName = "Re-certification Audit", Description = "Re-certification audit for certificate renewal", EstimatedDurationDays = 3 }
                };
                AuditTypes.AddRange(auditTypes);
                await SaveChangesAsync();
            }

            // Seed Audits
            if (!Audits.Any())
            {
                var audits = new List<Audit>
                {
                    new Audit 
                    { 
                        AuditNumber = "AUD-2024-001", 
                        AuditTitle = "ISO 9001:2015 Annual Surveillance Audit", 
                        CompanyId = 1, 
                        AuditTypeId = 1, 
                        StartDate = new DateTime(2024, 10, 1, 9, 0, 0), 
                        EndDate = new DateTime(2024, 10, 3, 17, 0, 0), 
                        Status = "PLANNED", 
                        LeadAuditorId = 1 
                    },
                    new Audit 
                    { 
                        AuditNumber = "AUD-2024-002", 
                        AuditTitle = "ISO 14001:2015 Initial Certification Audit", 
                        CompanyId = 2, 
                        AuditTypeId = 2, 
                        StartDate = new DateTime(2024, 11, 15, 9, 0, 0), 
                        EndDate = new DateTime(2024, 11, 17, 17, 0, 0), 
                        Status = "PLANNED", 
                        LeadAuditorId = 2 
                    }
                };
                Audits.AddRange(audits);
                await SaveChangesAsync();
            }
        }
    }
}