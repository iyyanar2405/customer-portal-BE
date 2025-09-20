using CustomerPortal.MasterService.Entities;
using CustomerPortal.Shared.Interfaces;

namespace CustomerPortal.MasterService.Repositories.Interfaces
{
    public interface ICountryRepository : IRepository<Country>
    {
        Task<Country?> GetByCountryCodeAsync(string countryCode);
        Task<IEnumerable<Country>> GetByRegionAsync(string region);
    }

    public interface ICityRepository : IRepository<City>
    {
        Task<IEnumerable<City>> GetByCountryIdAsync(int countryId);
        Task<City?> GetByNameAndCountryAsync(string cityName, int countryId);
    }

    public interface ICompanyRepository : IRepository<Company>
    {
        Task<Company?> GetByCompanyCodeAsync(string companyCode);
        Task<IEnumerable<Company>> GetByCountryIdAsync(int countryId);
        Task<IEnumerable<Company>> GetByCompanyTypeAsync(string companyType);
    }

    public interface ISiteRepository : IRepository<Site>
    {
        Task<IEnumerable<Site>> GetByCompanyIdAsync(int companyId);
        Task<Site?> GetBySiteCodeAsync(string siteCode);
        Task<IEnumerable<Site>> GetByCityIdAsync(int cityId);
    }

    public interface IServiceRepository : IRepository<Service>
    {
        Task<Service?> GetByServiceCodeAsync(string serviceCode);
        Task<IEnumerable<Service>> GetByCategoryAsync(string category);
    }

    public interface IRoleRepository : IRepository<Role>
    {
        Task<Role?> GetByRoleNameAsync(string roleName);
    }

    public interface IChapterRepository : IRepository<Chapter>
    {
        Task<Chapter?> GetByChapterNumberAsync(string chapterNumber);
        Task<IEnumerable<Chapter>> GetWithClausesAsync();
    }

    public interface IClauseRepository : IRepository<Clause>
    {
        Task<Clause?> GetByClauseNumberAsync(string clauseNumber);
        Task<IEnumerable<Clause>> GetByChapterIdAsync(int chapterId);
    }

    public interface IFocusAreaRepository : IRepository<FocusArea>
    {
        Task<FocusArea?> GetByNameAsync(string focusAreaName);
    }
}