using System.ComponentModel.DataAnnotations;

namespace CustomerPortal.ContractsService.Entities;

public class Site
{
    [Key]
    public int Id { get; set; }

    public int CompanyId { get; set; }

    [Required]
    [StringLength(100)]
    public string SiteName { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    public string SiteCode { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Address { get; set; }

    [StringLength(100)]
    public string? City { get; set; }

    [StringLength(100)]
    public string? Country { get; set; }

    [StringLength(20)]
    public string? PostalCode { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public virtual Company? Company { get; set; }
    public virtual ICollection<ContractSite> ContractSites { get; set; } = new List<ContractSite>();
}