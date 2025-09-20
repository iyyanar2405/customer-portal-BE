using CustomerPortal.Shared.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CustomerPortal.MasterService.Entities
{
    [Table("Countries")]
    public class Country : BaseEntity
    {
        [Required]
        [StringLength(100)]
        public string CountryName { get; set; } = string.Empty;

        [StringLength(3)]
        public string? CountryCode { get; set; }

        [StringLength(10)]
        public string? CurrencyCode { get; set; }

        [StringLength(50)]
        public string? Region { get; set; }

        // Navigation Properties
        public virtual ICollection<City> Cities { get; set; } = new List<City>();
    }

    [Table("Cities")]
    public class City : BaseEntity
    {
        [Required]
        [StringLength(100)]
        public string CityName { get; set; } = string.Empty;

        [Required]
        public int CountryId { get; set; }

        [StringLength(50)]
        public string? StateProvince { get; set; }

        [StringLength(20)]
        public string? PostalCode { get; set; }

        // Navigation Properties
        [ForeignKey("CountryId")]
        public virtual Country Country { get; set; } = null!;
    }

    [Table("Companies")]
    public class Company : BaseEntity
    {
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

        // Navigation Properties
        [ForeignKey("CountryId")]
        public virtual Country? Country { get; set; }
        public virtual ICollection<Site> Sites { get; set; } = new List<Site>();
    }

    [Table("Sites")]
    public class Site : BaseEntity
    {
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

        // Navigation Properties
        [ForeignKey("CompanyId")]
        public virtual Company Company { get; set; } = null!;
        
        [ForeignKey("CityId")]
        public virtual City? City { get; set; }
    }

    [Table("Services")]
    public class Service : BaseEntity
    {
        [Required]
        [StringLength(255)]
        public string ServiceName { get; set; } = string.Empty;

        [StringLength(100)]
        public string? ServiceCode { get; set; }

        [StringLength(1000)]
        public string? Description { get; set; }

        [StringLength(50)]
        public string? ServiceCategory { get; set; }
    }

    [Table("Roles")]
    public class Role : BaseEntity
    {
        [Required]
        [StringLength(100)]
        public string RoleName { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? Description { get; set; }

        [StringLength(500)]
        public string? Permissions { get; set; }
    }

    [Table("Chapters")]
    public class Chapter : BaseEntity
    {
        [Required]
        [StringLength(255)]
        public string ChapterName { get; set; } = string.Empty;

        [StringLength(50)]
        public string? ChapterNumber { get; set; }

        [StringLength(1000)]
        public string? Description { get; set; }

        // Navigation Properties
        public virtual ICollection<Clause> Clauses { get; set; } = new List<Clause>();
    }

    [Table("Clauses")]
    public class Clause : BaseEntity
    {
        [Required]
        [StringLength(255)]
        public string ClauseName { get; set; } = string.Empty;

        [StringLength(50)]
        public string? ClauseNumber { get; set; }

        public int? ChapterId { get; set; }

        [StringLength(2000)]
        public string? Description { get; set; }

        // Navigation Properties
        [ForeignKey("ChapterId")]
        public virtual Chapter? Chapter { get; set; }
    }

    [Table("FocusAreas")]
    public class FocusArea : BaseEntity
    {
        [Required]
        [StringLength(255)]
        public string FocusAreaName { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? Description { get; set; }
    }
}