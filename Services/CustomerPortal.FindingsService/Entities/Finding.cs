using CustomerPortal.Shared.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CustomerPortal.FindingsService.Entities;

public class Finding : BaseEntity
{
    [Required]
    [StringLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [StringLength(2000)]
    public string Description { get; set; } = string.Empty;

    [StringLength(50)]
    public string? ReferenceNumber { get; set; }

    public DateTime? IdentifiedDate { get; set; }

    public DateTime? RequiredCompletionDate { get; set; }

    public DateTime? ActualCompletionDate { get; set; }

    [StringLength(1000)]
    public string? RootCause { get; set; }

    [StringLength(2000)]
    public string? CorrectiveAction { get; set; }

    [StringLength(1000)]
    public string? PreventiveAction { get; set; }

    [StringLength(500)]
    public string? Evidence { get; set; }

    public int? AuditId { get; set; }

    public int? SiteId { get; set; }

    public int? CompanyId { get; set; }

    [StringLength(100)]
    public string? IdentifiedBy { get; set; }

    [StringLength(100)]
    public string? AssignedTo { get; set; }

    [StringLength(100)]
    public string? ReviewedBy { get; set; }

    public DateTime? ReviewedDate { get; set; }

    [Range(1, 5)]
    public int Severity { get; set; } = 1; // 1=Low, 2=Medium, 3=High, 4=Critical, 5=Emergency

    [Range(1, 5)]
    public int Priority { get; set; } = 1; // 1=Low, 2=Medium, 3=High, 4=Critical, 5=Emergency

    // Foreign Keys
    public int CategoryId { get; set; }
    public int StatusId { get; set; }

    // Navigation properties
    [ForeignKey("CategoryId")]
    public virtual FindingCategory Category { get; set; } = null!;

    [ForeignKey("StatusId")]
    public virtual FindingStatus Status { get; set; } = null!;
}