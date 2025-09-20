using Microsoft.EntityFrameworkCore;
using CustomerPortal.FinancialService.Models;

namespace CustomerPortal.FinancialService.Data;

public class FinancialDbContext : DbContext
{
    public FinancialDbContext(DbContextOptions<FinancialDbContext> options) : base(options)
    {
    }

    public DbSet<Company> Companies { get; set; }
    public DbSet<Service> Services { get; set; }
    public DbSet<Contract> Contracts { get; set; }
    public DbSet<Audit> Audits { get; set; }
    public DbSet<Invoice> Invoices { get; set; }
    public DbSet<InvoiceItem> InvoiceItems { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<PaymentMethod> PaymentMethods { get; set; }
    public DbSet<TaxRate> TaxRates { get; set; }
    public DbSet<Country> Countries { get; set; }
    public DbSet<InvoiceTax> InvoiceTaxes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Company configuration
        modelBuilder.Entity<Company>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.CompanyCode).IsUnique();
            entity.HasIndex(e => e.CompanyName);
            entity.Property(e => e.CompanyName).IsRequired();
            entity.Property(e => e.CompanyCode).IsRequired();
        });

        // Service configuration
        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.ServiceCode).IsUnique();
            entity.HasIndex(e => e.ServiceName);
            entity.HasIndex(e => e.Category);
            entity.Property(e => e.ServiceName).IsRequired();
            entity.Property(e => e.ServiceCode).IsRequired();
        });

        // Contract configuration
        modelBuilder.Entity<Contract>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.ContractNumber).IsUnique();
            entity.HasIndex(e => e.CompanyId);
            entity.Property(e => e.ContractNumber).IsRequired();
            entity.Property(e => e.Title).IsRequired();

            entity.HasOne(e => e.Company)
                .WithMany()
                .HasForeignKey(e => e.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Audit configuration
        modelBuilder.Entity<Audit>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.AuditNumber).IsUnique();
            entity.HasIndex(e => e.CompanyId);
            entity.Property(e => e.AuditNumber).IsRequired();
            entity.Property(e => e.AuditTitle).IsRequired();

            entity.HasOne(e => e.Company)
                .WithMany()
                .HasForeignKey(e => e.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Invoice configuration
        modelBuilder.Entity<Invoice>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.InvoiceNumber).IsUnique();
            entity.HasIndex(e => e.CompanyId);
            entity.HasIndex(e => e.ContractId);
            entity.HasIndex(e => e.AuditId);
            entity.HasIndex(e => e.Status);
            entity.HasIndex(e => e.DueDate);
            entity.Property(e => e.InvoiceNumber).IsRequired();
            entity.Property(e => e.Subtotal).HasPrecision(18, 2);
            entity.Property(e => e.TaxAmount).HasPrecision(18, 2);
            entity.Property(e => e.TotalAmount).HasPrecision(18, 2);

            entity.HasOne(e => e.Company)
                .WithMany(c => c.Invoices)
                .HasForeignKey(e => e.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Contract)
                .WithMany(c => c.Invoices)
                .HasForeignKey(e => e.ContractId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Audit)
                .WithMany(a => a.Invoices)
                .HasForeignKey(e => e.AuditId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // InvoiceItem configuration
        modelBuilder.Entity<InvoiceItem>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.InvoiceId);
            entity.HasIndex(e => e.ServiceId);
            entity.Property(e => e.UnitPrice).HasPrecision(18, 2);
            entity.Property(e => e.TaxRate).HasPrecision(5, 2);
            entity.Property(e => e.LineTotal).HasPrecision(18, 2);

            entity.HasOne(e => e.Invoice)
                .WithMany(i => i.Items)
                .HasForeignKey(e => e.InvoiceId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Service)
                .WithMany(s => s.InvoiceItems)
                .HasForeignKey(e => e.ServiceId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Payment configuration
        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.InvoiceId);
            entity.HasIndex(e => e.TransactionId);
            entity.HasIndex(e => e.Status);
            entity.Property(e => e.Amount).HasPrecision(18, 2);
            entity.Property(e => e.ProcessingFee).HasPrecision(18, 2);

            entity.HasOne(e => e.Invoice)
                .WithMany(i => i.Payments)
                .HasForeignKey(e => e.InvoiceId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // PaymentMethod configuration
        modelBuilder.Entity<PaymentMethod>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.PaymentId).IsUnique();

            entity.HasOne(e => e.Payment)
                .WithOne(p => p.PaymentMethodDetails)
                .HasForeignKey<PaymentMethod>(e => e.PaymentId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Country configuration
        modelBuilder.Entity<Country>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.CountryCode).IsUnique();
            entity.HasIndex(e => e.CountryName);
            entity.Property(e => e.CountryName).IsRequired();
            entity.Property(e => e.CountryCode).IsRequired();
        });

        // TaxRate configuration
        modelBuilder.Entity<TaxRate>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.CountryId);
            entity.HasIndex(e => e.TaxType);
            entity.Property(e => e.Rate).HasPrecision(5, 2);

            entity.HasOne(e => e.Country)
                .WithMany(c => c.TaxRates)
                .HasForeignKey(e => e.CountryId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // InvoiceTax configuration
        modelBuilder.Entity<InvoiceTax>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.InvoiceId);
            entity.Property(e => e.TaxRate).HasPrecision(5, 2);
            entity.Property(e => e.TaxAmount).HasPrecision(18, 2);
            entity.Property(e => e.TaxableAmount).HasPrecision(18, 2);

            entity.HasOne(e => e.Invoice)
                .WithMany(i => i.Taxes)
                .HasForeignKey(e => e.InvoiceId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Seed data
        SeedData(modelBuilder);
    }

    private void SeedData(ModelBuilder modelBuilder)
    {
        // Seed Countries
        modelBuilder.Entity<Country>().HasData(
            new Country { Id = 1, CountryName = "United States", CountryCode = "US", IsActive = true, CreatedDate = DateTime.UtcNow },
            new Country { Id = 2, CountryName = "Canada", CountryCode = "CA", IsActive = true, CreatedDate = DateTime.UtcNow },
            new Country { Id = 3, CountryName = "United Kingdom", CountryCode = "GB", IsActive = true, CreatedDate = DateTime.UtcNow }
        );

        // Seed Companies
        modelBuilder.Entity<Company>().HasData(
            new Company 
            { 
                Id = 1, 
                CompanyName = "Acme Corporation", 
                CompanyCode = "ACME001", 
                Address = "123 Business St, Business City",
                Email = "accounts@acme.com",
                Phone = "+1-555-0123",
                ContactPerson = "John Doe",
                IsActive = true, 
                CreatedDate = DateTime.UtcNow 
            },
            new Company 
            { 
                Id = 2, 
                CompanyName = "Global Industries Ltd", 
                CompanyCode = "GLOB001", 
                Address = "456 Corporate Ave, Metro City",
                Email = "billing@global.com",
                Phone = "+1-555-0200",
                ContactPerson = "Jane Smith",
                IsActive = true, 
                CreatedDate = DateTime.UtcNow 
            }
        );

        // Seed Services
        modelBuilder.Entity<Service>().HasData(
            new Service 
            { 
                Id = 1, 
                ServiceName = "ISO 9001 Audit", 
                ServiceCode = "ISO9001", 
                Description = "Quality Management System Audit",
                Category = "CERTIFICATION",
                IsActive = true, 
                CreatedDate = DateTime.UtcNow 
            },
            new Service 
            { 
                Id = 2, 
                ServiceName = "ISO 14001 Audit", 
                ServiceCode = "ISO14001", 
                Description = "Environmental Management System Audit",
                Category = "CERTIFICATION",
                IsActive = true, 
                CreatedDate = DateTime.UtcNow 
            },
            new Service 
            { 
                Id = 3, 
                ServiceName = "Quality Training", 
                ServiceCode = "QMT001", 
                Description = "Comprehensive quality management training",
                Category = "TRAINING",
                IsActive = true, 
                CreatedDate = DateTime.UtcNow 
            }
        );

        // Seed Contracts
        modelBuilder.Entity<Contract>().HasData(
            new Contract
            {
                Id = 1,
                ContractNumber = "CNT-2024-001",
                Title = "ISO Management System Certification Contract",
                CompanyId = 1,
                IsActive = true,
                CreatedDate = DateTime.UtcNow
            },
            new Contract
            {
                Id = 2,
                ContractNumber = "CNT-2024-002",
                Title = "Quality Management Training Contract",
                CompanyId = 2,
                IsActive = true,
                CreatedDate = DateTime.UtcNow
            }
        );

        // Seed Audits
        modelBuilder.Entity<Audit>().HasData(
            new Audit
            {
                Id = 1,
                AuditNumber = "AUD-2024-001",
                AuditTitle = "ISO 9001:2015 Annual Surveillance Audit",
                CompanyId = 1,
                IsActive = true,
                CreatedDate = DateTime.UtcNow
            },
            new Audit
            {
                Id = 2,
                AuditNumber = "AUD-2024-002",
                AuditTitle = "ISO 14001:2015 Initial Certification Audit",
                CompanyId = 2,
                IsActive = true,
                CreatedDate = DateTime.UtcNow
            }
        );

        // Seed Tax Rates
        modelBuilder.Entity<TaxRate>().HasData(
            new TaxRate
            {
                Id = 1,
                TaxType = "VAT",
                Rate = 10.0m,
                CountryId = 1,
                Region = "Standard Rate",
                EffectiveDate = new DateTime(2024, 1, 1),
                Description = "Standard VAT rate for services",
                IsActive = true,
                CreatedDate = DateTime.UtcNow
            },
            new TaxRate
            {
                Id = 2,
                TaxType = "GST",
                Rate = 12.0m,
                CountryId = 2,
                Region = "Federal",
                EffectiveDate = new DateTime(2024, 1, 1),
                Description = "Goods and Services Tax",
                IsActive = true,
                CreatedDate = DateTime.UtcNow
            }
        );

        // Seed Invoices
        modelBuilder.Entity<Invoice>().HasData(
            new Invoice
            {
                Id = 1,
                InvoiceNumber = "INV-2024-001",
                CompanyId = 1,
                ContractId = 1,
                AuditId = 1,
                InvoiceDate = new DateTime(2024, 9, 1),
                DueDate = new DateTime(2024, 10, 1),
                Subtotal = 15000.00m,
                TaxAmount = 1500.00m,
                TotalAmount = 16500.00m,
                Currency = "USD",
                Status = "SENT",
                PaymentTerms = "NET_30",
                Notes = "Payment for ISO 9001:2015 surveillance audit",
                IsActive = true,
                CreatedDate = DateTime.UtcNow
            },
            new Invoice
            {
                Id = 2,
                InvoiceNumber = "INV-2024-002",
                CompanyId = 2,
                ContractId = 2,
                AuditId = 2,
                InvoiceDate = new DateTime(2024, 9, 15),
                DueDate = new DateTime(2024, 10, 15),
                Subtotal = 18000.00m,
                TaxAmount = 2160.00m,
                TotalAmount = 20160.00m,
                Currency = "USD",
                Status = "PAID",
                PaymentTerms = "NET_30",
                Notes = "Payment for ISO 14001:2015 certification audit",
                IsActive = true,
                CreatedDate = DateTime.UtcNow,
                PaidDate = new DateTime(2024, 10, 10)
            }
        );

        // Seed Invoice Items
        modelBuilder.Entity<InvoiceItem>().HasData(
            new InvoiceItem
            {
                Id = 1,
                InvoiceId = 1,
                ServiceId = 1,
                Description = "ISO 9001:2015 Surveillance Audit - Main Production Facility",
                Quantity = 1,
                UnitPrice = 15000.00m,
                TaxRate = 10.0m,
                LineTotal = 15000.00m,
                CreatedDate = DateTime.UtcNow
            },
            new InvoiceItem
            {
                Id = 2,
                InvoiceId = 2,
                ServiceId = 2,
                Description = "ISO 14001:2015 Initial Certification Audit",
                Quantity = 1,
                UnitPrice = 18000.00m,
                TaxRate = 12.0m,
                LineTotal = 18000.00m,
                CreatedDate = DateTime.UtcNow
            }
        );

        // Seed Invoice Taxes
        modelBuilder.Entity<InvoiceTax>().HasData(
            new InvoiceTax
            {
                Id = 1,
                InvoiceId = 1,
                TaxType = "VAT",
                TaxRate = 10.0m,
                TaxAmount = 1500.00m,
                TaxableAmount = 15000.00m,
                CreatedDate = DateTime.UtcNow
            },
            new InvoiceTax
            {
                Id = 2,
                InvoiceId = 2,
                TaxType = "GST",
                TaxRate = 12.0m,
                TaxAmount = 2160.00m,
                TaxableAmount = 18000.00m,
                CreatedDate = DateTime.UtcNow
            }
        );

        // Seed Payments
        modelBuilder.Entity<Payment>().HasData(
            new Payment
            {
                Id = 1,
                InvoiceId = 2,
                PaymentDate = new DateTime(2024, 10, 10),
                Amount = 20160.00m,
                Currency = "USD",
                PaymentMethod = "CREDIT_CARD",
                TransactionId = "TXN-20241010-001",
                Status = "COMPLETED",
                ProcessingFee = 201.60m,
                Notes = "Payment processed via Stripe",
                CreatedDate = DateTime.UtcNow
            }
        );

        // Seed Payment Methods
        modelBuilder.Entity<PaymentMethod>().HasData(
            new PaymentMethod
            {
                Id = 1,
                PaymentId = 1,
                Type = "VISA",
                Last4 = "1234",
                ExpiryDate = "12/26",
                BankName = "First National Bank",
                CreatedDate = DateTime.UtcNow
            }
        );
    }
}