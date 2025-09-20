using CustomerPortal.CertificatesService.Entities;
using CustomerPortal.Shared.Interfaces;

namespace CustomerPortal.CertificatesService.Repositories
{
    /// <summary>
    /// Repository interface for Certificate entity
    /// </summary>
    public interface ICertificateRepository : IRepository<Certificate>
    {
        Task<IEnumerable<Certificate>> GetByCertificateNumberAsync(string certificateNumber);
        Task<IEnumerable<Certificate>> GetByCompanyIdAsync(int companyId);
        Task<IEnumerable<Certificate>> GetByStatusAsync(string status);
        Task<IEnumerable<Certificate>> GetExpiringCertificatesAsync(int withinDays);
        Task<IEnumerable<Certificate>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<Dictionary<string, int>> GetCertificatesByStatusAsync();
        Task<Dictionary<string, int>> GetCertificatesByTypeAsync();
        Task<IEnumerable<Certificate>> GetRenewalScheduleAsync(DateTime startDate, DateTime endDate);
        Task<Certificate?> ValidateCertificateAsync(string certificateNumber);
        Task<int> GetTotalCountAsync();
        Task<decimal> GetRenewalSuccessRateAsync();
        Task<double> GetAverageRenewalTimeAsync();
    }

    /// <summary>
    /// Repository interface for CertificateType entity
    /// </summary>
    public interface ICertificateTypeRepository : IRepository<CertificateType>
    {
        Task<IEnumerable<CertificateType>> GetByStandardAsync(string standard);
        Task<IEnumerable<CertificateType>> GetByCategoryAsync(string category);
        Task<IEnumerable<CertificateType>> GetAccreditedTypesAsync();
        Task<CertificateType?> GetByTypeNameAsync(string typeName);
    }

    /// <summary>
    /// Repository interface for CertificateSite entity
    /// </summary>
    public interface ICertificateSiteRepository : IRepository<CertificateSite>
    {
        Task<IEnumerable<CertificateSite>> GetByCertificateIdAsync(int certificateId);
        Task<IEnumerable<CertificateSite>> GetBySiteIdAsync(int siteId);
        Task<IEnumerable<CertificateSite>> GetActiveSitesAsync(int certificateId);
    }

    /// <summary>
    /// Repository interface for CertificateService entity
    /// </summary>
    public interface ICertificateServiceRepository : IRepository<CertificateService>
    {
        Task<IEnumerable<CertificateService>> GetByCertificateIdAsync(int certificateId);
        Task<IEnumerable<CertificateService>> GetByServiceIdAsync(int serviceId);
        Task<IEnumerable<CertificateService>> GetActiveServicesAsync(int certificateId);
    }

    /// <summary>
    /// Repository interface for CertificateAdditionalScope entity
    /// </summary>
    public interface ICertificateAdditionalScopeRepository : IRepository<CertificateAdditionalScope>
    {
        Task<IEnumerable<CertificateAdditionalScope>> GetByCertificateIdAsync(int certificateId);
        Task<IEnumerable<CertificateAdditionalScope>> GetIncludedScopesAsync(int certificateId);
    }

    /// <summary>
    /// Repository interface for CertificateRenewal entity
    /// </summary>
    public interface ICertificateRenewalRepository : IRepository<CertificateRenewal>
    {
        Task<IEnumerable<CertificateRenewal>> GetByCertificateIdAsync(int certificateId);
        Task<IEnumerable<CertificateRenewal>> GetByStatusAsync(string status);
        Task<IEnumerable<CertificateRenewal>> GetByAuditorIdAsync(int auditorId);
        Task<CertificateRenewal?> GetActiveRenewalAsync(int certificateId);
        Task<IEnumerable<CertificateRenewal>> GetScheduledRenewalsAsync(DateTime startDate, DateTime endDate);
    }

    /// <summary>
    /// Repository interface for CertificateValidation entity
    /// </summary>
    public interface ICertificateValidationRepository : IRepository<CertificateValidation>
    {
        Task<IEnumerable<CertificateValidation>> GetByCertificateIdAsync(int certificateId);
        Task<CertificateValidation?> GetByVerificationCodeAsync(string verificationCode);
        Task<IEnumerable<CertificateValidation>> GetByValidatorIdAsync(int validatorId);
        Task<CertificateValidation?> GetLatestValidationAsync(int certificateId);
    }

    /// <summary>
    /// Repository interface for CertificateDocument entity
    /// </summary>
    public interface ICertificateDocumentRepository : IRepository<CertificateDocument>
    {
        Task<IEnumerable<CertificateDocument>> GetByCertificateIdAsync(int certificateId);
        Task<IEnumerable<CertificateDocument>> GetByDocumentTypeAsync(string documentType);
        Task<CertificateDocument?> GetLatestDocumentAsync(int certificateId, string documentType);
        Task<IEnumerable<CertificateDocument>> GetLatestDocumentsAsync(int certificateId);
    }

    /// <summary>
    /// Repository interface for Company entity
    /// </summary>
    public interface ICompanyRepository : IRepository<Company>
    {
        Task<Company?> GetByCompanyCodeAsync(string companyCode);
        Task<IEnumerable<Company>> GetByCompanyNameAsync(string companyName);
        Task<IEnumerable<Company>> SearchCompaniesAsync(string searchTerm);
    }

    /// <summary>
    /// Repository interface for Audit entity
    /// </summary>
    public interface IAuditRepository : IRepository<Audit>
    {
        Task<Audit?> GetByAuditNumberAsync(string auditNumber);
        Task<IEnumerable<Audit>> GetByLeadAuditorIdAsync(int auditorId);
        Task<IEnumerable<Audit>> GetByStatusAsync(string status);
        Task<IEnumerable<Audit>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
    }

    /// <summary>
    /// Repository interface for Site entity
    /// </summary>
    public interface ISiteRepository : IRepository<Site>
    {
        Task<Site?> GetBySiteCodeAsync(string siteCode);
        Task<IEnumerable<Site>> GetByCompanyIdAsync(int companyId);
        Task<IEnumerable<Site>> GetByCityIdAsync(int cityId);
        Task<IEnumerable<Site>> SearchSitesAsync(string searchTerm);
    }

    /// <summary>
    /// Repository interface for Service entity
    /// </summary>
    public interface IServiceRepository : IRepository<Service>
    {
        Task<Service?> GetByServiceCodeAsync(string serviceCode);
        Task<IEnumerable<Service>> GetByCategoryAsync(string category);
        Task<IEnumerable<Service>> SearchServicesAsync(string searchTerm);
    }

    /// <summary>
    /// Repository interface for City entity
    /// </summary>
    public interface ICityRepository : IRepository<City>
    {
        Task<IEnumerable<City>> GetByCountryIdAsync(int countryId);
        Task<City?> GetByCityCodeAsync(string cityCode);
        Task<IEnumerable<City>> SearchCitiesAsync(string searchTerm);
    }

    /// <summary>
    /// Repository interface for Country entity
    /// </summary>
    public interface ICountryRepository : IRepository<Country>
    {
        Task<Country?> GetByCountryCodeAsync(string countryCode);
        Task<IEnumerable<Country>> SearchCountriesAsync(string searchTerm);
    }

    /// <summary>
    /// Repository interface for User entity
    /// </summary>
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> GetByEmailAsync(string email);
        Task<IEnumerable<User>> GetByRoleAsync(string role);
        Task<IEnumerable<User>> GetByDepartmentAsync(string department);
        Task<IEnumerable<User>> GetAuditorsAsync();
        Task<IEnumerable<User>> SearchUsersAsync(string searchTerm);
    }
}