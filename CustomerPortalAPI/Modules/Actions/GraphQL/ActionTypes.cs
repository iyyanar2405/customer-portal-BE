using HotChocolate;

namespace CustomerPortalAPI.Modules.Actions.GraphQL
{
    // Input Types
    public record ActionInput(
        string ActionName,
        string? Description,
        string? ActionType,
        string? Priority,
        string? Status,
        int? AssignedToUserId,
        int? CompanyId,
        int? SiteId,
        int? AuditId,
        int? FindingId,
        DateTime? DueDate,
        string? Comments
    );

    public record UpdateActionInput(
        int Id,
        string? ActionName,
        string? Description,
        string? ActionType,
        string? Priority,
        string? Status,
        int? AssignedToUserId,
        int? CompanyId,
        int? SiteId,
        int? AuditId,
        int? FindingId,
        DateTime? DueDate,
        string? Comments
    );

    public record CompleteActionInput(
        int Id,
        string? Comments
    );

    public record AssignActionInput(
        int Id,
        int AssignedToUserId
    );

    // Output Types
    public record ActionType(
        int Id,
        string ActionName,
        string? Description,
        string? ActionTypeValue,
        string? Priority,
        string? Status,
        int? AssignedToUserId,
        int? CompanyId,
        int? SiteId,
        int? AuditId,
        int? FindingId,
        DateTime? DueDate,
        DateTime? CompletedDate,
        DateTime CreatedDate,
        int? CreatedBy,
        DateTime? ModifiedDate,
        int? ModifiedBy,
        string? Comments,
        bool IsActive
    );

    public record ActionSummaryType(
        int Id,
        string ActionName,
        string? Priority,
        string? Status,
        DateTime? DueDate,
        bool IsOverdue
    );

    // Payload Types
    public record CreateActionPayload(
        ActionType? Action,
        string? ErrorMessage
    );

    public record UpdateActionPayload(
        ActionType? Action,
        string? ErrorMessage
    );

    public record CompleteActionPayload(
        ActionType? Action,
        string? ErrorMessage
    );

    public record AssignActionPayload(
        ActionType? Action,
        string? ErrorMessage
    );

    public record DeleteActionPayload(
        bool Success,
        string? ErrorMessage
    );

    // Filter Types
    public record ActionFilterInput(
        string? ActionName,
        string? Status,
        string? Priority,
        int? AssignedToUserId,
        int? CompanyId,
        int? SiteId,
        DateTime? DueDateFrom,
        DateTime? DueDateTo,
        bool? IsOverdue,
        bool? IsCompleted
    );

    // Sort Types
    public enum ActionSortField
    {
        ActionName,
        Priority,
        Status,
        DueDate,
        CreatedDate,
        ModifiedDate
    }

    public record ActionSortInput(
        ActionSortField Field,
        SortDirection Direction = SortDirection.Asc
    );

    public enum SortDirection
    {
        Asc,
        Desc
    }

    // Statistics Types
    public record ActionStatisticsType(
        int TotalActions,
        int CompletedActions,
        int PendingActions,
        int OverdueActions,
        int HighPriorityActions,
        int MediumPriorityActions,
        int LowPriorityActions
    );
}