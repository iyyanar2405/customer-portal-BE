using CustomerPortalAPI.Data.Repositories;
using CustomerPortalAPI.Modules.Widgets.Entities;

namespace CustomerPortalAPI.Modules.Widgets.Repositories
{
    public interface IWidgetRepository : IRepository<Widget>
    {
        Task<IEnumerable<Widget>> GetWidgetsByTypeAsync(string widgetType);
        Task<IEnumerable<Widget>> GetWidgetsByDataSourceAsync(string dataSource);
        Task<IEnumerable<Widget>> GetActiveWidgetsAsync();
        Task<IEnumerable<Widget>> GetWidgetsByUserAsync(int userId);
        Task<Widget?> GetWidgetWithConfigurationAsync(int widgetId);
        Task<IEnumerable<Widget>> SearchWidgetsAsync(string searchTerm);
        Task UpdateWidgetConfigurationAsync(int widgetId, string configuration, int modifiedBy);
        Task UpdateWidgetPositionAsync(int widgetId, int positionX, int positionY, int modifiedBy);
        Task UpdateWidgetSizeAsync(int widgetId, int width, int height, int modifiedBy);
        Task RefreshWidgetDataAsync(int widgetId);
        Task<IEnumerable<Widget>> GetWidgetsDueForRefreshAsync();
    }

    public interface IWidgetCategoryRepository : IRepository<WidgetCategory>
    {
        Task<IEnumerable<WidgetCategory>> GetActiveCategoriesAsync();
        Task<IEnumerable<WidgetCategory>> GetCategoriesByUserAsync(int userId);
        Task<WidgetCategory?> GetCategoryWithWidgetsAsync(int categoryId);
        Task<IEnumerable<WidgetCategory>> SearchCategoriesAsync(string searchTerm);
        Task UpdateCategorySortOrderAsync(int categoryId, int sortOrder, int modifiedBy);
        Task<IEnumerable<WidgetCategory>> GetCategoriesOrderedAsync();
    }

    public interface IWidgetUserAccessRepository : IRepository<WidgetUserAccess>
    {
        Task<IEnumerable<WidgetUserAccess>> GetUserWidgetAccessAsync(int userId);
        Task<IEnumerable<WidgetUserAccess>> GetWidgetUsersAsync(int widgetId);
        Task<WidgetUserAccess?> GetUserWidgetAccessAsync(int userId, int widgetId);
        Task<IEnumerable<WidgetUserAccess>> GetUserFavoriteWidgetsAsync(int userId);
        Task<IEnumerable<WidgetUserAccess>> GetUserOwnedWidgetsAsync(int userId);
        Task<IEnumerable<WidgetUserAccess>> GetDashboardWidgetsAsync(int dashboardId);
        Task GrantWidgetAccessAsync(int widgetId, int userId, bool canView, bool canEdit, bool canDelete, bool canConfigure, int grantedBy);
        Task RevokeWidgetAccessAsync(int widgetId, int userId);
        Task SetWidgetFavoriteAsync(int userId, int widgetId, bool isFavorite, int modifiedBy);
        Task UpdatePersonalConfigurationAsync(int userId, int widgetId, string configuration, int modifiedBy);
        Task UpdatePersonalLayoutAsync(int userId, int widgetId, int? positionX, int? positionY, int? width, int? height, int modifiedBy);
        Task TransferWidgetOwnershipAsync(int widgetId, int fromUserId, int toUserId, int modifiedBy);
        Task BulkUpdateWidgetVisibilityAsync(int userId, IEnumerable<int> widgetIds, bool isVisible, int modifiedBy);
    }

    public interface IWidgetDataRepository : IRepository<WidgetData>
    {
        Task<IEnumerable<WidgetData>> GetWidgetDataAsync(int widgetId);
        Task<WidgetData?> GetLatestWidgetDataAsync(int widgetId);
        Task<IEnumerable<WidgetData>> GetWidgetDataByDateRangeAsync(int widgetId, DateTime startDate, DateTime endDate);
        Task<IEnumerable<WidgetData>> GetWidgetDataByCompanyAsync(int widgetId, int companyId);
        Task<IEnumerable<WidgetData>> GetWidgetDataBySiteAsync(int widgetId, int siteId);
        Task<IEnumerable<WidgetData>> GetExpiredWidgetDataAsync();
        Task<IEnumerable<WidgetData>> GetCurrentWidgetDataAsync(int widgetId);
        Task UpdateWidgetDataAsync(int widgetId, string data, int? companyId, int? siteId, DateTime? validUntil, int createdBy);
        Task CleanupExpiredDataAsync(DateTime cutoffDate);
        Task<int> GetWidgetDataCountAsync(int widgetId);
        Task ArchiveOldWidgetDataAsync(int widgetId, DateTime cutoffDate);
        Task<IEnumerable<WidgetData>> GetWidgetDataHistoryAsync(int widgetId, int count);
    }
}