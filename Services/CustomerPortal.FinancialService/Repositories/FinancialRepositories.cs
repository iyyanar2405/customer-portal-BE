using Microsoft.EntityFrameworkCore;
using CustomerPortal.FinancialService.Data;
using CustomerPortal.FinancialService.Models;

namespace CustomerPortal.FinancialService.Repositories;

public class InvoiceRepository : IInvoiceRepository
{
    private readonly FinancialDbContext _context;

    public InvoiceRepository(FinancialDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Invoice>> GetAllAsync()
    {
        return await _context.Invoices
            .Include(i => i.Company)
            .Include(i => i.Contract)
            .Include(i => i.Audit)
            .Include(i => i.Items)
                .ThenInclude(item => item.Service)
            .Include(i => i.Payments)
                .ThenInclude(p => p.PaymentMethodDetails)
            .Include(i => i.Taxes)
            .Where(i => i.IsActive)
            .OrderByDescending(i => i.CreatedDate)
            .ToListAsync();
    }

    public async Task<Invoice?> GetByIdAsync(int id)
    {
        return await _context.Invoices
            .Include(i => i.Company)
            .Include(i => i.Contract)
            .Include(i => i.Audit)
            .Include(i => i.Items)
                .ThenInclude(item => item.Service)
            .Include(i => i.Payments)
                .ThenInclude(p => p.PaymentMethodDetails)
            .Include(i => i.Taxes)
            .FirstOrDefaultAsync(i => i.Id == id && i.IsActive);
    }

    public async Task<Invoice?> GetByInvoiceNumberAsync(string invoiceNumber)
    {
        return await _context.Invoices
            .Include(i => i.Company)
            .Include(i => i.Contract)
            .Include(i => i.Audit)
            .Include(i => i.Items)
                .ThenInclude(item => item.Service)
            .Include(i => i.Payments)
                .ThenInclude(p => p.PaymentMethodDetails)
            .Include(i => i.Taxes)
            .FirstOrDefaultAsync(i => i.InvoiceNumber == invoiceNumber && i.IsActive);
    }

    public async Task<IEnumerable<Invoice>> GetByCompanyIdAsync(int companyId)
    {
        return await _context.Invoices
            .Include(i => i.Company)
            .Include(i => i.Contract)
            .Include(i => i.Audit)
            .Include(i => i.Items)
                .ThenInclude(item => item.Service)
            .Include(i => i.Payments)
            .Include(i => i.Taxes)
            .Where(i => i.CompanyId == companyId && i.IsActive)
            .OrderByDescending(i => i.CreatedDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Invoice>> GetByStatusAsync(string status)
    {
        return await _context.Invoices
            .Include(i => i.Company)
            .Include(i => i.Contract)
            .Include(i => i.Audit)
            .Include(i => i.Items)
            .Include(i => i.Payments)
            .Include(i => i.Taxes)
            .Where(i => i.Status == status && i.IsActive)
            .OrderByDescending(i => i.CreatedDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Invoice>> GetOverdueInvoicesAsync()
    {
        var today = DateTime.UtcNow.Date;
        return await _context.Invoices
            .Include(i => i.Company)
            .Include(i => i.Contract)
            .Include(i => i.Items)
            .Where(i => i.DueDate < today && i.Status != "PAID" && i.Status != "CANCELLED" && i.IsActive)
            .OrderBy(i => i.DueDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Invoice>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await _context.Invoices
            .Include(i => i.Company)
            .Include(i => i.Contract)
            .Include(i => i.Audit)
            .Include(i => i.Items)
            .Include(i => i.Payments)
            .Include(i => i.Taxes)
            .Where(i => i.InvoiceDate >= startDate && i.InvoiceDate <= endDate && i.IsActive)
            .OrderByDescending(i => i.InvoiceDate)
            .ToListAsync();
    }

    public async Task<Invoice> CreateAsync(Invoice invoice)
    {
        _context.Invoices.Add(invoice);
        await _context.SaveChangesAsync();
        return invoice;
    }

    public async Task<Invoice> UpdateAsync(Invoice invoice)
    {
        invoice.ModifiedDate = DateTime.UtcNow;
        _context.Invoices.Update(invoice);
        await _context.SaveChangesAsync();
        return invoice;
    }

    public async Task DeleteAsync(int id)
    {
        var invoice = await GetByIdAsync(id);
        if (invoice != null)
        {
            invoice.IsActive = false;
            await UpdateAsync(invoice);
        }
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Invoices.AnyAsync(i => i.Id == id && i.IsActive);
    }
}

public class PaymentRepository : IPaymentRepository
{
    private readonly FinancialDbContext _context;

    public PaymentRepository(FinancialDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Payment>> GetAllAsync()
    {
        return await _context.Payments
            .Include(p => p.Invoice)
                .ThenInclude(i => i.Company)
            .Include(p => p.PaymentMethodDetails)
            .OrderByDescending(p => p.CreatedDate)
            .ToListAsync();
    }

    public async Task<Payment?> GetByIdAsync(int id)
    {
        return await _context.Payments
            .Include(p => p.Invoice)
                .ThenInclude(i => i.Company)
            .Include(p => p.PaymentMethodDetails)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<IEnumerable<Payment>> GetByInvoiceIdAsync(int invoiceId)
    {
        return await _context.Payments
            .Include(p => p.Invoice)
            .Include(p => p.PaymentMethodDetails)
            .Where(p => p.InvoiceId == invoiceId)
            .OrderByDescending(p => p.PaymentDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Payment>> GetByTransactionIdAsync(string transactionId)
    {
        return await _context.Payments
            .Include(p => p.Invoice)
            .Include(p => p.PaymentMethodDetails)
            .Where(p => p.TransactionId == transactionId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Payment>> GetByStatusAsync(string status)
    {
        return await _context.Payments
            .Include(p => p.Invoice)
                .ThenInclude(i => i.Company)
            .Include(p => p.PaymentMethodDetails)
            .Where(p => p.Status == status)
            .OrderByDescending(p => p.CreatedDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Payment>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await _context.Payments
            .Include(p => p.Invoice)
                .ThenInclude(i => i.Company)
            .Include(p => p.PaymentMethodDetails)
            .Where(p => p.PaymentDate >= startDate && p.PaymentDate <= endDate)
            .OrderByDescending(p => p.PaymentDate)
            .ToListAsync();
    }

    public async Task<Payment> CreateAsync(Payment payment)
    {
        _context.Payments.Add(payment);
        await _context.SaveChangesAsync();
        return payment;
    }

    public async Task<Payment> UpdateAsync(Payment payment)
    {
        payment.ModifiedDate = DateTime.UtcNow;
        _context.Payments.Update(payment);
        await _context.SaveChangesAsync();
        return payment;
    }

    public async Task DeleteAsync(int id)
    {
        var payment = await GetByIdAsync(id);
        if (payment != null)
        {
            _context.Payments.Remove(payment);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Payments.AnyAsync(p => p.Id == id);
    }
}

public class CompanyRepository : ICompanyRepository
{
    private readonly FinancialDbContext _context;

    public CompanyRepository(FinancialDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Company>> GetAllAsync()
    {
        return await _context.Companies
            .Where(c => c.IsActive)
            .OrderBy(c => c.CompanyName)
            .ToListAsync();
    }

    public async Task<Company?> GetByIdAsync(int id)
    {
        return await _context.Companies
            .FirstOrDefaultAsync(c => c.Id == id && c.IsActive);
    }

    public async Task<Company?> GetByCompanyCodeAsync(string companyCode)
    {
        return await _context.Companies
            .FirstOrDefaultAsync(c => c.CompanyCode == companyCode && c.IsActive);
    }

    public async Task<IEnumerable<Company>> GetActiveCompaniesAsync()
    {
        return await _context.Companies
            .Where(c => c.IsActive)
            .OrderBy(c => c.CompanyName)
            .ToListAsync();
    }

    public async Task<Company> CreateAsync(Company company)
    {
        _context.Companies.Add(company);
        await _context.SaveChangesAsync();
        return company;
    }

    public async Task<Company> UpdateAsync(Company company)
    {
        _context.Companies.Update(company);
        await _context.SaveChangesAsync();
        return company;
    }

    public async Task DeleteAsync(int id)
    {
        var company = await GetByIdAsync(id);
        if (company != null)
        {
            company.IsActive = false;
            await UpdateAsync(company);
        }
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Companies.AnyAsync(c => c.Id == id && c.IsActive);
    }
}