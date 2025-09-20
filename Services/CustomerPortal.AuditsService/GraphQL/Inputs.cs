namespace CustomerPortal.AuditsService.GraphQL.Inputs
{
    public record CreateAuditInput(
        string AuditTitle,
        int CompanyId,
        int AuditTypeId,
        DateTime StartDate,
        DateTime EndDate,
        int? LeadAuditorId,
        List<int>? SiteIds,
        List<int>? ServiceIds,
        List<CreateAuditTeamMemberInput>? TeamMembers
    );

    public record CreateAuditTeamMemberInput(
        int UserId,
        string Role
    );

    public record UpdateAuditStatusInput(
        int AuditId,
        AuditStatus Status
    );

    public record AssignTeamMemberInput(
        int AuditId,
        int UserId,
        string Role,
        string? Notes
    );

    public record AddSiteToAuditInput(
        int AuditId,
        int SiteId,
        DateTime? ScheduledDate,
        string? Notes
    );

    public record AuditScheduleFilterInput(
        DateTime StartDate,
        DateTime EndDate,
        int? CompanyId,
        int? AuditTypeId,
        AuditStatus? Status
    );

    public record AuditorAvailabilityInput(
        int AuditorId,
        DateTime StartDate,
        DateTime EndDate
    );

    public enum AuditStatus
    {
        PLANNED,
        IN_PROGRESS,
        COMPLETED,
        CANCELLED,
        ON_HOLD
    }

    public enum AuditTeamRole
    {
        LEAD_AUDITOR,
        AUDITOR,
        TECHNICAL_EXPERT,
        OBSERVER,
        TRAINEE
    }

    public enum AuditSiteStatus
    {
        SCHEDULED,
        IN_PROGRESS,
        COMPLETED,
        CANCELLED,
        POSTPONED
    }
}