using CustomerPortal.Shared.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CustomerPortal.AuditsService.Entities
{
    public class AuditLog : BaseEntity
    {
        [Required]
        public int AuditId { get; set; }

        [Required]
        [StringLength(100)]
        public string Action { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? Description { get; set; }

        public int? UserId { get; set; }

        public DateTime ActionDate { get; set; } = DateTime.UtcNow;

        [StringLength(50)]
        public string? IpAddress { get; set; }

        // Navigation properties
        [ForeignKey(nameof(AuditId))]
        public virtual Audit? Audit { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual User? User { get; set; }
    }
}