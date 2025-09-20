using CustomerPortalAPI.Data.Repositories;
using CustomerPortalAPI.Modules.Certificates.Entities;

namespace CustomerPortalAPI.Modules.Certificates.Repositories
{
    public interface ICertificateRepository : IRepository<Certificate>
    {
        Task<IEnumerable<Certificate>> GetCertificatesByCompanyAsync(int companyId);
        Task<IEnumerable<Certificate>> GetCertificatesBySiteAsync(int siteId);
        Task<IEnumerable<Certificate>> GetCertificatesByStatusAsync(string status);
        Task<IEnumerable<Certificate>> GetCertificatesByTypeAsync(string certificateType);
        Task<IEnumerable<Certificate>> GetActiveCertificatesAsync();
        Task<IEnumerable<Certificate>> GetExpiredCertificatesAsync();
        Task<IEnumerable<Certificate>> GetExpiringCertificatesAsync(int daysAhead);
        Task<Certificate?> GetCertificateWithDetailsAsync(int certificateId);
        Task<Certificate?> GetByCertificateNumberAsync(string certificateNumber);
        Task<IEnumerable<Certificate>> GetCertificatesByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<Certificate>> SearchCertificatesAsync(string searchTerm);
        Task UpdateCertificateStatusAsync(int certificateId, string status, int modifiedBy);
        Task<int> GetCertificateCountByStatusAsync(string status);
    }

    public interface ICertificateServiceRepository : IRepository<CertificateService>
    {
        Task<IEnumerable<CertificateService>> GetServicesByCertificateAsync(int certificateId);
        Task<IEnumerable<CertificateService>> GetCertificatesByServiceAsync(int serviceId);
        Task<IEnumerable<CertificateService>> GetCertificateServicesByStatusAsync(string status);
        Task<CertificateService?> GetCertificateServiceAsync(int certificateId, int serviceId);
        Task AddServiceToCertificateAsync(int certificateId, int serviceId, string? serviceScope, int createdBy);
        Task RemoveServiceFromCertificateAsync(int certificateId, int serviceId);
        Task UpdateCertificateServiceScopeAsync(int certificateServiceId, string? serviceScope, string? status);
    }

    public interface ICertificateSiteRepository : IRepository<CertificateSite>
    {
        Task<IEnumerable<CertificateSite>> GetSitesByCertificateAsync(int certificateId);
        Task<IEnumerable<CertificateSite>> GetCertificatesBySiteAsync(int siteId);
        Task<IEnumerable<CertificateSite>> GetCertificateSitesByStatusAsync(string status);
        Task<CertificateSite?> GetCertificateSiteAsync(int certificateId, int siteId);
        Task AddSiteToCertificateAsync(int certificateId, int siteId, string? siteScope, int createdBy);
        Task RemoveSiteFromCertificateAsync(int certificateId, int siteId);
        Task UpdateCertificateSiteScopeAsync(int certificateSiteId, string? siteScope, string? status);
    }

    public interface ICertificateAdditionalScopeRepository : IRepository<CertificateAdditionalScope>
    {
        Task<IEnumerable<CertificateAdditionalScope>> GetScopesByCertificateAsync(int certificateId);
        Task<IEnumerable<CertificateAdditionalScope>> GetScopesByTypeAsync(string scopeType);
        Task<IEnumerable<CertificateAdditionalScope>> GetScopesByStatusAsync(string status);
        Task AddScopeToCertificateAsync(int certificateId, string scopeDescription, string? scopeType, int createdBy);
        Task UpdateScopeAsync(int scopeId, string scopeDescription, string? scopeType, string? status, int modifiedBy);
        Task RemoveScopeAsync(int scopeId);
        Task<IEnumerable<CertificateAdditionalScope>> SearchScopesAsync(string searchTerm);
    }
}