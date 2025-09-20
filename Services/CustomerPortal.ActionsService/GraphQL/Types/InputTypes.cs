namespace CustomerPortal.ActionsService.GraphQL.Types
{
    // Action Input Types
    public class CreateActionInput
    {
        public string ActionNumber { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int ActionTypeId { get; set; }
        public int? AssignedToId { get; set; }
        public string Priority { get; set; } = "MEDIUM";
        public DateTime? StartDate { get; set; }
        public DateTime? DueDate { get; set; }
        public decimal? EstimatedHours { get; set; }
        public string? Comments { get; set; }
        public int? TemplateId { get; set; }
    }

    public class UpdateActionInput
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public int? ActionTypeId { get; set; }
        public int? AssignedToId { get; set; }
        public string? Priority { get; set; }
        public string? Status { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? CompletedDate { get; set; }
        public decimal? EstimatedHours { get; set; }
        public decimal? ActualHours { get; set; }
        public int? Progress { get; set; }
        public string? Comments { get; set; }
    }

    public class ActionSearchInput
    {
        public string? SearchTerm { get; set; }
        public string? Status { get; set; }
        public string? Priority { get; set; }
        public int? ActionTypeId { get; set; }
        public int? AssignedToId { get; set; }
        public int? CreatedById { get; set; }
        public int? TeamId { get; set; }
        public DateTime? DueDateFrom { get; set; }
        public DateTime? DueDateTo { get; set; }
        public bool? OverdueOnly { get; set; }
        public int Skip { get; set; } = 0;
        public int Take { get; set; } = 50;
    }

    // Action Type Input Types
    public class CreateActionTypeInput
    {
        public string TypeName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Category { get; set; } = "GENERAL";
        public string DefaultPriority { get; set; } = "MEDIUM";
        public decimal? DefaultEstimatedHours { get; set; }
        public bool RequiresApproval { get; set; } = false;
        public string? ColorCode { get; set; }
        public string? IconName { get; set; }
    }

    public class UpdateActionTypeInput
    {
        public int Id { get; set; }
        public string? TypeName { get; set; }
        public string? Description { get; set; }
        public string? Category { get; set; }
        public string? DefaultPriority { get; set; }
        public decimal? DefaultEstimatedHours { get; set; }
        public bool? RequiresApproval { get; set; }
        public string? ColorCode { get; set; }
        public string? IconName { get; set; }
    }

    // User Input Types
    public class CreateUserInput
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string? Department { get; set; }
        public string? JobTitle { get; set; }
        public string Role { get; set; } = "USER";
        public int? TeamId { get; set; }
    }

    public class UpdateUserInput
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Department { get; set; }
        public string? JobTitle { get; set; }
        public string? Role { get; set; }
        public int? TeamId { get; set; }
    }

    // Team Input Types
    public class CreateTeamInput
    {
        public string TeamName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Department { get; set; }
        public int? ManagerId { get; set; }
    }

    public class UpdateTeamInput
    {
        public int Id { get; set; }
        public string? TeamName { get; set; }
        public string? Description { get; set; }
        public string? Department { get; set; }
        public int? ManagerId { get; set; }
    }

    // Action Dependency Input Types
    public class CreateActionDependencyInput
    {
        public int ActionId { get; set; }
        public int DependsOnActionId { get; set; }
        public string DependencyType { get; set; } = "FINISH_TO_START";
        public string? Notes { get; set; }
    }

    public class UpdateActionDependencyInput
    {
        public int Id { get; set; }
        public string? DependencyType { get; set; }
        public string? Notes { get; set; }
    }

    // Action Comment Input Types
    public class CreateActionCommentInput
    {
        public int ActionId { get; set; }
        public string Comment { get; set; } = string.Empty;
        public int? ParentCommentId { get; set; }
    }

    public class UpdateActionCommentInput
    {
        public int Id { get; set; }
        public string Comment { get; set; } = string.Empty;
    }

    // Action Template Input Types
    public class CreateActionTemplateInput
    {
        public string TemplateName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Category { get; set; } = "GENERAL";
        public int ActionTypeId { get; set; }
        public string DefaultTitle { get; set; } = string.Empty;
        public string? DefaultDescription { get; set; }
        public string DefaultPriority { get; set; } = "MEDIUM";
        public decimal? DefaultEstimatedHours { get; set; }
        public string? Checklist { get; set; }
    }

    public class UpdateActionTemplateInput
    {
        public int Id { get; set; }
        public string? TemplateName { get; set; }
        public string? Description { get; set; }
        public string? Category { get; set; }
        public string? DefaultTitle { get; set; }
        public string? DefaultDescription { get; set; }
        public string? DefaultPriority { get; set; }
        public decimal? DefaultEstimatedHours { get; set; }
        public string? Checklist { get; set; }
    }

    // Workflow Input Types
    public class CreateWorkflowInput
    {
        public string WorkflowName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string TriggerType { get; set; } = "MANUAL";
        public string? TriggerConditions { get; set; }
        public List<CreateWorkflowStepInput> Steps { get; set; } = new();
    }

    public class UpdateWorkflowInput
    {
        public int Id { get; set; }
        public string? WorkflowName { get; set; }
        public string? Description { get; set; }
        public string? TriggerType { get; set; }
        public string? TriggerConditions { get; set; }
    }

    public class CreateWorkflowStepInput
    {
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
    }

    public class UpdateWorkflowStepInput
    {
        public int Id { get; set; }
        public int? StepNumber { get; set; }
        public string? StepName { get; set; }
        public string? Description { get; set; }
        public int? ActionTypeId { get; set; }
        public string? AssigneeRule { get; set; }
        public string? AssigneeValue { get; set; }
        public bool? ApprovalRequired { get; set; }
        public string? Priority { get; set; }
        public decimal? EstimatedHours { get; set; }
        public string? StepConditions { get; set; }
    }

    // Workflow Instance Input Types
    public class CreateWorkflowInstanceInput
    {
        public int WorkflowId { get; set; }
        public string InstanceNumber { get; set; } = string.Empty;
        public string? Context { get; set; }
    }

    public class UpdateWorkflowInstanceInput
    {
        public int Id { get; set; }
        public string? Status { get; set; }
        public DateTime? CompletedDate { get; set; }
        public string? Context { get; set; }
    }

    // File Upload Input
    public class FileUploadInput
    {
        public int ActionId { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string FileType { get; set; } = string.Empty;
        public long FileSize { get; set; }
        public byte[] FileContent { get; set; } = Array.Empty<byte>();
    }

    // Statistics Input Types
    public class ActionStatisticsInput
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? UserId { get; set; }
        public int? TeamId { get; set; }
        public string? Status { get; set; }
        public string? Priority { get; set; }
    }

    // Pagination Input
    public class PaginationInput
    {
        public int Skip { get; set; } = 0;
        public int Take { get; set; } = 50;
    }

    // Sorting Input
    public class SortingInput
    {
        public string? Field { get; set; }
        public string Direction { get; set; } = "ASC";
    }

    // Response Types
    public class ActionPagedResult
    {
        public List<ActionGraphQLType> Items { get; set; } = new();
        public int TotalCount { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }

    public class ActionStatistics
    {
        public int TotalActions { get; set; }
        public Dictionary<string, int> StatusDistribution { get; set; } = new();
        public Dictionary<string, int> PriorityDistribution { get; set; } = new();
        public int OverdueActions { get; set; }
        public int CompletedThisMonth { get; set; }
        public int AssignedToUser { get; set; }
        public double AverageCompletionDays { get; set; }
    }

    public class BulkOperationResult
    {
        public int ProcessedCount { get; set; }
        public int SuccessCount { get; set; }
        public int ErrorCount { get; set; }
        public List<string> Errors { get; set; } = new();
    }
}
