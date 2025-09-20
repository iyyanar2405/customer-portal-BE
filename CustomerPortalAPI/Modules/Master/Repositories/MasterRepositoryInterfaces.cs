using CustomerPortalAPI.Data.Repositories;
using CustomerPortalAPI.Modules.Master.Entities;

namespace CustomerPortalAPI.Modules.Master.Repositories
{
    public interface ICountryRepository : IRepository<Country>
    {
        Task<Country?> GetByCountryCodeAsync(string countryCode);
        Task<IEnumerable<Country>> GetByRegionAsync(string region);
        Task<IEnumerable<Country>> GetActiveCountriesAsync();
        Task<IEnumerable<Country>> SearchCountriesAsync(string searchTerm);
        Task<Country?> GetCountryWithCitiesAsync(int countryId);
    }

    public interface ICityRepository : IRepository<City>
    {
        Task<IEnumerable<City>> GetCitiesByCountryAsync(int countryId);
        Task<IEnumerable<City>> GetActiveCitiesAsync();
        Task<IEnumerable<City>> SearchCitiesAsync(string searchTerm);
        Task<City?> GetCityWithCountryAsync(int cityId);
        Task<IEnumerable<City>> GetCitiesByStateAsync(string stateProvince);
    }

    public interface ICompanyRepository : IRepository<Company>
    {
        Task<Company?> GetByCompanyCodeAsync(string companyCode);
        Task<IEnumerable<Company>> GetCompaniesByCountryAsync(int countryId);
        Task<IEnumerable<Company>> GetCompaniesByTypeAsync(string companyType);
        Task<IEnumerable<Company>> GetActiveCompaniesAsync();
        Task<IEnumerable<Company>> SearchCompaniesAsync(string searchTerm);
        Task<Company?> GetCompanyWithSitesAsync(int companyId);
        Task<Company?> GetCompanyWithDetailsAsync(int companyId);
    }

    public interface ISiteRepository : IRepository<Site>
    {
        Task<IEnumerable<Site>> GetSitesByCompanyAsync(int companyId);
        Task<IEnumerable<Site>> GetSitesByCityAsync(int cityId);
        Task<IEnumerable<Site>> GetSitesByTypeAsync(string siteType);
        Task<IEnumerable<Site>> GetActiveSitesAsync();
        Task<IEnumerable<Site>> SearchSitesAsync(string searchTerm);
        Task<Site?> GetSiteWithDetailsAsync(int siteId);
        Task<Site?> GetBySiteCodeAsync(string siteCode);
    }

    public interface IServiceRepository : IRepository<Service>
    {
        Task<Service?> GetByServiceCodeAsync(string serviceCode);
        Task<IEnumerable<Service>> GetServicesByCategoryAsync(string category);
        Task<IEnumerable<Service>> GetActiveServicesAsync();
        Task<IEnumerable<Service>> SearchServicesAsync(string searchTerm);
        Task<Service?> GetServiceWithDetailsAsync(int serviceId);
    }

    public interface IRoleRepository : IRepository<Role>
    {
        Task<Role?> GetByRoleNameAsync(string roleName);
        Task<IEnumerable<Role>> GetActiveRolesAsync();
        Task<IEnumerable<Role>> SearchRolesAsync(string searchTerm);
        Task<Role?> GetRoleWithPermissionsAsync(int roleId);
        Task<IEnumerable<Role>> GetRolesWithUsersAsync();
    }

    public interface IChapterRepository : IRepository<Chapter>
    {
        Task<Chapter?> GetByChapterNumberAsync(string chapterNumber);
        Task<IEnumerable<Chapter>> GetActiveChaptersAsync();
        Task<IEnumerable<Chapter>> SearchChaptersAsync(string searchTerm);
        Task<Chapter?> GetChapterWithClausesAsync(int chapterId);
    }

    public interface IClauseRepository : IRepository<Clause>
    {
        Task<Clause?> GetByClauseNumberAsync(string clauseNumber);
        Task<IEnumerable<Clause>> GetClausesByChapterAsync(int chapterId);
        Task<IEnumerable<Clause>> GetActiveClausesAsync();
        Task<IEnumerable<Clause>> SearchClausesAsync(string searchTerm);
        Task<Clause?> GetClauseWithChapterAsync(int clauseId);
    }

    public interface IFocusAreaRepository : IRepository<FocusArea>
    {
        Task<IEnumerable<FocusArea>> GetActiveFocusAreasAsync();
        Task<IEnumerable<FocusArea>> SearchFocusAreasAsync(string searchTerm);
        Task<FocusArea?> GetFocusAreaWithDetailsAsync(int focusAreaId);
    }
}