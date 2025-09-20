using System.ComponentModel.DataAnnotations;

namespace CustomerPortal.ContractsService.Entities;

public class Service
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string ServiceName { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    public string ServiceCode { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Description { get; set; }

    [StringLength(50)]
    public string Category { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public virtual ICollection<ContractService> ContractServices { get; set; } = new List<ContractService>();
}