using CustomerPortal.Shared.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CustomerPortal.AuditsService.Entities
{
    public class AuditTeamMember : BaseEntity
    {
        [Required]
        public int AuditId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        [StringLength(30)]
        public string Role { get; set; } = "AUDITOR";

        public DateTime AssignedDate { get; set; } = DateTime.UtcNow;

        [StringLength(1000)]
        public string? Notes { get; set; }

        // Navigation properties
        [ForeignKey(nameof(AuditId))]
        public virtual Audit? Audit { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual User? User { get; set; }
    }
}