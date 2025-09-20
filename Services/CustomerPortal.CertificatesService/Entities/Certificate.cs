using CustomerPortal.Shared.Entities;
using System.ComponentModel.DataAnnotations;

namespace CustomerPortal.CertificatesService.Entities
{
    /// <summary>
    /// Main certificate record entity
    /// </summary>
    public class Certificate : BaseEntity
    {
        [Required]
        [StringLength(50)]
        public string CertificateNumber { get; set; } = string.Empty;

        [Required]
        public int CompanyId { get; set; }

        [Required]
        public int AuditId { get; set; }

        [Required]
        public int CertificateTypeId { get; set; }

        [Required]
        [StringLength(2000)]
        public string Scope { get; set; } = string.Empty;

        [Required]
        public DateTime IssueDate { get; set; }

        [Required]
        public DateTime ExpiryDate { get; set; }

        public DateTime? RenewalDate { get; set; }

        [Required]
        [StringLength(20)]
        public string Status { get; set; } = "ACTIVE"; // ACTIVE, SUSPENDED, WITHDRAWN, EXPIRED, CANCELLED, UNDER_REVIEW

        // Navigation properties
        public virtual Company? Company { get; set; }
        public virtual Audit? Audit { get; set; }
        public virtual CertificateType? CertificateType { get; set; }
        public virtual ICollection<CertificateSite> Sites { get; set; } = new List<CertificateSite>();
        public virtual ICollection<CertificateService> Services { get; set; } = new List<CertificateService>();
        public virtual ICollection<CertificateAdditionalScope> AdditionalScopes { get; set; } = new List<CertificateAdditionalScope>();
        public virtual ICollection<CertificateRenewal> Renewals { get; set; } = new List<CertificateRenewal>();
        public virtual ICollection<CertificateValidation> Validations { get; set; } = new List<CertificateValidation>();
        public virtual ICollection<CertificateDocument> Documents { get; set; } = new List<CertificateDocument>();
    }

    /// <summary>
    /// Certificate type definition entity
    /// </summary>
    public class CertificateType : BaseEntity
    {
        [Required]
        [StringLength(100)]
        public string TypeName { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Standard { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }

        [Required]
        [StringLength(50)]
        public string Category { get; set; } = "MANAGEMENT_SYSTEMS";

        [Required]
        public int ValidityPeriodMonths { get; set; } = 36;

        [Required]
        public int RenewalNoticeDays { get; set; } = 90;

        public bool IsAccredited { get; set; } = true;

        [StringLength(100)]
        public string? AccreditationBody { get; set; }

        [StringLength(500)]
        public string? TemplateUrl { get; set; }

        public bool RequiresAnnualSurveillance { get; set; } = true;

        // Navigation properties
        public virtual ICollection<Certificate> Certificates { get; set; } = new List<Certificate>();
    }

    /// <summary>
    /// Sites covered by a certificate
    /// </summary>
    public class CertificateSite : BaseEntity
    {
        [Required]
        public int CertificateId { get; set; }

        [Required]
        public int SiteId { get; set; }

        [Required]
        public DateTime IncludeDate { get; set; } = DateTime.UtcNow;

        public DateTime? ExcludeDate { get; set; }

        [StringLength(1000)]
        public string? ScopeDescription { get; set; }

        // Navigation properties
        public virtual Certificate? Certificate { get; set; }
        public virtual Site? Site { get; set; }
    }

    /// <summary>
    /// Services covered by a certificate
    /// </summary>
    public class CertificateService : BaseEntity
    {
        [Required]
        public int CertificateId { get; set; }

        [Required]
        public int ServiceId { get; set; }

        [Required]
        public DateTime IncludeDate { get; set; } = DateTime.UtcNow;

        public DateTime? ExcludeDate { get; set; }

        [StringLength(1000)]
        public string? ScopeDescription { get; set; }

        // Navigation properties
        public virtual Certificate? Certificate { get; set; }
        public virtual Service? Service { get; set; }
    }

    /// <summary>
    /// Additional scope information for certificates
    /// </summary>
    public class CertificateAdditionalScope : BaseEntity
    {
        [Required]
        public int CertificateId { get; set; }

        [Required]
        [StringLength(1000)]
        public string ScopeDescription { get; set; } = string.Empty;

        public bool IsIncluded { get; set; } = true;

        [StringLength(500)]
        public string? Notes { get; set; }

        // Navigation properties
        public virtual Certificate? Certificate { get; set; }
    }

    /// <summary>
    /// Certificate renewal tracking
    /// </summary>
    public class CertificateRenewal : BaseEntity
    {
        [Required]
        public int CertificateId { get; set; }

        [Required]
        [StringLength(50)]
        public string RenewalNumber { get; set; } = string.Empty;

        [Required]
        public DateTime PlannedAuditDate { get; set; }

        [Required]
        public DateTime TargetCompletionDate { get; set; }

        public DateTime? ActualCompletionDate { get; set; }

        [Required]
        [StringLength(20)]
        public string Status { get; set; } = "INITIATED"; // INITIATED, IN_PROGRESS, COMPLETED, CANCELLED

        [StringLength(1000)]
        public string? Notes { get; set; }

        public int? AssignedAuditorId { get; set; }

        // Navigation properties
        public virtual Certificate? Certificate { get; set; }
        public virtual User? AssignedAuditor { get; set; }
    }

    /// <summary>
    /// Certificate validation and verification records
    /// </summary>
    public class CertificateValidation : BaseEntity
    {
        [Required]
        public int CertificateId { get; set; }

        [Required]
        [StringLength(100)]
        public string VerificationCode { get; set; } = string.Empty;

        [StringLength(500)]
        public string? DigitalSignature { get; set; }

        [StringLength(500)]
        public string? BlockchainHash { get; set; }

        [StringLength(500)]
        public string? VerificationUrl { get; set; }

        [StringLength(1000)]
        public string? QrCodeData { get; set; }

        [Required]
        public DateTime ValidationDate { get; set; } = DateTime.UtcNow;

        public int? ValidatedById { get; set; }

        [Required]
        [StringLength(50)]
        public string VerificationMethod { get; set; } = "MANUAL";

        [Required]
        [StringLength(20)]
        public string Result { get; set; } = "VALID"; // VALID, INVALID, EXPIRED, SUSPENDED

        // Navigation properties
        public virtual Certificate? Certificate { get; set; }
        public virtual User? ValidatedBy { get; set; }
    }

    /// <summary>
    /// Certificate document management
    /// </summary>
    public class CertificateDocument : BaseEntity
    {
        [Required]
        public int CertificateId { get; set; }

        [Required]
        [StringLength(20)]
        public string DocumentType { get; set; } = "CERTIFICATE"; // CERTIFICATE, SCOPE_DOCUMENT, ANNEXE, VERIFICATION_LETTER, QR_CODE

        [Required]
        [StringLength(255)]
        public string FileName { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string FileType { get; set; } = string.Empty;

        public long FileSize { get; set; }

        [Required]
        [StringLength(500)]
        public string FilePath { get; set; } = string.Empty;

        [StringLength(500)]
        public string? DownloadUrl { get; set; }

        [Required]
        public DateTime UploadDate { get; set; } = DateTime.UtcNow;

        [StringLength(20)]
        public string Version { get; set; } = "1.0";

        public bool IsLatest { get; set; } = true;

        [StringLength(500)]
        public string? DigitalSignature { get; set; }

        [StringLength(1000)]
        public string? QrCodeData { get; set; }

        public int? UploadedById { get; set; }

        // Navigation properties
        public virtual Certificate? Certificate { get; set; }
        public virtual User? UploadedBy { get; set; }
    }

    /// <summary>
    /// Company entity for certificate management
    /// </summary>
    public class Company : BaseEntity
    {
        [Required]
        [StringLength(100)]
        public string CompanyName { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string CompanyCode { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Address { get; set; }

        [StringLength(100)]
        public string? ContactPerson { get; set; }

        [StringLength(100)]
        public string? Email { get; set; }

        [StringLength(20)]
        public string? Phone { get; set; }

        // Navigation properties
        public virtual ICollection<Certificate> Certificates { get; set; } = new List<Certificate>();
        public virtual ICollection<Site> Sites { get; set; } = new List<Site>();
    }

    /// <summary>
    /// Audit entity for certificate management
    /// </summary>
    public class Audit : BaseEntity
    {
        [Required]
        [StringLength(50)]
        public string AuditNumber { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string AuditTitle { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }

        [Required]
        public DateTime AuditDate { get; set; }

        public int? LeadAuditorId { get; set; }

        [Required]
        [StringLength(20)]
        public string Status { get; set; } = "PLANNED"; // PLANNED, IN_PROGRESS, COMPLETED, CANCELLED

        // Navigation properties
        public virtual User? LeadAuditor { get; set; }
        public virtual ICollection<Certificate> Certificates { get; set; } = new List<Certificate>();
    }

    /// <summary>
    /// Site entity for certificate management
    /// </summary>
    public class Site : BaseEntity
    {
        [Required]
        [StringLength(100)]
        public string SiteName { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string SiteCode { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Address { get; set; }

        [Required]
        public int CompanyId { get; set; }

        public int? CityId { get; set; }

        // Navigation properties
        public virtual Company? Company { get; set; }
        public virtual City? City { get; set; }
        public virtual ICollection<CertificateSite> CertificateSites { get; set; } = new List<CertificateSite>();
    }

    /// <summary>
    /// Service entity for certificate management
    /// </summary>
    public class Service : BaseEntity
    {
        [Required]
        [StringLength(100)]
        public string ServiceName { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string ServiceCode { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }

        [StringLength(50)]
        public string Category { get; set; } = "GENERAL";

        // Navigation properties
        public virtual ICollection<CertificateService> CertificateServices { get; set; } = new List<CertificateService>();
    }

    /// <summary>
    /// City entity for certificate management
    /// </summary>
    public class City : BaseEntity
    {
        [Required]
        [StringLength(100)]
        public string CityName { get; set; } = string.Empty;

        [StringLength(20)]
        public string? CityCode { get; set; }

        [Required]
        public int CountryId { get; set; }

        // Navigation properties
        public virtual Country? Country { get; set; }
        public virtual ICollection<Site> Sites { get; set; } = new List<Site>();
    }

    /// <summary>
    /// Country entity for certificate management
    /// </summary>
    public class Country : BaseEntity
    {
        [Required]
        [StringLength(100)]
        public string CountryName { get; set; } = string.Empty;

        [StringLength(10)]
        public string? CountryCode { get; set; }

        // Navigation properties
        public virtual ICollection<City> Cities { get; set; } = new List<City>();
    }

    /// <summary>
    /// User entity for certificate management
    /// </summary>
    public class User : BaseEntity
    {
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [StringLength(20)]
        public string? Phone { get; set; }

        [StringLength(100)]
        public string? Department { get; set; }

        [StringLength(100)]
        public string? JobTitle { get; set; }

        [StringLength(20)]
        public string Role { get; set; } = "USER"; // USER, AUDITOR, MANAGER, ADMIN

        // Navigation properties
        public virtual ICollection<Audit> AuditsAsLeadAuditor { get; set; } = new List<Audit>();
        public virtual ICollection<CertificateRenewal> RenewalsAsAuditor { get; set; } = new List<CertificateRenewal>();
        public virtual ICollection<CertificateValidation> Validations { get; set; } = new List<CertificateValidation>();
        public virtual ICollection<CertificateDocument> UploadedDocuments { get; set; } = new List<CertificateDocument>();
    }
}