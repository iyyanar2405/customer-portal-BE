using CustomerPortalAPI.Data.Repositories;
using CustomerPortalAPI.Modules.Financial.Entities;

namespace CustomerPortalAPI.Modules.Financial.Repositories
{
    public interface IFinancialRepository : IRepository<FinancialTransaction>
    {
        Task<IEnumerable<FinancialTransaction>> GetFinancialsByCompanyAsync(int companyId);
        Task<IEnumerable<FinancialTransaction>> GetFinancialsBySiteAsync(int siteId);
        Task<IEnumerable<FinancialTransaction>> GetFinancialsByStatusAsync(string status);
        Task<IEnumerable<FinancialTransaction>> GetFinancialsByTypeAsync(string transactionType);
        Task<IEnumerable<FinancialTransaction>> GetFinancialsByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<FinancialTransaction?> GetByTransactionNumberAsync(string transactionNumber);
        Task<IEnumerable<FinancialTransaction>> SearchFinancialsAsync(string searchTerm);
        Task<decimal> GetTotalAmountByCompanyAsync(int companyId);
        Task<decimal> GetTotalAmountByTypeAsync(string transactionType);
        Task<IEnumerable<FinancialTransaction>> GetRecentTransactionsAsync(int count);
        Task UpdateFinancialStatusAsync(int financialId, string status, int modifiedBy);
    }

    public interface IInvoiceRepository : IRepository<Invoice>
    {
        Task<IEnumerable<Invoice>> GetInvoicesByCompanyAsync(int companyId);
        Task<IEnumerable<Invoice>> GetInvoicesBySiteAsync(int siteId);
        Task<IEnumerable<Invoice>> GetInvoicesByStatusAsync(string status);
        Task<IEnumerable<Invoice>> GetOverdueInvoicesAsync();
        Task<IEnumerable<Invoice>> GetInvoicesByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<Invoice?> GetByInvoiceNumberAsync(string invoiceNumber);
        Task<Invoice?> GetInvoiceWithDetailsAsync(int invoiceId);
        Task<IEnumerable<Invoice>> SearchInvoicesAsync(string searchTerm);
        Task<decimal> GetTotalInvoiceAmountByCompanyAsync(int companyId);
        Task<decimal> GetOutstandingAmountByCompanyAsync(int companyId);
        Task<decimal> GetTotalInvoiceAmountAsync();
        Task UpdateInvoiceStatusAsync(int invoiceId, string status, int modifiedBy);
        Task<int> GetInvoiceCountByStatusAsync(string status);
        Task LogInvoiceActionAsync(int invoiceId, string action, string? oldValues, string? newValues, int? userId, string? userName, string? comments);
    }
}