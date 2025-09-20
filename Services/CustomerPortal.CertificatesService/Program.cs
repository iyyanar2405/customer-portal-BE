using CustomerPortal.CertificatesService.Data;
using CustomerPortal.CertificatesService.GraphQL;
using CustomerPortal.CertificatesService.Mappings;
using CustomerPortal.CertificatesService.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();

// Configure Entity Framework
builder.Services.AddDbContext<CertificatesDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register repositories
builder.Services.AddScoped<ICertificateRepository, CertificateRepository>();
builder.Services.AddScoped<ICertificateTypeRepository, CertificateTypeRepository>();
builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
// Temporarily commented out until interface methods are fully implemented
// builder.Services.AddScoped<ISiteRepository, SiteRepository>();
// builder.Services.AddScoped<IServiceRepository, ServiceRepository>();
// builder.Services.AddScoped<IAuditRepository, AuditRepository>();
// builder.Services.AddScoped<ICertificateRenewalRepository, CertificateRenewalRepository>();
// builder.Services.AddScoped<ICertificateValidationRepository, CertificateValidationRepository>();
// builder.Services.AddScoped<ICertificateDocumentRepository, CertificateDocumentRepository>();
// builder.Services.AddScoped<ICertificateSiteRepository, CertificateSiteRepository>();
// builder.Services.AddScoped<ICertificateServiceRepository, CertificateServiceRepository>();
// builder.Services.AddScoped<IAuditSiteAuditRepository, AuditSiteAuditRepository>();
// builder.Services.AddScoped<IAuditServiceRepository, AuditServiceRepository>();
// builder.Services.AddScoped<ICountryRepository, CountryRepository>();
// builder.Services.AddScoped<ICityRepository, CityRepository>();

// Configure AutoMapper
builder.Services.AddAutoMapper(typeof(CertificateMappingProfile));

// Configure GraphQL
builder.Services
    .AddGraphQLServer()
    .AddQueryType<Query>()
    .AddMutationType<Mutation>()
    .AddProjections()
    .AddFiltering()
    .AddSorting();

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

// Map GraphQL endpoint
app.MapGraphQL("/graphql");

// Create database if it doesn't exist and apply migrations
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<CertificatesDbContext>();
    try
    {
        context.Database.EnsureCreated();
        Console.WriteLine("Database created successfully.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Database creation failed: {ex.Message}");
    }
}

Console.WriteLine("Certificate Service is running on port 5005");
Console.WriteLine("GraphQL endpoint: https://localhost:5005/graphql");
Console.WriteLine("GraphQL Playground (Banana Cake Pop): https://localhost:5005/graphql");

app.Run();
