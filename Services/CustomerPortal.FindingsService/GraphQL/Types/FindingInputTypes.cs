namespace CustomerPortal.FindingsService.GraphQL.Types;

public class CreateFindingInput
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? ReferenceNumber { get; set; }
    public DateTime? IdentifiedDate { get; set; }
    public DateTime? RequiredCompletionDate { get; set; }
    public string? RootCause { get; set; }
    public string? CorrectiveAction { get; set; }
    public string? PreventiveAction { get; set; }
    public string? Evidence { get; set; }
    public int? AuditId { get; set; }
    public int? SiteId { get; set; }
    public int? CompanyId { get; set; }
    public string? IdentifiedBy { get; set; }
    public string? AssignedTo { get; set; }
    public int Severity { get; set; } = 1;
    public int Priority { get; set; } = 1;
    public int CategoryId { get; set; }
    public int StatusId { get; set; }
}

public class UpdateFindingInput
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? ReferenceNumber { get; set; }
    public DateTime? IdentifiedDate { get; set; }
    public DateTime? RequiredCompletionDate { get; set; }
    public DateTime? ActualCompletionDate { get; set; }
    public string? RootCause { get; set; }
    public string? CorrectiveAction { get; set; }
    public string? PreventiveAction { get; set; }
    public string? Evidence { get; set; }
    public int? AuditId { get; set; }
    public int? SiteId { get; set; }
    public int? CompanyId { get; set; }
    public string? IdentifiedBy { get; set; }
    public string? AssignedTo { get; set; }
    public string? ReviewedBy { get; set; }
    public DateTime? ReviewedDate { get; set; }
    public int? Severity { get; set; }
    public int? Priority { get; set; }
    public int? CategoryId { get; set; }
    public int? StatusId { get; set; }
}

public class UpdateFindingStatusInput
{
    public int Id { get; set; }
    public int StatusId { get; set; }
    public string? ReviewedBy { get; set; }
    public DateTime? ReviewedDate { get; set; }
    public DateTime? ActualCompletionDate { get; set; }
}