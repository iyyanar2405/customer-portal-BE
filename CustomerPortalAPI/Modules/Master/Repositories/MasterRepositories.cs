using CustomerPortalAPI.Data;
using CustomerPortalAPI.Data.Repositories;
using CustomerPortalAPI.Modules.Master.Entities;
using Microsoft.EntityFrameworkCore;

namespace CustomerPortalAPI.Modules.Master.Repositories
{
    public class CountryRepository : Repository<Country>, ICountryRepository
    {
        public CountryRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Country?> GetByCountryCodeAsync(string countryCode)
        {
            return await _dbSet.FirstOrDefaultAsync(c => c.CountryCode == countryCode);
        }

        public async Task<IEnumerable<Country>> GetByRegionAsync(string region)
        {
            return await _dbSet.Where(c => c.Region == region).ToListAsync();
        }

        public async Task<IEnumerable<Country>> GetActiveCountriesAsync()
        {
            return await _dbSet.Where(c => c.IsActive).ToListAsync();
        }

        public async Task<IEnumerable<Country>> SearchCountriesAsync(string searchTerm)
        {
            return await _dbSet.Where(c => 
                c.CountryName.Contains(searchTerm) || 
                (c.CountryCode != null && c.CountryCode.Contains(searchTerm)) ||
                (c.Region != null && c.Region.Contains(searchTerm)))
                .ToListAsync();
        }

        public async Task<Country?> GetCountryWithCitiesAsync(int countryId)
        {
            return await _dbSet
                .Include(c => c.Cities)
                .FirstOrDefaultAsync(c => c.Id == countryId);
        }
    }

    public class CityRepository : Repository<City>, ICityRepository
    {
        public CityRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<City>> GetCitiesByCountryAsync(int countryId)
        {
            return await _dbSet.Where(c => c.CountryId == countryId).ToListAsync();
        }

        public async Task<IEnumerable<City>> GetActiveCitiesAsync()
        {
            return await _dbSet.Where(c => c.IsActive).ToListAsync();
        }

        public async Task<IEnumerable<City>> SearchCitiesAsync(string searchTerm)
        {
            return await _dbSet.Where(c => 
                c.CityName.Contains(searchTerm) ||
                (c.StateProvince != null && c.StateProvince.Contains(searchTerm)))
                .ToListAsync();
        }

        public async Task<City?> GetCityWithCountryAsync(int cityId)
        {
            return await _dbSet
                .Include(c => c.Country)
                .FirstOrDefaultAsync(c => c.Id == cityId);
        }

        public async Task<IEnumerable<City>> GetCitiesByStateAsync(string stateProvince)
        {
            return await _dbSet.Where(c => c.StateProvince == stateProvince).ToListAsync();
        }
    }

    public class CompanyRepository : Repository<Company>, ICompanyRepository
    {
        public CompanyRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Company?> GetByCompanyCodeAsync(string companyCode)
        {
            return await _dbSet.FirstOrDefaultAsync(c => c.CompanyCode == companyCode);
        }

        public async Task<IEnumerable<Company>> GetCompaniesByCountryAsync(int countryId)
        {
            return await _dbSet.Where(c => c.CountryId == countryId).ToListAsync();
        }

        public async Task<IEnumerable<Company>> GetCompaniesByTypeAsync(string companyType)
        {
            return await _dbSet.Where(c => c.CompanyType == companyType).ToListAsync();
        }

        public async Task<IEnumerable<Company>> GetActiveCompaniesAsync()
        {
            return await _dbSet.Where(c => c.IsActive).ToListAsync();
        }

        public async Task<IEnumerable<Company>> SearchCompaniesAsync(string searchTerm)
        {
            return await _dbSet.Where(c => 
                c.CompanyName.Contains(searchTerm) ||
                (c.CompanyCode != null && c.CompanyCode.Contains(searchTerm)) ||
                (c.Email != null && c.Email.Contains(searchTerm)))
                .ToListAsync();
        }

        public async Task<Company?> GetCompanyWithSitesAsync(int companyId)
        {
            return await _dbSet
                .Include(c => c.Sites)
                .FirstOrDefaultAsync(c => c.Id == companyId);
        }

        public async Task<Company?> GetCompanyWithDetailsAsync(int companyId)
        {
            return await _dbSet
                .Include(c => c.Country)
                .Include(c => c.Sites)
                .FirstOrDefaultAsync(c => c.Id == companyId);
        }
    }

    public class SiteRepository : Repository<Site>, ISiteRepository
    {
        public SiteRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Site>> GetSitesByCompanyAsync(int companyId)
        {
            return await _dbSet.Where(s => s.CompanyId == companyId).ToListAsync();
        }

        public async Task<IEnumerable<Site>> GetSitesByCityAsync(int cityId)
        {
            return await _dbSet.Where(s => s.CityId == cityId).ToListAsync();
        }

        public async Task<IEnumerable<Site>> GetSitesByTypeAsync(string siteType)
        {
            return await _dbSet.Where(s => s.SiteType == siteType).ToListAsync();
        }

        public async Task<IEnumerable<Site>> GetActiveSitesAsync()
        {
            return await _dbSet.Where(s => s.IsActive).ToListAsync();
        }

        public async Task<IEnumerable<Site>> SearchSitesAsync(string searchTerm)
        {
            return await _dbSet.Where(s => 
                s.SiteName.Contains(searchTerm) ||
                (s.SiteCode != null && s.SiteCode.Contains(searchTerm)) ||
                (s.Address != null && s.Address.Contains(searchTerm)))
                .ToListAsync();
        }

        public async Task<Site?> GetSiteWithDetailsAsync(int siteId)
        {
            return await _dbSet
                .Include(s => s.Company)
                .Include(s => s.City!)
                .ThenInclude(c => c.Country)
                .FirstOrDefaultAsync(s => s.Id == siteId);
        }

        public async Task<Site?> GetBySiteCodeAsync(string siteCode)
        {
            return await _dbSet.FirstOrDefaultAsync(s => s.SiteCode == siteCode);
        }
    }

    public class ServiceRepository : Repository<Service>, IServiceRepository
    {
        public ServiceRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Service?> GetByServiceCodeAsync(string serviceCode)
        {
            return await _dbSet.FirstOrDefaultAsync(s => s.ServiceCode == serviceCode);
        }

        public async Task<IEnumerable<Service>> GetServicesByCategoryAsync(string category)
        {
            return await _dbSet.Where(s => s.ServiceCategory == category).ToListAsync();
        }

        public async Task<IEnumerable<Service>> GetActiveServicesAsync()
        {
            return await _dbSet.Where(s => s.IsActive).ToListAsync();
        }

        public async Task<IEnumerable<Service>> SearchServicesAsync(string searchTerm)
        {
            return await _dbSet.Where(s => 
                s.ServiceName.Contains(searchTerm) ||
                (s.ServiceCode != null && s.ServiceCode.Contains(searchTerm)) ||
                (s.Description != null && s.Description.Contains(searchTerm)))
                .ToListAsync();
        }

        public async Task<Service?> GetServiceWithDetailsAsync(int serviceId)
        {
            return await _dbSet.FirstOrDefaultAsync(s => s.Id == serviceId);
        }
    }

    public class RoleRepository : Repository<Role>, IRoleRepository
    {
        public RoleRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Role?> GetByRoleNameAsync(string roleName)
        {
            return await _dbSet.FirstOrDefaultAsync(r => r.RoleName == roleName);
        }

        public async Task<IEnumerable<Role>> GetActiveRolesAsync()
        {
            return await _dbSet.Where(r => r.IsActive).ToListAsync();
        }

        public async Task<IEnumerable<Role>> SearchRolesAsync(string searchTerm)
        {
            return await _dbSet.Where(r => 
                r.RoleName.Contains(searchTerm) ||
                (r.Description != null && r.Description.Contains(searchTerm)))
                .ToListAsync();
        }

        public async Task<Role?> GetRoleWithPermissionsAsync(int roleId)
        {
            return await _dbSet.FirstOrDefaultAsync(r => r.Id == roleId);
        }

        public async Task<IEnumerable<Role>> GetRolesWithUsersAsync()
        {
            return await _dbSet.Where(r => r.IsActive).ToListAsync();
        }
    }

    public class ChapterRepository : Repository<Chapter>, IChapterRepository
    {
        public ChapterRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Chapter?> GetByChapterNumberAsync(string chapterNumber)
        {
            return await _dbSet.FirstOrDefaultAsync(c => c.ChapterNumber == chapterNumber);
        }

        public async Task<IEnumerable<Chapter>> GetActiveChaptersAsync()
        {
            return await _dbSet.Where(c => c.IsActive).ToListAsync();
        }

        public async Task<IEnumerable<Chapter>> SearchChaptersAsync(string searchTerm)
        {
            return await _dbSet.Where(c => 
                c.ChapterName.Contains(searchTerm) ||
                (c.ChapterNumber != null && c.ChapterNumber.Contains(searchTerm)) ||
                (c.Description != null && c.Description.Contains(searchTerm)))
                .ToListAsync();
        }

        public async Task<Chapter?> GetChapterWithClausesAsync(int chapterId)
        {
            return await _dbSet
                .Include(c => c.Clauses)
                .FirstOrDefaultAsync(c => c.Id == chapterId);
        }
    }

    public class ClauseRepository : Repository<Clause>, IClauseRepository
    {
        public ClauseRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Clause?> GetByClauseNumberAsync(string clauseNumber)
        {
            return await _dbSet.FirstOrDefaultAsync(c => c.ClauseNumber == clauseNumber);
        }

        public async Task<IEnumerable<Clause>> GetClausesByChapterAsync(int chapterId)
        {
            return await _dbSet.Where(c => c.ChapterId == chapterId).ToListAsync();
        }

        public async Task<IEnumerable<Clause>> GetActiveClausesAsync()
        {
            return await _dbSet.Where(c => c.IsActive).ToListAsync();
        }

        public async Task<IEnumerable<Clause>> SearchClausesAsync(string searchTerm)
        {
            return await _dbSet.Where(c => 
                c.ClauseName.Contains(searchTerm) ||
                (c.ClauseNumber != null && c.ClauseNumber.Contains(searchTerm)) ||
                (c.Description != null && c.Description.Contains(searchTerm)))
                .ToListAsync();
        }

        public async Task<Clause?> GetClauseWithChapterAsync(int clauseId)
        {
            return await _dbSet
                .Include(c => c.Chapter)
                .FirstOrDefaultAsync(c => c.Id == clauseId);
        }
    }

    public class FocusAreaRepository : Repository<FocusArea>, IFocusAreaRepository
    {
        public FocusAreaRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<FocusArea>> GetActiveFocusAreasAsync()
        {
            return await _dbSet.Where(f => f.IsActive).ToListAsync();
        }

        public async Task<IEnumerable<FocusArea>> SearchFocusAreasAsync(string searchTerm)
        {
            return await _dbSet.Where(f => 
                f.FocusAreaName.Contains(searchTerm) ||
                (f.Description != null && f.Description.Contains(searchTerm)))
                .ToListAsync();
        }

        public async Task<FocusArea?> GetFocusAreaWithDetailsAsync(int focusAreaId)
        {
            return await _dbSet.FirstOrDefaultAsync(f => f.Id == focusAreaId);
        }
    }
}