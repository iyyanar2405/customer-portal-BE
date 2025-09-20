using AutoMapper;
using CustomerPortal.FinancialService.Models;
using CustomerPortal.FinancialService.Repositories;

namespace CustomerPortal.FinancialService.GraphQL;

// Input Types for Mutations
public class CreateInvoiceInput
{
    public int CompanyId { get; set; }
    public int? ContractId { get; set; }
    public int? AuditId { get; set; }
    public DateTime InvoiceDate { get; set; }
    public DateTime DueDate { get; set; }
    public string Currency { get; set; } = "USD";
    public string PaymentTerms { get; set; } = "NET_30";
    public string Notes { get; set; } = string.Empty;
    public List<CreateInvoiceItemInput> Items { get; set; } = new();
}

public class CreateInvoiceItemInput
{
    public int? ServiceId { get; set; }
    public string Description { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TaxRate { get; set; }
}

public class RecordPaymentInput
{
    public int InvoiceId { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "USD";
    public string PaymentMethod { get; set; } = string.Empty;
    public string TransactionId { get; set; } = string.Empty;
    public DateTime PaymentDate { get; set; }
    public string Notes { get; set; } = string.Empty;
    public PaymentMethodDetailsInput? PaymentMethodDetails { get; set; }
}

public class PaymentMethodDetailsInput
{
    public string Type { get; set; } = string.Empty;
    public string Last4 { get; set; } = string.Empty;
    public string ExpiryDate { get; set; } = string.Empty;
    public string BankName { get; set; } = string.Empty;
}

public class Mutation
{
    private readonly IMapper _mapper;

    public Mutation(IMapper mapper)
    {
        _mapper = mapper;
    }

    public async Task<InvoiceGraphQLType> CreateInvoiceAsync(
        CreateInvoiceInput input,
        [Service] IInvoiceRepository invoiceRepository,
        [Service] ITaxRateRepository taxRateRepository)
    {
        // Generate invoice number
        var invoiceNumber = $"INV-{DateTime.UtcNow:yyyy}-{new Random().Next(1000, 9999)}";

        // Calculate totals
        var subtotal = input.Items.Sum(item => item.Quantity * item.UnitPrice);
        var taxAmount = input.Items.Sum(item => item.Quantity * item.UnitPrice * (item.TaxRate / 100));
        var totalAmount = subtotal + taxAmount;

        var invoice = new Invoice
        {
            InvoiceNumber = invoiceNumber,
            CompanyId = input.CompanyId,
            ContractId = input.ContractId,
            AuditId = input.AuditId,
            InvoiceDate = input.InvoiceDate,
            DueDate = input.DueDate,
            Subtotal = subtotal,
            TaxAmount = taxAmount,
            TotalAmount = totalAmount,
            Currency = input.Currency,
            Status = "DRAFT",
            PaymentTerms = input.PaymentTerms,
            Notes = input.Notes,
            IsActive = true,
            CreatedDate = DateTime.UtcNow
        };

        // Add invoice items
        foreach (var itemInput in input.Items)
        {
            var lineTotal = itemInput.Quantity * itemInput.UnitPrice;
            invoice.Items.Add(new InvoiceItem
            {
                ServiceId = itemInput.ServiceId,
                Description = itemInput.Description,
                Quantity = itemInput.Quantity,
                UnitPrice = itemInput.UnitPrice,
                TaxRate = itemInput.TaxRate,
                LineTotal = lineTotal,
                CreatedDate = DateTime.UtcNow
            });
        }

        // Add tax details
        var taxByType = input.Items
            .GroupBy(item => item.TaxRate)
            .Where(g => g.Key > 0)
            .Select(g => new InvoiceTax
            {
                TaxType = "VAT", // Simplified - in real world, determine from tax rate lookup
                TaxRate = g.Key,
                TaxableAmount = g.Sum(item => item.Quantity * item.UnitPrice),
                TaxAmount = g.Sum(item => item.Quantity * item.UnitPrice * (item.TaxRate / 100)),
                CreatedDate = DateTime.UtcNow
            });

        foreach (var tax in taxByType)
        {
            invoice.Taxes.Add(tax);
        }

        var createdInvoice = await invoiceRepository.CreateAsync(invoice);
        return _mapper.Map<InvoiceGraphQLType>(createdInvoice);
    }

    public async Task<InvoiceGraphQLType> UpdateInvoiceStatusAsync(
        int invoiceId,
        string status,
        string? notes,
        [Service] IInvoiceRepository invoiceRepository)
    {
        var invoice = await invoiceRepository.GetByIdAsync(invoiceId);
        if (invoice == null)
        {
            throw new ArgumentException($"Invoice with ID {invoiceId} not found");
        }

        invoice.Status = status;
        if (!string.IsNullOrEmpty(notes))
        {
            invoice.Notes = notes;
        }

        if (status == "PAID" && invoice.PaidDate == null)
        {
            invoice.PaidDate = DateTime.UtcNow;
        }

        var updatedInvoice = await invoiceRepository.UpdateAsync(invoice);
        return _mapper.Map<InvoiceGraphQLType>(updatedInvoice);
    }

    public async Task<PaymentGraphQLType> RecordPaymentAsync(
        RecordPaymentInput input,
        [Service] IPaymentRepository paymentRepository,
        [Service] IInvoiceRepository invoiceRepository)
    {
        // Verify invoice exists
        var invoice = await invoiceRepository.GetByIdAsync(input.InvoiceId);
        if (invoice == null)
        {
            throw new ArgumentException($"Invoice with ID {input.InvoiceId} not found");
        }

        var payment = new Payment
        {
            InvoiceId = input.InvoiceId,
            PaymentDate = input.PaymentDate,
            Amount = input.Amount,
            Currency = input.Currency,
            PaymentMethod = input.PaymentMethod,
            TransactionId = input.TransactionId,
            Status = "COMPLETED",
            ProcessingFee = input.Amount * 0.029m, // Example: 2.9% processing fee
            Notes = input.Notes,
            CreatedDate = DateTime.UtcNow
        };

        var createdPayment = await paymentRepository.CreateAsync(payment);

        // Add payment method details if provided
        if (input.PaymentMethodDetails != null)
        {
            var paymentMethodDetails = new PaymentMethod
            {
                PaymentId = createdPayment.Id,
                Type = input.PaymentMethodDetails.Type,
                Last4 = input.PaymentMethodDetails.Last4,
                ExpiryDate = input.PaymentMethodDetails.ExpiryDate,
                BankName = input.PaymentMethodDetails.BankName,
                CreatedDate = DateTime.UtcNow
            };
            
            createdPayment.PaymentMethodDetails = paymentMethodDetails;
        }

        // Check if invoice is fully paid
        var totalPayments = invoice.Payments.Sum(p => p.Amount) + createdPayment.Amount;
        if (totalPayments >= invoice.TotalAmount)
        {
            invoice.Status = "PAID";
            invoice.PaidDate = input.PaymentDate;
            await invoiceRepository.UpdateAsync(invoice);
        }

        return _mapper.Map<PaymentGraphQLType>(createdPayment);
    }

    public async Task<PaymentGraphQLType> ProcessRefundAsync(
        int paymentId,
        decimal amount,
        string reason,
        [Service] IPaymentRepository paymentRepository)
    {
        var payment = await paymentRepository.GetByIdAsync(paymentId);
        if (payment == null)
        {
            throw new ArgumentException($"Payment with ID {paymentId} not found");
        }

        if (amount > payment.Amount)
        {
            throw new ArgumentException("Refund amount cannot exceed original payment amount");
        }

        // Create refund record (simplified - in real world, this would be a separate entity)
        var refund = new Payment
        {
            InvoiceId = payment.InvoiceId,
            PaymentDate = DateTime.UtcNow,
            Amount = -amount, // Negative amount for refund
            Currency = payment.Currency,
            PaymentMethod = payment.PaymentMethod,
            TransactionId = $"REFUND-{payment.TransactionId}",
            Status = "COMPLETED",
            ProcessingFee = 0,
            Notes = $"Refund for payment {payment.TransactionId}. Reason: {reason}",
            CreatedDate = DateTime.UtcNow
        };

        var createdRefund = await paymentRepository.CreateAsync(refund);
        return _mapper.Map<PaymentGraphQLType>(createdRefund);
    }

    public async Task<InvoiceGraphQLType> SendInvoiceAsync(
        int invoiceId,
        List<string> recipientEmails,
        [Service] IInvoiceRepository invoiceRepository)
    {
        var invoice = await invoiceRepository.GetByIdAsync(invoiceId);
        if (invoice == null)
        {
            throw new ArgumentException($"Invoice with ID {invoiceId} not found");
        }

        // In a real implementation, this would integrate with an email service
        // For now, just update the status to SENT
        invoice.Status = "SENT";
        invoice.Notes += $"\nSent to: {string.Join(", ", recipientEmails)} on {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}";

        var updatedInvoice = await invoiceRepository.UpdateAsync(invoice);
        return _mapper.Map<InvoiceGraphQLType>(updatedInvoice);
    }

    public async Task<CurrencyConversionGraphQLType> ConvertCurrencyAsync(
        decimal amount,
        string fromCurrency,
        string toCurrency,
        DateTime? date)
    {
        // Simplified currency conversion - in real world, this would use a currency exchange API
        var exchangeRates = new Dictionary<string, decimal>
        {
            {"USD", 1.0m},
            {"EUR", 0.85m},
            {"GBP", 0.73m},
            {"CAD", 1.25m}
        };

        var fromRate = exchangeRates.GetValueOrDefault(fromCurrency, 1.0m);
        var toRate = exchangeRates.GetValueOrDefault(toCurrency, 1.0m);
        var exchangeRate = toRate / fromRate;
        var convertedAmount = amount * exchangeRate;

        return new CurrencyConversionGraphQLType
        {
            OriginalAmount = amount,
            ConvertedAmount = convertedAmount,
            FromCurrency = fromCurrency,
            ToCurrency = toCurrency,
            ExchangeRate = exchangeRate,
            ConversionDate = date ?? DateTime.UtcNow
        };
    }

    // Alias methods for compatibility with documentation
    public async Task<InvoiceGraphQLType> CreateInvoice(
        CreateInvoiceInput input,
        [Service] IInvoiceRepository invoiceRepository,
        [Service] ITaxRateRepository taxRateRepository) =>
        await CreateInvoiceAsync(input, invoiceRepository, taxRateRepository);

    public async Task<InvoiceGraphQLType> UpdateInvoiceStatus(
        int invoiceId,
        string status,
        string? notes,
        [Service] IInvoiceRepository invoiceRepository) =>
        await UpdateInvoiceStatusAsync(invoiceId, status, notes, invoiceRepository);

    public async Task<PaymentGraphQLType> RecordPayment(
        RecordPaymentInput input,
        [Service] IPaymentRepository paymentRepository,
        [Service] IInvoiceRepository invoiceRepository) =>
        await RecordPaymentAsync(input, paymentRepository, invoiceRepository);

    public async Task<PaymentGraphQLType> ProcessRefund(
        int paymentId,
        decimal amount,
        string reason,
        [Service] IPaymentRepository paymentRepository) =>
        await ProcessRefundAsync(paymentId, amount, reason, paymentRepository);

    public async Task<InvoiceGraphQLType> SendInvoice(
        int invoiceId,
        List<string> recipientEmails,
        [Service] IInvoiceRepository invoiceRepository) =>
        await SendInvoiceAsync(invoiceId, recipientEmails, invoiceRepository);

    public async Task<CurrencyConversionGraphQLType> ConvertCurrency(
        decimal amount,
        string fromCurrency,
        string toCurrency,
        DateTime? date) =>
        await ConvertCurrencyAsync(amount, fromCurrency, toCurrency, date);
}