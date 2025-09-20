using Microsoft.EntityFrameworkCore;
using CustomerPortalAPI.Data;
using CustomerPortalAPI.Data.Repositories;
using CustomerPortalAPI.Modules.Certificates.Entities;

namespace CustomerPortalAPI.Modules.Certificates.Repositories
{
    public class CertificateRepository : Repository<Certificate>, ICertificateRepository
    {
        public CertificateRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Certificate>> GetCertificatesByCompanyAsync(int companyId)
        {
            return await _dbSet.Where(c => c.CompanyId == companyId).ToListAsync();
        }

        public async Task<IEnumerable<Certificate>> GetCertificatesBySiteAsync(int siteId)
        {
            return await _dbSet.Where(c => c.SiteId == siteId).ToListAsync();
        }

        public async Task<IEnumerable<Certificate>> GetCertificatesByStatusAsync(string status)
        {
            return await _dbSet.Where(c => c.Status == status).ToListAsync();
        }

        public async Task<IEnumerable<Certificate>> GetCertificatesByTypeAsync(string certificateType)
        {
            return await _dbSet.Where(c => c.CertificateType == certificateType).ToListAsync();
        }

        public async Task<IEnumerable<Certificate>> GetActiveCertificatesAsync()
        {
            return await _dbSet.Where(c => c.IsActive && c.Status == "Active").ToListAsync();
        }

        public async Task<IEnumerable<Certificate>> GetExpiredCertificatesAsync()
        {
            var today = DateTime.Today;
            return await _dbSet.Where(c => c.ExpiryDate < today).ToListAsync();
        }

        public async Task<IEnumerable<Certificate>> GetExpiringCertificatesAsync(int daysAhead)
        {
            var today = DateTime.Today;
            var futureDate = today.AddDays(daysAhead);
            return await _dbSet.Where(c => c.ExpiryDate >= today && c.ExpiryDate <= futureDate).ToListAsync();
        }

        public async Task<Certificate?> GetCertificateWithDetailsAsync(int certificateId)
        {
            return await _dbSet
                .Include(c => c.Company)
                .Include(c => c.Site)
                .Include(c => c.CertificateServices)
                .Include(c => c.CertificateSites)
                .Include(c => c.CertificateAdditionalScopes)
                .FirstOrDefaultAsync(c => c.Id == certificateId);
        }

        public async Task<Certificate?> GetByCertificateNumberAsync(string certificateNumber)
        {
            return await _dbSet.FirstOrDefaultAsync(c => c.CertificateNumber == certificateNumber);
        }

        public async Task<IEnumerable<Certificate>> GetCertificatesByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _dbSet.Where(c => c.IssueDate >= startDate && c.IssueDate <= endDate).ToListAsync();
        }

        public async Task<IEnumerable<Certificate>> SearchCertificatesAsync(string searchTerm)
        {
            return await _dbSet.Where(c => 
                c.CertificateName.Contains(searchTerm) ||
                c.CertificateNumber!.Contains(searchTerm) ||
                c.Standard!.Contains(searchTerm) ||
                c.IssuingBody!.Contains(searchTerm)).ToListAsync();
        }

        public async Task UpdateCertificateStatusAsync(int certificateId, string status, int modifiedBy)
        {
            var certificate = await GetByIdAsync(certificateId);
            if (certificate != null)
            {
                certificate.Status = status;
                certificate.ModifiedBy = modifiedBy;
                certificate.ModifiedDate = DateTime.UtcNow;
                await UpdateAsync(certificate);
            }
        }

        public async Task<int> GetCertificateCountByStatusAsync(string status)
        {
            return await _dbSet.CountAsync(c => c.Status == status);
        }
    }

    public class CertificateServiceRepository : Repository<CertificateService>, ICertificateServiceRepository
    {
        public CertificateServiceRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<CertificateService>> GetServicesByCertificateAsync(int certificateId)
        {
            return await _dbSet.Where(cs => cs.CertificateId == certificateId).ToListAsync();
        }

        public async Task<IEnumerable<CertificateService>> GetCertificatesByServiceAsync(int serviceId)
        {
            return await _dbSet.Where(cs => cs.ServiceId == serviceId).ToListAsync();
        }

        public async Task<IEnumerable<CertificateService>> GetCertificateServicesByStatusAsync(string status)
        {
            return await _dbSet.Where(cs => cs.Status == status).ToListAsync();
        }

        public async Task<CertificateService?> GetCertificateServiceAsync(int certificateId, int serviceId)
        {
            return await _dbSet.FirstOrDefaultAsync(cs => cs.CertificateId == certificateId && cs.ServiceId == serviceId);
        }

        public async Task AddServiceToCertificateAsync(int certificateId, int serviceId, string? serviceScope, int createdBy)
        {
            var existing = await GetCertificateServiceAsync(certificateId, serviceId);
            if (existing == null)
            {
                var certificateService = new CertificateService
                {
                    CertificateId = certificateId,
                    ServiceId = serviceId,
                    ServiceScope = serviceScope,
                    Status = "Active",
                    CreatedBy = createdBy,
                    CreatedDate = DateTime.UtcNow
                };
                await AddAsync(certificateService);
            }
        }

        public async Task RemoveServiceFromCertificateAsync(int certificateId, int serviceId)
        {
            var certificateService = await GetCertificateServiceAsync(certificateId, serviceId);
            if (certificateService != null)
            {
                await DeleteAsync(certificateService);
            }
        }

        public async Task UpdateCertificateServiceScopeAsync(int certificateServiceId, string? serviceScope, string? status)
        {
            var certificateService = await GetByIdAsync(certificateServiceId);
            if (certificateService != null)
            {
                certificateService.ServiceScope = serviceScope;
                certificateService.Status = status;
                await UpdateAsync(certificateService);
            }
        }
    }

    public class CertificateSiteRepository : Repository<CertificateSite>, ICertificateSiteRepository
    {
        public CertificateSiteRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<CertificateSite>> GetSitesByCertificateAsync(int certificateId)
        {
            return await _dbSet.Where(cs => cs.CertificateId == certificateId).ToListAsync();
        }

        public async Task<IEnumerable<CertificateSite>> GetCertificatesBySiteAsync(int siteId)
        {
            return await _dbSet.Where(cs => cs.SiteId == siteId).ToListAsync();
        }

        public async Task<IEnumerable<CertificateSite>> GetCertificateSitesByStatusAsync(string status)
        {
            return await _dbSet.Where(cs => cs.Status == status).ToListAsync();
        }

        public async Task<CertificateSite?> GetCertificateSiteAsync(int certificateId, int siteId)
        {
            return await _dbSet.FirstOrDefaultAsync(cs => cs.CertificateId == certificateId && cs.SiteId == siteId);
        }

        public async Task AddSiteToCertificateAsync(int certificateId, int siteId, string? siteScope, int createdBy)
        {
            var existing = await GetCertificateSiteAsync(certificateId, siteId);
            if (existing == null)
            {
                var certificateSite = new CertificateSite
                {
                    CertificateId = certificateId,
                    SiteId = siteId,
                    SiteScope = siteScope,
                    Status = "Active",
                    CreatedBy = createdBy,
                    CreatedDate = DateTime.UtcNow
                };
                await AddAsync(certificateSite);
            }
        }

        public async Task RemoveSiteFromCertificateAsync(int certificateId, int siteId)
        {
            var certificateSite = await GetCertificateSiteAsync(certificateId, siteId);
            if (certificateSite != null)
            {
                await DeleteAsync(certificateSite);
            }
        }

        public async Task UpdateCertificateSiteScopeAsync(int certificateSiteId, string? siteScope, string? status)
        {
            var certificateSite = await GetByIdAsync(certificateSiteId);
            if (certificateSite != null)
            {
                certificateSite.SiteScope = siteScope;
                certificateSite.Status = status;
                await UpdateAsync(certificateSite);
            }
        }
    }

    public class CertificateAdditionalScopeRepository : Repository<CertificateAdditionalScope>, ICertificateAdditionalScopeRepository
    {
        public CertificateAdditionalScopeRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<CertificateAdditionalScope>> GetScopesByCertificateAsync(int certificateId)
        {
            return await _dbSet.Where(cas => cas.CertificateId == certificateId).ToListAsync();
        }

        public async Task<IEnumerable<CertificateAdditionalScope>> GetScopesByTypeAsync(string scopeType)
        {
            return await _dbSet.Where(cas => cas.ScopeType == scopeType).ToListAsync();
        }

        public async Task<IEnumerable<CertificateAdditionalScope>> GetScopesByStatusAsync(string status)
        {
            return await _dbSet.Where(cas => cas.Status == status).ToListAsync();
        }

        public async Task AddScopeToCertificateAsync(int certificateId, string scopeDescription, string? scopeType, int createdBy)
        {
            var scope = new CertificateAdditionalScope
            {
                CertificateId = certificateId,
                ScopeDescription = scopeDescription,
                ScopeType = scopeType,
                Status = "Active",
                CreatedBy = createdBy,
                CreatedDate = DateTime.UtcNow
            };
            await AddAsync(scope);
        }

        public async Task UpdateScopeAsync(int scopeId, string scopeDescription, string? scopeType, string? status, int modifiedBy)
        {
            var scope = await GetByIdAsync(scopeId);
            if (scope != null)
            {
                scope.ScopeDescription = scopeDescription;
                scope.ScopeType = scopeType;
                scope.Status = status;
                scope.ModifiedBy = modifiedBy;
                scope.ModifiedDate = DateTime.UtcNow;
                await UpdateAsync(scope);
            }
        }

        public async Task RemoveScopeAsync(int scopeId)
        {
            var scope = await GetByIdAsync(scopeId);
            if (scope != null)
            {
                await DeleteAsync(scope);
            }
        }

        public async Task<IEnumerable<CertificateAdditionalScope>> SearchScopesAsync(string searchTerm)
        {
            return await _dbSet.Where(cas => 
                cas.ScopeDescription.Contains(searchTerm) ||
                cas.ScopeType!.Contains(searchTerm)).ToListAsync();
        }
    }
}