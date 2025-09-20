using CustomerPortal.CertificatesService.Data;
using CustomerPortal.CertificatesService.Entities;
using Microsoft.EntityFrameworkCore;

namespace CustomerPortal.CertificatesService.Repositories
{
    /// <summary>
    /// Base repository implementation with common functionality
    /// </summary>
    public class BaseRepository<T> where T : class
    {
        protected readonly CertificatesDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public BaseRepository(CertificatesDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public virtual async Task<T?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public virtual async Task<IEnumerable<T>> GetPagedAsync(int pageNumber, int pageSize)
        {
            return await _dbSet
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public virtual async Task<T> CreateAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public virtual async Task<T> AddAsync(T entity)
        {
            return await CreateAsync(entity);
        }

        public virtual async Task<T> UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public virtual async Task<bool> DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public virtual async Task<bool> HardDeleteAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity == null) return false;
            
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public virtual async Task<int> CountAsync()
        {
            return await _dbSet.CountAsync();
        }

        public virtual async Task<bool> ExistsAsync(int id)
        {
            return await _dbSet.FindAsync(id) != null;
        }

        public virtual async Task<IEnumerable<T>> FindAsync(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }
    }

    /// <summary>
    /// Certificate repository implementation
    /// </summary>
    public class CertificateRepository : BaseRepository<Certificate>, ICertificateRepository
    {
        public CertificateRepository(CertificatesDbContext context) : base(context) { }

        public async Task<IEnumerable<Certificate>> GetByCompanyIdAsync(int companyId)
        {
            return await _dbSet
                .Include(c => c.Company)
                .Include(c => c.CertificateType)
                .Where(c => c.CompanyId == companyId && c.IsActive)
                .OrderByDescending(c => c.CreatedDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Certificate>> GetByCertificateNumberAsync(string certificateNumber)
        {
            return await _dbSet
                .Include(c => c.Company)
                .Include(c => c.CertificateType)
                .Include(c => c.Audit)
                .ThenInclude(a => a!.LeadAuditor)
                .Where(c => c.CertificateNumber.Contains(certificateNumber))
                .ToListAsync();
        }

        public async Task<IEnumerable<Certificate>> GetByStatusAsync(string status)
        {
            return await _dbSet
                .Include(c => c.Company)
                .Include(c => c.CertificateType)
                .Where(c => c.Status == status && c.IsActive)
                .OrderByDescending(c => c.CreatedDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Certificate>> GetExpiringCertificatesAsync(int withinDays)
        {
            var thresholdDate = DateTime.UtcNow.AddDays(withinDays);
            return await _dbSet
                .Include(c => c.Company)
                .Include(c => c.CertificateType)
                .Where(c => c.ExpiryDate <= thresholdDate && c.Status == "ACTIVE" && c.IsActive)
                .OrderBy(c => c.ExpiryDate)
                .ToListAsync();
        }

        public async Task<Dictionary<string, int>> GetCertificatesByStatusAsync()
        {
            return await _dbSet
                .Where(c => c.IsActive)
                .GroupBy(c => c.Status)
                .ToDictionaryAsync(g => g.Key, g => g.Count());
        }

        public async Task<Dictionary<string, int>> GetCertificatesByTypeAsync()
        {
            return await _dbSet
                .Include(c => c.CertificateType)
                .Where(c => c.IsActive)
                .GroupBy(c => c.CertificateType!.TypeName)
                .ToDictionaryAsync(g => g.Key, g => g.Count());
        }

        public async Task<IEnumerable<Certificate>> GetRenewalScheduleAsync(DateTime startDate, DateTime endDate)
        {
            return await _dbSet
                .Include(c => c.Company)
                .Include(c => c.CertificateType)
                .Where(c => c.RenewalDate >= startDate && c.RenewalDate <= endDate && c.Status == "ACTIVE" && c.IsActive)
                .OrderBy(c => c.RenewalDate)
                .ToListAsync();
        }

        public async Task<Certificate?> ValidateCertificateAsync(string certificateNumber)
        {
            return await _dbSet
                .Include(c => c.Company)
                .Include(c => c.CertificateType)
                .Include(c => c.Sites)
                .ThenInclude(cs => cs.Site)
                .FirstOrDefaultAsync(c => c.CertificateNumber == certificateNumber && c.IsActive);
        }

        public async Task<int> GetTotalCountAsync()
        {
            return await _dbSet.CountAsync(c => c.IsActive);
        }

        public async Task<decimal> GetRenewalSuccessRateAsync()
        {
            var totalRenewals = await _context.CertificateRenewals.CountAsync();
            if (totalRenewals == 0) return 0;

            var successfulRenewals = await _context.CertificateRenewals.CountAsync(cr => cr.Status == "COMPLETED");
            return (decimal)successfulRenewals / totalRenewals * 100;
        }

        public async Task<double> GetAverageRenewalTimeAsync()
        {
            var completedRenewals = await _context.CertificateRenewals
                .Where(cr => cr.Status == "COMPLETED" && cr.ActualCompletionDate.HasValue)
                .Select(cr => new { cr.PlannedAuditDate, cr.ActualCompletionDate })
                .ToListAsync();

            if (!completedRenewals.Any()) return 0;

            var totalDays = completedRenewals.Sum(cr => (cr.ActualCompletionDate!.Value - cr.PlannedAuditDate).TotalDays);
            return totalDays / completedRenewals.Count;
        }

        public async Task<IEnumerable<Certificate>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _dbSet
                .Include(c => c.Company)
                .Include(c => c.CertificateType)
                .Where(c => c.CreatedDate >= startDate && c.CreatedDate <= endDate && c.IsActive)
                .OrderByDescending(c => c.CreatedDate)
                .ToListAsync();
        }

        public override async Task<IEnumerable<Certificate>> GetAllAsync()
        {
            return await _dbSet
                .Include(c => c.Company)
                .Include(c => c.CertificateType)
                .Include(c => c.Audit)
                .ThenInclude(a => a!.LeadAuditor)
                .Include(c => c.Sites)
                .ThenInclude(cs => cs.Site)
                .Include(c => c.Services)
                .ThenInclude(cs => cs.Service)
                .Include(c => c.AdditionalScopes)
                .Where(c => c.IsActive)
                .OrderByDescending(c => c.CreatedDate)
                .ToListAsync();
        }
    }

    /// <summary>
    /// CertificateType repository implementation
    /// </summary>
    public class CertificateTypeRepository : BaseRepository<CertificateType>, ICertificateTypeRepository
    {
        public CertificateTypeRepository(CertificatesDbContext context) : base(context) { }

        public async Task<IEnumerable<CertificateType>> GetByStandardAsync(string standard)
        {
            return await _dbSet
                .Where(ct => ct.Standard == standard && ct.IsActive)
                .OrderBy(ct => ct.TypeName)
                .ToListAsync();
        }

        public async Task<IEnumerable<CertificateType>> GetByCategoryAsync(string category)
        {
            return await _dbSet
                .Where(ct => ct.Category == category && ct.IsActive)
                .OrderBy(ct => ct.TypeName)
                .ToListAsync();
        }

        public async Task<IEnumerable<CertificateType>> GetAccreditedTypesAsync()
        {
            return await _dbSet
                .Where(ct => ct.IsAccredited && ct.IsActive)
                .OrderBy(ct => ct.TypeName)
                .ToListAsync();
        }

        public async Task<CertificateType?> GetByTypeNameAsync(string typeName)
        {
            return await _dbSet
                .FirstOrDefaultAsync(ct => ct.TypeName == typeName && ct.IsActive);
        }

        public override async Task<IEnumerable<CertificateType>> GetAllAsync()
        {
            return await _dbSet
                .Where(ct => ct.IsActive)
                .OrderBy(ct => ct.TypeName)
                .ToListAsync();
        }
    }

    /// <summary>
    /// Company repository implementation
    /// </summary>
    public class CompanyRepository : BaseRepository<Company>, ICompanyRepository
    {
        public CompanyRepository(CertificatesDbContext context) : base(context) { }

        public async Task<Company?> GetByCompanyCodeAsync(string companyCode)
        {
            return await _dbSet
                .Include(c => c.Sites)
                .FirstOrDefaultAsync(c => c.CompanyCode == companyCode && c.IsActive);
        }

        public async Task<IEnumerable<Company>> GetByCompanyNameAsync(string companyName)
        {
            return await _dbSet
                .Where(c => c.CompanyName.Contains(companyName) && c.IsActive)
                .OrderBy(c => c.CompanyName)
                .ToListAsync();
        }

        public async Task<IEnumerable<Company>> SearchCompaniesAsync(string searchTerm)
        {
            return await _dbSet
                .Where(c => (c.CompanyName.Contains(searchTerm) || 
                           c.CompanyCode.Contains(searchTerm) || 
                           c.ContactPerson!.Contains(searchTerm)) && c.IsActive)
                .OrderBy(c => c.CompanyName)
                .ToListAsync();
        }

        public override async Task<IEnumerable<Company>> GetAllAsync()
        {
            return await _dbSet
                .Where(c => c.IsActive)
                .OrderBy(c => c.CompanyName)
                .ToListAsync();
        }
    }

    /// <summary>
    /// User repository implementation
    /// </summary>
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(CertificatesDbContext context) : base(context) { }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _dbSet
                .FirstOrDefaultAsync(u => u.Email == email && u.IsActive);
        }

        public async Task<IEnumerable<User>> GetByRoleAsync(string role)
        {
            return await _dbSet
                .Where(u => u.Role == role && u.IsActive)
                .OrderBy(u => u.LastName)
                .ThenBy(u => u.FirstName)
                .ToListAsync();
        }

        public async Task<IEnumerable<User>> GetByDepartmentAsync(string department)
        {
            return await _dbSet
                .Where(u => u.Department == department && u.IsActive)
                .OrderBy(u => u.LastName)
                .ThenBy(u => u.FirstName)
                .ToListAsync();
        }

        public async Task<IEnumerable<User>> GetAuditorsAsync()
        {
            return await _dbSet
                .Where(u => u.Role == "AUDITOR" && u.IsActive)
                .OrderBy(u => u.LastName)
                .ThenBy(u => u.FirstName)
                .ToListAsync();
        }

        public async Task<IEnumerable<User>> SearchUsersAsync(string searchTerm)
        {
            return await _dbSet
                .Where(u => (u.FirstName.Contains(searchTerm) || 
                           u.LastName.Contains(searchTerm) || 
                           u.Email.Contains(searchTerm)) && u.IsActive)
                .OrderBy(u => u.LastName)
                .ThenBy(u => u.FirstName)
                .ToListAsync();
        }

        public override async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _dbSet
                .Where(u => u.IsActive)
                .OrderBy(u => u.LastName)
                .ThenBy(u => u.FirstName)
                .ToListAsync();
        }
    }
}