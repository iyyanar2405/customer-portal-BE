using CustomerPortal.MasterService.Data;
using CustomerPortal.MasterService.Entities;
using Microsoft.EntityFrameworkCore;

namespace CustomerPortal.MasterService
{
    public class DataSeeder
    {
        public static async Task SeedAsync(MasterDbContext context)
        {
            // Ensure database is created
            await context.Database.EnsureCreatedAsync();

            // Check if data already exists
            if (await context.Countries.AnyAsync())
            {
                return; // Data already seeded
            }

            // Add Countries
            var countries = new[]
            {
                new Country { CountryName = "United States", CountryCode = "US", CurrencyCode = "USD", Region = "North America", IsActive = true, CreatedDate = DateTime.UtcNow, ModifiedDate = DateTime.UtcNow },
                new Country { CountryName = "Canada", CountryCode = "CA", CurrencyCode = "CAD", Region = "North America", IsActive = true, CreatedDate = DateTime.UtcNow, ModifiedDate = DateTime.UtcNow },
                new Country { CountryName = "United Kingdom", CountryCode = "GB", CurrencyCode = "GBP", Region = "Europe", IsActive = true, CreatedDate = DateTime.UtcNow, ModifiedDate = DateTime.UtcNow },
                new Country { CountryName = "Germany", CountryCode = "DE", CurrencyCode = "EUR", Region = "Europe", IsActive = true, CreatedDate = DateTime.UtcNow, ModifiedDate = DateTime.UtcNow },
                new Country { CountryName = "Australia", CountryCode = "AU", CurrencyCode = "AUD", Region = "Oceania", IsActive = true, CreatedDate = DateTime.UtcNow, ModifiedDate = DateTime.UtcNow }
            };

            await context.Countries.AddRangeAsync(countries);
            await context.SaveChangesAsync();

            // Add Cities
            var cities = new[]
            {
                new City { CityName = "New York", CountryId = 1, StateProvince = "NY", PostalCode = "10001", IsActive = true, CreatedDate = DateTime.UtcNow, ModifiedDate = DateTime.UtcNow },
                new City { CityName = "Los Angeles", CountryId = 1, StateProvince = "CA", PostalCode = "90001", IsActive = true, CreatedDate = DateTime.UtcNow, ModifiedDate = DateTime.UtcNow },
                new City { CityName = "Toronto", CountryId = 2, StateProvince = "ON", PostalCode = "M5V", IsActive = true, CreatedDate = DateTime.UtcNow, ModifiedDate = DateTime.UtcNow },
                new City { CityName = "London", CountryId = 3, StateProvince = "England", PostalCode = "SW1A", IsActive = true, CreatedDate = DateTime.UtcNow, ModifiedDate = DateTime.UtcNow },
                new City { CityName = "Berlin", CountryId = 4, StateProvince = "Berlin", PostalCode = "10115", IsActive = true, CreatedDate = DateTime.UtcNow, ModifiedDate = DateTime.UtcNow },
                new City { CityName = "Sydney", CountryId = 5, StateProvince = "NSW", PostalCode = "2000", IsActive = true, CreatedDate = DateTime.UtcNow, ModifiedDate = DateTime.UtcNow }
            };

            await context.Cities.AddRangeAsync(cities);
            await context.SaveChangesAsync();

            // Add Companies
            var companies = new[]
            {
                new Company { CompanyName = "Acme Corporation", CompanyCode = "ACME001", Address = "123 Business St", CountryId = 1, Phone = "+1-555-0123", Email = "contact@acme.com", Website = "https://www.acme.com", ContactPerson = "John Doe", CompanyType = "Manufacturing", IsActive = true, CreatedDate = DateTime.UtcNow, ModifiedDate = DateTime.UtcNow },
                new Company { CompanyName = "TechCorp Ltd", CompanyCode = "TECH001", Address = "456 Tech Ave", CountryId = 1, Phone = "+1-555-0124", Email = "info@techcorp.com", Website = "https://www.techcorp.com", ContactPerson = "Jane Smith", CompanyType = "Technology", IsActive = true, CreatedDate = DateTime.UtcNow, ModifiedDate = DateTime.UtcNow },
                new Company { CompanyName = "Global Industries", CompanyCode = "GLOB001", Address = "789 Global Blvd", CountryId = 3, Phone = "+44-20-7123-4567", Email = "hello@globalind.com", Website = "https://www.globalind.com", ContactPerson = "Mike Johnson", CompanyType = "Consulting", IsActive = true, CreatedDate = DateTime.UtcNow, ModifiedDate = DateTime.UtcNow }
            };

            await context.Companies.AddRangeAsync(companies);
            await context.SaveChangesAsync();

            // Add Services
            var services = new[]
            {
                new Service { ServiceName = "ISO 9001 Audit", ServiceCode = "ISO9001", Description = "Quality Management System Audit", ServiceCategory = "Quality", IsActive = true, CreatedDate = DateTime.UtcNow, ModifiedDate = DateTime.UtcNow },
                new Service { ServiceName = "ISO 14001 Audit", ServiceCode = "ISO14001", Description = "Environmental Management System Audit", ServiceCategory = "Environmental", IsActive = true, CreatedDate = DateTime.UtcNow, ModifiedDate = DateTime.UtcNow },
                new Service { ServiceName = "ISO 45001 Audit", ServiceCode = "ISO45001", Description = "Occupational Health and Safety Management System Audit", ServiceCategory = "Safety", IsActive = true, CreatedDate = DateTime.UtcNow, ModifiedDate = DateTime.UtcNow },
                new Service { ServiceName = "Training Services", ServiceCode = "TRAIN001", Description = "Professional training and development", ServiceCategory = "Training", IsActive = true, CreatedDate = DateTime.UtcNow, ModifiedDate = DateTime.UtcNow }
            };

            await context.Services.AddRangeAsync(services);
            await context.SaveChangesAsync();

            // Add Roles
            var roles = new[]
            {
                new Role { RoleName = "Administrator", Description = "Full system access and control", Permissions = "CREATE,READ,UPDATE,DELETE,ADMIN", IsActive = true, CreatedDate = DateTime.UtcNow, ModifiedDate = DateTime.UtcNow },
                new Role { RoleName = "Auditor", Description = "Audit management and execution", Permissions = "CREATE,READ,UPDATE", IsActive = true, CreatedDate = DateTime.UtcNow, ModifiedDate = DateTime.UtcNow },
                new Role { RoleName = "Client User", Description = "Client portal access", Permissions = "READ,UPDATE", IsActive = true, CreatedDate = DateTime.UtcNow, ModifiedDate = DateTime.UtcNow },
                new Role { RoleName = "Viewer", Description = "Read-only access", Permissions = "READ", IsActive = true, CreatedDate = DateTime.UtcNow, ModifiedDate = DateTime.UtcNow }
            };

            await context.Roles.AddRangeAsync(roles);
            await context.SaveChangesAsync();

            // Add Chapters
            var chapters = new[]
            {
                new Chapter { ChapterName = "Quality Management", ChapterNumber = "4.0", Description = "Quality management system requirements", IsActive = true, CreatedDate = DateTime.UtcNow, ModifiedDate = DateTime.UtcNow },
                new Chapter { ChapterName = "Leadership", ChapterNumber = "5.0", Description = "Leadership and commitment requirements", IsActive = true, CreatedDate = DateTime.UtcNow, ModifiedDate = DateTime.UtcNow },
                new Chapter { ChapterName = "Planning", ChapterNumber = "6.0", Description = "Planning requirements for QMS", IsActive = true, CreatedDate = DateTime.UtcNow, ModifiedDate = DateTime.UtcNow }
            };

            await context.Chapters.AddRangeAsync(chapters);
            await context.SaveChangesAsync();

            // Add Focus Areas
            var focusAreas = new[]
            {
                new FocusArea { FocusAreaName = "Document Control", Description = "Management of documented information", IsActive = true, CreatedDate = DateTime.UtcNow, ModifiedDate = DateTime.UtcNow },
                new FocusArea { FocusAreaName = "Risk Management", Description = "Risk-based thinking and risk management", IsActive = true, CreatedDate = DateTime.UtcNow, ModifiedDate = DateTime.UtcNow },
                new FocusArea { FocusAreaName = "Customer Satisfaction", Description = "Customer focus and satisfaction monitoring", IsActive = true, CreatedDate = DateTime.UtcNow, ModifiedDate = DateTime.UtcNow }
            };

            await context.FocusAreas.AddRangeAsync(focusAreas);
            await context.SaveChangesAsync();
        }
    }
}