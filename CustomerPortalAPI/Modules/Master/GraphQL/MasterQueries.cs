using CustomerPortalAPI.Modules.Master.Repositories;
using CustomerPortalAPI.Modules.Master.GraphQL;
using HotChocolate;

namespace CustomerPortalAPI.Modules.Master.GraphQL
{
    public class MasterQueries
    {
        // Country Queries
        public async Task<IEnumerable<CountryOutput>> GetCountries(
            [Service] ICountryRepository repository,
            MasterFilterInput? filter = null,
            CancellationToken cancellationToken = default)
        {
            var countries = await repository.GetAllAsync();
            
            if (filter != null)
            {
                if (filter.Name != null)
                    countries = countries.Where(c => c.CountryName.Contains(filter.Name, StringComparison.OrdinalIgnoreCase));
                if (filter.IsActive.HasValue)
                    countries = countries.Where(c => c.IsActive == filter.IsActive.Value);
            }
            
            return countries.Select(c => new CountryOutput(
                c.Id,
                c.CountryName,
                c.CountryCode,
                c.CurrencyCode,
                c.Region,
                c.IsActive,
                c.CreatedDate
            ));
        }

        public async Task<CountryOutput?> GetCountryById(
            int id,
            [Service] ICountryRepository repository,
            CancellationToken cancellationToken = default)
        {
            var country = await repository.GetByIdAsync(id);
            if (country == null) return null;

            return new CountryOutput(
                country.Id,
                country.CountryName,
                country.CountryCode,
                country.CurrencyCode,
                country.Region,
                country.IsActive,
                country.CreatedDate
            );
        }

        // City Queries
        public async Task<IEnumerable<CityOutput>> GetCities(
            [Service] ICityRepository repository,
            MasterFilterInput? filter = null)
        {
            var cities = await repository.GetAllAsync();
            
            if (filter != null)
            {
                if (filter.Name != null)
                    cities = cities.Where(c => c.CityName.Contains(filter.Name, StringComparison.OrdinalIgnoreCase));
                if (filter.IsActive.HasValue)
                    cities = cities.Where(c => c.IsActive == filter.IsActive.Value);
            }
            
            return cities.Select(c => new CityOutput(
                c.Id,
                c.CityName,
                c.CountryId,
                c.StateProvince,
                c.PostalCode,
                c.IsActive,
                c.CreatedDate
            ));
        }

        public async Task<CityOutput?> GetCityById(
            int id,
            [Service] ICityRepository repository)
        {
            var city = await repository.GetByIdAsync(id);
            if (city == null) return null;

            return new CityOutput(
                city.Id,
                city.CityName,
                city.CountryId,
                city.StateProvince,
                city.PostalCode,
                city.IsActive,
                city.CreatedDate
            );
        }

        // Company Queries
        public async Task<IEnumerable<CompanyOutput>> GetCompanies(
            [Service] ICompanyRepository repository,
            MasterFilterInput? filter = null)
        {
            var companies = await repository.GetAllAsync();
            
            if (filter != null)
            {
                if (filter.Name != null)
                    companies = companies.Where(c => c.CompanyName.Contains(filter.Name, StringComparison.OrdinalIgnoreCase));
                if (filter.IsActive.HasValue)
                    companies = companies.Where(c => c.IsActive == filter.IsActive.Value);
            }
            
            return companies.Select(c => new CompanyOutput(
                c.Id,
                c.CompanyName,
                c.CompanyCode,
                c.Address,
                c.CountryId,
                c.Phone,
                c.Email,
                c.Website,
                c.ContactPerson,
                c.CompanyType,
                c.IsActive,
                c.CreatedDate
            ));
        }

        public async Task<CompanyOutput?> GetCompanyById(
            int id,
            [Service] ICompanyRepository repository)
        {
            var company = await repository.GetByIdAsync(id);
            if (company == null) return null;

            return new CompanyOutput(
                company.Id,
                company.CompanyName,
                company.CompanyCode,
                company.Address,
                company.CountryId,
                company.Phone,
                company.Email,
                company.Website,
                company.ContactPerson,
                company.CompanyType,
                company.IsActive,
                company.CreatedDate
            );
        }

        // Site Queries
        public async Task<IEnumerable<SiteOutput>> GetSites(
            [Service] ISiteRepository repository,
            MasterFilterInput? filter = null)
        {
            var sites = await repository.GetAllAsync();
            
            if (filter != null)
            {
                if (filter.Name != null)
                    sites = sites.Where(s => s.SiteName.Contains(filter.Name, StringComparison.OrdinalIgnoreCase));
                if (filter.IsActive.HasValue)
                    sites = sites.Where(s => s.IsActive == filter.IsActive.Value);
            }
            
            return sites.Select(s => new SiteOutput(
                s.Id,
                s.SiteName,
                s.SiteCode,
                s.CompanyId,
                s.CityId,
                s.Address,
                s.Phone,
                s.Email,
                s.ContactPerson,
                s.SiteType,
                s.IsActive,
                s.CreatedDate
            ));
        }

        public async Task<SiteOutput?> GetSiteById(
            int id,
            [Service] ISiteRepository repository)
        {
            var site = await repository.GetByIdAsync(id);
            if (site == null) return null;

            return new SiteOutput(
                site.Id,
                site.SiteName,
                site.SiteCode,
                site.CompanyId,
                site.CityId,
                site.Address,
                site.Phone,
                site.Email,
                site.ContactPerson,
                site.SiteType,
                site.IsActive,
                site.CreatedDate
            );
        }

        // Service Queries
        public async Task<IEnumerable<ServiceOutput>> GetServices(
            [Service] IServiceRepository repository,
            MasterFilterInput? filter = null)
        {
            var services = await repository.GetAllAsync();
            
            if (filter != null)
            {
                if (filter.Name != null)
                    services = services.Where(s => s.ServiceName.Contains(filter.Name, StringComparison.OrdinalIgnoreCase));
                if (filter.IsActive.HasValue)
                    services = services.Where(s => s.IsActive == filter.IsActive.Value);
            }
            
            return services.Select(s => new ServiceOutput(
                s.Id,
                s.ServiceName,
                s.ServiceCode,
                s.Description,
                s.ServiceCategory,
                s.IsActive,
                s.CreatedDate
            ));
        }

        public async Task<ServiceOutput?> GetServiceById(
            int id,
            [Service] IServiceRepository repository)
        {
            var service = await repository.GetByIdAsync(id);
            if (service == null) return null;

            return new ServiceOutput(
                service.Id,
                service.ServiceName,
                service.ServiceCode,
                service.Description,
                service.ServiceCategory,
                service.IsActive,
                service.CreatedDate
            );
        }
    }
}