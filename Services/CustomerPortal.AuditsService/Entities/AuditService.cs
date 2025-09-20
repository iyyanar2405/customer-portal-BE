using CustomerPortal.Shared.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CustomerPortal.AuditsService.Entities
{
    public class AuditService : BaseEntity
    {
        [Required]
        public int AuditId { get; set; }

        [Required]
        public int ServiceId { get; set; }

        [StringLength(20)]
        public string Status { get; set; } = "ACTIVE";

        [StringLength(1000)]
        public string? Notes { get; set; }

        // Navigation properties
        [ForeignKey(nameof(AuditId))]
        public virtual Audit? Audit { get; set; }

        [ForeignKey(nameof(ServiceId))]
        public virtual Service? Service { get; set; }
    }
}