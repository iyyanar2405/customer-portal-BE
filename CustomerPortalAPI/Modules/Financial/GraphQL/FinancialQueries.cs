using CustomerPortalAPI.Modules.Financial.Entities;
using CustomerPortalAPI.Modules.Financial.Repositories;
using HotChocolate;

namespace CustomerPortalAPI.Modules.Financial.GraphQL
{
    // Input Types
    public record CreateFinancialInput(string TransactionName, string? TransactionType, decimal? Amount);
    public record UpdateFinancialInput(int Id, string? TransactionName, string? TransactionType, decimal? Amount, bool? IsActive);
    public record CreateInvoiceInput(string InvoiceNumber, int CompanyId, decimal? TotalAmount);
    public record UpdateInvoiceInput(int Id, string? InvoiceNumber, int? CompanyId, decimal? TotalAmount, bool? IsActive);

    // Output Types
    public record FinancialOutput(int Id, string TransactionName, string? TransactionType, decimal? Amount, bool IsActive, DateTime CreatedDate);
    public record InvoiceOutput(int Id, string InvoiceNumber, int CompanyId, decimal? TotalAmount, bool IsActive, DateTime CreatedDate);

    // Payloads
    public record CreateFinancialPayload(FinancialOutput? Financial, string? Error);
    public record UpdateFinancialPayload(FinancialOutput? Financial, string? Error);
    // Filter
    public record FinancialFilterInput(string? TransactionName, string? TransactionType, bool? IsActive);

    [ExtendObjectType("Query")]
    public class FinancialQueries
    {
        public async Task<IEnumerable<FinancialOutput>> GetFinancials(
            [Service] IFinancialRepository repository,
            FinancialFilterInput? filter = null)
        {
            var financials = await repository.GetAllAsync();
            
            if (filter != null)
            {
                if (filter.TransactionName != null)
                    financials = financials.Where(f => f.TransactionName.Contains(filter.TransactionName, StringComparison.OrdinalIgnoreCase));
                if (filter.TransactionType != null)
                    financials = financials.Where(f => f.TransactionType != null && f.TransactionType.Contains(filter.TransactionType, StringComparison.OrdinalIgnoreCase));
                if (filter.IsActive.HasValue)
                    financials = financials.Where(f => f.IsActive == filter.IsActive.Value);
            }
            
            return financials.Select(f => new FinancialOutput(f.Id, f.TransactionName, f.TransactionType, f.Amount, f.IsActive, f.CreatedDate));
        }

        public async Task<FinancialOutput?> GetFinancialById(int id, [Service] IFinancialRepository repository)
        {
            var financial = await repository.GetByIdAsync(id);
            return financial == null ? null : new FinancialOutput(financial.Id, financial.TransactionName, financial.TransactionType, financial.Amount, financial.IsActive, financial.CreatedDate);
        }

        public async Task<IEnumerable<InvoiceOutput>> GetInvoices([Service] IInvoiceRepository repository)
        {
            var invoices = await repository.GetAllAsync();
            return invoices.Select(i => new InvoiceOutput(i.Id, i.InvoiceNumber, i.CompanyId, i.TotalAmount, i.IsActive, i.CreatedDate));
        }

        public async Task<InvoiceOutput?> GetInvoiceById(int id, [Service] IInvoiceRepository repository)
        {
            var invoice = await repository.GetByIdAsync(id);
            return invoice == null ? null : new InvoiceOutput(invoice.Id, invoice.InvoiceNumber, invoice.CompanyId, invoice.TotalAmount, invoice.IsActive, invoice.CreatedDate);
        }
    }
}
