using CustomerPortal.MasterService.Data;
using CustomerPortal.MasterService.Entities;
using CustomerPortal.MasterService.Repositories.Interfaces;
using CustomerPortal.Shared.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CustomerPortal.MasterService.Repositories
{
    public class CountryRepository : BaseRepository<Country>, ICountryRepository
    {
        public CountryRepository(MasterDbContext context) : base(context)
        {
        }

        public async Task<Country?> GetByCountryCodeAsync(string countryCode)
        {
            return await _context.Set<Country>()
                .FirstOrDefaultAsync(c => c.CountryCode == countryCode);
        }

        public async Task<IEnumerable<Country>> GetByRegionAsync(string region)
        {
            return await _context.Set<Country>()
                .Where(c => c.Region == region)
                .ToListAsync();
        }
    }

    public class CityRepository : BaseRepository<City>, ICityRepository
    {
        public CityRepository(MasterDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<City>> GetByCountryIdAsync(int countryId)
        {
            return await _context.Set<City>()
                .Include(c => c.Country)
                .Where(c => c.CountryId == countryId)
                .ToListAsync();
        }

        public async Task<City?> GetByNameAndCountryAsync(string cityName, int countryId)
        {
            return await _context.Set<City>()
                .Include(c => c.Country)
                .FirstOrDefaultAsync(c => c.CityName == cityName && c.CountryId == countryId);
        }
    }

    public class CompanyRepository : BaseRepository<Company>, ICompanyRepository
    {
        public CompanyRepository(MasterDbContext context) : base(context)
        {
        }

        public async Task<Company?> GetByCompanyCodeAsync(string companyCode)
        {
            return await _context.Set<Company>()
                .Include(c => c.Country)
                .Include(c => c.Sites)
                .FirstOrDefaultAsync(c => c.CompanyCode == companyCode);
        }

        public async Task<IEnumerable<Company>> GetByCountryIdAsync(int countryId)
        {
            return await _context.Set<Company>()
                .Include(c => c.Country)
                .Where(c => c.CountryId == countryId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Company>> GetByCompanyTypeAsync(string companyType)
        {
            return await _context.Set<Company>()
                .Include(c => c.Country)
                .Where(c => c.CompanyType == companyType)
                .ToListAsync();
        }
    }

    public class SiteRepository : BaseRepository<Site>, ISiteRepository
    {
        public SiteRepository(MasterDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Site>> GetByCompanyIdAsync(int companyId)
        {
            return await _context.Set<Site>()
                .Include(s => s.Company)
                .Include(s => s.City)
                .Where(s => s.CompanyId == companyId)
                .ToListAsync();
        }

        public async Task<Site?> GetBySiteCodeAsync(string siteCode)
        {
            return await _context.Set<Site>()
                .Include(s => s.Company)
                .Include(s => s.City)
                .FirstOrDefaultAsync(s => s.SiteCode == siteCode);
        }

        public async Task<IEnumerable<Site>> GetByCityIdAsync(int cityId)
        {
            return await _context.Set<Site>()
                .Include(s => s.Company)
                .Include(s => s.City)
                .Where(s => s.CityId == cityId)
                .ToListAsync();
        }
    }

    public class ServiceRepository : BaseRepository<Service>, IServiceRepository
    {
        public ServiceRepository(MasterDbContext context) : base(context)
        {
        }

        public async Task<Service?> GetByServiceCodeAsync(string serviceCode)
        {
            return await _context.Set<Service>()
                .FirstOrDefaultAsync(s => s.ServiceCode == serviceCode);
        }

        public async Task<IEnumerable<Service>> GetByCategoryAsync(string category)
        {
            return await _context.Set<Service>()
                .Where(s => s.ServiceCategory == category)
                .ToListAsync();
        }
    }

    public class RoleRepository : BaseRepository<Role>, IRoleRepository
    {
        public RoleRepository(MasterDbContext context) : base(context)
        {
        }

        public async Task<Role?> GetByRoleNameAsync(string roleName)
        {
            return await _context.Set<Role>()
                .FirstOrDefaultAsync(r => r.RoleName == roleName);
        }
    }

    public class ChapterRepository : BaseRepository<Chapter>, IChapterRepository
    {
        public ChapterRepository(MasterDbContext context) : base(context)
        {
        }

        public async Task<Chapter?> GetByChapterNumberAsync(string chapterNumber)
        {
            return await _context.Set<Chapter>()
                .Include(c => c.Clauses)
                .FirstOrDefaultAsync(c => c.ChapterNumber == chapterNumber);
        }

        public async Task<IEnumerable<Chapter>> GetWithClausesAsync()
        {
            return await _context.Set<Chapter>()
                .Include(c => c.Clauses)
                .ToListAsync();
        }
    }

    public class ClauseRepository : BaseRepository<Clause>, IClauseRepository
    {
        public ClauseRepository(MasterDbContext context) : base(context)
        {
        }

        public async Task<Clause?> GetByClauseNumberAsync(string clauseNumber)
        {
            return await _context.Set<Clause>()
                .Include(c => c.Chapter)
                .FirstOrDefaultAsync(c => c.ClauseNumber == clauseNumber);
        }

        public async Task<IEnumerable<Clause>> GetByChapterIdAsync(int chapterId)
        {
            return await _context.Set<Clause>()
                .Include(c => c.Chapter)
                .Where(c => c.ChapterId == chapterId)
                .ToListAsync();
        }
    }

    public class FocusAreaRepository : BaseRepository<FocusArea>, IFocusAreaRepository
    {
        public FocusAreaRepository(MasterDbContext context) : base(context)
        {
        }

        public async Task<FocusArea?> GetByNameAsync(string focusAreaName)
        {
            return await _context.Set<FocusArea>()
                .FirstOrDefaultAsync(f => f.FocusAreaName == focusAreaName);
        }
    }
}