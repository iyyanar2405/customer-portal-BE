using Microsoft.EntityFrameworkCore;
using CustomerPortalAPI.Data;
using CustomerPortalAPI.Data.Repositories;
using CustomerPortalAPI.Modules.Settings.Entities;

namespace CustomerPortalAPI.Modules.Settings.Repositories
{
    public class TrainingRepository : Repository<Training>, ITrainingRepository
    {
        public TrainingRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Training>> GetActiveTrainingsAsync()
        {
            return await _dbSet.Where(t => t.IsActive).OrderBy(t => t.TrainingName).ToListAsync();
        }

        public async Task<IEnumerable<Training>> GetTrainingsByTypeAsync(string trainingType)
        {
            return await _dbSet.Where(t => t.TrainingType == trainingType && t.IsActive).ToListAsync();
        }

        public async Task<IEnumerable<Training>> GetTrainingsByCategoryAsync(string category)
        {
            return await _dbSet.Where(t => t.Category == category && t.IsActive).ToListAsync();
        }

        public async Task<IEnumerable<Training>> GetTrainingsWithAssessmentAsync()
        {
            return await _dbSet.Where(t => t.AssessmentRequired && t.IsActive).ToListAsync();
        }

        public async Task<IEnumerable<Training>> GetTrainingsWithCertificateAsync()
        {
            return await _dbSet.Where(t => t.CertificateIssued && t.IsActive).ToListAsync();
        }

        public async Task<IEnumerable<Training>> GetExpiredTrainingsAsync()
        {
            var today = DateTime.Today;
            return await _dbSet.Where(t => t.DueDate < today && t.IsActive).ToListAsync();
        }

        public async Task<IEnumerable<Training>> GetUpcomingTrainingsAsync(int daysAhead)
        {
            var today = DateTime.Today;
            var futureDate = today.AddDays(daysAhead);
            return await _dbSet.Where(t => t.DueDate >= today && t.DueDate <= futureDate && t.IsActive).ToListAsync();
        }

        public async Task<Training?> GetByTrainingCodeAsync(string trainingCode)
        {
            return await _dbSet.FirstOrDefaultAsync(t => t.TrainingCode == trainingCode);
        }

        public async Task<IEnumerable<Training>> SearchTrainingsAsync(string searchTerm)
        {
            return await _dbSet.Where(t => 
                t.TrainingName.Contains(searchTerm) ||
                t.TrainingCode.Contains(searchTerm) ||
                t.Description!.Contains(searchTerm) ||
                t.Category!.Contains(searchTerm)).ToListAsync();
        }

        public async Task<IEnumerable<Training>> GetTrainingsByDurationRangeAsync(int minHours, int maxHours)
        {
            return await _dbSet.Where(t => t.Duration >= minHours && t.Duration <= maxHours && t.IsActive).ToListAsync();
        }

        public async Task<IEnumerable<Training>> GetTrainingsByCostRangeAsync(decimal? minCost, decimal? maxCost)
        {
            var query = _dbSet.Where(t => t.IsActive);
            
            if (minCost.HasValue)
                query = query.Where(t => t.Cost >= minCost);
            
            if (maxCost.HasValue)
                query = query.Where(t => t.Cost <= maxCost);
                
            return await query.ToListAsync();
        }

        public async Task<decimal> GetTotalTrainingCostAsync()
        {
            return await _dbSet.Where(t => t.Cost.HasValue && t.IsActive).SumAsync(t => t.Cost.Value);
        }

        public async Task<int> GetTrainingCountByTypeAsync(string trainingType)
        {
            return await _dbSet.CountAsync(t => t.TrainingType == trainingType && t.IsActive);
        }

        public async Task<IEnumerable<Training>> GetRecentTrainingsAsync(int count)
        {
            return await _dbSet.Where(t => t.IsActive)
                .OrderByDescending(t => t.CreatedDate).Take(count).ToListAsync();
        }
    }

    public class ErrorLogRepository : Repository<ErrorLog>, IErrorLogRepository
    {
        public ErrorLogRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<ErrorLog>> GetErrorLogsBySeverityAsync(string severity)
        {
            return await _dbSet.Where(e => e.Severity == severity).OrderByDescending(e => e.CreatedDate).ToListAsync();
        }

        public async Task<IEnumerable<ErrorLog>> GetErrorLogsByTypeAsync(string errorType)
        {
            return await _dbSet.Where(e => e.ErrorType == errorType).OrderByDescending(e => e.CreatedDate).ToListAsync();
        }

        public async Task<IEnumerable<ErrorLog>> GetErrorLogsByUserAsync(int userId)
        {
            return await _dbSet.Where(e => e.UserId == userId).OrderByDescending(e => e.CreatedDate).ToListAsync();
        }

        public async Task<IEnumerable<ErrorLog>> GetErrorLogsBySourceAsync(string source)
        {
            return await _dbSet.Where(e => e.Source == source).OrderByDescending(e => e.CreatedDate).ToListAsync();
        }

        public async Task<IEnumerable<ErrorLog>> GetErrorLogsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _dbSet.Where(e => e.CreatedDate >= startDate && e.CreatedDate <= endDate)
                .OrderByDescending(e => e.CreatedDate).ToListAsync();
        }

        public async Task<IEnumerable<ErrorLog>> GetErrorLogsByCorrelationIdAsync(string correlationId)
        {
            return await _dbSet.Where(e => e.CorrelationId == correlationId)
                .OrderByDescending(e => e.CreatedDate).ToListAsync();
        }

        public async Task<IEnumerable<ErrorLog>> GetErrorLogsByEnvironmentAsync(string environment)
        {
            return await _dbSet.Where(e => e.Environment == environment)
                .OrderByDescending(e => e.CreatedDate).ToListAsync();
        }

        public async Task<IEnumerable<ErrorLog>> GetRecentErrorLogsAsync(int count)
        {
            return await _dbSet.OrderByDescending(e => e.CreatedDate).Take(count).ToListAsync();
        }

        public async Task<IEnumerable<ErrorLog>> SearchErrorLogsAsync(string searchTerm)
        {
            return await _dbSet.Where(e => 
                e.ErrorMessage.Contains(searchTerm) ||
                e.Source!.Contains(searchTerm) ||
                e.ErrorCode!.Contains(searchTerm) ||
                e.RequestUrl!.Contains(searchTerm))
                .OrderByDescending(e => e.CreatedDate).ToListAsync();
        }

        public async Task<int> GetErrorCountBySeverityAsync(string severity)
        {
            return await _dbSet.CountAsync(e => e.Severity == severity);
        }

        public async Task<int> GetErrorCountByTypeAsync(string errorType)
        {
            return await _dbSet.CountAsync(e => e.ErrorType == errorType);
        }

        public async Task<IEnumerable<ErrorLog>> GetCriticalErrorsAsync()
        {
            return await _dbSet.Where(e => e.Severity == "Critical")
                .OrderByDescending(e => e.CreatedDate).ToListAsync();
        }

        public async Task LogErrorAsync(string errorMessage, string? errorType, string severity, string? source, 
            string? stackTrace, int? userId, string? sessionId, string? ipAddress, string? userAgent,
            string? requestUrl, string? requestMethod, string? requestBody, string? errorCode,
            string? innerException, string? correlationId, string? additionalData)
        {
            var errorLog = new ErrorLog
            {
                ErrorMessage = errorMessage,
                ErrorType = errorType,
                Severity = severity,
                Source = source,
                StackTrace = stackTrace,
                UserId = userId,
                SessionId = sessionId,
                IPAddress = ipAddress,
                UserAgent = userAgent,
                RequestUrl = requestUrl,
                RequestMethod = requestMethod,
                RequestBody = requestBody,
                ErrorCode = errorCode,
                InnerException = innerException,
                CorrelationId = correlationId,
                AdditionalData = additionalData,
                CreatedDate = DateTime.UtcNow,
                MachineName = System.Environment.MachineName,
                ProcessId = System.Environment.ProcessId
            };

            await AddAsync(errorLog);
        }

        public async Task BulkDeleteOldErrorLogsAsync(DateTime cutoffDate)
        {
            var oldLogs = await _dbSet.Where(e => e.CreatedDate < cutoffDate).ToListAsync();
            
            if (oldLogs.Any())
            {
                _dbSet.RemoveRange(oldLogs);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Dictionary<string, int>> GetErrorStatsByTypeAsync(DateTime? fromDate = null)
        {
            var query = _dbSet.AsQueryable();
            
            if (fromDate.HasValue)
                query = query.Where(e => e.CreatedDate >= fromDate);

            return await query
                .GroupBy(e => e.ErrorType ?? "Unknown")
                .Select(g => new { Type = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.Type, x => x.Count);
        }

        public async Task<Dictionary<string, int>> GetErrorStatsBySeverityAsync(DateTime? fromDate = null)
        {
            var query = _dbSet.AsQueryable();
            
            if (fromDate.HasValue)
                query = query.Where(e => e.CreatedDate >= fromDate);

            return await query
                .GroupBy(e => e.Severity)
                .Select(g => new { Severity = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.Severity, x => x.Count);
        }
    }
}