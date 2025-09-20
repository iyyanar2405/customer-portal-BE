using CustomerPortalAPI.Data.Repositories;
using CustomerPortalAPI.Modules.Settings.Entities;

namespace CustomerPortalAPI.Modules.Settings.Repositories
{
    public interface ITrainingRepository : IRepository<Training>
    {
        Task<IEnumerable<Training>> GetActiveTrainingsAsync();
        Task<IEnumerable<Training>> GetTrainingsByTypeAsync(string trainingType);
        Task<IEnumerable<Training>> GetTrainingsByCategoryAsync(string category);
        Task<IEnumerable<Training>> GetTrainingsWithAssessmentAsync();
        Task<IEnumerable<Training>> GetTrainingsWithCertificateAsync();
        Task<IEnumerable<Training>> GetExpiredTrainingsAsync();
        Task<IEnumerable<Training>> GetUpcomingTrainingsAsync(int daysAhead);
        Task<Training?> GetByTrainingCodeAsync(string trainingCode);
        Task<IEnumerable<Training>> SearchTrainingsAsync(string searchTerm);
        Task<IEnumerable<Training>> GetTrainingsByDurationRangeAsync(int minHours, int maxHours);
        Task<IEnumerable<Training>> GetTrainingsByCostRangeAsync(decimal? minCost, decimal? maxCost);
        Task<decimal> GetTotalTrainingCostAsync();
        Task<int> GetTrainingCountByTypeAsync(string trainingType);
        Task<IEnumerable<Training>> GetRecentTrainingsAsync(int count);
    }

    public interface IErrorLogRepository : IRepository<ErrorLog>
    {
        Task<IEnumerable<ErrorLog>> GetErrorLogsBySeverityAsync(string severity);
        Task<IEnumerable<ErrorLog>> GetErrorLogsByTypeAsync(string errorType);
        Task<IEnumerable<ErrorLog>> GetErrorLogsByUserAsync(int userId);
        Task<IEnumerable<ErrorLog>> GetErrorLogsBySourceAsync(string source);
        Task<IEnumerable<ErrorLog>> GetErrorLogsByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<ErrorLog>> GetErrorLogsByCorrelationIdAsync(string correlationId);
        Task<IEnumerable<ErrorLog>> GetErrorLogsByEnvironmentAsync(string environment);
        Task<IEnumerable<ErrorLog>> GetRecentErrorLogsAsync(int count);
        Task<IEnumerable<ErrorLog>> SearchErrorLogsAsync(string searchTerm);
        Task<int> GetErrorCountBySeverityAsync(string severity);
        Task<int> GetErrorCountByTypeAsync(string errorType);
        Task<IEnumerable<ErrorLog>> GetCriticalErrorsAsync();
        Task LogErrorAsync(string errorMessage, string? errorType, string severity, string? source, 
            string? stackTrace, int? userId, string? sessionId, string? ipAddress, string? userAgent,
            string? requestUrl, string? requestMethod, string? requestBody, string? errorCode,
            string? innerException, string? correlationId, string? additionalData);
        Task BulkDeleteOldErrorLogsAsync(DateTime cutoffDate);
        Task<Dictionary<string, int>> GetErrorStatsByTypeAsync(DateTime? fromDate = null);
        Task<Dictionary<string, int>> GetErrorStatsBySeverityAsync(DateTime? fromDate = null);
    }
}