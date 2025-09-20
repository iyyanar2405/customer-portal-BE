using HotChocolate;

namespace CustomerPortalAPI.Modules.Audits.GraphQL
{
    // Input Types for API/GraphQL
    public record AuditInput(
        string Sites,
        string Services,
        int CompanyId,
        string Status,
        DateTime StartDate,
        DateTime? EndDate,
        string? LeadAuditor,
        string? Type,
        string? AuditNumber,
        string? Description,
        int? AuditTypeId,
        int? CreatedBy,
        int? ModifiedBy
    );

    public record UpdateAuditInput(
        int AuditId,
        string? Sites,
        string? Services,
        int? CompanyId,
        string? Status,
        DateTime? StartDate,
        DateTime? EndDate,
        string? LeadAuditor,
        string? Type,
        string? AuditNumber,
        string? Description,
        int? AuditTypeId,
        int? ModifiedBy
    );

    public record AuditScheduleInput(
        int AuditId,
        DateTime StartDate,
        DateTime EndDate,
        string? Comments,
        int? ModifiedBy
    );

    public record AuditTeamMemberInput(
        int AuditId,
        int UserId,
        string Role,
        string? Responsibilities,
        bool IsLead,
        int? CreatedBy
    );

    public record AuditSiteInput(
        int AuditId,
        int SiteId,
        DateTime? PlannedStartDate,
        DateTime? PlannedEndDate,
        DateTime? ActualStartDate,
        DateTime? ActualEndDate,
        string? Status,
        string? Notes,
        int? CreatedBy
    );

    // Output/Response Types
    public record AuditType(
        int AuditId,
        string Sites,
        string Services,
        int CompanyId,
        string? CompanyName,
        string Status,
        DateTime StartDate,
        DateTime? EndDate,
        string? LeadAuditor,
        string? Type,
        string? AuditNumber,
        string? Description,
        int? AuditTypeId,
        string? AuditTypeName,
        bool IsActive,
        DateTime CreatedDate,
        DateTime? ModifiedDate,
        int? CreatedBy,
        int? ModifiedBy,
        string? CreatedByName,
        string? ModifiedByName,
        List<AuditTeamMemberType>? TeamMembers,
        List<AuditSiteType>? AuditSites,
        List<AuditServiceType>? AuditServices
    );

    public record AuditTeamMemberType(
        int Id,
        int AuditId,
        int UserId,
        string? UserName,
        string Role,
        string? Responsibilities,
        bool IsLead,
        DateTime CreatedDate,
        int? CreatedBy
    );

    public record AuditSiteType(
        int Id,
        int AuditId,
        int SiteId,
        string? SiteName,
        DateTime? PlannedStartDate,
        DateTime? PlannedEndDate,
        DateTime? ActualStartDate,
        DateTime? ActualEndDate,
        string? Status,
        string? Notes,
        DateTime CreatedDate,
        int? CreatedBy
    );

    public record AuditServiceType(
        int Id,
        int AuditId,
        string Service,
        string? ServiceDescription,
        bool IsActive,
        DateTime CreatedDate,
        int? CreatedBy
    );

    public record AuditTypeInfo(
        int Id,
        string TypeName,
        string? Description,
        bool IsActive,
        DateTime CreatedDate
    );

    // Payload Types for GraphQL Mutations
    public record CreateAuditPayload(
        AuditType? Audit,
        string? ErrorMessage
    );

    public record UpdateAuditPayload(
        AuditType? Audit,
        string? ErrorMessage
    );

    public record ScheduleAuditPayload(
        AuditType? Audit,
        string? ErrorMessage
    );

    public record AssignTeamMemberPayload(
        AuditTeamMemberType? TeamMember,
        string? ErrorMessage
    );

    public record AssignSitePayload(
        AuditSiteType? AuditSite,
        string? ErrorMessage
    );

    public record CompleteAuditPayload(
        AuditType? Audit,
        string? ErrorMessage
    );

    // Filter and Search Types
    public record AuditFilterInput(
        List<int>? CompanyIds,
        List<int>? SiteIds,
        List<string>? Services,
        List<string>? Statuses,
        DateTime? StartDateFrom,
        DateTime? StartDateTo,
        DateTime? EndDateFrom,
        DateTime? EndDateTo,
        string? LeadAuditor,
        string? Type,
        string? AuditNumber,
        bool? IsActive
    );

    public record AuditSearchInput(
        string? SearchTerm,
        int? PageNumber,
        int? PageSize,
        string? SortBy,
        string? SortDirection,
        AuditFilterInput? Filters
    );

    // Response wrappers
    public record AuditListResponse(
        List<AuditType> Audits,
        int TotalCount,
        int PageNumber,
        int PageSize,
        bool HasNextPage,
        bool HasPreviousPage
    );

    public record BaseAuditResponse(
        bool IsSuccess,
        string? Message,
        string? ErrorCode
    );

    public record AuditResponse(
        AuditType? Data,
        bool IsSuccess,
        string? Message,
        string? ErrorCode
    ) : BaseAuditResponse(IsSuccess, Message, ErrorCode);

    public record AuditListResponseWrapper(
        AuditListResponse? Data,
        bool IsSuccess,
        string? Message,
        string? ErrorCode
    ) : BaseAuditResponse(IsSuccess, Message, ErrorCode);
}