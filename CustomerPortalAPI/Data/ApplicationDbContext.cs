using Microsoft.EntityFrameworkCore;
using CustomerPortalAPI.Modules.Actions.Entities;
using CustomerPortalAPI.Modules.Audits.Entities;
using CustomerPortalAPI.Modules.Certificates.Entities;
using CustomerPortalAPI.Modules.Contracts.Entities;
using CustomerPortalAPI.Modules.Financial.Entities;
using CustomerPortalAPI.Modules.Master.Entities;
using CustomerPortalAPI.Modules.Users.Entities;
using CustomerPortalAPI.Modules.Findings.Entities;
using CustomerPortalAPI.Modules.Notifications.Entities;
using CustomerPortalAPI.Modules.Settings.Entities;
using CustomerPortalAPI.Modules.Overview.Entities;
using CustomerPortalAPI.Modules.Widgets.Entities;

namespace CustomerPortalAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // Actions Module
        public DbSet<Modules.Actions.Entities.Action> Actions { get; set; }

        // Audits Module
        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<Audit> Audits { get; set; }
        public DbSet<AuditService> AuditServices { get; set; }
        public DbSet<AuditSiteAudit> AuditSiteAudits { get; set; }
        public DbSet<AuditSiteRepresentative> AuditSiteRepresentatives { get; set; }
        public DbSet<AuditSite> AuditSites { get; set; }
        public DbSet<AuditSiteService> AuditSiteServices { get; set; }
        public DbSet<AuditTeamMember> AuditTeamMembers { get; set; }
        public DbSet<AuditType> AuditTypes { get; set; }

        // Certificates Module
        public DbSet<CertificateAdditionalScope> CertificateAdditionalScopes { get; set; }
        public DbSet<Certificate> Certificates { get; set; }
        public DbSet<CertificateService> CertificateServices { get; set; }
        public DbSet<CertificateSite> CertificateSites { get; set; }

        // Contracts Module
        public DbSet<Contract> Contracts { get; set; }
        public DbSet<ContractService> ContractServices { get; set; }
        public DbSet<ContractSite> ContractSites { get; set; }

        // Financial Module
        public DbSet<FinancialTransaction> Financials { get; set; }
        public DbSet<InvoiceAuditLog> InvoiceAuditLogs { get; set; }
        public DbSet<Invoice> Invoices { get; set; }

        // Findings Module
        public DbSet<FindingCategory> FindingCategories { get; set; }
        public DbSet<Finding> Findings { get; set; }
        public DbSet<FindingStatus> FindingStatuses { get; set; }

        // Notifications Module
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<NotificationCategory> NotificationCategories { get; set; }

        // Settings Module
        public DbSet<Training> Trainings { get; set; }
        public DbSet<ErrorLog> ErrorLogs { get; set; }

        // Overview Module
        public DbSet<OverviewDashboard> OverviewDashboards { get; set; }
        public DbSet<OverviewMetric> OverviewMetrics { get; set; }
        public DbSet<OverviewReport> OverviewReports { get; set; }

        // Widgets Module
        public DbSet<Widget> Widgets { get; set; }
        public DbSet<WidgetCategory> WidgetCategories { get; set; }
        public DbSet<WidgetUserAccess> WidgetUserAccesses { get; set; }
        public DbSet<WidgetData> WidgetData { get; set; }

        // Master Module
        public DbSet<Chapter> Chapters { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Clause> Clauses { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<FocusArea> FocusAreas { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Site> Sites { get; set; }

        // Notifications Module - entities to be implemented
        // public DbSet<NotificationCategory> NotificationCategories { get; set; }
        // public DbSet<Notification> Notifications { get; set; }

        // Settings Module - entities to be implemented
        // public DbSet<ErrorLog> ErrorLogs { get; set; }
        // public DbSet<Training> Trainings { get; set; }

        // Users Module
        public DbSet<User> Users { get; set; }
        public DbSet<UserCityAccess> UserCityAccesses { get; set; }
        public DbSet<UserCompanyAccess> UserCompanyAccesses { get; set; }
        public DbSet<UserCountryAccess> UserCountryAccesses { get; set; }
        public DbSet<UserNotificationAccess> UserNotificationAccesses { get; set; }
        public DbSet<UserPreference> UserPreferences { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<UserServiceAccess> UserServiceAccesses { get; set; }
        public DbSet<UserSiteAccess> UserSiteAccesses { get; set; }
        public DbSet<UserTraining> UserTrainings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Apply entity configurations
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

            // Configure table names to match existing database schema
            ConfigureTableNames(modelBuilder);

            // Configure relationships
            ConfigureRelationships(modelBuilder);
        }

        private void ConfigureTableNames(ModelBuilder modelBuilder)
        {
            // Actions Module
            modelBuilder.Entity<Modules.Actions.Entities.Action>().ToTable("Actions");

            // Audits Module
            modelBuilder.Entity<AuditLog>().ToTable("AuditLogs");
            modelBuilder.Entity<Audit>().ToTable("Audits");
            modelBuilder.Entity<AuditService>().ToTable("AuditServices");
            modelBuilder.Entity<AuditSiteAudit>().ToTable("AuditSiteAudits");
            modelBuilder.Entity<AuditSiteRepresentative>().ToTable("AuditSiteRepresentatives");
            modelBuilder.Entity<AuditSite>().ToTable("AuditSites");
            modelBuilder.Entity<AuditSiteService>().ToTable("AuditSiteServices");
            modelBuilder.Entity<AuditTeamMember>().ToTable("AuditTeamMembers");
            modelBuilder.Entity<AuditType>().ToTable("AuditTypes");

            // Certificates Module
            modelBuilder.Entity<CertificateAdditionalScope>().ToTable("CertificateAdditionalScopes");
            modelBuilder.Entity<Certificate>().ToTable("Certificates");
            modelBuilder.Entity<CertificateService>().ToTable("CertificateServices");
            modelBuilder.Entity<CertificateSite>().ToTable("CertificateSites");

            // Contracts Module
            modelBuilder.Entity<Contract>().ToTable("Contracts");
            modelBuilder.Entity<ContractService>().ToTable("ContractServices");
            modelBuilder.Entity<ContractSite>().ToTable("ContractSites");

            // Financial Module
            modelBuilder.Entity<FinancialTransaction>().ToTable("Financials");
            modelBuilder.Entity<InvoiceAuditLog>().ToTable("InvoiceAuditLog");
            modelBuilder.Entity<Invoice>().ToTable("Invoices");

            // Findings Module
            modelBuilder.Entity<FindingCategory>().ToTable("FindingCategories");
            modelBuilder.Entity<Finding>().ToTable("Findings");
            modelBuilder.Entity<FindingStatus>().ToTable("FindingStatuses");

            // Notifications Module
            modelBuilder.Entity<Notification>().ToTable("Notifications");
            modelBuilder.Entity<NotificationCategory>().ToTable("NotificationCategories");

            // Settings Module
            modelBuilder.Entity<Training>().ToTable("Trainings");
            modelBuilder.Entity<ErrorLog>().ToTable("ErrorLogs");

            // Overview Module
            modelBuilder.Entity<OverviewDashboard>().ToTable("OverviewDashboard");
            modelBuilder.Entity<OverviewMetric>().ToTable("OverviewMetrics");
            modelBuilder.Entity<OverviewReport>().ToTable("OverviewReports");

            // Widgets Module
            modelBuilder.Entity<Widget>().ToTable("Widgets");
            modelBuilder.Entity<WidgetCategory>().ToTable("WidgetCategories");
            modelBuilder.Entity<WidgetUserAccess>().ToTable("WidgetUserAccess");
            modelBuilder.Entity<WidgetData>().ToTable("WidgetData");

            // Master Module
            modelBuilder.Entity<Chapter>().ToTable("Chapters");
            modelBuilder.Entity<City>().ToTable("Cities");
            modelBuilder.Entity<Clause>().ToTable("Clauses");
            modelBuilder.Entity<Company>().ToTable("Companies");
            modelBuilder.Entity<Country>().ToTable("Countries");
            modelBuilder.Entity<FocusArea>().ToTable("FocusAreas");
            modelBuilder.Entity<Role>().ToTable("Roles");
            modelBuilder.Entity<Service>().ToTable("Services");
            modelBuilder.Entity<Site>().ToTable("Sites");

            // Notifications Module - table configurations to be implemented
            // modelBuilder.Entity<NotificationCategory>().ToTable("NotificationCategories");
            // modelBuilder.Entity<Notification>().ToTable("Notifications");

            // Settings Module - table configurations to be implemented
            // modelBuilder.Entity<ErrorLog>().ToTable("ErrorLogs");
            // modelBuilder.Entity<Training>().ToTable("Trainings");

            // Users Module
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<UserCityAccess>().ToTable("UserCityAccess");
            modelBuilder.Entity<UserCompanyAccess>().ToTable("UserCompanyAccess");
            modelBuilder.Entity<UserCountryAccess>().ToTable("UserCountryAccess");
            modelBuilder.Entity<UserNotificationAccess>().ToTable("UserNotificationAccess");
            modelBuilder.Entity<UserPreference>().ToTable("UserPreferences");
            modelBuilder.Entity<UserRole>().ToTable("UserRoles");
            modelBuilder.Entity<UserServiceAccess>().ToTable("UserServiceAccess");
            modelBuilder.Entity<UserSiteAccess>().ToTable("UserSiteAccess");
            modelBuilder.Entity<UserTraining>().ToTable("UserTrainings");
        }

        private void ConfigureRelationships(ModelBuilder modelBuilder)
        {
            // Configure foreign key relationships based on existing database schema
            // This will be implemented when entity models are created
        }
    }
}