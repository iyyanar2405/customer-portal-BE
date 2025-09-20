using CustomerPortal.FindingsService.Entities;

namespace CustomerPortal.FindingsService.GraphQL.Types;

public class FindingType : ObjectType<Finding>
{
    protected override void Configure(IObjectTypeDescriptor<Finding> descriptor)
    {
        descriptor.Description("Represents an audit finding or non-conformity");

        descriptor
            .Field(f => f.Id)
            .Description("Unique identifier for the finding");

        descriptor
            .Field(f => f.Title)
            .Description("Title or brief description of the finding");

        descriptor
            .Field(f => f.Description)
            .Description("Detailed description of the finding");

        descriptor
            .Field(f => f.ReferenceNumber)
            .Description("Unique reference number for the finding");

        descriptor
            .Field(f => f.IdentifiedDate)
            .Description("Date when the finding was identified");

        descriptor
            .Field(f => f.RequiredCompletionDate)
            .Description("Required completion date for corrective actions");

        descriptor
            .Field(f => f.ActualCompletionDate)
            .Description("Actual completion date");

        descriptor
            .Field(f => f.RootCause)
            .Description("Root cause analysis of the finding");

        descriptor
            .Field(f => f.CorrectiveAction)
            .Description("Corrective actions taken");

        descriptor
            .Field(f => f.PreventiveAction)
            .Description("Preventive actions planned");

        descriptor
            .Field(f => f.Evidence)
            .Description("Evidence or documentation related to the finding");

        descriptor
            .Field(f => f.AuditId)
            .Description("Related audit ID");

        descriptor
            .Field(f => f.SiteId)
            .Description("Related site ID");

        descriptor
            .Field(f => f.CompanyId)
            .Description("Related company ID");

        descriptor
            .Field(f => f.IdentifiedBy)
            .Description("Person who identified the finding");

        descriptor
            .Field(f => f.AssignedTo)
            .Description("Person assigned to resolve the finding");

        descriptor
            .Field(f => f.ReviewedBy)
            .Description("Person who reviewed the finding");

        descriptor
            .Field(f => f.ReviewedDate)
            .Description("Date when the finding was reviewed");

        descriptor
            .Field(f => f.Severity)
            .Description("Severity level (1=Low, 2=Medium, 3=High, 4=Critical, 5=Emergency)");

        descriptor
            .Field(f => f.Priority)
            .Description("Priority level (1=Low, 2=Medium, 3=High, 4=Critical, 5=Emergency)");

        descriptor
            .Field(f => f.CategoryId)
            .Description("Finding category ID");

        descriptor
            .Field(f => f.StatusId)
            .Description("Finding status ID");

        descriptor
            .Field(f => f.Category)
            .Description("Finding category");

        descriptor
            .Field(f => f.Status)
            .Description("Finding status");

        descriptor
            .Field(f => f.CreatedDate)
            .Description("Creation timestamp");

        descriptor
            .Field(f => f.ModifiedDate)
            .Description("Last update timestamp");
    }
}