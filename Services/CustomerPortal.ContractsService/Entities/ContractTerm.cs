using System.ComponentModel.DataAnnotations;

namespace CustomerPortal.ContractsService.Entities;

public class ContractTerm
{
    [Key]
    public int Id { get; set; }

    public int ContractId { get; set; }

    [Required]
    [StringLength(50)]
    public string TermType { get; set; } = string.Empty;

    [Required]
    [StringLength(1000)]
    public string Description { get; set; } = string.Empty;

    [StringLength(200)]
    public string? Value { get; set; }

    [StringLength(50)]
    public string? Unit { get; set; }

    public bool IsRequired { get; set; } = false;

    public DateTime? EffectiveDate { get; set; }

    public DateTime? ExpiryDate { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public virtual Contract? Contract { get; set; }
}