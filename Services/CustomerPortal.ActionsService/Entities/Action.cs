using CustomerPortal.Shared.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CustomerPortal.ActionsService.Entities
{
    public class Action : BaseEntity
    {
        [Required]
        [StringLength(50)]
        public string ActionNumber { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [StringLength(2000)]
        public string? Description { get; set; }

        [Required]
        public int ActionTypeId { get; set; }

        public int? AssignedToId { get; set; }

        public int? CreatedById { get; set; }

        [Required]
        [StringLength(20)]
        public string Priority { get; set; } = "MEDIUM";

        [Required]
        [StringLength(20)]
        public string Status { get; set; } = "NOT_STARTED";

        public DateTime? StartDate { get; set; }

        public DateTime? DueDate { get; set; }

        public DateTime? CompletedDate { get; set; }

        public int? EstimatedHours { get; set; }

        public decimal? ActualHours { get; set; }

        [Range(0, 100)]
        public int Progress { get; set; } = 0;

        public int? RelatedFindingId { get; set; }

        [StringLength(1000)]
        public string? Comments { get; set; }

        public bool IsOverdue => DueDate.HasValue && DueDate < DateTime.UtcNow && Status != "COMPLETED" && Status != "CANCELLED";

        // Navigation properties
        public virtual ActionType? ActionType { get; set; }
        public virtual User? AssignedTo { get; set; }
        public virtual User? CreatedBy { get; set; }
        public virtual ICollection<ActionDependency> Dependencies { get; set; } = new List<ActionDependency>();
        public virtual ICollection<ActionDependency> DependentActions { get; set; } = new List<ActionDependency>();
        public virtual ICollection<ActionAttachment> Attachments { get; set; } = new List<ActionAttachment>();
        public virtual ICollection<ActionComment> ActionComments { get; set; } = new List<ActionComment>();
        public virtual ICollection<ActionHistory> History { get; set; } = new List<ActionHistory>();
    }

    public class ActionType : BaseEntity
    {
        [Required]
        [StringLength(100)]
        public string TypeName { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }

        [Required]
        [StringLength(50)]
        public string Category { get; set; } = "GENERAL";

        [Required]
        [StringLength(20)]
        public string DefaultPriority { get; set; } = "MEDIUM";

        public int? DefaultEstimatedHours { get; set; }

        public bool RequiresApproval { get; set; } = false;

        [StringLength(20)]
        public string? ColorCode { get; set; }

        [StringLength(50)]
        public string? IconName { get; set; }

        // Navigation properties
        public virtual ICollection<Action> Actions { get; set; } = new List<Action>();
        public virtual ICollection<ActionTemplate> ActionTemplates { get; set; } = new List<ActionTemplate>();
    }

    public class User : BaseEntity
    {
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [StringLength(20)]
        public string? Phone { get; set; }

        [StringLength(100)]
        public string? Department { get; set; }

        [StringLength(100)]
        public string? JobTitle { get; set; }

        [StringLength(20)]
        public string Role { get; set; } = "USER";

        public int? TeamId { get; set; }

        // Navigation properties
        public virtual Team? Team { get; set; }
        public virtual ICollection<Action> AssignedActions { get; set; } = new List<Action>();
        public virtual ICollection<Action> CreatedActions { get; set; } = new List<Action>();
        public virtual ICollection<ActionComment> ActionComments { get; set; } = new List<ActionComment>();
    }

    public class Team : BaseEntity
    {
        [Required]
        [StringLength(100)]
        public string TeamName { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }

        public int? ManagerId { get; set; }

        [StringLength(100)]
        public string? Department { get; set; }

        // Navigation properties
        public virtual User? Manager { get; set; }
        public virtual ICollection<User> Members { get; set; } = new List<User>();
    }

    public class ActionDependency : BaseEntity
    {
        [Required]
        public int ActionId { get; set; }

        [Required]
        public int DependsOnActionId { get; set; }

        [Required]
        [StringLength(30)]
        public string DependencyType { get; set; } = "FINISH_TO_START";

        [StringLength(500)]
        public string? Notes { get; set; }

        // Navigation properties
        [ForeignKey(nameof(ActionId))]
        public virtual Action? DependentAction { get; set; }

        [ForeignKey(nameof(DependsOnActionId))]
        public virtual Action? DependsOn { get; set; }
    }

    public class ActionAttachment : BaseEntity
    {
        [Required]
        public int ActionId { get; set; }

        [Required]
        [StringLength(255)]
        public string FileName { get; set; } = string.Empty;

        [Required]
        [StringLength(500)]
        public string FilePath { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string FileType { get; set; } = string.Empty;

        public long FileSize { get; set; }

        public DateTime UploadDate { get; set; } = DateTime.UtcNow;

        public int? UploadedById { get; set; }

        // Navigation properties
        [ForeignKey(nameof(ActionId))]
        public virtual Action? Action { get; set; }

        [ForeignKey(nameof(UploadedById))]
        public virtual User? UploadedBy { get; set; }
    }

    public class ActionComment : BaseEntity
    {
        [Required]
        public int ActionId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        [StringLength(2000)]
        public string Comment { get; set; } = string.Empty;

        public DateTime CommentDate { get; set; } = DateTime.UtcNow;

        public int? ParentCommentId { get; set; }

        // Navigation properties
        [ForeignKey(nameof(ActionId))]
        public virtual Action? Action { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual User? User { get; set; }

        [ForeignKey(nameof(ParentCommentId))]
        public virtual ActionComment? ParentComment { get; set; }

        public virtual ICollection<ActionComment> Replies { get; set; } = new List<ActionComment>();
    }

    public class ActionHistory : BaseEntity
    {
        [Required]
        public int ActionId { get; set; }

        [Required]
        [StringLength(100)]
        public string ChangeType { get; set; } = string.Empty;

        [StringLength(100)]
        public string? FieldName { get; set; }

        [StringLength(500)]
        public string? OldValue { get; set; }

        [StringLength(500)]
        public string? NewValue { get; set; }

        public int? ChangedById { get; set; }

        public DateTime ChangeDate { get; set; } = DateTime.UtcNow;

        [StringLength(1000)]
        public string? Notes { get; set; }

        // Navigation properties
        [ForeignKey(nameof(ActionId))]
        public virtual Action? Action { get; set; }

        [ForeignKey(nameof(ChangedById))]
        public virtual User? ChangedBy { get; set; }
    }
}