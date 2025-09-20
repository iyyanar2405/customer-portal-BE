using CustomerPortal.AuditsService.Data;
using CustomerPortal.AuditsService.Entities;
using CustomerPortal.Shared.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CustomerPortal.AuditsService.Repositories
{
    public class Repository<T> : IRepository<T> where T : CustomerPortal.Shared.Entities.BaseEntity
    {
        protected readonly AuditsDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public Repository(AuditsDbContext context)
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
            return await _dbSet.Where(e => e.IsActive).ToListAsync();
        }

        public virtual async Task<IEnumerable<T>> GetPagedAsync(int pageNumber, int pageSize)
        {
            return await _dbSet
                .Where(e => e.IsActive)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public virtual async Task<T> AddAsync(T entity)
        {
            entity.CreatedDate = DateTime.UtcNow;
            entity.ModifiedDate = DateTime.UtcNow;
            _dbSet.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public virtual async Task<T> UpdateAsync(T entity)
        {
            entity.ModifiedDate = DateTime.UtcNow;
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public virtual async Task<bool> DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity == null) return false;

            entity.IsActive = false;
            entity.ModifiedDate = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }

        public virtual async Task<bool> HardDeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity == null) return false;

            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public virtual async Task<int> CountAsync()
        {
            return await _dbSet.Where(e => e.IsActive).CountAsync();
        }

        public virtual async Task<bool> ExistsAsync(int id)
        {
            return await _dbSet.AnyAsync(e => e.Id == id && e.IsActive);
        }

        public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.Where(predicate).Where(e => e.IsActive).ToListAsync();
        }
    }

    public class AuditRepository : Repository<Audit>, IAuditRepository
    {
        public AuditRepository(AuditsDbContext context) : base(context) { }

        public async Task<IEnumerable<Audit>> GetAuditsWithDetailsAsync()
        {
            return await _dbSet
                .Include(a => a.Company)
                .Include(a => a.AuditType)
                .Include(a => a.LeadAuditor)
                .Include(a => a.AuditSites)
                    .ThenInclude(as_ => as_.Site)
                .Include(a => a.AuditTeamMembers)
                    .ThenInclude(atm => atm.User)
                .Where(a => a.IsActive)
                .ToListAsync();
        }

        public async Task<Audit?> GetAuditWithDetailsAsync(int id)
        {
            return await _dbSet
                .Include(a => a.Company)
                .Include(a => a.AuditType)
                .Include(a => a.LeadAuditor)
                .Include(a => a.AuditSites)
                    .ThenInclude(as_ => as_.Site)
                .Include(a => a.AuditTeamMembers)
                    .ThenInclude(atm => atm.User)
                .Include(a => a.AuditServices)
                    .ThenInclude(as_ => as_.Service)
                .FirstOrDefaultAsync(a => a.Id == id && a.IsActive);
        }

        public async Task<IEnumerable<Audit>> GetAuditScheduleAsync(DateTime startDate, DateTime endDate)
        {
            return await _dbSet
                .Include(a => a.Company)
                .Include(a => a.LeadAuditor)
                .Where(a => a.IsActive && 
                           a.StartDate >= startDate && 
                           a.EndDate <= endDate)
                .OrderBy(a => a.StartDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Audit>> GetAuditorScheduleAsync(int auditorId, DateTime startDate, DateTime endDate)
        {
            return await _dbSet
                .Where(a => a.IsActive && 
                           a.LeadAuditorId == auditorId &&
                           a.StartDate >= startDate && 
                           a.EndDate <= endDate)
                .ToListAsync();
        }

        public async Task<string> GenerateAuditNumberAsync()
        {
            var year = DateTime.Now.Year;
            var lastAudit = await _dbSet
                .Where(a => a.AuditNumber.StartsWith($"AUD-{year}-"))
                .OrderByDescending(a => a.AuditNumber)
                .FirstOrDefaultAsync();

            if (lastAudit == null)
            {
                return $"AUD-{year}-001";
            }

            var lastNumber = lastAudit.AuditNumber.Split('-').Last();
            if (int.TryParse(lastNumber, out int number))
            {
                return $"AUD-{year}-{(number + 1):D3}";
            }

            return $"AUD-{year}-001";
        }
    }

    public class AuditTeamMemberRepository : Repository<AuditTeamMember>, IAuditTeamMemberRepository
    {
        public AuditTeamMemberRepository(AuditsDbContext context) : base(context) { }

        public async Task<IEnumerable<AuditTeamMember>> GetAuditTeamAsync(int auditId)
        {
            return await _dbSet
                .Include(atm => atm.User)
                .Where(atm => atm.AuditId == auditId && atm.IsActive)
                .ToListAsync();
        }

        public async Task<IEnumerable<AuditTeamMember>> GetUserAuditsAsync(int userId)
        {
            return await _dbSet
                .Include(atm => atm.Audit)
                .Where(atm => atm.UserId == userId && atm.IsActive)
                .ToListAsync();
        }
    }

    public class AuditSiteRepository : Repository<AuditSite>, IAuditSiteRepository
    {
        public AuditSiteRepository(AuditsDbContext context) : base(context) { }

        public async Task<IEnumerable<AuditSite>> GetAuditSitesAsync(int auditId)
        {
            return await _dbSet
                .Include(as_ => as_.Site)
                .Where(as_ => as_.AuditId == auditId && as_.IsActive)
                .ToListAsync();
        }
    }

    // Repository Interfaces
    public interface IAuditRepository : IRepository<Audit>
    {
        Task<IEnumerable<Audit>> GetAuditsWithDetailsAsync();
        Task<Audit?> GetAuditWithDetailsAsync(int id);
        Task<IEnumerable<Audit>> GetAuditScheduleAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<Audit>> GetAuditorScheduleAsync(int auditorId, DateTime startDate, DateTime endDate);
        Task<string> GenerateAuditNumberAsync();
    }

    public interface IAuditTeamMemberRepository : IRepository<AuditTeamMember>
    {
        Task<IEnumerable<AuditTeamMember>> GetAuditTeamAsync(int auditId);
        Task<IEnumerable<AuditTeamMember>> GetUserAuditsAsync(int userId);
    }

    public interface IAuditSiteRepository : IRepository<AuditSite>
    {
        Task<IEnumerable<AuditSite>> GetAuditSitesAsync(int auditId);
    }
}