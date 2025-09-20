using CustomerPortal.AuditsService.Data;
using CustomerPortal.AuditsService.Entities;
using CustomerPortal.AuditsService.GraphQL.Resolvers;
using CustomerPortal.AuditsService.GraphQL.Types;
using CustomerPortal.AuditsService.Repositories;
using CustomerPortal.Shared.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddDbContext<AuditsDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
        ?? "Server=(localdb)\\mssqllocaldb;Database=CustomerPortalAuditsDB;Trusted_Connection=true;MultipleActiveResultSets=true";
    options.UseSqlServer(connectionString);
});

// Register repositories
builder.Services.AddScoped<IAuditRepository, AuditRepository>();
builder.Services.AddScoped<IAuditTeamMemberRepository, AuditTeamMemberRepository>();
builder.Services.AddScoped<IAuditSiteRepository, AuditSiteRepository>();
builder.Services.AddScoped<IRepository<Company>, Repository<Company>>();
builder.Services.AddScoped<IRepository<User>, Repository<User>>();
builder.Services.AddScoped<IRepository<Site>, Repository<Site>>();
builder.Services.AddScoped<IRepository<Service>, Repository<Service>>();
builder.Services.AddScoped<IRepository<CustomerPortal.AuditsService.Entities.AuditType>, Repository<CustomerPortal.AuditsService.Entities.AuditType>>();
builder.Services.AddScoped<IRepository<AuditService>, Repository<AuditService>>();
builder.Services.AddScoped<IRepository<AuditLog>, Repository<AuditLog>>();
builder.Services.AddScoped<IRepository<AuditSiteAudit>, Repository<AuditSiteAudit>>();

// Add GraphQL services
builder.Services
    .AddGraphQLServer()
    .AddQueryType<Query>()
    .AddMutationType<Mutation>()
    .AddType<CustomerPortal.AuditsService.GraphQL.Types.AuditType>()
    .AddType<CompanyType>()
    .AddType<UserType>()
    .AddType<SiteType>()
    .AddType<ServiceType>()
    .AddType<AuditTypeType>()
    .AddType<AuditSiteType>()
    .AddType<AuditServiceType>()
    .AddType<AuditTeamMemberType>()
    .AddType<AuditLogType>()
    .AddType<AuditSiteAuditType>()
    .AddProjections()
    .AddFiltering()
    .AddSorting();

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseCors();
app.UseRouting();

// Map GraphQL endpoint
app.MapGraphQL("/graphql");

// Ensure database is created and seeded
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AuditsDbContext>();
    
    try
    {
        await context.Database.EnsureCreatedAsync();
        Console.WriteLine("Database ensured created successfully.");
        
        await context.SeedDataAsync();
        Console.WriteLine("Sample data seeded successfully.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error setting up database: {ex.Message}");
    }
}

Console.WriteLine($"CustomerPortal.AuditsService is running at: {builder.Configuration["urls"] ?? "http://localhost:5002"}");
Console.WriteLine("GraphQL endpoint available at: /graphql");

app.Run();
