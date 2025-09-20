using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CustomerPortal.ContractsService.Entities;

public class ContractRenewal
{
    [Key]
    public int Id { get; set; }

    public int ContractId { get; set; }

    [Required]
    [StringLength(50)]
    public string RenewalNumber { get; set; } = string.Empty;

    public DateTime ProposedStartDate { get; set; }

    public DateTime ProposedEndDate { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal ProposedValue { get; set; }

    [StringLength(30)]
    public string Status { get; set; } = "INITIATED";

    public bool AutoRenewal { get; set; } = false;

    public DateTime? NotificationSentDate { get; set; }

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    public DateTime? CompletedDate { get; set; }

    [StringLength(100)]
    public string? ProcessedBy { get; set; }

    // Navigation properties
    public virtual Contract? Contract { get; set; }
}