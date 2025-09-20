using Microsoft.EntityFrameworkCore;
using CustomerPortalAPI.Data;
using CustomerPortalAPI.Data.Repositories;
using CustomerPortalAPI.Modules.Overview.Entities;

namespace CustomerPortalAPI.Modules.Overview.Repositories
{
    public class OverviewDashboardRepository : Repository<OverviewDashboard>, IOverviewDashboardRepository
    {
        public OverviewDashboardRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<OverviewDashboard>> GetDashboardsByUserAsync(int userId)
        {
            return await _dbSet.Where(d => d.UserId == userId && d.IsActive).ToListAsync();
        }

        public async Task<IEnumerable<OverviewDashboard>> GetDashboardsByCompanyAsync(int companyId)
        {
            return await _dbSet.Where(d => d.CompanyId == companyId && d.IsActive).ToListAsync();
        }

        public async Task<IEnumerable<OverviewDashboard>> GetDashboardsByTypeAsync(string dashboardType)
        {
            return await _dbSet.Where(d => d.DashboardType == dashboardType && d.IsActive).ToListAsync();
        }

        public async Task<OverviewDashboard?> GetDefaultDashboardAsync(int userId)
        {
            return await _dbSet.FirstOrDefaultAsync(d => d.UserId == userId && d.IsDefault && d.IsActive);
        }

        public async Task<OverviewDashboard?> GetDashboardWithConfigurationAsync(int dashboardId)
        {
            return await _dbSet.FirstOrDefaultAsync(d => d.Id == dashboardId && d.IsActive);
        }

        public async Task SetDefaultDashboardAsync(int userId, int dashboardId, int modifiedBy)
        {
            // First, remove default from all user dashboards
            var userDashboards = await _dbSet.Where(d => d.UserId == userId).ToListAsync();
            foreach (var dashboard in userDashboards)
            {
                dashboard.IsDefault = false;
                dashboard.ModifiedBy = modifiedBy;
                dashboard.ModifiedDate = DateTime.UtcNow;
            }

            // Set the new default
            var targetDashboard = await GetByIdAsync(dashboardId);
            if (targetDashboard != null && targetDashboard.UserId == userId)
            {
                targetDashboard.IsDefault = true;
                targetDashboard.ModifiedBy = modifiedBy;
                targetDashboard.ModifiedDate = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<OverviewDashboard>> GetActiveDashboardsAsync()
        {
            return await _dbSet.Where(d => d.IsActive && d.Status == "Active").ToListAsync();
        }

        public async Task UpdateDashboardConfigurationAsync(int dashboardId, string configuration, int modifiedBy)
        {
            var dashboard = await GetByIdAsync(dashboardId);
            if (dashboard != null)
            {
                dashboard.Configuration = configuration;
                dashboard.ModifiedBy = modifiedBy;
                dashboard.ModifiedDate = DateTime.UtcNow;
                await UpdateAsync(dashboard);
            }
        }
    }

    public class OverviewMetricRepository : Repository<OverviewMetric>, IOverviewMetricRepository
    {
        public OverviewMetricRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<OverviewMetric>> GetMetricsByCompanyAsync(int companyId)
        {
            return await _dbSet.Where(m => m.CompanyId == companyId).ToListAsync();
        }

        public async Task<IEnumerable<OverviewMetric>> GetMetricsBySiteAsync(int siteId)
        {
            return await _dbSet.Where(m => m.SiteId == siteId).ToListAsync();
        }

        public async Task<IEnumerable<OverviewMetric>> GetMetricsByCategoryAsync(string category)
        {
            return await _dbSet.Where(m => m.MetricCategory == category).ToListAsync();
        }

        public async Task<IEnumerable<OverviewMetric>> GetMetricsByTypeAsync(string metricType)
        {
            return await _dbSet.Where(m => m.MetricType == metricType).ToListAsync();
        }

        public async Task<IEnumerable<OverviewMetric>> GetMetricsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _dbSet.Where(m => m.PeriodStart >= startDate && m.PeriodEnd <= endDate).ToListAsync();
        }

        public async Task<IEnumerable<OverviewMetric>> GetCurrentMetricsAsync()
        {
            return await _dbSet.Where(m => m.Status == "Current").ToListAsync();
        }

        public async Task<IEnumerable<OverviewMetric>> GetHistoricalMetricsAsync(string category, DateTime startDate, DateTime endDate)
        {
            return await _dbSet.Where(m => 
                m.MetricCategory == category && 
                m.CalculatedDate >= startDate && 
                m.CalculatedDate <= endDate)
                .OrderBy(m => m.CalculatedDate)
                .ToListAsync();
        }

        public async Task<OverviewMetric?> GetLatestMetricAsync(string metricName, int? companyId, int? siteId)
        {
            var query = _dbSet.Where(m => m.MetricName == metricName);
            
            if (companyId.HasValue)
                query = query.Where(m => m.CompanyId == companyId);
            
            if (siteId.HasValue)
                query = query.Where(m => m.SiteId == siteId);

            return await query.OrderByDescending(m => m.CalculatedDate).FirstOrDefaultAsync();
        }

        public async Task<decimal?> GetMetricTrendAsync(string metricName, int? companyId, int days)
        {
            var endDate = DateTime.UtcNow;
            var startDate = endDate.AddDays(-days);

            var query = _dbSet.Where(m => 
                m.MetricName == metricName && 
                m.CalculatedDate >= startDate && 
                m.CalculatedDate <= endDate);

            if (companyId.HasValue)
                query = query.Where(m => m.CompanyId == companyId);

            var metrics = await query.OrderBy(m => m.CalculatedDate).ToListAsync();
            
            if (metrics.Count < 2) return null;

            var firstValue = metrics.First().MetricValue ?? 0;
            var lastValue = metrics.Last().MetricValue ?? 0;

            if (firstValue == 0) return null;

            return ((lastValue - firstValue) / firstValue) * 100;
        }

        public async Task BulkInsertMetricsAsync(IEnumerable<OverviewMetric> metrics)
        {
            await _dbSet.AddRangeAsync(metrics);
            await _context.SaveChangesAsync();
        }

        public async Task ArchiveOldMetricsAsync(DateTime cutoffDate)
        {
            var oldMetrics = await _dbSet.Where(m => m.CalculatedDate < cutoffDate && m.Status == "Current").ToListAsync();
            foreach (var metric in oldMetrics)
            {
                metric.Status = "Historical";
            }
            await _context.SaveChangesAsync();
        }
    }

    public class OverviewReportRepository : Repository<OverviewReport>, IOverviewReportRepository
    {
        public OverviewReportRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<OverviewReport>> GetReportsByCompanyAsync(int companyId)
        {
            return await _dbSet.Where(r => r.CompanyId == companyId).ToListAsync();
        }

        public async Task<IEnumerable<OverviewReport>> GetReportsBySiteAsync(int siteId)
        {
            return await _dbSet.Where(r => r.SiteId == siteId).ToListAsync();
        }

        public async Task<IEnumerable<OverviewReport>> GetReportsByTypeAsync(string reportType)
        {
            return await _dbSet.Where(r => r.ReportType == reportType).ToListAsync();
        }

        public async Task<IEnumerable<OverviewReport>> GetReportsByCategoryAsync(string category)
        {
            return await _dbSet.Where(r => r.ReportCategory == category).ToListAsync();
        }

        public async Task<IEnumerable<OverviewReport>> GetReportsByUserAsync(int userId)
        {
            return await _dbSet.Where(r => r.GeneratedBy == userId).ToListAsync();
        }

        public async Task<IEnumerable<OverviewReport>> GetReportsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _dbSet.Where(r => r.GeneratedDate >= startDate && r.GeneratedDate <= endDate).ToListAsync();
        }

        public async Task<IEnumerable<OverviewReport>> GetScheduledReportsAsync()
        {
            return await _dbSet.Where(r => r.IsScheduled && r.Status == "Generated").ToListAsync();
        }

        public async Task<IEnumerable<OverviewReport>> GetExpiredReportsAsync()
        {
            return await _dbSet.Where(r => r.ExpiryDate.HasValue && r.ExpiryDate < DateTime.UtcNow).ToListAsync();
        }

        public async Task<OverviewReport?> GetReportWithDataAsync(int reportId)
        {
            return await _dbSet.FirstOrDefaultAsync(r => r.Id == reportId);
        }

        public async Task<IEnumerable<OverviewReport>> SearchReportsAsync(string searchTerm)
        {
            return await _dbSet.Where(r => 
                r.ReportName.Contains(searchTerm) || 
                r.Description!.Contains(searchTerm) ||
                r.ReportCategory.Contains(searchTerm) ||
                r.ReportType.Contains(searchTerm)).ToListAsync();
        }

        public async Task UpdateReportStatusAsync(int reportId, string status, int modifiedBy)
        {
            var report = await GetByIdAsync(reportId);
            if (report != null)
            {
                report.Status = status;
                report.ModifiedBy = modifiedBy;
                report.ModifiedDate = DateTime.UtcNow;
                await UpdateAsync(report);
            }
        }

        public async Task ArchiveExpiredReportsAsync()
        {
            var expiredReports = await GetExpiredReportsAsync();
            foreach (var report in expiredReports)
            {
                report.Status = "Archived";
                report.ModifiedDate = DateTime.UtcNow;
            }
            await _context.SaveChangesAsync();
        }

        public async Task<int> GetReportCountByTypeAsync(string reportType)
        {
            return await _dbSet.CountAsync(r => r.ReportType == reportType && r.Status == "Generated");
        }

        public async Task<IEnumerable<OverviewReport>> GetRecentReportsAsync(int count)
        {
            return await _dbSet.OrderByDescending(r => r.GeneratedDate).Take(count).ToListAsync();
        }
    }
}