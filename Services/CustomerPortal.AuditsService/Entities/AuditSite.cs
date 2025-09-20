using CustomerPortal.Shared.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CustomerPortal.AuditsService.Entities
{
    public class AuditSite : BaseEntity
    {
        [Required]
        public int AuditId { get; set; }

        [Required]
        public int SiteId { get; set; }

        public DateTime? ScheduledDate { get; set; }

        [StringLength(20)]
        public string Status { get; set; } = "SCHEDULED";

        [StringLength(1000)]
        public string? Notes { get; set; }

        // Navigation properties
        [ForeignKey(nameof(AuditId))]
        public virtual Audit? Audit { get; set; }

        [ForeignKey(nameof(SiteId))]
        public virtual Site? Site { get; set; }
    }
}