using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using CustomerPortalAPI.Data;
using CustomerPortalAPI.Data.Repositories;
using CustomerPortalAPI.Modules.Actions.Repositories;
using CustomerPortalAPI.Modules.Actions.GraphQL;
using CustomerPortalAPI.Modules.Users.Repositories;
using CustomerPortalAPI.Modules.Master.Repositories;
using CustomerPortalAPI.Modules.Master.GraphQL;
using CustomerPortalAPI.Modules.Audits.Repositories;
using CustomerPortalAPI.Modules.Audits.GraphQL;
using CustomerPortalAPI.Modules.Users.GraphQL;
using CustomerPortalAPI.Modules.Certificates.Repositories;
using CustomerPortalAPI.Modules.Certificates.GraphQL;
using CustomerPortalAPI.Modules.Contracts.Repositories;
using CustomerPortalAPI.Modules.Financial.Repositories;
using CustomerPortalAPI.Modules.Findings.Repositories;
using CustomerPortalAPI.Modules.Notifications.Repositories;
using CustomerPortalAPI.Modules.Settings.Repositories;
using CustomerPortalAPI.Modules.Settings.GraphQL;
using CustomerPortalAPI.Modules.Overview.Repositories;
using CustomerPortalAPI.Modules.Overview.GraphQL;
using CustomerPortalAPI.Modules.Widgets.Repositories;
using CustomerPortalAPI.Modules.Widgets.GraphQL;
using CustomerPortalAPI.Modules.Financial.GraphQL;
using CustomerPortalAPI.Modules.Contracts.GraphQL;
using CustomerPortalAPI.Modules.Findings.GraphQL;
using CustomerPortalAPI.Modules.Notifications.GraphQL;

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

// Actions module repositories
builder.Services.AddScoped<IActionRepository, ActionRepository>();

// Audits module repositories
builder.Services.AddScoped<IAuditRepository, AuditRepository>();
builder.Services.AddScoped<IAuditTypeRepository, AuditTypeRepository>();
builder.Services.AddScoped<IAuditServiceRepository, AuditServiceRepository>();
builder.Services.AddScoped<IAuditTeamMemberRepository, AuditTeamMemberRepository>();
builder.Services.AddScoped<IAuditSiteRepository, AuditSiteRepository>();
builder.Services.AddScoped<IAuditLogRepository, AuditLogRepository>();

// Master module repositories  
builder.Services.AddScoped<ICountryRepository, CountryRepository>();
builder.Services.AddScoped<ICityRepository, CityRepository>();
builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();
builder.Services.AddScoped<ISiteRepository, SiteRepository>();
builder.Services.AddScoped<IServiceRepository, ServiceRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IChapterRepository, ChapterRepository>();
builder.Services.AddScoped<IClauseRepository, ClauseRepository>();
builder.Services.AddScoped<IFocusAreaRepository, FocusAreaRepository>();

// Users module repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserRoleRepository, UserRoleRepository>();
builder.Services.AddScoped<IUserCompanyAccessRepository, UserCompanyAccessRepository>();
builder.Services.AddScoped<IUserSiteAccessRepository, UserSiteAccessRepository>();
builder.Services.AddScoped<IUserServiceAccessRepository, UserServiceAccessRepository>();
builder.Services.AddScoped<IUserCityAccessRepository, UserCityAccessRepository>();
builder.Services.AddScoped<IUserCountryAccessRepository, UserCountryAccessRepository>();
builder.Services.AddScoped<IUserNotificationAccessRepository, UserNotificationAccessRepository>();
builder.Services.AddScoped<IUserPreferenceRepository, UserPreferenceRepository>();
builder.Services.AddScoped<IUserTrainingRepository, UserTrainingRepository>();

// Certificates module repositories
builder.Services.AddScoped<ICertificateRepository, CertificateRepository>();
builder.Services.AddScoped<ICertificateServiceRepository, CertificateServiceRepository>();
builder.Services.AddScoped<ICertificateSiteRepository, CertificateSiteRepository>();
builder.Services.AddScoped<ICertificateAdditionalScopeRepository, CertificateAdditionalScopeRepository>();

// Contracts module repositories
builder.Services.AddScoped<IContractRepository, ContractRepository>();
builder.Services.AddScoped<IContractServiceRepository, ContractServiceRepository>();
builder.Services.AddScoped<IContractSiteRepository, ContractSiteRepository>();

// Financial module repositories
builder.Services.AddScoped<IFinancialRepository, FinancialRepository>();
builder.Services.AddScoped<IInvoiceRepository, InvoiceRepository>();

// Findings module repositories
builder.Services.AddScoped<IFindingRepository, FindingRepository>();
builder.Services.AddScoped<IFindingCategoryRepository, FindingCategoryRepository>();
builder.Services.AddScoped<IFindingStatusRepository, FindingStatusRepository>();

// Notifications module repositories
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<INotificationCategoryRepository, NotificationCategoryRepository>();

// Settings module repositories
builder.Services.AddScoped<ITrainingRepository, TrainingRepository>();
builder.Services.AddScoped<IErrorLogRepository, ErrorLogRepository>();

// Overview module repositories
builder.Services.AddScoped<IOverviewDashboardRepository, OverviewDashboardRepository>();
builder.Services.AddScoped<IOverviewMetricRepository, OverviewMetricRepository>();
builder.Services.AddScoped<IOverviewReportRepository, OverviewReportRepository>();

// Widgets module repositories
builder.Services.AddScoped<IWidgetRepository, WidgetRepository>();
builder.Services.AddScoped<IWidgetCategoryRepository, WidgetCategoryRepository>();
builder.Services.AddScoped<IWidgetUserAccessRepository, WidgetUserAccessRepository>();
builder.Services.AddScoped<IWidgetDataRepository, WidgetDataRepository>();

// Overview and Widgets modules (implementation classes will be created later)
// builder.Services.AddScoped<IOverviewRepository, OverviewRepository>();
// builder.Services.AddScoped<IWidgetRepository, WidgetRepository>();

// Add Unit of Work (implementation class will be created later)
// builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Configure GraphQL
builder.Services
    .AddGraphQLServer()
    .AddQueryType()
    .AddMutationType()
    .AddTypeExtension<ActionQueries>()
    .AddTypeExtension<ActionMutations>()
    .AddTypeExtension<AuditMutations>()
    .AddTypeExtension<MasterQueries>()
    .AddTypeExtension<MasterMutations>()
    .AddTypeExtension<UserQueries>()
    .AddTypeExtension<UserMutations>()
    .AddTypeExtension<CertificateQueries>()
    .AddTypeExtension<CertificateMutations>()
    .AddTypeExtension<FinancialQueries>()
    .AddTypeExtension<FinancialMutations>()
    .AddTypeExtension<ContractsQueries>()
    .AddTypeExtension<ContractsMutations>()
    .AddTypeExtension<FindingsQueries>()
    .AddTypeExtension<FindingsMutations>()
    .AddTypeExtension<NotificationsQueries>()
    .AddTypeExtension<NotificationsMutations>()
    .AddTypeExtension<SettingsQueries>()
    .AddTypeExtension<SettingsMutations>()
    .AddTypeExtension<OverviewQueries>()
    .AddTypeExtension<WidgetsQueries>()
    .AddTypeExtension<WidgetsMutations>();

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
