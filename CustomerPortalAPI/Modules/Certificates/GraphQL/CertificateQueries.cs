using CustomerPortalAPI.Modules.Certificates.Repositories;
using CustomerPortalAPI.Modules.Certificates.GraphQL;
using HotChocolate;

namespace CustomerPortalAPI.Modules.Certificates.GraphQL
{
    public class CertificateQueries
    {
        // Certificate Queries
        public async Task<IEnumerable<CertificateOutput>> GetCertificates(
            [Service] ICertificateRepository repository,
            CertificateFilterInput? filter = null)
        {
            var certificates = await repository.GetAllAsync();
            
            if (filter != null)
            {
                if (filter.CertificateName != null)
                    certificates = certificates.Where(c => c.CertificateName.Contains(filter.CertificateName, StringComparison.OrdinalIgnoreCase));
                if (filter.CertificateNumber != null)
                    certificates = certificates.Where(c => c.CertificateNumber != null && c.CertificateNumber.Contains(filter.CertificateNumber, StringComparison.OrdinalIgnoreCase));
                if (filter.CompanyId.HasValue)
                    certificates = certificates.Where(c => c.CompanyId == filter.CompanyId.Value);
                if (filter.SiteId.HasValue)
                    certificates = certificates.Where(c => c.SiteId == filter.SiteId.Value);
                if (filter.CertificateType != null)
                    certificates = certificates.Where(c => c.CertificateType != null && c.CertificateType.Contains(filter.CertificateType, StringComparison.OrdinalIgnoreCase));
                if (filter.Standard != null)
                    certificates = certificates.Where(c => c.Standard != null && c.Standard.Contains(filter.Standard, StringComparison.OrdinalIgnoreCase));
                if (filter.Status != null)
                    certificates = certificates.Where(c => c.Status != null && c.Status.Equals(filter.Status, StringComparison.OrdinalIgnoreCase));
                if (filter.IsActive.HasValue)
                    certificates = certificates.Where(c => c.IsActive == filter.IsActive.Value);
                if (filter.IsExpiring.HasValue && filter.IsExpiring.Value)
                    certificates = certificates.Where(c => c.ExpiryDate.HasValue && c.ExpiryDate.Value <= DateTime.UtcNow.AddDays(90));
            }
            
            return certificates.Select(c => new CertificateOutput(
                c.Id,
                c.CertificateName,
                c.CertificateNumber,
                c.CompanyId,
                c.SiteId,
                c.CertificateType,
                c.Standard,
                c.IssueDate,
                c.ExpiryDate,
                c.IssuingBody,
                c.CertificationBody,
                c.Status,
                c.Scope,
                c.ExclusionsLimitations,
                c.DocumentPath,
                c.IsActive,
                c.CreatedDate
            ));
        }

        public async Task<CertificateOutput?> GetCertificateById(
            int id,
            [Service] ICertificateRepository repository)
        {
            var certificate = await repository.GetByIdAsync(id);
            if (certificate == null) return null;

            return new CertificateOutput(
                certificate.Id,
                certificate.CertificateName,
                certificate.CertificateNumber,
                certificate.CompanyId,
                certificate.SiteId,
                certificate.CertificateType,
                certificate.Standard,
                certificate.IssueDate,
                certificate.ExpiryDate,
                certificate.IssuingBody,
                certificate.CertificationBody,
                certificate.Status,
                certificate.Scope,
                certificate.ExclusionsLimitations,
                certificate.DocumentPath,
                certificate.IsActive,
                certificate.CreatedDate
            );
        }

        // Certificate Service Queries
        public async Task<IEnumerable<CertificateServiceOutput>> GetCertificateServices(
            int certificateId,
            [Service] ICertificateServiceRepository repository)
        {
            var certificateServices = await repository.GetServicesByCertificateAsync(certificateId);
            
            return certificateServices.Select(cs => new CertificateServiceOutput(
                cs.Id,
                cs.CertificateId,
                cs.ServiceId,
                cs.Status,
                cs.ServiceScope,
                cs.CreatedDate
            ));
        }

        // Certificate Site Queries
        public async Task<IEnumerable<CertificateSiteOutput>> GetCertificateSites(
            int certificateId,
            [Service] ICertificateSiteRepository repository)
        {
            var certificateSites = await repository.GetSitesByCertificateAsync(certificateId);
            
            return certificateSites.Select(cs => new CertificateSiteOutput(
                cs.Id,
                cs.CertificateId,
                cs.SiteId,
                cs.Status,
                cs.SiteScope,
                cs.CreatedDate
            ));
        }

        // Certificate Additional Scope Queries
        public async Task<IEnumerable<CertificateAdditionalScopeOutput>> GetCertificateAdditionalScopes(
            int certificateId,
            [Service] ICertificateAdditionalScopeRepository repository)
        {
            var scopes = await repository.GetScopesByCertificateAsync(certificateId);
            
            return scopes.Select(s => new CertificateAdditionalScopeOutput(
                s.Id,
                s.CertificateId,
                s.ScopeDescription,
                s.ScopeType,
                s.Status,
                s.CreatedDate
            ));
        }
    }
}