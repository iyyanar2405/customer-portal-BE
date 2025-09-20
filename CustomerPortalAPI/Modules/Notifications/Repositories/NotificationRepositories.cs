using Microsoft.EntityFrameworkCore;
using CustomerPortalAPI.Data;
using CustomerPortalAPI.Data.Repositories;
using CustomerPortalAPI.Modules.Notifications.Entities;
using System.Text.Json;

namespace CustomerPortalAPI.Modules.Notifications.Repositories
{
    public class NotificationRepository : Repository<Notification>, INotificationRepository
    {
        public NotificationRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Notification>> GetNotificationsByCategoryAsync(int categoryId)
        {
            return await _dbSet.Where(n => n.CategoryId == categoryId).OrderByDescending(n => n.CreatedDate).ToListAsync();
        }

        public async Task<IEnumerable<Notification>> GetNotificationsByCompanyAsync(int companyId)
        {
            return await _dbSet.Where(n => n.CompanyId == companyId).OrderByDescending(n => n.CreatedDate).ToListAsync();
        }

        public async Task<IEnumerable<Notification>> GetNotificationsBySiteAsync(int siteId)
        {
            return await _dbSet.Where(n => n.SiteId == siteId).OrderByDescending(n => n.CreatedDate).ToListAsync();
        }

        public async Task<IEnumerable<Notification>> GetNotificationsByServiceAsync(int serviceId)
        {
            return await _dbSet.Where(n => n.ServiceId == serviceId).OrderByDescending(n => n.CreatedDate).ToListAsync();
        }

        public async Task<IEnumerable<Notification>> GetNotificationsByPriorityAsync(string priority)
        {
            return await _dbSet.Where(n => n.Priority == priority).OrderByDescending(n => n.CreatedDate).ToListAsync();
        }

        public async Task<IEnumerable<Notification>> GetNotificationsByStatusAsync(string status)
        {
            return await _dbSet.Where(n => n.Status == status).OrderByDescending(n => n.CreatedDate).ToListAsync();
        }

        public async Task<IEnumerable<Notification>> GetActiveNotificationsAsync()
        {
            return await _dbSet.Where(n => n.IsActive && n.Status == "Active")
                .OrderByDescending(n => n.CreatedDate).ToListAsync();
        }

        public async Task<IEnumerable<Notification>> GetUnreadNotificationsAsync(int userId)
        {
            return await _dbSet.Where(n => n.IsActive && n.Status == "Active" && 
                (n.ReadBy == null || !n.ReadBy.Contains(userId.ToString())))
                .OrderByDescending(n => n.CreatedDate).ToListAsync();
        }

        public async Task<IEnumerable<Notification>> GetNotificationsForUserAsync(int userId, int? companyId = null, int? siteId = null)
        {
            var query = _dbSet.Where(n => n.IsActive && 
                (n.TargetAudience == "All" || 
                 (companyId.HasValue && n.CompanyId == companyId) ||
                 (siteId.HasValue && n.SiteId == siteId)));

            return await query.OrderByDescending(n => n.CreatedDate).ToListAsync();
        }

        public async Task<IEnumerable<Notification>> GetExpiredNotificationsAsync()
        {
            var today = DateTime.Today;
            return await _dbSet.Where(n => n.ExpiryDate < today && n.IsActive).ToListAsync();
        }

        public async Task<IEnumerable<Notification>> GetExpiringNotificationsAsync(int daysAhead)
        {
            var today = DateTime.Today;
            var futureDate = today.AddDays(daysAhead);
            return await _dbSet.Where(n => n.ExpiryDate >= today && n.ExpiryDate <= futureDate && n.IsActive).ToListAsync();
        }

        public async Task<IEnumerable<Notification>> GetNotificationsByTargetAudienceAsync(string targetAudience)
        {
            return await _dbSet.Where(n => n.TargetAudience == targetAudience).OrderByDescending(n => n.CreatedDate).ToListAsync();
        }

        public async Task<IEnumerable<Notification>> GetActionRequiredNotificationsAsync()
        {
            return await _dbSet.Where(n => n.ActionRequired && n.IsActive && n.Status == "Active")
                .OrderByDescending(n => n.CreatedDate).ToListAsync();
        }

        public async Task<IEnumerable<Notification>> GetNotificationsByRelatedEntityAsync(string entityType, int entityId)
        {
            return await _dbSet.Where(n => n.RelatedEntityType == entityType && n.RelatedEntityId == entityId)
                .OrderByDescending(n => n.CreatedDate).ToListAsync();
        }

        public async Task<IEnumerable<Notification>> GetRecentNotificationsAsync(int count)
        {
            return await _dbSet.Where(n => n.IsActive)
                .OrderByDescending(n => n.CreatedDate).Take(count).ToListAsync();
        }

        public async Task<IEnumerable<Notification>> SearchNotificationsAsync(string searchTerm)
        {
            return await _dbSet.Where(n => 
                n.Title.Contains(searchTerm) ||
                n.Message.Contains(searchTerm))
                .OrderByDescending(n => n.CreatedDate).ToListAsync();
        }

        public async Task MarkAsReadAsync(int notificationId, int userId)
        {
            var notification = await GetByIdAsync(notificationId);
            if (notification != null)
            {
                var readByList = new List<int>();
                if (!string.IsNullOrEmpty(notification.ReadBy))
                {
                    try
                    {
                        readByList = JsonSerializer.Deserialize<List<int>>(notification.ReadBy) ?? new List<int>();
                    }
                    catch
                    {
                        readByList = new List<int>();
                    }
                }

                if (!readByList.Contains(userId))
                {
                    readByList.Add(userId);
                    notification.ReadBy = JsonSerializer.Serialize(readByList);
                    await UpdateAsync(notification);
                }
            }
        }

        public async Task MarkMultipleAsReadAsync(IEnumerable<int> notificationIds, int userId)
        {
            foreach (var notificationId in notificationIds)
            {
                await MarkAsReadAsync(notificationId, userId);
            }
        }

        public async Task UpdateNotificationStatusAsync(int notificationId, string status, int modifiedBy)
        {
            var notification = await GetByIdAsync(notificationId);
            if (notification != null)
            {
                notification.Status = status;
                notification.ModifiedBy = modifiedBy;
                notification.ModifiedDate = DateTime.UtcNow;
                await UpdateAsync(notification);
            }
        }

        public async Task<int> GetUnreadCountForUserAsync(int userId)
        {
            return await _dbSet.CountAsync(n => n.IsActive && n.Status == "Active" && 
                (n.ReadBy == null || !n.ReadBy.Contains(userId.ToString())));
        }

        public async Task<int> GetNotificationCountByPriorityAsync(string priority)
        {
            return await _dbSet.CountAsync(n => n.Priority == priority && n.IsActive);
        }

        public async Task BulkArchiveExpiredNotificationsAsync()
        {
            var today = DateTime.Today;
            var expiredNotifications = await _dbSet.Where(n => n.ExpiryDate < today && n.IsActive).ToListAsync();
            
            foreach (var notification in expiredNotifications)
            {
                notification.Status = "Archived";
                notification.ModifiedDate = DateTime.UtcNow;
            }
            
            if (expiredNotifications.Any())
            {
                await _context.SaveChangesAsync();
            }
        }
    }

    public class NotificationCategoryRepository : Repository<NotificationCategory>, INotificationCategoryRepository
    {
        public NotificationCategoryRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<NotificationCategory>> GetActiveCategoriesAsync()
        {
            return await _dbSet.Where(nc => nc.IsActive).OrderBy(nc => nc.DisplayOrder).ToListAsync();
        }

        public async Task<IEnumerable<NotificationCategory>> GetCategoriesByPriorityAsync()
        {
            return await _dbSet.Where(nc => nc.IsActive).OrderBy(nc => nc.Priority).ToListAsync();
        }

        public async Task<IEnumerable<NotificationCategory>> GetCategoriesByDisplayOrderAsync()
        {
            return await _dbSet.Where(nc => nc.IsActive).OrderBy(nc => nc.DisplayOrder).ToListAsync();
        }

        public async Task<NotificationCategory?> GetByCategoryCodeAsync(string categoryCode)
        {
            return await _dbSet.FirstOrDefaultAsync(nc => nc.CategoryCode == categoryCode);
        }

        public async Task<IEnumerable<NotificationCategory>> SearchCategoriesAsync(string searchTerm)
        {
            return await _dbSet.Where(nc => 
                nc.CategoryName.Contains(searchTerm) ||
                nc.CategoryCode.Contains(searchTerm) ||
                nc.Description!.Contains(searchTerm)).ToListAsync();
        }

        public async Task UpdateCategoryOrderAsync(int categoryId, int displayOrder, int modifiedBy)
        {
            var category = await GetByIdAsync(categoryId);
            if (category != null)
            {
                category.DisplayOrder = displayOrder;
                category.ModifiedBy = modifiedBy;
                category.ModifiedDate = DateTime.UtcNow;
                await UpdateAsync(category);
            }
        }

        public async Task UpdateCategoryPriorityAsync(int categoryId, int priority, int modifiedBy)
        {
            var category = await GetByIdAsync(categoryId);
            if (category != null)
            {
                category.Priority = priority;
                category.ModifiedBy = modifiedBy;
                category.ModifiedDate = DateTime.UtcNow;
                await UpdateAsync(category);
            }
        }

        public async Task<NotificationCategory?> GetCategoryWithNotificationsAsync(int categoryId)
        {
            return await _dbSet
                .Include(nc => nc.Notifications)
                .FirstOrDefaultAsync(nc => nc.Id == categoryId);
        }
    }
}