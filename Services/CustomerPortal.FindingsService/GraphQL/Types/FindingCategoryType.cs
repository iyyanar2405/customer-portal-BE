using CustomerPortal.FindingsService.Entities;

namespace CustomerPortal.FindingsService.GraphQL.Types;

public class FindingCategoryType : ObjectType<FindingCategory>
{
    protected override void Configure(IObjectTypeDescriptor<FindingCategory> descriptor)
    {
        descriptor.Description("Represents a finding category");

        descriptor
            .Field(f => f.Id)
            .Description("Unique identifier for the category");

        descriptor
            .Field(f => f.Name)
            .Description("Category name");

        descriptor
            .Field(f => f.Description)
            .Description("Category description");

        descriptor
            .Field(f => f.Code)
            .Description("Category code");

        descriptor
            .Field(f => f.IsActive)
            .Description("Whether the category is active");

        descriptor
            .Field(f => f.CreatedDate)
            .Description("Creation timestamp");

        descriptor
            .Field(f => f.ModifiedDate)
            .Description("Last update timestamp");

        descriptor
            .Field(f => f.Findings)
            .Description("Findings in this category");
    }
}