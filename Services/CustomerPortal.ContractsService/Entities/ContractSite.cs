using System.ComponentModel.DataAnnotations;

namespace CustomerPortal.ContractsService.Entities;

public class ContractSite
{
    [Key]
    public int Id { get; set; }

    public int ContractId { get; set; }

    public int SiteId { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public virtual Contract? Contract { get; set; }
    public virtual Site? Site { get; set; }
}