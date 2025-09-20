using CustomerPortal.FindingsService.Entities;

namespace CustomerPortal.FindingsService.GraphQL.Types;

public class FindingStatusType : ObjectType<FindingStatus>
{
    protected override void Configure(IObjectTypeDescriptor<FindingStatus> descriptor)
    {
        descriptor.Description("Represents a finding status");

        descriptor
            .Field(f => f.Id)
            .Description("Unique identifier for the status");

        descriptor
            .Field(f => f.Name)
            .Description("Status name");

        descriptor
            .Field(f => f.Description)
            .Description("Status description");

        descriptor
            .Field(f => f.Code)
            .Description("Status code");

        descriptor
            .Field(f => f.Color)
            .Description("Display color for the status");

        descriptor
            .Field(f => f.DisplayOrder)
            .Description("Display order");

        descriptor
            .Field(f => f.IsActive)
            .Description("Whether the status is active");

        descriptor
            .Field(f => f.IsFinalStatus)
            .Description("Whether this is a final status");

        descriptor
            .Field(f => f.CreatedDate)
            .Description("Creation timestamp");

        descriptor
            .Field(f => f.ModifiedDate)
            .Description("Last update timestamp");

        descriptor
            .Field(f => f.Findings)
            .Description("Findings with this status");
    }
}