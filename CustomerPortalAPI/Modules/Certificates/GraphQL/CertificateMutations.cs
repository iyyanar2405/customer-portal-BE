using CustomerPortalAPI.Modules.Certificates.Entities;
using CustomerPortalAPI.Modules.Certificates.Repositories;
using CustomerPortalAPI.Modules.Certificates.GraphQL;
using HotChocolate;

namespace CustomerPortalAPI.Modules.Certificates.GraphQL
{
    public class CertificateMutations
    {
        // Certificate Mutations
        public async Task<CreateCertificatePayload> CreateCertificate(
            CreateCertificateInput input,
            [Service] ICertificateRepository repository)
        {
            try
            {
                var certificate = new Certificate
                {
                    CertificateName = input.CertificateName,
                    CertificateNumber = input.CertificateNumber,
                    CompanyId = input.CompanyId,
                    SiteId = input.SiteId,
                    CertificateType = input.CertificateType,
                    Standard = input.Standard,
                    IssueDate = input.IssueDate,
                    ExpiryDate = input.ExpiryDate,
                    IssuingBody = input.IssuingBody,
                    CertificationBody = input.CertificationBody,
                    Status = input.Status,
                    Scope = input.Scope,
                    ExclusionsLimitations = input.ExclusionsLimitations,
                    DocumentPath = input.DocumentPath,
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow
                };

                var created = await repository.AddAsync(certificate);
                
                return new CreateCertificatePayload(
                    new CertificateOutput(
                        created.Id,
                        created.CertificateName,
                        created.CertificateNumber,
                        created.CompanyId,
                        created.SiteId,
                        created.CertificateType,
                        created.Standard,
                        created.IssueDate,
                        created.ExpiryDate,
                        created.IssuingBody,
                        created.CertificationBody,
                        created.Status,
                        created.Scope,
                        created.ExclusionsLimitations,
                        created.DocumentPath,
                        created.IsActive,
                        created.CreatedDate
                    ),
                    null
                );
            }
            catch (Exception ex)
            {
                return new CreateCertificatePayload(null, ex.Message);
            }
        }

        public async Task<UpdateCertificatePayload> UpdateCertificate(
            UpdateCertificateInput input,
            [Service] ICertificateRepository repository)
        {
            try
            {
                var certificate = await repository.GetByIdAsync(input.Id);
                if (certificate == null)
                    return new UpdateCertificatePayload(null, "Certificate not found");

                if (input.CertificateName != null) certificate.CertificateName = input.CertificateName;
                if (input.CertificateNumber != null) certificate.CertificateNumber = input.CertificateNumber;
                if (input.CompanyId.HasValue) certificate.CompanyId = input.CompanyId.Value;
                if (input.SiteId.HasValue) certificate.SiteId = input.SiteId.Value;
                if (input.CertificateType != null) certificate.CertificateType = input.CertificateType;
                if (input.Standard != null) certificate.Standard = input.Standard;
                if (input.IssueDate.HasValue) certificate.IssueDate = input.IssueDate;
                if (input.ExpiryDate.HasValue) certificate.ExpiryDate = input.ExpiryDate;
                if (input.IssuingBody != null) certificate.IssuingBody = input.IssuingBody;
                if (input.CertificationBody != null) certificate.CertificationBody = input.CertificationBody;
                if (input.Status != null) certificate.Status = input.Status;
                if (input.Scope != null) certificate.Scope = input.Scope;
                if (input.ExclusionsLimitations != null) certificate.ExclusionsLimitations = input.ExclusionsLimitations;
                if (input.DocumentPath != null) certificate.DocumentPath = input.DocumentPath;
                if (input.IsActive.HasValue) certificate.IsActive = input.IsActive.Value;
                certificate.ModifiedDate = DateTime.UtcNow;

                await repository.UpdateAsync(certificate);
                
                return new UpdateCertificatePayload(
                    new CertificateOutput(
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
                    ),
                    null
                );
            }
            catch (Exception ex)
            {
                return new UpdateCertificatePayload(null, ex.Message);
            }
        }

        public async Task<DeletePayload> DeleteCertificate(
            int id,
            [Service] ICertificateRepository repository)
        {
            try
            {
                await repository.DeleteAsync(id);
                return new DeletePayload(true, null);
            }
            catch (Exception ex)
            {
                return new DeletePayload(false, ex.Message);
            }
        }

        // Certificate Service Mutations
        public async Task<DeletePayload> CreateCertificateService(
            CreateCertificateServiceInput input,
            [Service] ICertificateServiceRepository repository)
        {
            try
            {
                var certificateService = new CertificateService
                {
                    CertificateId = input.CertificateId,
                    ServiceId = input.ServiceId,
                    Status = input.Status,
                    ServiceScope = input.ServiceScope,
                    CreatedDate = DateTime.UtcNow
                };

                await repository.AddAsync(certificateService);
                return new DeletePayload(true, null);
            }
            catch (Exception ex)
            {
                return new DeletePayload(false, ex.Message);
            }
        }

        public async Task<DeletePayload> DeleteCertificateService(
            int id,
            [Service] ICertificateServiceRepository repository)
        {
            try
            {
                await repository.DeleteAsync(id);
                return new DeletePayload(true, null);
            }
            catch (Exception ex)
            {
                return new DeletePayload(false, ex.Message);
            }
        }

        // Certificate Site Mutations
        public async Task<DeletePayload> CreateCertificateSite(
            CreateCertificateSiteInput input,
            [Service] ICertificateSiteRepository repository)
        {
            try
            {
                var certificateSite = new CertificateSite
                {
                    CertificateId = input.CertificateId,
                    SiteId = input.SiteId,
                    Status = input.Status,
                    SiteScope = input.SiteScope,
                    CreatedDate = DateTime.UtcNow
                };

                await repository.AddAsync(certificateSite);
                return new DeletePayload(true, null);
            }
            catch (Exception ex)
            {
                return new DeletePayload(false, ex.Message);
            }
        }

        public async Task<DeletePayload> DeleteCertificateSite(
            int id,
            [Service] ICertificateSiteRepository repository)
        {
            try
            {
                await repository.DeleteAsync(id);
                return new DeletePayload(true, null);
            }
            catch (Exception ex)
            {
                return new DeletePayload(false, ex.Message);
            }
        }

        // Certificate Additional Scope Mutations
        public async Task<DeletePayload> CreateCertificateAdditionalScope(
            CreateCertificateAdditionalScopeInput input,
            [Service] ICertificateAdditionalScopeRepository repository)
        {
            try
            {
                var scope = new CertificateAdditionalScope
                {
                    CertificateId = input.CertificateId,
                    ScopeDescription = input.ScopeDescription,
                    ScopeType = input.ScopeType,
                    Status = input.Status,
                    CreatedDate = DateTime.UtcNow
                };

                await repository.AddAsync(scope);
                return new DeletePayload(true, null);
            }
            catch (Exception ex)
            {
                return new DeletePayload(false, ex.Message);
            }
        }

        public async Task<DeletePayload> DeleteCertificateAdditionalScope(
            int id,
            [Service] ICertificateAdditionalScopeRepository repository)
        {
            try
            {
                await repository.DeleteAsync(id);
                return new DeletePayload(true, null);
            }
            catch (Exception ex)
            {
                return new DeletePayload(false, ex.Message);
            }
        }
    }
}