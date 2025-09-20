using CustomerPortalAPI.Data.Repositories;
using CustomerPortalAPI.Modules.Notifications.Entities;

namespace CustomerPortalAPI.Modules.Notifications.Repositories
{
    public interface INotificationRepository : IRepository<Notification>
    {
        Task<IEnumerable<Notification>> GetNotificationsByCategoryAsync(int categoryId);
        Task<IEnumerable<Notification>> GetNotificationsByCompanyAsync(int companyId);
        Task<IEnumerable<Notification>> GetNotificationsBySiteAsync(int siteId);
        Task<IEnumerable<Notification>> GetNotificationsByServiceAsync(int serviceId);
        Task<IEnumerable<Notification>> GetNotificationsByPriorityAsync(string priority);
        Task<IEnumerable<Notification>> GetNotificationsByStatusAsync(string status);
        Task<IEnumerable<Notification>> GetActiveNotificationsAsync();
        Task<IEnumerable<Notification>> GetUnreadNotificationsAsync(int userId);
        Task<IEnumerable<Notification>> GetNotificationsForUserAsync(int userId, int? companyId = null, int? siteId = null);
        Task<IEnumerable<Notification>> GetExpiredNotificationsAsync();
        Task<IEnumerable<Notification>> GetExpiringNotificationsAsync(int daysAhead);
        Task<IEnumerable<Notification>> GetNotificationsByTargetAudienceAsync(string targetAudience);
        Task<IEnumerable<Notification>> GetActionRequiredNotificationsAsync();
        Task<IEnumerable<Notification>> GetNotificationsByRelatedEntityAsync(string entityType, int entityId);
        Task<IEnumerable<Notification>> GetRecentNotificationsAsync(int count);
        Task<IEnumerable<Notification>> SearchNotificationsAsync(string searchTerm);
        Task MarkAsReadAsync(int notificationId, int userId);
        Task MarkMultipleAsReadAsync(IEnumerable<int> notificationIds, int userId);
        Task UpdateNotificationStatusAsync(int notificationId, string status, int modifiedBy);
        Task<int> GetUnreadCountForUserAsync(int userId);
        Task<int> GetNotificationCountByPriorityAsync(string priority);
        Task BulkArchiveExpiredNotificationsAsync();
    }

    public interface INotificationCategoryRepository : IRepository<NotificationCategory>
    {
        Task<IEnumerable<NotificationCategory>> GetActiveCategoriesAsync();
        Task<IEnumerable<NotificationCategory>> GetCategoriesByPriorityAsync();
        Task<IEnumerable<NotificationCategory>> GetCategoriesByDisplayOrderAsync();
        Task<NotificationCategory?> GetByCategoryCodeAsync(string categoryCode);
        Task<IEnumerable<NotificationCategory>> SearchCategoriesAsync(string searchTerm);
        Task UpdateCategoryOrderAsync(int categoryId, int displayOrder, int modifiedBy);
        Task UpdateCategoryPriorityAsync(int categoryId, int priority, int modifiedBy);
        Task<NotificationCategory?> GetCategoryWithNotificationsAsync(int categoryId);
    }
}