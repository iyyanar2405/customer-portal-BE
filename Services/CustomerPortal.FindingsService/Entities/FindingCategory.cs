using CustomerPortal.Shared.Entities;
using System.ComponentModel.DataAnnotations;

namespace CustomerPortal.FindingsService.Entities;

public class FindingCategory : BaseEntity
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Description { get; set; }

    [Required]
    [StringLength(20)]
    public string Code { get; set; } = string.Empty;

    // Navigation properties
    public virtual ICollection<Finding> Findings { get; set; } = new List<Finding>();
}