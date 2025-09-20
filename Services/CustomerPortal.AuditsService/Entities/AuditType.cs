using CustomerPortal.Shared.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CustomerPortal.AuditsService.Entities
{
    public class AuditType : BaseEntity
    {
        [Required]
        [StringLength(100)]
        public string AuditTypeName { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }

        public int EstimatedDurationDays { get; set; } = 1;

        // Navigation properties
        public virtual ICollection<Audit> Audits { get; set; } = new List<Audit>();
    }
}