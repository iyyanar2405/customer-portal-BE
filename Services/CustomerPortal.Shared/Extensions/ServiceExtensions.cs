using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace CustomerPortal.Shared.Extensions
{
    /// <summary>
    /// Extension methods for service configuration
    /// </summary>
    public static class ServiceExtensions
    {
        /// <summary>
        /// Configure database context for a service
        /// </summary>
        public static IServiceCollection AddDatabaseContext<TContext>(
            this IServiceCollection services, 
            IConfiguration configuration,
            string connectionStringName = "DefaultConnection") 
            where TContext : DbContext
        {
            services.AddDbContext<TContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString(connectionStringName)));

            return services;
        }

        /// <summary>
        /// Configure repository pattern
        /// </summary>
        public static IServiceCollection AddRepositoryPattern<TRepository, TImplementation>(
            this IServiceCollection services)
            where TRepository : class
            where TImplementation : class, TRepository
        {
            services.AddScoped<TRepository, TImplementation>();
            return services;
        }
    }
}