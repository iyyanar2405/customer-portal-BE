using CustomerPortal.ActionsService.Data;
using CustomerPortal.ActionsService.Repositories;
using CustomerPortal.ActionsService.GraphQL;
using CustomerPortal.ActionsService.Mapping;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Database Configuration
builder.Services.AddDbContext<ActionsDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection") ?? 
                        "Server=(localdb)\\MSSQLLocalDB;Database=CustomerPortal_ActionsService;Trusted_Connection=True;MultipleActiveResultSets=true"));

// AutoMapper Configuration
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

// Repository Registration
builder.Services.AddScoped<IActionRepository, ActionRepository>();
builder.Services.AddScoped<IActionTypeRepository, ActionTypeRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ITeamRepository, TeamRepository>();
builder.Services.AddScoped<IActionDependencyRepository, ActionDependencyRepository>();
builder.Services.AddScoped<IActionAttachmentRepository, ActionAttachmentRepository>();
builder.Services.AddScoped<IActionCommentRepository, ActionCommentRepository>();
builder.Services.AddScoped<IActionHistoryRepository, ActionHistoryRepository>();
builder.Services.AddScoped<IActionTemplateRepository, ActionTemplateRepository>();
builder.Services.AddScoped<IWorkflowRepository, WorkflowRepository>();
builder.Services.AddScoped<IWorkflowStepRepository, WorkflowStepRepository>();
builder.Services.AddScoped<IWorkflowInstanceRepository, WorkflowInstanceRepository>();

// GraphQL Configuration
builder.Services
    .AddGraphQLServer()
    .AddQueryType<Query>()
    .AddMutationType<Mutation>()
    .AddFiltering()
    .AddSorting()
    .AddProjections();

// CORS Configuration
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Ensure database is created and seeded
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ActionsDbContext>();
    context.Database.EnsureCreated();
    await context.SeedDataAsync();
}

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Map GraphQL endpoint
app.MapGraphQL("/graphql");

Console.WriteLine("🚀 CustomerPortal Actions Service starting...");
Console.WriteLine("📊 GraphQL endpoint: http://localhost:5001/graphql");
Console.WriteLine("📚 Swagger UI: http://localhost:5001/swagger");

app.Run();
