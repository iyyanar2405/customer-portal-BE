using HotChocolate;

namespace CustomerPortal.CertificatesService.GraphQL.Types
{
    /// <summary>
    /// GraphQL object types for Certificate entities
    /// </summary>
    
    public class CertificateGraphQLType
    {
        public int Id { get; set; }
        public string CertificateNumber { get; set; } = string.Empty;
        public int CompanyId { get; set; }
        public int AuditId { get; set; }
        public int CertificateTypeId { get; set; }
        public string Scope { get; set; } = string.Empty;
        public DateTime IssueDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public DateTime? RenewalDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

        // Navigation properties
        public CompanyGraphQLType? Company { get; set; }
        public AuditGraphQLType? Audit { get; set; }
        public CertificateTypeGraphQLType? CertificateType { get; set; }
        public IEnumerable<CertificateSiteGraphQLType> Sites { get; set; } = new List<CertificateSiteGraphQLType>();
        public IEnumerable<CertificateServiceGraphQLType> Services { get; set; } = new List<CertificateServiceGraphQLType>();
        public IEnumerable<CertificateAdditionalScopeGraphQLType> AdditionalScopes { get; set; } = new List<CertificateAdditionalScopeGraphQLType>();
    }

    public class CertificateTypeGraphQLType
    {
        public int Id { get; set; }
        public string TypeName { get; set; } = string.Empty;
        public string Standard { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Category { get; set; } = string.Empty;
        public int ValidityPeriodMonths { get; set; }
        public int RenewalNoticeDays { get; set; }
        public bool IsAccredited { get; set; }
        public string? AccreditationBody { get; set; }
        public string? TemplateUrl { get; set; }
        public bool RequiresAnnualSurveillance { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class CompanyGraphQLType
    {
        public int Id { get; set; }
        public string CompanyName { get; set; } = string.Empty;
        public string CompanyCode { get; set; } = string.Empty;
        public string? Address { get; set; }
        public string? ContactPerson { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public bool IsActive { get; set; }
    }

    public class AuditGraphQLType
    {
        public int Id { get; set; }
        public string AuditNumber { get; set; } = string.Empty;
        public string AuditTitle { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime AuditDate { get; set; }
        public int? LeadAuditorId { get; set; }
        public string Status { get; set; } = string.Empty;
        
        public UserGraphQLType? LeadAuditor { get; set; }
    }

    public class UserGraphQLType
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string? Department { get; set; }
        public string? JobTitle { get; set; }
        public string Role { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }

    public class SiteGraphQLType
    {
        public int Id { get; set; }
        public string SiteName { get; set; } = string.Empty;
        public string SiteCode { get; set; } = string.Empty;
        public string? Address { get; set; }
        public int CompanyId { get; set; }
        public int? CityId { get; set; }
        public bool IsActive { get; set; }

        public CityGraphQLType? City { get; set; }
    }

    public class ServiceGraphQLType
    {
        public int Id { get; set; }
        public string ServiceName { get; set; } = string.Empty;
        public string ServiceCode { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Category { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }

    public class CityGraphQLType
    {
        public int Id { get; set; }
        public string CityName { get; set; } = string.Empty;
        public string? CityCode { get; set; }
        public int CountryId { get; set; }
        public bool IsActive { get; set; }

        public CountryGraphQLType? Country { get; set; }
    }

    public class CountryGraphQLType
    {
        public int Id { get; set; }
        public string CountryName { get; set; } = string.Empty;
        public string? CountryCode { get; set; }
        public bool IsActive { get; set; }
    }

    public class CertificateSiteGraphQLType
    {
        public int Id { get; set; }
        public int CertificateId { get; set; }
        public int SiteId { get; set; }
        public DateTime IncludeDate { get; set; }
        public DateTime? ExcludeDate { get; set; }
        public string? ScopeDescription { get; set; }
        public bool IsActive { get; set; }

        public SiteGraphQLType? Site { get; set; }
    }

    public class CertificateServiceGraphQLType
    {
        public int Id { get; set; }
        public int CertificateId { get; set; }
        public int ServiceId { get; set; }
        public DateTime IncludeDate { get; set; }
        public DateTime? ExcludeDate { get; set; }
        public string? ScopeDescription { get; set; }
        public bool IsActive { get; set; }

        public ServiceGraphQLType? Service { get; set; }
    }

    public class CertificateAdditionalScopeGraphQLType
    {
        public int Id { get; set; }
        public int CertificateId { get; set; }
        public string ScopeDescription { get; set; } = string.Empty;
        public bool IsIncluded { get; set; }
        public string? Notes { get; set; }
        public bool IsActive { get; set; }
    }

    public class CertificateRenewalGraphQLType
    {
        public int Id { get; set; }
        public int CertificateId { get; set; }
        public string RenewalNumber { get; set; } = string.Empty;
        public DateTime PlannedAuditDate { get; set; }
        public DateTime TargetCompletionDate { get; set; }
        public DateTime? ActualCompletionDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? Notes { get; set; }
        public int? AssignedAuditorId { get; set; }
        public bool IsActive { get; set; }

        public UserGraphQLType? AssignedAuditor { get; set; }
    }

    public class CertificateValidationGraphQLType
    {
        public int Id { get; set; }
        public int CertificateId { get; set; }
        public string VerificationCode { get; set; } = string.Empty;
        public string? DigitalSignature { get; set; }
        public string? BlockchainHash { get; set; }
        public string? VerificationUrl { get; set; }
        public string? QrCodeData { get; set; }
        public DateTime ValidationDate { get; set; }
        public int? ValidatedById { get; set; }
        public string VerificationMethod { get; set; } = string.Empty;
        public string Result { get; set; } = string.Empty;
        public bool IsActive { get; set; }

        public UserGraphQLType? ValidatedBy { get; set; }
    }

    public class CertificateDocumentGraphQLType
    {
        public int Id { get; set; }
        public int CertificateId { get; set; }
        public string DocumentType { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
        public string FileType { get; set; } = string.Empty;
        public long FileSize { get; set; }
        public string FilePath { get; set; } = string.Empty;
        public string? DownloadUrl { get; set; }
        public DateTime UploadDate { get; set; }
        public string Version { get; set; } = string.Empty;
        public bool IsLatest { get; set; }
        public string? DigitalSignature { get; set; }
        public string? QrCodeData { get; set; }
        public int? UploadedById { get; set; }
        public bool IsActive { get; set; }

        public UserGraphQLType? UploadedBy { get; set; }
    }

    // Analytics and Dashboard Types
    public class CertificateDashboardGraphQLType
    {
        public int TotalCertificates { get; set; }
        public int ActiveCertificates { get; set; }
        public int ExpiredCertificates { get; set; }
        public int SuspendedCertificates { get; set; }
        public int ExpiringWithin30Days { get; set; }
        public int ExpiringWithin90Days { get; set; }
        public decimal RenewalSuccessRate { get; set; }
        public double AverageRenewalTime { get; set; }
        public IEnumerable<CertificateTypeStatsGraphQLType> CertificatesByType { get; set; } = new List<CertificateTypeStatsGraphQLType>();
        public IEnumerable<StatusStatsGraphQLType> CertificatesByStatus { get; set; } = new List<StatusStatsGraphQLType>();
        public IEnumerable<MonthlyIssuanceGraphQLType> MonthlyIssuance { get; set; } = new List<MonthlyIssuanceGraphQLType>();
    }

    public class CertificateTypeStatsGraphQLType
    {
        public string TypeName { get; set; } = string.Empty;
        public int Count { get; set; }
        public int ActiveCount { get; set; }
    }

    public class StatusStatsGraphQLType
    {
        public string Status { get; set; } = string.Empty;
        public int Count { get; set; }
        public decimal Percentage { get; set; }
    }

    public class MonthlyIssuanceGraphQLType
    {
        public string Month { get; set; } = string.Empty;
        public int Issued { get; set; }
        public int Renewed { get; set; }
        public int Expired { get; set; }
    }

    public class RenewalScheduleGraphQLType
    {
        public int CertificateId { get; set; }
        public string CertificateNumber { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        public string CertificateType { get; set; } = string.Empty;
        public DateTime CurrentExpiryDate { get; set; }
        public DateTime? RenewalDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public int DaysUntilRenewal { get; set; }
        public string Priority { get; set; } = string.Empty;
    }

    public class ExpiringCertificateGraphQLType
    {
        public int Id { get; set; }
        public string CertificateNumber { get; set; } = string.Empty;
        public CompanyGraphQLType? Company { get; set; }
        public CertificateTypeGraphQLType? CertificateType { get; set; }
        public DateTime ExpiryDate { get; set; }
        public int DaysUntilExpiry { get; set; }
        public bool RenewalRequired { get; set; }
        public bool HasActiveRenewalProcess { get; set; }
    }

    public class CertificateValidationResultGraphQLType
    {
        public bool IsValid { get; set; }
        public string CertificateNumber { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        public string CertificateType { get; set; } = string.Empty;
        public string Scope { get; set; } = string.Empty;
        public DateTime IssueDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public IEnumerable<SiteGraphQLType> Sites { get; set; } = new List<SiteGraphQLType>();
        public DateTime ValidationDate { get; set; }
        public string? QrCodeUrl { get; set; }
    }

    public class ComplianceStatusGraphQLType
    {
        public int CompanyId { get; set; }
        public int? SiteId { get; set; }
        public decimal OverallComplianceScore { get; set; }
        public int ActiveCertifications { get; set; }
        public int RequiredCertifications { get; set; }
        public int ComplianceGaps { get; set; }
        public IEnumerable<StandardComplianceGraphQLType> CertificationsByStandard { get; set; } = new List<StandardComplianceGraphQLType>();
        public string RiskLevel { get; set; } = string.Empty;
        public IEnumerable<string> Recommendations { get; set; } = new List<string>();
    }

    public class StandardComplianceGraphQLType
    {
        public string Standard { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime? ExpiryDate { get; set; }
        public decimal ComplianceLevel { get; set; }
    }
}