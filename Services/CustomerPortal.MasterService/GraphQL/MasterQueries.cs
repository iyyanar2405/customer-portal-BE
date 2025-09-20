using CustomerPortal.MasterService.Entities;
using CustomerPortal.MasterService.GraphQL.Types;
using CustomerPortal.MasterService.Repositories.Interfaces;

namespace CustomerPortal.MasterService.GraphQL
{
    public class MasterQueries
    {
        // Country Queries
        public async Task<IEnumerable<CountryType>> GetCountriesAsync(
            [Service] ICountryRepository countryRepository)
        {
            var countries = await countryRepository.GetAllAsync();
            return countries.Select(MapToCountryType);
        }

        public async Task<CountryType?> GetCountryByIdAsync(
            int id,
            [Service] ICountryRepository countryRepository)
        {
            var country = await countryRepository.GetByIdAsync(id);
            return country != null ? MapToCountryType(country) : null;
        }

        public async Task<CountryType?> GetCountryByCodeAsync(
            string countryCode,
            [Service] ICountryRepository countryRepository)
        {
            var country = await countryRepository.GetByCountryCodeAsync(countryCode);
            return country != null ? MapToCountryType(country) : null;
        }

        public async Task<IEnumerable<CountryType>> GetCountriesByRegionAsync(
            string region,
            [Service] ICountryRepository countryRepository)
        {
            var countries = await countryRepository.GetByRegionAsync(region);
            return countries.Select(MapToCountryType);
        }

        // City Queries
        public async Task<IEnumerable<CityType>> GetCitiesAsync(
            [Service] ICityRepository cityRepository)
        {
            var cities = await cityRepository.GetAllAsync();
            return cities.Select(MapToCityType);
        }

        public async Task<CityType?> GetCityByIdAsync(
            int id,
            [Service] ICityRepository cityRepository)
        {
            var city = await cityRepository.GetByIdAsync(id);
            return city != null ? MapToCityType(city) : null;
        }

        public async Task<IEnumerable<CityType>> GetCitiesByCountryAsync(
            int countryId,
            [Service] ICityRepository cityRepository)
        {
            var cities = await cityRepository.GetByCountryIdAsync(countryId);
            return cities.Select(MapToCityType);
        }

        // Company Queries
        public async Task<IEnumerable<CompanyType>> GetCompaniesAsync(
            [Service] ICompanyRepository companyRepository)
        {
            var companies = await companyRepository.GetAllAsync();
            return companies.Select(MapToCompanyType);
        }

        public async Task<CompanyType?> GetCompanyByIdAsync(
            int id,
            [Service] ICompanyRepository companyRepository)
        {
            var company = await companyRepository.GetByIdAsync(id);
            return company != null ? MapToCompanyType(company) : null;
        }

        public async Task<CompanyType?> GetCompanyByCodeAsync(
            string companyCode,
            [Service] ICompanyRepository companyRepository)
        {
            var company = await companyRepository.GetByCompanyCodeAsync(companyCode);
            return company != null ? MapToCompanyType(company) : null;
        }

        // Site Queries
        public async Task<IEnumerable<SiteType>> GetSitesAsync(
            [Service] ISiteRepository siteRepository)
        {
            var sites = await siteRepository.GetAllAsync();
            return sites.Select(MapToSiteType);
        }

        public async Task<SiteType?> GetSiteByIdAsync(
            int id,
            [Service] ISiteRepository siteRepository)
        {
            var site = await siteRepository.GetByIdAsync(id);
            return site != null ? MapToSiteType(site) : null;
        }

        public async Task<IEnumerable<SiteType>> GetSitesByCompanyAsync(
            int companyId,
            [Service] ISiteRepository siteRepository)
        {
            var sites = await siteRepository.GetByCompanyIdAsync(companyId);
            return sites.Select(MapToSiteType);
        }

        // Service Queries
        public async Task<IEnumerable<ServiceType>> GetServicesAsync(
            [Service] IServiceRepository serviceRepository)
        {
            var services = await serviceRepository.GetAllAsync();
            return services.Select(MapToServiceType);
        }

        public async Task<ServiceType?> GetServiceByIdAsync(
            int id,
            [Service] IServiceRepository serviceRepository)
        {
            var service = await serviceRepository.GetByIdAsync(id);
            return service != null ? MapToServiceType(service) : null;
        }

        // Role Queries
        public async Task<IEnumerable<RoleType>> GetRolesAsync(
            [Service] IRoleRepository roleRepository)
        {
            var roles = await roleRepository.GetAllAsync();
            return roles.Select(MapToRoleType);
        }

        public async Task<RoleType?> GetRoleByIdAsync(
            int id,
            [Service] IRoleRepository roleRepository)
        {
            var role = await roleRepository.GetByIdAsync(id);
            return role != null ? MapToRoleType(role) : null;
        }

        // Chapter Queries
        public async Task<IEnumerable<ChapterType>> GetChaptersAsync(
            [Service] IChapterRepository chapterRepository)
        {
            var chapters = await chapterRepository.GetAllAsync();
            return chapters.Select(MapToChapterType);
        }

        public async Task<ChapterType?> GetChapterByIdAsync(
            int id,
            [Service] IChapterRepository chapterRepository)
        {
            var chapter = await chapterRepository.GetByIdAsync(id);
            return chapter != null ? MapToChapterType(chapter) : null;
        }

        // Clause Queries
        public async Task<IEnumerable<ClauseType>> GetClausesAsync(
            [Service] IClauseRepository clauseRepository)
        {
            var clauses = await clauseRepository.GetAllAsync();
            return clauses.Select(MapToClauseType);
        }

        public async Task<ClauseType?> GetClauseByIdAsync(
            int id,
            [Service] IClauseRepository clauseRepository)
        {
            var clause = await clauseRepository.GetByIdAsync(id);
            return clause != null ? MapToClauseType(clause) : null;
        }

        // FocusArea Queries
        public async Task<IEnumerable<FocusAreaType>> GetFocusAreasAsync(
            [Service] IFocusAreaRepository focusAreaRepository)
        {
            var focusAreas = await focusAreaRepository.GetAllAsync();
            return focusAreas.Select(MapToFocusAreaType);
        }

        public async Task<FocusAreaType?> GetFocusAreaByIdAsync(
            int id,
            [Service] IFocusAreaRepository focusAreaRepository)
        {
            var focusArea = await focusAreaRepository.GetByIdAsync(id);
            return focusArea != null ? MapToFocusAreaType(focusArea) : null;
        }

        // Mapping methods
        private static CountryType MapToCountryType(Country country)
        {
            return new CountryType
            {
                Id = country.Id,
                CountryName = country.CountryName,
                CountryCode = country.CountryCode,
                CurrencyCode = country.CurrencyCode,
                Region = country.Region,
                IsActive = country.IsActive,
                CreatedDate = country.CreatedDate,
                ModifiedDate = country.ModifiedDate,
                Cities = country.Cities?.Select(MapToCityType)
            };
        }

        private static CityType MapToCityType(City city)
        {
            return new CityType
            {
                Id = city.Id,
                CityName = city.CityName,
                CountryId = city.CountryId,
                StateProvince = city.StateProvince,
                PostalCode = city.PostalCode,
                IsActive = city.IsActive,
                CreatedDate = city.CreatedDate,
                ModifiedDate = city.ModifiedDate,
                Country = city.Country != null ? MapToCountryType(city.Country) : null
            };
        }

        private static CompanyType MapToCompanyType(Company company)
        {
            return new CompanyType
            {
                Id = company.Id,
                CompanyName = company.CompanyName,
                CompanyCode = company.CompanyCode,
                Address = company.Address,
                CountryId = company.CountryId,
                Phone = company.Phone,
                Email = company.Email,
                Website = company.Website,
                ContactPerson = company.ContactPerson,
                CompanyTypeName = company.CompanyType,
                IsActive = company.IsActive,
                CreatedDate = company.CreatedDate,
                ModifiedDate = company.ModifiedDate,
                Country = company.Country != null ? MapToCountryType(company.Country) : null,
                Sites = company.Sites?.Select(MapToSiteType)
            };
        }

        private static SiteType MapToSiteType(Site site)
        {
            return new SiteType
            {
                Id = site.Id,
                SiteName = site.SiteName,
                SiteCode = site.SiteCode,
                CompanyId = site.CompanyId,
                CityId = site.CityId,
                Address = site.Address,
                Phone = site.Phone,
                Email = site.Email,
                ContactPerson = site.ContactPerson,
                SiteTypeName = site.SiteType,
                IsActive = site.IsActive,
                CreatedDate = site.CreatedDate,
                ModifiedDate = site.ModifiedDate,
                Company = site.Company != null ? MapToCompanyType(site.Company) : null,
                City = site.City != null ? MapToCityType(site.City) : null
            };
        }

        private static ServiceType MapToServiceType(Service service)
        {
            return new ServiceType
            {
                Id = service.Id,
                ServiceName = service.ServiceName,
                ServiceCode = service.ServiceCode,
                Description = service.Description,
                ServiceCategory = service.ServiceCategory,
                IsActive = service.IsActive,
                CreatedDate = service.CreatedDate,
                ModifiedDate = service.ModifiedDate
            };
        }

        private static RoleType MapToRoleType(Role role)
        {
            return new RoleType
            {
                Id = role.Id,
                RoleName = role.RoleName,
                Description = role.Description,
                Permissions = role.Permissions,
                IsActive = role.IsActive,
                CreatedDate = role.CreatedDate,
                ModifiedDate = role.ModifiedDate
            };
        }

        private static ChapterType MapToChapterType(Chapter chapter)
        {
            return new ChapterType
            {
                Id = chapter.Id,
                ChapterName = chapter.ChapterName,
                ChapterNumber = chapter.ChapterNumber,
                Description = chapter.Description,
                IsActive = chapter.IsActive,
                CreatedDate = chapter.CreatedDate,
                ModifiedDate = chapter.ModifiedDate,
                Clauses = chapter.Clauses?.Select(MapToClauseType)
            };
        }

        private static ClauseType MapToClauseType(Clause clause)
        {
            return new ClauseType
            {
                Id = clause.Id,
                ClauseName = clause.ClauseName,
                ClauseNumber = clause.ClauseNumber,
                ChapterId = clause.ChapterId,
                Description = clause.Description,
                IsActive = clause.IsActive,
                CreatedDate = clause.CreatedDate,
                ModifiedDate = clause.ModifiedDate,
                Chapter = clause.Chapter != null ? MapToChapterType(clause.Chapter) : null
            };
        }

        private static FocusAreaType MapToFocusAreaType(FocusArea focusArea)
        {
            return new FocusAreaType
            {
                Id = focusArea.Id,
                FocusAreaName = focusArea.FocusAreaName,
                Description = focusArea.Description,
                IsActive = focusArea.IsActive,
                CreatedDate = focusArea.CreatedDate,
                ModifiedDate = focusArea.ModifiedDate
            };
        }
    }
}