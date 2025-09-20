using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CustomerPortal.ContractsService.Entities;

public class ContractService
{
    [Key]
    public int Id { get; set; }

    public int ContractId { get; set; }

    public int ServiceId { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal UnitPrice { get; set; }

    public int Quantity { get; set; } = 1;

    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalPrice { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public virtual Contract? Contract { get; set; }
    public virtual Service? Service { get; set; }
}