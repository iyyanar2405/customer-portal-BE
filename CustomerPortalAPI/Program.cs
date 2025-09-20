using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using CustomerPortalAPI.Data;
using CustomerPortalAPI.Data.Repositories;
using CustomerPortalAPI.Modules.Actions.Repositories;
using CustomerPortalAPI.Modules.Actions.GraphQL;
using CustomerPortalAPI.Modules.Users.Repositories;
using CustomerPortalAPI.Modules.Master.Repositories;
using CustomerPortalAPI.Modules.Audits.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();

// Configure Entity Framework
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Register Repository Dependencies
// Generic repository
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

// Actions module repositories (placeholder - implementation classes will be created later)
// builder.Services.AddScoped<IActionRepository, ActionRepository>();

// Users module repositories (placeholder - implementation classes will be created later)
// builder.Services.AddScoped<IUserRepository, UserRepository>();
// builder.Services.AddScoped<IUserRoleRepository, UserRoleRepository>();

// Master module repositories (placeholder - implementation classes will be created later)  
// builder.Services.AddScoped<ICountryRepository, CountryRepository>();
// builder.Services.AddScoped<ICityRepository, CityRepository>();
// builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();

// Audits module repositories (placeholder - implementation classes will be created later)
// builder.Services.AddScoped<IAuditRepository, AuditRepository>();

// Add Unit of Work (placeholder - implementation class will be created later)
// builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Configure GraphQL
builder.Services
    .AddGraphQLServer()
    .AddQueryType()
    .AddMutationType()
    .AddTypeExtension<ActionQueries>()
    .AddTypeExtension<ActionMutations>();

// Add OpenAPI/Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { 
        Title = "Customer Portal API", 
        Version = "v1",
        Description = "A comprehensive API for customer portal management including actions, audits, certificates, contracts, and more."
    });
    
    // Include XML comments if available
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = System.IO.Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Customer Portal API v1");
        c.RoutePrefix = "swagger";
    });
    
    // Add GraphQL IDE (Banana Cake Pop)
    app.MapBananaCakePop("/graphql-ui");
}

app.UseHttpsRedirection();

// Enable CORS
app.UseCors("AllowAll");

// Add authentication and authorization middleware (when implemented)
// app.UseAuthentication();
// app.UseAuthorization();

// Map GraphQL endpoint
app.MapGraphQL("/graphql");

// Map REST API controllers
app.MapControllers();

// Health check endpoint
app.MapGet("/health", () => new { 
    status = "healthy", 
    timestamp = DateTime.UtcNow,
    version = "1.0.0",
    environment = app.Environment.EnvironmentName
});

// Sample data endpoint for testing
app.MapGet("/", () => new { 
    message = "Customer Portal API is running",
    endpoints = new {
        graphql = "/graphql",
        graphqlIde = "/graphql-ui",
        swagger = "/swagger",
        health = "/health",
        api = "/api"
    }
});

app.Run();
