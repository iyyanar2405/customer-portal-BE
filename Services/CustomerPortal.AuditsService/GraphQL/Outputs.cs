namespace CustomerPortal.AuditsService.GraphQL.Outputs
{
    public record AuditSummaryOutput(
        int AuditId,
        int TotalFindings,
        int CriticalFindings,
        int MajorFindings,
        int MinorFindings,
        int ObservationsCount,
        decimal CompliancePercentage,
        string? RecommendationForCertification,
        int AuditDuration,
        int SitesAudited,
        int TeamMembersCount
    );

    public record AuditorAvailabilityOutput(
        int AuditorId,
        List<DateTime> AvailableDates,
        List<ConflictingAuditOutput> ConflictingAudits
    );

    public record ConflictingAuditOutput(
        string AuditNumber,
        DateTime StartDate,
        DateTime EndDate
    );

    public record AuditResponse(
        int Id,
        string AuditNumber,
        string AuditTitle,
        string Status,
        DateTime CreatedDate,
        DateTime? ModifiedDate
    );
}