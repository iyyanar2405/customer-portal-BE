using System.ComponentModel.DataAnnotations;

namespace CustomerPortal.FinancialService.Models;

public class Company
{
    public int Id { get; set; }
    
    [MaxLength(100)]
    public string CompanyName { get; set; } = string.Empty;
    
    [MaxLength(20)]
    public string CompanyCode { get; set; } = string.Empty;
    
    [MaxLength(200)]
    public string Address { get; set; } = string.Empty;
    
    [MaxLength(100)]
    public string Email { get; set; } = string.Empty;
    
    [MaxLength(20)]
    public string Phone { get; set; } = string.Empty;
    
    [MaxLength(100)]
    public string ContactPerson { get; set; } = string.Empty;
    
    public bool IsActive { get; set; } = true;
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    
    // Navigation properties
    public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();
}

public class Service
{
    public int Id { get; set; }
    
    [MaxLength(100)]
    public string ServiceName { get; set; } = string.Empty;
    
    [MaxLength(20)]
    public string ServiceCode { get; set; } = string.Empty;
    
    [MaxLength(500)]
    public string Description { get; set; } = string.Empty;
    
    [MaxLength(50)]
    public string Category { get; set; } = string.Empty;
    
    public bool IsActive { get; set; } = true;
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    
    // Navigation properties
    public virtual ICollection<InvoiceItem> InvoiceItems { get; set; } = new List<InvoiceItem>();
}

public class Contract
{
    public int Id { get; set; }
    
    [MaxLength(50)]
    public string ContractNumber { get; set; } = string.Empty;
    
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;
    
    public int CompanyId { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    
    // Navigation properties
    public virtual Company Company { get; set; } = null!;
    public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();
}

public class Audit
{
    public int Id { get; set; }
    
    [MaxLength(50)]
    public string AuditNumber { get; set; } = string.Empty;
    
    [MaxLength(200)]
    public string AuditTitle { get; set; } = string.Empty;
    
    public int CompanyId { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    
    // Navigation properties
    public virtual Company Company { get; set; } = null!;
    public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();
}

public class Invoice
{
    public int Id { get; set; }
    
    [MaxLength(50)]
    public string InvoiceNumber { get; set; } = string.Empty;
    
    public int CompanyId { get; set; }
    public int? ContractId { get; set; }
    public int? AuditId { get; set; }
    
    public DateTime InvoiceDate { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime? PaidDate { get; set; }
    
    public decimal Subtotal { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal TotalAmount { get; set; }
    
    [MaxLength(10)]
    public string Currency { get; set; } = "USD";
    
    [MaxLength(20)]
    public string Status { get; set; } = "DRAFT"; // DRAFT, SENT, VIEWED, PAID, OVERDUE, CANCELLED, REFUNDED
    
    [MaxLength(20)]
    public string PaymentTerms { get; set; } = "NET_30"; // NET_15, NET_30, NET_45, NET_60
    
    [MaxLength(1000)]
    public string Notes { get; set; } = string.Empty;
    
    public bool IsActive { get; set; } = true;
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public DateTime? ModifiedDate { get; set; }
    
    // Navigation properties
    public virtual Company Company { get; set; } = null!;
    public virtual Contract? Contract { get; set; }
    public virtual Audit? Audit { get; set; }
    public virtual ICollection<InvoiceItem> Items { get; set; } = new List<InvoiceItem>();
    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
    public virtual ICollection<InvoiceTax> Taxes { get; set; } = new List<InvoiceTax>();
}

public class InvoiceItem
{
    public int Id { get; set; }
    
    public int InvoiceId { get; set; }
    public int? ServiceId { get; set; }
    
    [MaxLength(500)]
    public string Description { get; set; } = string.Empty;
    
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TaxRate { get; set; }
    public decimal LineTotal { get; set; }
    
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    
    // Navigation properties
    public virtual Invoice Invoice { get; set; } = null!;
    public virtual Service? Service { get; set; }
}

public class Payment
{
    public int Id { get; set; }
    
    public int InvoiceId { get; set; }
    
    public DateTime PaymentDate { get; set; }
    public decimal Amount { get; set; }
    
    [MaxLength(10)]
    public string Currency { get; set; } = "USD";
    
    [MaxLength(30)]
    public string PaymentMethod { get; set; } = string.Empty; // CREDIT_CARD, BANK_TRANSFER, CHECK, CASH
    
    [MaxLength(100)]
    public string TransactionId { get; set; } = string.Empty;
    
    [MaxLength(20)]
    public string Status { get; set; } = "PENDING"; // PENDING, COMPLETED, FAILED, REFUNDED
    
    public decimal ProcessingFee { get; set; }
    
    [MaxLength(1000)]
    public string Notes { get; set; } = string.Empty;
    
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public DateTime? ModifiedDate { get; set; }
    
    // Navigation properties
    public virtual Invoice Invoice { get; set; } = null!;
    public virtual PaymentMethod? PaymentMethodDetails { get; set; }
}

public class PaymentMethod
{
    public int Id { get; set; }
    
    public int PaymentId { get; set; }
    
    [MaxLength(20)]
    public string Type { get; set; } = string.Empty; // VISA, MASTERCARD, AMEX, etc.
    
    [MaxLength(4)]
    public string Last4 { get; set; } = string.Empty;
    
    [MaxLength(7)]
    public string ExpiryDate { get; set; } = string.Empty; // MM/YY
    
    [MaxLength(100)]
    public string BankName { get; set; } = string.Empty;
    
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    
    // Navigation properties
    public virtual Payment Payment { get; set; } = null!;
}

public class TaxRate
{
    public int Id { get; set; }
    
    [MaxLength(20)]
    public string TaxType { get; set; } = string.Empty; // VAT, GST, SALES_TAX
    
    public decimal Rate { get; set; }
    
    public int CountryId { get; set; }
    
    [MaxLength(100)]
    public string Region { get; set; } = string.Empty;
    
    public DateTime EffectiveDate { get; set; }
    public DateTime? ExpiryDate { get; set; }
    
    public bool IsActive { get; set; } = true;
    
    [MaxLength(500)]
    public string Description { get; set; } = string.Empty;
    
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    
    // Navigation properties
    public virtual Country Country { get; set; } = null!;
}

public class Country
{
    public int Id { get; set; }
    
    [MaxLength(100)]
    public string CountryName { get; set; } = string.Empty;
    
    [MaxLength(3)]
    public string CountryCode { get; set; } = string.Empty;
    
    public bool IsActive { get; set; } = true;
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    
    // Navigation properties
    public virtual ICollection<TaxRate> TaxRates { get; set; } = new List<TaxRate>();
}

public class InvoiceTax
{
    public int Id { get; set; }
    
    public int InvoiceId { get; set; }
    
    [MaxLength(20)]
    public string TaxType { get; set; } = string.Empty;
    
    public decimal TaxRate { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal TaxableAmount { get; set; }
    
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    
    // Navigation properties
    public virtual Invoice Invoice { get; set; } = null!;
}