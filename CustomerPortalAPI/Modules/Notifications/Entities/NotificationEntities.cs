using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CustomerPortalAPI.Modules.Master.Entities;
using CustomerPortalAPI.Modules.Users.Entities;

namespace CustomerPortalAPI.Modules.Notifications.Entities
{
    [Table("Notifications")]
    public class Notification
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Message { get; set; } = string.Empty;

        [Required]
        public int CategoryId { get; set; }

        public int? CompanyId { get; set; }

        public int? SiteId { get; set; }

        public int? ServiceId { get; set; }

        [Required]
        [StringLength(50)]
        public string Priority { get; set; } = "Medium"; // 'Low', 'Medium', 'High', 'Critical'

        [Required]
        [StringLength(50)]
        public string Status { get; set; } = "Active"; // 'Active', 'Read', 'Archived', 'Dismissed'

        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public DateTime ModifiedDate { get; set; } = DateTime.UtcNow;

        public int? CreatedBy { get; set; }

        public int? ModifiedBy { get; set; }

        public DateTime? ExpiryDate { get; set; }

        public bool IsActive { get; set; } = true;

        public string? ReadBy { get; set; } // JSON array of user IDs who have read this notification

        [StringLength(100)]
        public string? TargetAudience { get; set; } // 'All', 'Company', 'Site', 'Role'

        public bool ActionRequired { get; set; } = false;

        [StringLength(500)]
        public string? ActionUrl { get; set; }

        [StringLength(500)]
        public string? AttachmentPath { get; set; }

        [StringLength(50)]
        public string? RelatedEntityType { get; set; } // 'Audit', 'Finding', 'Certificate', 'Invoice'

        public int? RelatedEntityId { get; set; }

        // Navigation Properties
        [ForeignKey("CategoryId")]
        public virtual NotificationCategory Category { get; set; } = null!;

        [ForeignKey("CompanyId")]
        public virtual Company? Company { get; set; }

        [ForeignKey("SiteId")]
        public virtual Site? Site { get; set; }

        [ForeignKey("ServiceId")]
        public virtual Service? Service { get; set; }

        [ForeignKey("CreatedBy")]
        public virtual User? CreatedByUser { get; set; }

        [ForeignKey("ModifiedBy")]
        public virtual User? ModifiedByUser { get; set; }
    }

    [Table("NotificationCategories")]
    public class NotificationCategory
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string CategoryName { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string CategoryCode { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }

        public bool IsActive { get; set; } = true;

        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public DateTime ModifiedDate { get; set; } = DateTime.UtcNow;

        public int? CreatedBy { get; set; }

        public int? ModifiedBy { get; set; }

        [StringLength(7)]
        public string? Color { get; set; } // Hex color code for UI display

        [StringLength(50)]
        public string? Icon { get; set; } // Icon name for UI display

        public int Priority { get; set; } = 5; // 1-10 priority level

        public int DisplayOrder { get; set; } = 999;

        // Navigation Properties
        public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

        [ForeignKey("CreatedBy")]
        public virtual User? CreatedByUser { get; set; }

        [ForeignKey("ModifiedBy")]
        public virtual User? ModifiedByUser { get; set; }
    }
}