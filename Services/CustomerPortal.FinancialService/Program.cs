using Microsoft.EntityFrameworkCore;
using CustomerPortal.FinancialService.Data;
using CustomerPortal.FinancialService.Repositories;
using CustomerPortal.FinancialService.GraphQL;

var builder = WebApplication.CreateBuilder(args);

// Add database context
builder.Services.AddDbContext<FinancialDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add AutoMapper
builder.Services.AddAutoMapper(typeof(FinancialMappingProfile));

// Add repositories
builder.Services.AddScoped<IInvoiceRepository, InvoiceRepository>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();
builder.Services.AddScoped<IServiceRepository, ServiceRepository>();
builder.Services.AddScoped<ITaxRateRepository, TaxRateRepository>();
builder.Services.AddScoped<IFinancialReportingRepository, FinancialReportingRepository>();

// Add GraphQL
builder.Services
    .AddGraphQLServer()
    .AddQueryType<Query>()
    .AddMutationType<Mutation>()
    .AddProjections()
    .AddFiltering()
    .AddSorting();

var app = builder.Build();

// Ensure database is created and seeded
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<FinancialDbContext>();
    try
    {
        Console.WriteLine("Creating database...");
        await context.Database.EnsureCreatedAsync();
        
        // Check if database has data
        var hasData = await context.Companies.AnyAsync();
        if (hasData)
        {
            Console.WriteLine("Database already contains data.");
        }
        else
        {
            Console.WriteLine("Database created and seeded successfully.");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error setting up database: {ex.Message}");
    }
}

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseRouting();

app.MapGraphQL();

Console.WriteLine("Financial Service is running on http://localhost:5007");
Console.WriteLine("GraphQL endpoint: /graphql");
Console.WriteLine("GraphQL Playground: /graphql (in development mode)");

app.Run();
