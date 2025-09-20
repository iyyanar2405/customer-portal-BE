using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CustomerPortalAPI.Modules.Actions.Entities
{
    [Table("Actions")]
    public class Action
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string ActionName { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? Description { get; set; }

        [StringLength(50)]
        public string? ActionType { get; set; }

        [StringLength(50)]
        public string? Priority { get; set; }

        [StringLength(50)]
        public string? Status { get; set; }

        public int? AssignedToUserId { get; set; }

        public int? CompanyId { get; set; }

        public int? SiteId { get; set; }

        public int? AuditId { get; set; }

        public int? FindingId { get; set; }

        public DateTime? DueDate { get; set; }

        public DateTime? CompletedDate { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public int? CreatedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public int? ModifiedBy { get; set; }

        [StringLength(2000)]
        public string? Comments { get; set; }

        public bool IsActive { get; set; } = true;

        // Navigation Properties
        // These will be configured in the DbContext
        // public virtual User? AssignedToUser { get; set; }
        // public virtual Company? Company { get; set; }
        // public virtual Site? Site { get; set; }
        // public virtual Audit? Audit { get; set; }
        // public virtual Finding? Finding { get; set; }
    }
}