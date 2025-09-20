using CustomerPortal.FindingsService.Data;
using CustomerPortal.FindingsService.GraphQL;
using CustomerPortal.FindingsService.GraphQL.Types;
using CustomerPortal.FindingsService.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add Entity Framework
builder.Services.AddDbContext<FindingsDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add repositories
builder.Services.AddScoped<IFindingRepository, FindingRepository>();
builder.Services.AddScoped<IFindingCategoryRepository, FindingCategoryRepository>();
builder.Services.AddScoped<IFindingStatusRepository, FindingStatusRepository>();

// Add GraphQL
builder.Services
    .AddGraphQLServer()
    .AddQueryType<Query>()
    .AddMutationType<Mutation>()
    .AddType<FindingType>()
    .AddType<FindingCategoryType>()
    .AddType<FindingStatusType>()
    .AddProjections()
    .AddFiltering()
    .AddSorting();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

// Configure GraphQL
app.MapGraphQL();

// Ensure database is created and seeded
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<FindingsDbContext>();
    await context.Database.EnsureCreatedAsync();
    
    // Seed data if empty
    await SeedDataAsync(context);
}

app.Run();

static async Task SeedDataAsync(FindingsDbContext context)
{
    if (!await context.FindingCategories.AnyAsync())
    {
        var categories = new[]
        {
            new CustomerPortal.FindingsService.Entities.FindingCategory { Name = "Documentation", Code = "DOC", Description = "Documentation related findings", IsActive = true, CreatedDate = DateTime.UtcNow, ModifiedDate = DateTime.UtcNow },
            new CustomerPortal.FindingsService.Entities.FindingCategory { Name = "Process", Code = "PROC", Description = "Process related findings", IsActive = true, CreatedDate = DateTime.UtcNow, ModifiedDate = DateTime.UtcNow },
            new CustomerPortal.FindingsService.Entities.FindingCategory { Name = "Training", Code = "TRAIN", Description = "Training related findings", IsActive = true, CreatedDate = DateTime.UtcNow, ModifiedDate = DateTime.UtcNow },
            new CustomerPortal.FindingsService.Entities.FindingCategory { Name = "Equipment", Code = "EQUIP", Description = "Equipment related findings", IsActive = true, CreatedDate = DateTime.UtcNow, ModifiedDate = DateTime.UtcNow },
            new CustomerPortal.FindingsService.Entities.FindingCategory { Name = "Management", Code = "MGMT", Description = "Management system findings", IsActive = true, CreatedDate = DateTime.UtcNow, ModifiedDate = DateTime.UtcNow }
        };

        await context.FindingCategories.AddRangeAsync(categories);
        await context.SaveChangesAsync();
    }

    if (!await context.FindingStatuses.AnyAsync())
    {
        var statuses = new[]
        {
            new CustomerPortal.FindingsService.Entities.FindingStatus { Name = "Open", Code = "OPEN", Description = "Finding is open and pending action", Color = "#FF6B35", DisplayOrder = 1, IsActive = true, IsFinalStatus = false, CreatedDate = DateTime.UtcNow, ModifiedDate = DateTime.UtcNow },
            new CustomerPortal.FindingsService.Entities.FindingStatus { Name = "In Progress", Code = "PROG", Description = "Finding is being addressed", Color = "#F7931E", DisplayOrder = 2, IsActive = true, IsFinalStatus = false, CreatedDate = DateTime.UtcNow, ModifiedDate = DateTime.UtcNow },
            new CustomerPortal.FindingsService.Entities.FindingStatus { Name = "Under Review", Code = "REVIEW", Description = "Finding is under review", Color = "#FFD23F", DisplayOrder = 3, IsActive = true, IsFinalStatus = false, CreatedDate = DateTime.UtcNow, ModifiedDate = DateTime.UtcNow },
            new CustomerPortal.FindingsService.Entities.FindingStatus { Name = "Resolved", Code = "RESOLVED", Description = "Finding has been resolved", Color = "#06FFA5", DisplayOrder = 4, IsActive = true, IsFinalStatus = true, CreatedDate = DateTime.UtcNow, ModifiedDate = DateTime.UtcNow },
            new CustomerPortal.FindingsService.Entities.FindingStatus { Name = "Closed", Code = "CLOSED", Description = "Finding is closed", Color = "#4CAF50", DisplayOrder = 5, IsActive = true, IsFinalStatus = true, CreatedDate = DateTime.UtcNow, ModifiedDate = DateTime.UtcNow },
            new CustomerPortal.FindingsService.Entities.FindingStatus { Name = "Cancelled", Code = "CANCEL", Description = "Finding has been cancelled", Color = "#9E9E9E", DisplayOrder = 6, IsActive = true, IsFinalStatus = true, CreatedDate = DateTime.UtcNow, ModifiedDate = DateTime.UtcNow }
        };

        await context.FindingStatuses.AddRangeAsync(statuses);
        await context.SaveChangesAsync();
    }

    if (!await context.Findings.AnyAsync())
    {
        var openStatus = await context.FindingStatuses.FirstAsync(s => s.Code == "OPEN");
        var docCategory = await context.FindingCategories.FirstAsync(c => c.Code == "DOC");
        var procCategory = await context.FindingCategories.FirstAsync(c => c.Code == "PROC");

        var findings = new[]
        {
            new CustomerPortal.FindingsService.Entities.Finding
            {
                Title = "Missing procedure documentation",
                Description = "The quality manual lacks detailed procedures for calibration activities",
                ReferenceNumber = "F001-2025",
                IdentifiedDate = DateTime.UtcNow.AddDays(-10),
                RequiredCompletionDate = DateTime.UtcNow.AddDays(30),
                IdentifiedBy = "John Auditor",
                AssignedTo = "Quality Manager",
                Severity = 3,
                Priority = 2,
                CategoryId = docCategory.Id,
                StatusId = openStatus.Id,
                AuditId = 1001,
                SiteId = 1,
                CompanyId = 1,
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = DateTime.UtcNow
            },
            new CustomerPortal.FindingsService.Entities.Finding
            {
                Title = "Non-conforming process execution",
                Description = "Observed deviation from documented process during production line inspection",
                ReferenceNumber = "F002-2025",
                IdentifiedDate = DateTime.UtcNow.AddDays(-5),
                RequiredCompletionDate = DateTime.UtcNow.AddDays(15),
                IdentifiedBy = "Sarah Inspector",
                AssignedTo = "Production Manager",
                Severity = 4,
                Priority = 3,
                CategoryId = procCategory.Id,
                StatusId = openStatus.Id,
                AuditId = 1002,
                SiteId = 1,
                CompanyId = 1,
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = DateTime.UtcNow
            }
        };

        await context.Findings.AddRangeAsync(findings);
        await context.SaveChangesAsync();
    }
}
