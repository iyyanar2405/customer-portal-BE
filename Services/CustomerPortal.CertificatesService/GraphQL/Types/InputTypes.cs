using HotChocolate;

namespace CustomerPortal.CertificatesService.GraphQL.Types
{
    /// <summary>
    /// GraphQL input types for Certificate mutations
    /// </summary>

    public class CreateCertificateInput
    {
        public int CompanyId { get; set; }
        public int AuditId { get; set; }
        public int CertificateTypeId { get; set; }
        public string Scope { get; set; } = string.Empty;
        public DateTime IssueDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public DateTime? RenewalDate { get; set; }
        public IEnumerable<int> SiteIds { get; set; } = new List<int>();
        public IEnumerable<int> ServiceIds { get; set; } = new List<int>();
        public IEnumerable<CreateAdditionalScopeInput> AdditionalScopes { get; set; } = new List<CreateAdditionalScopeInput>();
    }

    public class UpdateCertificateInput
    {
        public string? Scope { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public DateTime? RenewalDate { get; set; }
        public string? Status { get; set; }
    }

    public class CreateCertificateTypeInput
    {
        public string TypeName { get; set; } = string.Empty;
        public string Standard { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Category { get; set; } = "MANAGEMENT_SYSTEMS";
        public int ValidityPeriodMonths { get; set; } = 36;
        public int RenewalNoticeDays { get; set; } = 90;
        public bool IsAccredited { get; set; } = true;
        public string? AccreditationBody { get; set; }
        public string? TemplateUrl { get; set; }
        public bool RequiresAnnualSurveillance { get; set; } = true;
    }

    public class UpdateCertificateTypeInput
    {
        public string? TypeName { get; set; }
        public string? Description { get; set; }
        public string? Category { get; set; }
        public int? ValidityPeriodMonths { get; set; }
        public int? RenewalNoticeDays { get; set; }
        public bool? IsAccredited { get; set; }
        public string? AccreditationBody { get; set; }
        public string? TemplateUrl { get; set; }
        public bool? RequiresAnnualSurveillance { get; set; }
    }

    public class AddSiteToCertificateInput
    {
        public int CertificateId { get; set; }
        public int SiteId { get; set; }
        public DateTime IncludeDate { get; set; } = DateTime.UtcNow;
        public string? ScopeDescription { get; set; }
    }

    public class AddServiceToCertificateInput
    {
        public int CertificateId { get; set; }
        public int ServiceId { get; set; }
        public DateTime IncludeDate { get; set; } = DateTime.UtcNow;
        public string? ScopeDescription { get; set; }
    }

    public class CreateAdditionalScopeInput
    {
        public string ScopeDescription { get; set; } = string.Empty;
        public bool IsIncluded { get; set; } = true;
        public string? Notes { get; set; }
    }

    public class UpdateAdditionalScopeInput
    {
        public string? ScopeDescription { get; set; }
        public bool? IsIncluded { get; set; }
        public string? Notes { get; set; }
    }

    public class StartRenewalInput
    {
        public DateTime PlannedAuditDate { get; set; }
        public DateTime TargetCompletionDate { get; set; }
        public int? AssignedAuditorId { get; set; }
        public string? Notes { get; set; }
    }

    public class UpdateRenewalInput
    {
        public DateTime? PlannedAuditDate { get; set; }
        public DateTime? TargetCompletionDate { get; set; }
        public DateTime? ActualCompletionDate { get; set; }
        public string? Status { get; set; }
        public int? AssignedAuditorId { get; set; }
        public string? Notes { get; set; }
    }

    public class CreateValidationInput
    {
        public int CertificateId { get; set; }
        public string VerificationCode { get; set; } = string.Empty;
        public string? DigitalSignature { get; set; }
        public string? BlockchainHash { get; set; }
        public string? VerificationUrl { get; set; }
        public string? QrCodeData { get; set; }
        public string VerificationMethod { get; set; } = "MANUAL";
        public string Result { get; set; } = "VALID";
        public int? ValidatedById { get; set; }
    }

    public class GenerateDocumentInput
    {
        public int CertificateId { get; set; }
        public string DocumentType { get; set; } = "CERTIFICATE";
        public string? Version { get; set; }
        public bool IncludeDigitalSignature { get; set; } = true;
        public bool IncludeQrCode { get; set; } = true;
    }

    public class UploadDocumentInput
    {
        public int CertificateId { get; set; }
        public string DocumentType { get; set; } = "CERTIFICATE";
        public string FileName { get; set; } = string.Empty;
        public string FileType { get; set; } = string.Empty;
        public long FileSize { get; set; }
        public string FilePath { get; set; } = string.Empty;
        public string? Version { get; set; }
        public int? UploadedById { get; set; }
    }

    public class CreateCompanyInput
    {
        public string CompanyName { get; set; } = string.Empty;
        public string CompanyCode { get; set; } = string.Empty;
        public string? Address { get; set; }
        public string? ContactPerson { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
    }

    public class UpdateCompanyInput
    {
        public string? CompanyName { get; set; }
        public string? Address { get; set; }
        public string? ContactPerson { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
    }

    public class CreateAuditInput
    {
        public string AuditNumber { get; set; } = string.Empty;
        public string AuditTitle { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime AuditDate { get; set; }
        public int? LeadAuditorId { get; set; }
        public string Status { get; set; } = "PLANNED";
    }

    public class UpdateAuditInput
    {
        public string? AuditTitle { get; set; }
        public string? Description { get; set; }
        public DateTime? AuditDate { get; set; }
        public int? LeadAuditorId { get; set; }
        public string? Status { get; set; }
    }

    public class CreateSiteInput
    {
        public string SiteName { get; set; } = string.Empty;
        public string SiteCode { get; set; } = string.Empty;
        public string? Address { get; set; }
        public int CompanyId { get; set; }
        public int? CityId { get; set; }
    }

    public class UpdateSiteInput
    {
        public string? SiteName { get; set; }
        public string? Address { get; set; }
        public int? CityId { get; set; }
    }

    public class CreateServiceInput
    {
        public string ServiceName { get; set; } = string.Empty;
        public string ServiceCode { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Category { get; set; } = "GENERAL";
    }

    public class UpdateServiceInput
    {
        public string? ServiceName { get; set; }
        public string? Description { get; set; }
        public string? Category { get; set; }
    }

    public class CreateUserInput
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string? Department { get; set; }
        public string? JobTitle { get; set; }
        public string Role { get; set; } = "USER";
    }

    public class UpdateUserInput
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Phone { get; set; }
        public string? Department { get; set; }
        public string? JobTitle { get; set; }
        public string? Role { get; set; }
    }

    // Enums for GraphQL
    public enum CertificateStatus
    {
        ACTIVE,
        SUSPENDED,
        WITHDRAWN,
        EXPIRED,
        CANCELLED,
        UNDER_REVIEW
    }

    public enum DocumentType
    {
        CERTIFICATE,
        SCOPE_DOCUMENT,
        ANNEXE,
        VERIFICATION_LETTER,
        QR_CODE
    }

    public enum RenewalStatus
    {
        INITIATED,
        IN_PROGRESS,
        COMPLETED,
        CANCELLED
    }

    public enum ValidationResult
    {
        VALID,
        INVALID,
        EXPIRED,
        SUSPENDED
    }

    public enum UserRole
    {
        USER,
        AUDITOR,
        MANAGER,
        ADMIN
    }

    public enum AuditStatus
    {
        PLANNED,
        IN_PROGRESS,
        COMPLETED,
        CANCELLED
    }
}