using Microsoft.EntityFrameworkCore;
using CustomerPortalAPI.Data;
using CustomerPortalAPI.Data.Repositories;
using CustomerPortalAPI.Modules.Financial.Entities;

namespace CustomerPortalAPI.Modules.Financial.Repositories
{
    public class FinancialRepository : Repository<FinancialTransaction>, IFinancialRepository
    {
        public FinancialRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<FinancialTransaction>> GetFinancialsByCompanyAsync(int companyId)
        {
            return await _dbSet.Where(f => f.CompanyId == companyId).ToListAsync();
        }

        public async Task<IEnumerable<FinancialTransaction>> GetFinancialsBySiteAsync(int siteId)
        {
            return await _dbSet.Where(f => f.SiteId == siteId).ToListAsync();
        }

        public async Task<IEnumerable<FinancialTransaction>> GetFinancialsByStatusAsync(string status)
        {
            return await _dbSet.Where(f => f.Status == status).ToListAsync();
        }

        public async Task<IEnumerable<FinancialTransaction>> GetFinancialsByTypeAsync(string transactionType)
        {
            return await _dbSet.Where(f => f.TransactionType == transactionType).ToListAsync();
        }

        public async Task<IEnumerable<FinancialTransaction>> GetFinancialsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _dbSet.Where(f => f.TransactionDate >= startDate && f.TransactionDate <= endDate).ToListAsync();
        }

        public async Task<FinancialTransaction?> GetByTransactionNumberAsync(string transactionNumber)
        {
            return await _dbSet.FirstOrDefaultAsync(f => f.TransactionNumber == transactionNumber);
        }

        public async Task<IEnumerable<FinancialTransaction>> SearchFinancialsAsync(string searchTerm)
        {
            return await _dbSet.Where(f => 
                f.TransactionName.Contains(searchTerm) ||
                f.TransactionNumber!.Contains(searchTerm) ||
                f.Description!.Contains(searchTerm) ||
                f.Reference!.Contains(searchTerm)).ToListAsync();
        }

        public async Task<decimal> GetTotalAmountByCompanyAsync(int companyId)
        {
            return await _dbSet.Where(f => f.CompanyId == companyId && f.Amount.HasValue)
                .SumAsync(f => f.Amount!.Value);
        }

        public async Task<decimal> GetTotalAmountByTypeAsync(string transactionType)
        {
            return await _dbSet.Where(f => f.TransactionType == transactionType && f.Amount.HasValue)
                .SumAsync(f => f.Amount!.Value);
        }

        public async Task<IEnumerable<FinancialTransaction>> GetRecentTransactionsAsync(int count)
        {
            return await _dbSet.OrderByDescending(f => f.TransactionDate).Take(count).ToListAsync();
        }

        public async Task UpdateFinancialStatusAsync(int financialId, string status, int modifiedBy)
        {
            var financial = await GetByIdAsync(financialId);
            if (financial != null)
            {
                financial.Status = status;
                financial.ModifiedBy = modifiedBy;
                financial.ModifiedDate = DateTime.UtcNow;
                await UpdateAsync(financial);
            }
        }
    }

    public class InvoiceRepository : Repository<Invoice>, IInvoiceRepository
    {
        public InvoiceRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Invoice>> GetInvoicesByCompanyAsync(int companyId)
        {
            return await _dbSet.Where(i => i.CompanyId == companyId).ToListAsync();
        }

        public async Task<IEnumerable<Invoice>> GetInvoicesBySiteAsync(int siteId)
        {
            return await _dbSet.Where(i => i.SiteId == siteId).ToListAsync();
        }

        public async Task<IEnumerable<Invoice>> GetInvoicesByStatusAsync(string status)
        {
            return await _dbSet.Where(i => i.Status == status).ToListAsync();
        }

        public async Task<IEnumerable<Invoice>> GetOverdueInvoicesAsync()
        {
            var today = DateTime.Today;
            return await _dbSet.Where(i => i.DueDate < today && i.Status != "Paid").ToListAsync();
        }

        public async Task<IEnumerable<Invoice>> GetInvoicesByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _dbSet.Where(i => i.InvoiceDate >= startDate && i.InvoiceDate <= endDate).ToListAsync();
        }

        public async Task<Invoice?> GetByInvoiceNumberAsync(string invoiceNumber)
        {
            return await _dbSet.FirstOrDefaultAsync(i => i.InvoiceNumber == invoiceNumber);
        }

        public async Task<Invoice?> GetInvoiceWithDetailsAsync(int invoiceId)
        {
            return await _dbSet
                .Include(i => i.Company)
                .Include(i => i.Site)
                .Include(i => i.InvoiceAuditLogs)
                .FirstOrDefaultAsync(i => i.Id == invoiceId);
        }

        public async Task<IEnumerable<Invoice>> SearchInvoicesAsync(string searchTerm)
        {
            return await _dbSet.Where(i => 
                i.InvoiceNumber.Contains(searchTerm) ||
                i.Description!.Contains(searchTerm) ||
                i.BillingAddress!.Contains(searchTerm)).ToListAsync();
        }

        public async Task<decimal> GetTotalInvoiceAmountByCompanyAsync(int companyId)
        {
            return await _dbSet.Where(i => i.CompanyId == companyId && i.TotalAmount.HasValue)
                .SumAsync(i => i.TotalAmount.Value);
        }

        public async Task<decimal> GetOutstandingAmountByCompanyAsync(int companyId)
        {
            return await _dbSet.Where(i => i.CompanyId == companyId && i.Status != "Paid" && i.TotalAmount.HasValue)
                .SumAsync(i => i.TotalAmount.Value);
        }

        public async Task<decimal> GetTotalInvoiceAmountAsync()
        {
            return await _dbSet.Where(i => i.TotalAmount.HasValue).SumAsync(i => i.TotalAmount.Value);
        }

        public async Task UpdateInvoiceStatusAsync(int invoiceId, string status, int modifiedBy)
        {
            var invoice = await GetByIdAsync(invoiceId);
            if (invoice != null)
            {
                var oldStatus = invoice.Status;
                invoice.Status = status;
                invoice.ModifiedBy = modifiedBy;
                invoice.ModifiedDate = DateTime.UtcNow;
                await UpdateAsync(invoice);

                // Log the status change
                await LogInvoiceActionAsync(invoiceId, "Status Updated", 
                    $"Status: {oldStatus}", $"Status: {status}", 
                    modifiedBy, null, $"Invoice status changed from {oldStatus} to {status}");
            }
        }

        public async Task<int> GetInvoiceCountByStatusAsync(string status)
        {
            return await _dbSet.CountAsync(i => i.Status == status);
        }

        public async Task LogInvoiceActionAsync(int invoiceId, string action, string? oldValues, string? newValues, int? userId, string? userName, string? comments)
        {
            var auditLog = new InvoiceAuditLog
            {
                InvoiceId = invoiceId,
                Action = action,
                OldValues = oldValues,
                NewValues = newValues,
                UserId = userId,
                UserName = userName,
                Comments = comments,
                ActionDate = DateTime.UtcNow
            };

            _context.Set<InvoiceAuditLog>().Add(auditLog);
            await _context.SaveChangesAsync();
        }
    }
}