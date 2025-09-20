using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CustomerPortalAPI.Modules.Master.Entities;

namespace CustomerPortalAPI.Modules.Certificates.Entities
{
    [Table("Certificates")]
    public class Certificate
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string CertificateName { get; set; } = string.Empty;

        [StringLength(100)]
        public string? CertificateNumber { get; set; }

        [Required]
        public int CompanyId { get; set; }

        [Required]
        public int SiteId { get; set; }

        [StringLength(100)]
        public string? CertificateType { get; set; }

        [StringLength(100)]
        public string? Standard { get; set; }

        public DateTime? IssueDate { get; set; }

        public DateTime? ExpiryDate { get; set; }

        [StringLength(255)]
        public string? IssuingBody { get; set; }

        [StringLength(100)]
        public string? CertificationBody { get; set; }

        [StringLength(50)]
        public string? Status { get; set; }

        [StringLength(2000)]
        public string? Scope { get; set; }

        [StringLength(2000)]
        public string? ExclusionsLimitations { get; set; }

        [StringLength(500)]
        public string? DocumentPath { get; set; }

        public bool IsActive { get; set; } = true;

        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public int? CreatedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public int? ModifiedBy { get; set; }

        // Navigation Properties
        [ForeignKey("CompanyId")]
        public virtual Company Company { get; set; } = null!;

        [ForeignKey("SiteId")]
        public virtual Site Site { get; set; } = null!;

        public virtual ICollection<CertificateService> CertificateServices { get; set; } = new List<CertificateService>();
        public virtual ICollection<CertificateSite> CertificateSites { get; set; } = new List<CertificateSite>();
        public virtual ICollection<CertificateAdditionalScope> CertificateAdditionalScopes { get; set; } = new List<CertificateAdditionalScope>();
    }

    [Table("CertificateServices")]
    public class CertificateService
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int CertificateId { get; set; }

        [Required]
        public int ServiceId { get; set; }

        [StringLength(50)]
        public string? Status { get; set; }

        [StringLength(1000)]
        public string? ServiceScope { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public int? CreatedBy { get; set; }

        // Navigation Properties
        [ForeignKey("CertificateId")]
        public virtual Certificate Certificate { get; set; } = null!;

        [ForeignKey("ServiceId")]
        public virtual Service Service { get; set; } = null!;
    }

    [Table("CertificateSites")]
    public class CertificateSite
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int CertificateId { get; set; }

        [Required]
        public int SiteId { get; set; }

        [StringLength(50)]
        public string? Status { get; set; }

        [StringLength(1000)]
        public string? SiteScope { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public int? CreatedBy { get; set; }

        // Navigation Properties
        [ForeignKey("CertificateId")]
        public virtual Certificate Certificate { get; set; } = null!;

        [ForeignKey("SiteId")]
        public virtual Site Site { get; set; } = null!;
    }

    [Table("CertificateAdditionalScopes")]
    public class CertificateAdditionalScope
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int CertificateId { get; set; }

        [Required]
        [StringLength(1000)]
        public string ScopeDescription { get; set; } = string.Empty;

        [StringLength(100)]
        public string? ScopeType { get; set; }

        [StringLength(50)]
        public string? Status { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public int? CreatedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public int? ModifiedBy { get; set; }

        // Navigation Properties
        [ForeignKey("CertificateId")]
        public virtual Certificate Certificate { get; set; } = null!;
    }
}