using CustomerPortal.MasterService.Data;
using CustomerPortal.MasterService.GraphQL;
using CustomerPortal.MasterService.Repositories;
using CustomerPortal.MasterService.Repositories.Interfaces;
using CustomerPortal.Shared.Extensions;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();

// Configure database
builder.Services.AddDatabaseContext<MasterDbContext>(builder.Configuration);

// Register repositories
builder.Services.AddRepositoryPattern<ICountryRepository, CountryRepository>();
builder.Services.AddRepositoryPattern<ICityRepository, CityRepository>();
builder.Services.AddRepositoryPattern<ICompanyRepository, CompanyRepository>();
builder.Services.AddRepositoryPattern<ISiteRepository, SiteRepository>();
builder.Services.AddRepositoryPattern<IServiceRepository, ServiceRepository>();
builder.Services.AddRepositoryPattern<IRoleRepository, RoleRepository>();
builder.Services.AddRepositoryPattern<IChapterRepository, ChapterRepository>();
builder.Services.AddRepositoryPattern<IClauseRepository, ClauseRepository>();
builder.Services.AddRepositoryPattern<IFocusAreaRepository, FocusAreaRepository>();

// Configure GraphQL
builder.Services
    .AddGraphQLServer()
    .AddQueryType<MasterQueries>()
    .AddFiltering()
    .AddSorting();

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

// Add Swagger/OpenAPI
builder.Services.AddOpenApi();

var app = builder.Build();

// Seed database with sample data
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<MasterDbContext>();
    await CustomerPortal.MasterService.DataSeeder.SeedAsync(context);
}

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// Use CORS
app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

// Map GraphQL endpoint
app.MapGraphQL("/graphql");

app.Run();
