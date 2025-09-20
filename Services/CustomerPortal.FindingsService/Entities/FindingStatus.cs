using CustomerPortal.Shared.Entities;
using System.ComponentModel.DataAnnotations;

namespace CustomerPortal.FindingsService.Entities;

public class FindingStatus : BaseEntity
{
    [Required]
    [StringLength(50)]
    public string Name { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Description { get; set; }

    [Required]
    [StringLength(20)]
    public string Code { get; set; } = string.Empty;

    [StringLength(7)]
    public string? Color { get; set; } // Hex color code

    public int DisplayOrder { get; set; } = 0;

    public bool IsFinalStatus { get; set; } = false;

    // Navigation properties
    public virtual ICollection<Finding> Findings { get; set; } = new List<Finding>();
}