using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CustomerPortalAPI.Modules.Master.Entities
{
    [Table("Countries")]
    public class Country
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string CountryName { get; set; } = string.Empty;

        [StringLength(3)]
        public string? CountryCode { get; set; }

        [StringLength(10)]
        public string? CurrencyCode { get; set; }

        [StringLength(50)]
        public string? Region { get; set; }

        public bool IsActive { get; set; } = true;

        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public int? CreatedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public int? ModifiedBy { get; set; }

        // Navigation Properties
        public virtual ICollection<City> Cities { get; set; } = new List<City>();
        // public virtual ICollection<Company> Companies { get; set; } = new List<Company>();
    }

    [Table("Cities")]
    public class City
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string CityName { get; set; } = string.Empty;

        [Required]
        public int CountryId { get; set; }

        [StringLength(50)]
        public string? StateProvince { get; set; }

        [StringLength(20)]
        public string? PostalCode { get; set; }

        public bool IsActive { get; set; } = true;

        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public int? CreatedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public int? ModifiedBy { get; set; }

        // Navigation Properties
        [ForeignKey("CountryId")]
        public virtual Country Country { get; set; } = null!;
        // public virtual ICollection<Site> Sites { get; set; } = new List<Site>();
    }

    [Table("Companies")]
    public class Company
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string CompanyName { get; set; } = string.Empty;

        [StringLength(100)]
        public string? CompanyCode { get; set; }

        [StringLength(500)]
        public string? Address { get; set; }

        public int? CountryId { get; set; }

        [StringLength(50)]
        public string? Phone { get; set; }

        [StringLength(100)]
        public string? Email { get; set; }

        [StringLength(200)]
        public string? Website { get; set; }

        [StringLength(100)]
        public string? ContactPerson { get; set; }

        [StringLength(50)]
        public string? CompanyType { get; set; }

        public bool IsActive { get; set; } = true;

        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public int? CreatedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public int? ModifiedBy { get; set; }

        // Navigation Properties
        [ForeignKey("CountryId")]
        public virtual Country? Country { get; set; }
        public virtual ICollection<Site> Sites { get; set; } = new List<Site>();
        // public virtual ICollection<Contract> Contracts { get; set; } = new List<Contract>();
    }

    [Table("Sites")]
    public class Site
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string SiteName { get; set; } = string.Empty;

        [StringLength(100)]
        public string? SiteCode { get; set; }

        [Required]
        public int CompanyId { get; set; }

        public int? CityId { get; set; }

        [StringLength(500)]
        public string? Address { get; set; }

        [StringLength(50)]
        public string? Phone { get; set; }

        [StringLength(100)]
        public string? Email { get; set; }

        [StringLength(100)]
        public string? ContactPerson { get; set; }

        [StringLength(50)]
        public string? SiteType { get; set; }

        public bool IsActive { get; set; } = true;

        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public int? CreatedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public int? ModifiedBy { get; set; }

        // Navigation Properties
        [ForeignKey("CompanyId")]
        public virtual Company Company { get; set; } = null!;
        
        [ForeignKey("CityId")]
        public virtual City? City { get; set; }
        
        // public virtual ICollection<Audit> Audits { get; set; } = new List<Audit>();
        // public virtual ICollection<Certificate> Certificates { get; set; } = new List<Certificate>();
    }

    [Table("Services")]
    public class Service
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string ServiceName { get; set; } = string.Empty;

        [StringLength(100)]
        public string? ServiceCode { get; set; }

        [StringLength(1000)]
        public string? Description { get; set; }

        [StringLength(50)]
        public string? ServiceCategory { get; set; }

        public bool IsActive { get; set; } = true;

        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public int? CreatedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public int? ModifiedBy { get; set; }

        // Navigation Properties
        // public virtual ICollection<AuditService> AuditServices { get; set; } = new List<AuditService>();
        // public virtual ICollection<CertificateService> CertificateServices { get; set; } = new List<CertificateService>();
        // public virtual ICollection<ContractService> ContractServices { get; set; } = new List<ContractService>();
    }

    [Table("Roles")]
    public class Role
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string RoleName { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? Description { get; set; }

        [StringLength(500)]
        public string? Permissions { get; set; }

        public bool IsActive { get; set; } = true;

        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public int? CreatedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public int? ModifiedBy { get; set; }

        // Navigation Properties
        // public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }

    [Table("Chapters")]
    public class Chapter
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string ChapterName { get; set; } = string.Empty;

        [StringLength(50)]
        public string? ChapterNumber { get; set; }

        [StringLength(1000)]
        public string? Description { get; set; }

        public bool IsActive { get; set; } = true;

        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public int? CreatedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public int? ModifiedBy { get; set; }

        // Navigation Properties
        public virtual ICollection<Clause> Clauses { get; set; } = new List<Clause>();
    }

    [Table("Clauses")]
    public class Clause
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string ClauseName { get; set; } = string.Empty;

        [StringLength(50)]
        public string? ClauseNumber { get; set; }

        public int? ChapterId { get; set; }

        [StringLength(2000)]
        public string? Description { get; set; }

        public bool IsActive { get; set; } = true;

        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public int? CreatedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public int? ModifiedBy { get; set; }

        // Navigation Properties
        [ForeignKey("ChapterId")]
        public virtual Chapter? Chapter { get; set; }
        
        // public virtual ICollection<FindingClause> FindingClauses { get; set; } = new List<FindingClause>();
    }

    [Table("FocusAreas")]
    public class FocusArea
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string FocusAreaName { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? Description { get; set; }

        public bool IsActive { get; set; } = true;

        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public int? CreatedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public int? ModifiedBy { get; set; }

        // Navigation Properties
        // public virtual ICollection<FindingFocusArea> FindingFocusAreas { get; set; } = new List<FindingFocusArea>();
    }
}