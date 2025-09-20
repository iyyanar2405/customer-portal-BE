using CustomerPortalAPI.Data.Repositories;
using CustomerPortalAPI.Modules.Overview.Entities;

namespace CustomerPortalAPI.Modules.Overview.Repositories
{
    public interface IOverviewDashboardRepository : IRepository<OverviewDashboard>
    {
        Task<IEnumerable<OverviewDashboard>> GetDashboardsByUserAsync(int userId);
        Task<IEnumerable<OverviewDashboard>> GetDashboardsByCompanyAsync(int companyId);
        Task<IEnumerable<OverviewDashboard>> GetDashboardsByTypeAsync(string dashboardType);
        Task<OverviewDashboard?> GetDefaultDashboardAsync(int userId);
        Task<OverviewDashboard?> GetDashboardWithConfigurationAsync(int dashboardId);
        Task SetDefaultDashboardAsync(int userId, int dashboardId, int modifiedBy);
        Task<IEnumerable<OverviewDashboard>> GetActiveDashboardsAsync();
        Task UpdateDashboardConfigurationAsync(int dashboardId, string configuration, int modifiedBy);
    }

    public interface IOverviewMetricRepository : IRepository<OverviewMetric>
    {
        Task<IEnumerable<OverviewMetric>> GetMetricsByCompanyAsync(int companyId);
        Task<IEnumerable<OverviewMetric>> GetMetricsBySiteAsync(int siteId);
        Task<IEnumerable<OverviewMetric>> GetMetricsByCategoryAsync(string category);
        Task<IEnumerable<OverviewMetric>> GetMetricsByTypeAsync(string metricType);
        Task<IEnumerable<OverviewMetric>> GetMetricsByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<OverviewMetric>> GetCurrentMetricsAsync();
        Task<IEnumerable<OverviewMetric>> GetHistoricalMetricsAsync(string category, DateTime startDate, DateTime endDate);
        Task<OverviewMetric?> GetLatestMetricAsync(string metricName, int? companyId, int? siteId);
        Task<decimal?> GetMetricTrendAsync(string metricName, int? companyId, int days);
        Task BulkInsertMetricsAsync(IEnumerable<OverviewMetric> metrics);
        Task ArchiveOldMetricsAsync(DateTime cutoffDate);
    }

    public interface IOverviewReportRepository : IRepository<OverviewReport>
    {
        Task<IEnumerable<OverviewReport>> GetReportsByCompanyAsync(int companyId);
        Task<IEnumerable<OverviewReport>> GetReportsBySiteAsync(int siteId);
        Task<IEnumerable<OverviewReport>> GetReportsByTypeAsync(string reportType);
        Task<IEnumerable<OverviewReport>> GetReportsByCategoryAsync(string category);
        Task<IEnumerable<OverviewReport>> GetReportsByUserAsync(int userId);
        Task<IEnumerable<OverviewReport>> GetReportsByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<OverviewReport>> GetScheduledReportsAsync();
        Task<IEnumerable<OverviewReport>> GetExpiredReportsAsync();
        Task<OverviewReport?> GetReportWithDataAsync(int reportId);
        Task<IEnumerable<OverviewReport>> SearchReportsAsync(string searchTerm);
        Task UpdateReportStatusAsync(int reportId, string status, int modifiedBy);
        Task ArchiveExpiredReportsAsync();
        Task<int> GetReportCountByTypeAsync(string reportType);
        Task<IEnumerable<OverviewReport>> GetRecentReportsAsync(int count);
    }
}