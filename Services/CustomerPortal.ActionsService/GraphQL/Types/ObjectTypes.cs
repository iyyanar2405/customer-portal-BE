namespace CustomerPortal.ActionsService.GraphQL.Types
{
    public class ActionGraphQLType
    {
        public int Id { get; set; }
        public string ActionNumber { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int ActionTypeId { get; set; }
        public int? AssignedToId { get; set; }
        public int? CreatedById { get; set; }
        public string Priority { get; set; } = "MEDIUM";
        public string Status { get; set; } = "NOT_STARTED";
        public DateTime? StartDate { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? CompletedDate { get; set; }
        public decimal? EstimatedHours { get; set; }
        public decimal? ActualHours { get; set; }
        public int Progress { get; set; } = 0;
        public string? Comments { get; set; }
        public int? TemplateId { get; set; }
        public int? WorkflowInstanceId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        // Navigation properties
        public ActionTypeGraphQLType? ActionTypeEntity { get; set; }
        public UserType? AssignedTo { get; set; }
        public UserType? CreatedBy { get; set; }
        public List<ActionDependencyType> Dependencies { get; set; } = new();
        public List<ActionDependencyType> DependentActions { get; set; } = new();
        public List<ActionAttachmentType> Attachments { get; set; } = new();
        public List<ActionCommentType> ActionComments { get; set; } = new();
        public List<ActionHistoryType> History { get; set; } = new();
    }

    public class ActionTypeGraphQLType
    {
        public int Id { get; set; }
        public string TypeName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Category { get; set; } = "GENERAL";
        public string DefaultPriority { get; set; } = "MEDIUM";
        public decimal? DefaultEstimatedHours { get; set; }
        public bool RequiresApproval { get; set; } = false;
        public bool IsActive { get; set; } = true;
        public string? ColorCode { get; set; }
        public string? IconName { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        // Navigation properties
        public List<ActionGraphQLType> Actions { get; set; } = new();
        public List<ActionTemplateType> ActionTemplates { get; set; } = new();
    }

    public class UserType
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string? Department { get; set; }
        public string? JobTitle { get; set; }
        public string Role { get; set; } = "USER";
        public int? TeamId { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        // Computed properties
        public string FullName => $"{FirstName} {LastName}";

        // Navigation properties
        public TeamType? Team { get; set; }
        public List<ActionGraphQLType> AssignedActions { get; set; } = new();
        public List<ActionGraphQLType> CreatedActions { get; set; } = new();
        public List<ActionCommentType> ActionComments { get; set; } = new();
    }

    public class TeamType
    {
        public int Id { get; set; }
        public string TeamName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Department { get; set; }
        public int? ManagerId { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        // Navigation properties
        public UserType? Manager { get; set; }
        public List<UserType> Members { get; set; } = new();
    }

    public class ActionDependencyType
    {
        public int Id { get; set; }
        public int ActionId { get; set; }
        public int DependsOnActionId { get; set; }
        public string DependencyType { get; set; } = "FINISH_TO_START";
        public string? Notes { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        // Navigation properties
        public ActionGraphQLType? DependentAction { get; set; }
        public ActionGraphQLType? DependsOn { get; set; }
    }

    public class ActionAttachmentType
    {
        public int Id { get; set; }
        public int ActionId { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
        public string FileType { get; set; } = string.Empty;
        public long FileSize { get; set; }
        public int? UploadedById { get; set; }
        public DateTime UploadDate { get; set; }

        // Navigation properties
        public ActionGraphQLType? Action { get; set; }
        public UserType? UploadedBy { get; set; }
    }

    public class ActionCommentType
    {
        public int Id { get; set; }
        public int ActionId { get; set; }
        public int UserId { get; set; }
        public string Comment { get; set; } = string.Empty;
        public DateTime CommentDate { get; set; }
        public int? ParentCommentId { get; set; }

        // Navigation properties
        public ActionGraphQLType? Action { get; set; }
        public UserType? User { get; set; }
        public ActionCommentType? ParentComment { get; set; }
        public List<ActionCommentType> Replies { get; set; } = new();
    }

    public class ActionHistoryType
    {
        public int Id { get; set; }
        public int ActionId { get; set; }
        public string ChangeType { get; set; } = string.Empty;
        public string? FieldName { get; set; }
        public string? OldValue { get; set; }
        public string? NewValue { get; set; }
        public DateTime ChangeDate { get; set; }
        public int? ChangedById { get; set; }
        public string? Notes { get; set; }

        // Navigation properties
        public ActionGraphQLType? Action { get; set; }
        public UserType? ChangedBy { get; set; }
    }

    public class ActionTemplateType
    {
        public int Id { get; set; }
        public string TemplateName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Category { get; set; } = "GENERAL";
        public int ActionTypeId { get; set; }
        public string DefaultTitle { get; set; } = string.Empty;
        public string? DefaultDescription { get; set; }
        public string DefaultPriority { get; set; } = "MEDIUM";
        public decimal? DefaultEstimatedHours { get; set; }
        public string? Checklist { get; set; }
        public bool IsActive { get; set; } = true;
        public int UsageCount { get; set; } = 0;
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        // Navigation properties
        public ActionTypeGraphQLType? ActionType { get; set; }
    }

    public class WorkflowType
    {
        public int Id { get; set; }
        public string WorkflowName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string TriggerType { get; set; } = "MANUAL";
        public string? TriggerConditions { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        // Navigation properties
        public List<WorkflowStepType> Steps { get; set; } = new();
        public List<WorkflowInstanceType> Instances { get; set; } = new();
    }

    public class WorkflowStepType
    {
        public int Id { get; set; }
        public int WorkflowId { get; set; }
        public int StepNumber { get; set; }
        public string StepName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int ActionTypeId { get; set; }
        public string AssigneeRule { get; set; } = "MANUAL";
        public string? AssigneeValue { get; set; }
        public bool ApprovalRequired { get; set; } = false;
        public string Priority { get; set; } = "MEDIUM";
        public decimal? EstimatedHours { get; set; }
        public string? StepConditions { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        // Navigation properties
        public WorkflowType? Workflow { get; set; }
        public ActionTypeGraphQLType? ActionType { get; set; }
    }

    public class WorkflowInstanceType
    {
        public int Id { get; set; }
        public int WorkflowId { get; set; }
        public string InstanceNumber { get; set; } = string.Empty;
        public string Status { get; set; } = "ACTIVE";
        public DateTime StartedDate { get; set; }
        public DateTime? CompletedDate { get; set; }
        public int? StartedById { get; set; }
        public string? Context { get; set; }

        // Navigation properties
        public WorkflowType? Workflow { get; set; }
        public UserType? StartedBy { get; set; }
    }
}
