using CustomerPortalAPI.Modules.Actions.Repositories;
using CustomerPortalAPI.Modules.Audits.Repositories;
// using CustomerPortalAPI.Modules.Certificates.Repositories;
// using CustomerPortalAPI.Modules.Contracts.Repositories;
// using CustomerPortalAPI.Modules.Financial.Repositories;
// using CustomerPortalAPI.Modules.Findings.Repositories;
using CustomerPortalAPI.Modules.Master.Repositories;
// using CustomerPortalAPI.Modules.Notifications.Repositories;
// using CustomerPortalAPI.Modules.Settings.Repositories;
using CustomerPortalAPI.Modules.Users.Repositories;

namespace CustomerPortalAPI.Data
{
    public interface IUnitOfWork : IDisposable
    {
        // Actions Module
        IActionRepository Actions { get; }

        // Audits Module
        IAuditRepository Audits { get; }
        IAuditLogRepository AuditLogs { get; }
        IAuditServiceRepository AuditServices { get; }
        IAuditSiteRepository AuditSites { get; }
        IAuditTeamMemberRepository AuditTeamMembers { get; }
        IAuditTypeRepository AuditTypes { get; }

        // Certificates Module - to be implemented
        // ICertificateRepository Certificates { get; }
        // ICertificateServiceRepository CertificateServices { get; }
        // ICertificateSiteRepository CertificateSites { get; }

        // Contracts Module - to be implemented
        // IContractRepository Contracts { get; }
        // IContractServiceRepository ContractServices { get; }
        // IContractSiteRepository ContractSites { get; }

        // Financial Module - to be implemented
        // IFinancialRepository Financials { get; }
        // IInvoiceRepository Invoices { get; }

        // Findings Module - to be implemented
        // IFindingRepository Findings { get; }
        // IFindingCategoryRepository FindingCategories { get; }
        // IFindingStatusRepository FindingStatuses { get; }

        // Master Module
        ICityRepository Cities { get; }
        ICompanyRepository Companies { get; }
        ICountryRepository Countries { get; }
        IRoleRepository Roles { get; }
        IServiceRepository Services { get; }
        ISiteRepository Sites { get; }

        // Notifications Module - to be implemented
        // INotificationRepository Notifications { get; }
        // INotificationCategoryRepository NotificationCategories { get; }

        // Settings Module - to be implemented
        // ITrainingRepository Trainings { get; }
        // IErrorLogRepository ErrorLogs { get; }

        // Users Module
        IUserRepository Users { get; }
        IUserRoleRepository UserRoles { get; }
        IUserCompanyAccessRepository UserCompanyAccesses { get; }
        IUserSiteAccessRepository UserSiteAccesses { get; }

        Task<int> SaveChangesAsync();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}