using CustomerPortal.Shared.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CustomerPortal.ActionsService.Entities
{
    public class ActionTemplate : BaseEntity
    {
        [Required]
        [StringLength(100)]
        public string TemplateName { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }

        [Required]
        [StringLength(50)]
        public string Category { get; set; } = "GENERAL";

        [Required]
        public int ActionTypeId { get; set; }

        [Required]
        [StringLength(200)]
        public string DefaultTitle { get; set; } = string.Empty;

        [StringLength(2000)]
        public string? DefaultDescription { get; set; }

        [Required]
        [StringLength(20)]
        public string DefaultPriority { get; set; } = "MEDIUM";

        public int? DefaultEstimatedHours { get; set; }

        [StringLength(5000)]
        public string? Checklist { get; set; } // JSON format

        public int UsageCount { get; set; } = 0;

        // Navigation properties
        public virtual ActionType? ActionType { get; set; }
    }

    public class Workflow : BaseEntity
    {
        [Required]
        [StringLength(100)]
        public string WorkflowName { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }

        [Required]
        [StringLength(50)]
        public string TriggerType { get; set; } = "MANUAL";

        [StringLength(1000)]
        public string? TriggerConditions { get; set; } // JSON format

        // Navigation properties
        public virtual ICollection<WorkflowStep> Steps { get; set; } = new List<WorkflowStep>();
        public virtual ICollection<WorkflowInstance> Instances { get; set; } = new List<WorkflowInstance>();
    }

    public class WorkflowStep : BaseEntity
    {
        [Required]
        public int WorkflowId { get; set; }

        [Required]
        public int StepNumber { get; set; }

        [Required]
        [StringLength(100)]
        public string StepName { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }

        [Required]
        public int ActionTypeId { get; set; }

        [Required]
        [StringLength(50)]
        public string AssigneeRule { get; set; } = "MANUAL"; // MANUAL, ROLE_BASED, PREVIOUS_STEP, etc.

        [StringLength(100)]
        public string? AssigneeValue { get; set; } // Role name, user ID, etc.

        public bool ApprovalRequired { get; set; } = false;

        public int? EstimatedHours { get; set; }

        [StringLength(20)]
        public string Priority { get; set; } = "MEDIUM";

        [StringLength(1000)]
        public string? StepConditions { get; set; } // JSON format

        // Navigation properties
        [ForeignKey(nameof(WorkflowId))]
        public virtual Workflow? Workflow { get; set; }

        public virtual ActionType? ActionType { get; set; }
    }

    public class WorkflowInstance : BaseEntity
    {
        [Required]
        public int WorkflowId { get; set; }

        [Required]
        [StringLength(50)]
        public string InstanceNumber { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string Status { get; set; } = "ACTIVE";

        public DateTime StartedDate { get; set; } = DateTime.UtcNow;

        public DateTime? CompletedDate { get; set; }

        public int? StartedById { get; set; }

        [StringLength(1000)]
        public string? Context { get; set; } // JSON format for workflow variables

        // Navigation properties
        [ForeignKey(nameof(WorkflowId))]
        public virtual Workflow? Workflow { get; set; }

        [ForeignKey(nameof(StartedById))]
        public virtual User? StartedBy { get; set; }

        public virtual ICollection<Action> CreatedActions { get; set; } = new List<Action>();
    }
}