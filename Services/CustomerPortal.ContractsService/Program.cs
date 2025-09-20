using Microsoft.EntityFrameworkCore;
using CustomerPortal.ContractsService.Data;
using CustomerPortal.ContractsService.Repositories;
using CustomerPortal.ContractsService.GraphQL;
using CustomerPortal.ContractsService.Mappings;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddDbContext<ContractsDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add AutoMapper
builder.Services.AddAutoMapper(typeof(ContractMappingProfile));

// Register repositories
builder.Services.AddScoped<IContractRepository, ContractRepository>();
builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();
builder.Services.AddScoped<IServiceRepository, ServiceRepository>();
builder.Services.AddScoped<ISiteRepository, SiteRepository>();
builder.Services.AddScoped<IContractTermRepository, ContractTermRepository>();
builder.Services.AddScoped<IContractAmendmentRepository, ContractAmendmentRepository>();
builder.Services.AddScoped<IContractRenewalRepository, ContractRenewalRepository>();

// Add GraphQL
builder.Services
    .AddGraphQLServer()
    .AddQueryType<Query>()
    .AddMutationType<Mutation>()
    .AddProjections()
    .AddFiltering()
    .AddSorting();

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
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

app.UseRouting();
app.UseCors();

// Map GraphQL endpoint
app.MapGraphQL("/graphql");

// Create database and seed data
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ContractsDbContext>();
    try
    {
        Console.WriteLine("Creating database...");
        context.Database.EnsureCreated();
        Console.WriteLine("Database created successfully.");
        
        // Check if data already exists
        if (!context.Companies.Any())
        {
            Console.WriteLine("Seeding initial data...");
            // Data will be seeded automatically via the DbContext configuration
            context.SaveChanges();
            Console.WriteLine("Data seeded successfully.");
        }
        else
        {
            Console.WriteLine("Database already contains data.");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error setting up database: {ex.Message}");
    }
}

Console.WriteLine($"Contracts Service is running on {builder.Configuration["Urls"] ?? "http://localhost:6006"}");
Console.WriteLine("GraphQL endpoint: /graphql");
Console.WriteLine("GraphQL Playground: /graphql (in development mode)");

app.Run();
