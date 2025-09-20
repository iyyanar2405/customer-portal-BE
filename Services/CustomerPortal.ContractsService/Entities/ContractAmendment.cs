using System.ComponentModel.DataAnnotations;

namespace CustomerPortal.ContractsService.Entities;

public class ContractAmendment
{
    [Key]
    public int Id { get; set; }

    public int ContractId { get; set; }

    [Required]
    [StringLength(50)]
    public string AmendmentNumber { get; set; } = string.Empty;

    [Required]
    [StringLength(1000)]
    public string Description { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string AmendmentType { get; set; } = string.Empty;

    public DateTime EffectiveDate { get; set; }

    public decimal? ValueChange { get; set; }

    [StringLength(30)]
    public string Status { get; set; } = "PENDING";

    [StringLength(100)]
    public string? ApprovedBy { get; set; }

    public DateTime? ApprovedDate { get; set; }

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public virtual Contract? Contract { get; set; }
}