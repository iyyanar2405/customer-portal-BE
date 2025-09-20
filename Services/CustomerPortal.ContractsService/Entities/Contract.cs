using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CustomerPortal.ContractsService.Entities;

public class Contract
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string ContractNumber { get; set; } = string.Empty;

    public int CompanyId { get; set; }

    [Required]
    [StringLength(50)]
    public string ContractType { get; set; } = string.Empty;

    [Required]
    [StringLength(200)]
    public string Title { get; set; } = string.Empty;

    [StringLength(1000)]
    public string? Description { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public DateTime? RenewalDate { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal Value { get; set; }

    [StringLength(10)]
    public string Currency { get; set; } = "USD";

    [StringLength(30)]
    public string Status { get; set; } = "DRAFT";

    [StringLength(50)]
    public string PaymentTerms { get; set; } = "NET_30";

    public bool IsActive { get; set; } = true;

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    public DateTime ModifiedDate { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public virtual Company? Company { get; set; }
    public virtual ICollection<ContractService> Services { get; set; } = new List<ContractService>();
    public virtual ICollection<ContractSite> Sites { get; set; } = new List<ContractSite>();
    public virtual ICollection<ContractTerm> Terms { get; set; } = new List<ContractTerm>();
    public virtual ICollection<ContractAmendment> Amendments { get; set; } = new List<ContractAmendment>();
    public virtual ICollection<ContractRenewal> Renewals { get; set; } = new List<ContractRenewal>();
}