using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CustomerPortalAPI.Modules.Master.Entities;

namespace CustomerPortalAPI.Modules.Contracts.Entities
{
    [Table("Contracts")]
    public class Contract
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string ContractName { get; set; } = string.Empty;

        [StringLength(100)]
        public string? ContractNumber { get; set; }

        [Required]
        public int CompanyId { get; set; }

        [StringLength(100)]
        public string? ContractType { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? ContractValue { get; set; }

        [StringLength(10)]
        public string? Currency { get; set; }

        [StringLength(50)]
        public string? Status { get; set; }

        [StringLength(2000)]
        public string? Description { get; set; }

        [StringLength(2000)]
        public string? Terms { get; set; }

        [StringLength(255)]
        public string? ContractManager { get; set; }

        [StringLength(255)]
        public string? ClientContact { get; set; }

        public bool IsActive { get; set; } = true;

        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public int? CreatedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public int? ModifiedBy { get; set; }

        // Navigation Properties
        [ForeignKey("CompanyId")]
        public virtual Company Company { get; set; } = null!;

        public virtual ICollection<ContractService> ContractServices { get; set; } = new List<ContractService>();
        public virtual ICollection<ContractSite> ContractSites { get; set; } = new List<ContractSite>();
    }

    [Table("ContractServices")]
    public class ContractService
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ContractId { get; set; }

        [Required]
        public int ServiceId { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? ServicePrice { get; set; }

        [StringLength(10)]
        public string? Currency { get; set; }

        [StringLength(1000)]
        public string? ServiceDescription { get; set; }

        [StringLength(50)]
        public string? Status { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public int? CreatedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public int? ModifiedBy { get; set; }

        // Navigation Properties
        [ForeignKey("ContractId")]
        public virtual Contract Contract { get; set; } = null!;

        [ForeignKey("ServiceId")]
        public virtual Service Service { get; set; } = null!;
    }

    [Table("ContractSites")]
    public class ContractSite
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ContractId { get; set; }

        [Required]
        public int SiteId { get; set; }

        [StringLength(50)]
        public string? Status { get; set; }

        [StringLength(1000)]
        public string? SiteSpecificTerms { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public int? CreatedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public int? ModifiedBy { get; set; }

        // Navigation Properties
        [ForeignKey("ContractId")]
        public virtual Contract Contract { get; set; } = null!;

        [ForeignKey("SiteId")]
        public virtual Site Site { get; set; } = null!;
    }
}