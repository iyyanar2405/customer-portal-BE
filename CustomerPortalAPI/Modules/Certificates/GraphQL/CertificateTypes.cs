using HotChocolate;

namespace CustomerPortalAPI.Modules.Certificates.GraphQL
{
    // Input Types
    public record CreateCertificateInput(
        string CertificateName,
        string? CertificateNumber,
        int CompanyId,
        int SiteId,
        string? CertificateType,
        string? Standard,
        DateTime? IssueDate,
        DateTime? ExpiryDate,
        string? IssuingBody,
        string? CertificationBody,
        string? Status,
        string? Scope,
        string? ExclusionsLimitations,
        string? DocumentPath);

    public record UpdateCertificateInput(
        int Id,
        string? CertificateName,
        string? CertificateNumber,
        int? CompanyId,
        int? SiteId,
        string? CertificateType,
        string? Standard,
        DateTime? IssueDate,
        DateTime? ExpiryDate,
        string? IssuingBody,
        string? CertificationBody,
        string? Status,
        string? Scope,
        string? ExclusionsLimitations,
        string? DocumentPath,
        bool? IsActive);

    public record CreateCertificateServiceInput(
        int CertificateId,
        int ServiceId,
        string? Status,
        string? ServiceScope);

    public record CreateCertificateSiteInput(
        int CertificateId,
        int SiteId,
        string? Status,
        string? SiteScope);

    public record CreateCertificateAdditionalScopeInput(
        int CertificateId,
        string ScopeDescription,
        string? ScopeType,
        string? Status);

    // Output Types
    public record CertificateOutput(
        int Id,
        string CertificateName,
        string? CertificateNumber,
        int CompanyId,
        int SiteId,
        string? CertificateType,
        string? Standard,
        DateTime? IssueDate,
        DateTime? ExpiryDate,
        string? IssuingBody,
        string? CertificationBody,
        string? Status,
        string? Scope,
        string? ExclusionsLimitations,
        string? DocumentPath,
        bool IsActive,
        DateTime CreatedDate);

    public record CertificateServiceOutput(
        int Id,
        int CertificateId,
        int ServiceId,
        string? Status,
        string? ServiceScope,
        DateTime CreatedDate);

    public record CertificateSiteOutput(
        int Id,
        int CertificateId,
        int SiteId,
        string? Status,
        string? SiteScope,
        DateTime CreatedDate);

    public record CertificateAdditionalScopeOutput(
        int Id,
        int CertificateId,
        string ScopeDescription,
        string? ScopeType,
        string? Status,
        DateTime CreatedDate);

    // Payload Types
    public record CreateCertificatePayload(CertificateOutput? Certificate, string? Error);
    public record UpdateCertificatePayload(CertificateOutput? Certificate, string? Error);
    // Filter Input
    public record CertificateFilterInput(
        string? CertificateName,
        string? CertificateNumber,
        int? CompanyId,
        int? SiteId,
        string? CertificateType,
        string? Standard,
        string? Status,
        bool? IsActive,
        bool? IsExpiring);
}
