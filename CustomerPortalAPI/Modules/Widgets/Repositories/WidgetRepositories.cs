using Microsoft.EntityFrameworkCore;
using CustomerPortalAPI.Data;
using CustomerPortalAPI.Data.Repositories;
using CustomerPortalAPI.Modules.Widgets.Entities;

namespace CustomerPortalAPI.Modules.Widgets.Repositories
{
    public class WidgetRepository : Repository<Widget>, IWidgetRepository
    {
        public WidgetRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Widget>> GetWidgetsByTypeAsync(string widgetType)
        {
            return await _dbSet.Where(w => w.WidgetType == widgetType && w.IsActive).ToListAsync();
        }

        public async Task<IEnumerable<Widget>> GetWidgetsByDataSourceAsync(string dataSource)
        {
            return await _dbSet.Where(w => w.DataSource == dataSource && w.IsActive).ToListAsync();
        }

        public async Task<IEnumerable<Widget>> GetActiveWidgetsAsync()
        {
            return await _dbSet.Where(w => w.IsActive && w.Status == "Active").ToListAsync();
        }

        public async Task<IEnumerable<Widget>> GetWidgetsByUserAsync(int userId)
        {
            return await _dbSet.Where(w => w.CreatedBy == userId && w.IsActive).ToListAsync();
        }

        public async Task<Widget?> GetWidgetWithConfigurationAsync(int widgetId)
        {
            return await _dbSet.FirstOrDefaultAsync(w => w.Id == widgetId && w.IsActive);
        }

        public async Task<IEnumerable<Widget>> SearchWidgetsAsync(string searchTerm)
        {
            return await _dbSet.Where(w => 
                (w.WidgetName.Contains(searchTerm) ||
                w.Description!.Contains(searchTerm) ||
                w.WidgetType.Contains(searchTerm) ||
                w.DataSource.Contains(searchTerm)) && w.IsActive).ToListAsync();
        }

        public async Task UpdateWidgetConfigurationAsync(int widgetId, string configuration, int modifiedBy)
        {
            var widget = await GetByIdAsync(widgetId);
            if (widget != null)
            {
                widget.Configuration = configuration;
                widget.ModifiedBy = modifiedBy;
                widget.ModifiedDate = DateTime.UtcNow;
                await UpdateAsync(widget);
            }
        }

        public async Task UpdateWidgetPositionAsync(int widgetId, int positionX, int positionY, int modifiedBy)
        {
            var widget = await GetByIdAsync(widgetId);
            if (widget != null)
            {
                widget.PositionX = positionX;
                widget.PositionY = positionY;
                widget.ModifiedBy = modifiedBy;
                widget.ModifiedDate = DateTime.UtcNow;
                await UpdateAsync(widget);
            }
        }

        public async Task UpdateWidgetSizeAsync(int widgetId, int width, int height, int modifiedBy)
        {
            var widget = await GetByIdAsync(widgetId);
            if (widget != null)
            {
                widget.Width = width;
                widget.Height = height;
                widget.ModifiedBy = modifiedBy;
                widget.ModifiedDate = DateTime.UtcNow;
                await UpdateAsync(widget);
            }
        }

        public async Task RefreshWidgetDataAsync(int widgetId)
        {
            var widget = await GetByIdAsync(widgetId);
            if (widget != null)
            {
                widget.LastRefreshed = DateTime.UtcNow;
                await UpdateAsync(widget);
            }
        }

        public async Task<IEnumerable<Widget>> GetWidgetsDueForRefreshAsync()
        {
            var cutoffTime = DateTime.UtcNow;
            return await _dbSet.Where(w => 
                w.IsActive && 
                w.RefreshInterval.HasValue && 
                (!w.LastRefreshed.HasValue || 
                w.LastRefreshed.Value.AddMinutes(w.RefreshInterval.Value) <= cutoffTime)).ToListAsync();
        }
    }

    public class WidgetCategoryRepository : Repository<WidgetCategory>, IWidgetCategoryRepository
    {
        public WidgetCategoryRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<WidgetCategory>> GetActiveCategoriesAsync()
        {
            return await _dbSet.Where(c => c.IsActive && c.Status == "Active").ToListAsync();
        }

        public async Task<IEnumerable<WidgetCategory>> GetCategoriesByUserAsync(int userId)
        {
            return await _dbSet.Where(c => c.CreatedBy == userId && c.IsActive).ToListAsync();
        }

        public async Task<WidgetCategory?> GetCategoryWithWidgetsAsync(int categoryId)
        {
            return await _dbSet
                .Include(c => c.WidgetUserAccesses)
                .FirstOrDefaultAsync(c => c.Id == categoryId && c.IsActive);
        }

        public async Task<IEnumerable<WidgetCategory>> SearchCategoriesAsync(string searchTerm)
        {
            return await _dbSet.Where(c => 
                (c.CategoryName.Contains(searchTerm) ||
                c.Description!.Contains(searchTerm)) && c.IsActive).ToListAsync();
        }

        public async Task UpdateCategorySortOrderAsync(int categoryId, int sortOrder, int modifiedBy)
        {
            var category = await GetByIdAsync(categoryId);
            if (category != null)
            {
                category.SortOrder = sortOrder;
                category.ModifiedBy = modifiedBy;
                category.ModifiedDate = DateTime.UtcNow;
                await UpdateAsync(category);
            }
        }

        public async Task<IEnumerable<WidgetCategory>> GetCategoriesOrderedAsync()
        {
            return await _dbSet
                .Where(c => c.IsActive && c.Status == "Active")
                .OrderBy(c => c.SortOrder ?? int.MaxValue)
                .ThenBy(c => c.CategoryName)
                .ToListAsync();
        }
    }

    public class WidgetUserAccessRepository : Repository<WidgetUserAccess>, IWidgetUserAccessRepository
    {
        public WidgetUserAccessRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<WidgetUserAccess>> GetUserWidgetAccessAsync(int userId)
        {
            return await _dbSet
                .Include(w => w.Widget)
                .Include(w => w.WidgetCategory)
                .Where(w => w.UserId == userId && w.IsActive).ToListAsync();
        }

        public async Task<IEnumerable<WidgetUserAccess>> GetWidgetUsersAsync(int widgetId)
        {
            return await _dbSet.Where(w => w.WidgetId == widgetId && w.IsActive).ToListAsync();
        }

        public async Task<WidgetUserAccess?> GetUserWidgetAccessAsync(int userId, int widgetId)
        {
            return await _dbSet.FirstOrDefaultAsync(w => w.UserId == userId && w.WidgetId == widgetId && w.IsActive);
        }

        public async Task<IEnumerable<WidgetUserAccess>> GetUserFavoriteWidgetsAsync(int userId)
        {
            return await _dbSet
                .Include(w => w.Widget)
                .Where(w => w.UserId == userId && w.IsFavorite && w.IsActive).ToListAsync();
        }

        public async Task<IEnumerable<WidgetUserAccess>> GetUserOwnedWidgetsAsync(int userId)
        {
            return await _dbSet
                .Include(w => w.Widget)
                .Where(w => w.UserId == userId && w.IsOwner && w.IsActive).ToListAsync();
        }

        public async Task<IEnumerable<WidgetUserAccess>> GetDashboardWidgetsAsync(int dashboardId)
        {
            return await _dbSet
                .Include(w => w.Widget)
                .Where(w => w.DashboardId == dashboardId && w.IsActive && w.IsVisible).ToListAsync();
        }

        public async Task GrantWidgetAccessAsync(int widgetId, int userId, bool canView, bool canEdit, bool canDelete, bool canConfigure, int grantedBy)
        {
            var existingAccess = await GetUserWidgetAccessAsync(userId, widgetId);
            if (existingAccess != null)
            {
                existingAccess.CanView = canView;
                existingAccess.CanEdit = canEdit;
                existingAccess.CanDelete = canDelete;
                existingAccess.CanConfigure = canConfigure;
                existingAccess.ModifiedBy = grantedBy;
                existingAccess.ModifiedDate = DateTime.UtcNow;
                await UpdateAsync(existingAccess);
            }
            else
            {
                var newAccess = new WidgetUserAccess
                {
                    WidgetId = widgetId,
                    UserId = userId,
                    CanView = canView,
                    CanEdit = canEdit,
                    CanDelete = canDelete,
                    CanConfigure = canConfigure,
                    CreatedBy = grantedBy
                };
                await AddAsync(newAccess);
            }
        }

        public async Task RevokeWidgetAccessAsync(int widgetId, int userId)
        {
            var access = await GetUserWidgetAccessAsync(userId, widgetId);
            if (access != null)
            {
                access.IsActive = false;
                access.ModifiedDate = DateTime.UtcNow;
                await UpdateAsync(access);
            }
        }

        public async Task SetWidgetFavoriteAsync(int userId, int widgetId, bool isFavorite, int modifiedBy)
        {
            var access = await GetUserWidgetAccessAsync(userId, widgetId);
            if (access != null)
            {
                access.IsFavorite = isFavorite;
                access.ModifiedBy = modifiedBy;
                access.ModifiedDate = DateTime.UtcNow;
                await UpdateAsync(access);
            }
        }

        public async Task UpdatePersonalConfigurationAsync(int userId, int widgetId, string configuration, int modifiedBy)
        {
            var access = await GetUserWidgetAccessAsync(userId, widgetId);
            if (access != null)
            {
                access.PersonalConfiguration = configuration;
                access.ModifiedBy = modifiedBy;
                access.ModifiedDate = DateTime.UtcNow;
                await UpdateAsync(access);
            }
        }

        public async Task UpdatePersonalLayoutAsync(int userId, int widgetId, int? positionX, int? positionY, int? width, int? height, int modifiedBy)
        {
            var access = await GetUserWidgetAccessAsync(userId, widgetId);
            if (access != null)
            {
                access.PersonalPositionX = positionX;
                access.PersonalPositionY = positionY;
                access.PersonalWidth = width;
                access.PersonalHeight = height;
                access.ModifiedBy = modifiedBy;
                access.ModifiedDate = DateTime.UtcNow;
                await UpdateAsync(access);
            }
        }

        public async Task TransferWidgetOwnershipAsync(int widgetId, int fromUserId, int toUserId, int modifiedBy)
        {
            var fromAccess = await GetUserWidgetAccessAsync(fromUserId, widgetId);
            var toAccess = await GetUserWidgetAccessAsync(toUserId, widgetId);

            if (fromAccess != null && fromAccess.IsOwner)
            {
                fromAccess.IsOwner = false;
                fromAccess.ModifiedBy = modifiedBy;
                fromAccess.ModifiedDate = DateTime.UtcNow;
                await UpdateAsync(fromAccess);
            }

            if (toAccess != null)
            {
                toAccess.IsOwner = true;
                toAccess.CanView = true;
                toAccess.CanEdit = true;
                toAccess.CanDelete = true;
                toAccess.CanConfigure = true;
                toAccess.ModifiedBy = modifiedBy;
                toAccess.ModifiedDate = DateTime.UtcNow;
                await UpdateAsync(toAccess);
            }
            else
            {
                var newOwnerAccess = new WidgetUserAccess
                {
                    WidgetId = widgetId,
                    UserId = toUserId,
                    IsOwner = true,
                    CanView = true,
                    CanEdit = true,
                    CanDelete = true,
                    CanConfigure = true,
                    CreatedBy = modifiedBy
                };
                await AddAsync(newOwnerAccess);
            }
        }

        public async Task BulkUpdateWidgetVisibilityAsync(int userId, IEnumerable<int> widgetIds, bool isVisible, int modifiedBy)
        {
            var accessList = await _dbSet.Where(w => w.UserId == userId && widgetIds.Contains(w.WidgetId)).ToListAsync();
            foreach (var access in accessList)
            {
                access.IsVisible = isVisible;
                access.ModifiedBy = modifiedBy;
                access.ModifiedDate = DateTime.UtcNow;
            }
            await _context.SaveChangesAsync();
        }
    }

    public class WidgetDataRepository : Repository<WidgetData>, IWidgetDataRepository
    {
        public WidgetDataRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<WidgetData>> GetWidgetDataAsync(int widgetId)
        {
            return await _dbSet.Where(d => d.WidgetId == widgetId).OrderByDescending(d => d.DataDate).ToListAsync();
        }

        public async Task<WidgetData?> GetLatestWidgetDataAsync(int widgetId)
        {
            return await _dbSet
                .Where(d => d.WidgetId == widgetId && d.Status == "Current")
                .OrderByDescending(d => d.DataDate)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<WidgetData>> GetWidgetDataByDateRangeAsync(int widgetId, DateTime startDate, DateTime endDate)
        {
            return await _dbSet.Where(d => 
                d.WidgetId == widgetId && 
                d.DataDate >= startDate && 
                d.DataDate <= endDate).ToListAsync();
        }

        public async Task<IEnumerable<WidgetData>> GetWidgetDataByCompanyAsync(int widgetId, int companyId)
        {
            return await _dbSet.Where(d => d.WidgetId == widgetId && d.CompanyId == companyId).ToListAsync();
        }

        public async Task<IEnumerable<WidgetData>> GetWidgetDataBySiteAsync(int widgetId, int siteId)
        {
            return await _dbSet.Where(d => d.WidgetId == widgetId && d.SiteId == siteId).ToListAsync();
        }

        public async Task<IEnumerable<WidgetData>> GetExpiredWidgetDataAsync()
        {
            return await _dbSet.Where(d => 
                d.ValidUntil.HasValue && 
                d.ValidUntil < DateTime.UtcNow && 
                d.Status == "Current").ToListAsync();
        }

        public async Task<IEnumerable<WidgetData>> GetCurrentWidgetDataAsync(int widgetId)
        {
            return await _dbSet.Where(d => d.WidgetId == widgetId && d.Status == "Current").ToListAsync();
        }

        public async Task UpdateWidgetDataAsync(int widgetId, string data, int? companyId, int? siteId, DateTime? validUntil, int createdBy)
        {
            // Mark existing current data as expired
            var existingData = await GetCurrentWidgetDataAsync(widgetId);
            foreach (var item in existingData)
            {
                item.Status = "Expired";
            }

            // Add new data
            var newData = new WidgetData
            {
                WidgetId = widgetId,
                Data = data,
                CompanyId = companyId,
                SiteId = siteId,
                ValidUntil = validUntil,
                CreatedBy = createdBy,
                Status = "Current"
            };

            await AddAsync(newData);
        }

        public async Task CleanupExpiredDataAsync(DateTime cutoffDate)
        {
            var expiredData = await _dbSet.Where(d => 
                d.DataDate < cutoffDate && 
                d.Status == "Expired").ToListAsync();
            
            _dbSet.RemoveRange(expiredData);
            await _context.SaveChangesAsync();
        }

        public async Task<int> GetWidgetDataCountAsync(int widgetId)
        {
            return await _dbSet.CountAsync(d => d.WidgetId == widgetId);
        }

        public async Task ArchiveOldWidgetDataAsync(int widgetId, DateTime cutoffDate)
        {
            var oldData = await _dbSet.Where(d => 
                d.WidgetId == widgetId && 
                d.DataDate < cutoffDate && 
                d.Status == "Current").ToListAsync();
            
            foreach (var item in oldData)
            {
                item.Status = "Expired";
            }
            
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<WidgetData>> GetWidgetDataHistoryAsync(int widgetId, int count)
        {
            return await _dbSet
                .Where(d => d.WidgetId == widgetId)
                .OrderByDescending(d => d.DataDate)
                .Take(count)
                .ToListAsync();
        }
    }
}