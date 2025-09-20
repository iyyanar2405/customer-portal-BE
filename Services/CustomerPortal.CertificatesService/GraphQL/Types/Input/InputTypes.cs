namespace CustomerPortal.CertificatesService.GraphQL.Types.Input
{
    public class CreateCertificateInput
    {
        public int CompanyId { get; set; }
        public int CertificateTypeId { get; set; }
        public int? AuditId { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public DateTime? RenewalDate { get; set; }
    }

    public class UpdateCertificateInput
    {
        public int CertificateId { get; set; }
        public DateTime? IssueDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public DateTime? RenewalDate { get; set; }
        public string? Status { get; set; }
    }

    public class CreateCertificateTypeInput
    {
        public string TypeName { get; set; } = string.Empty;
        public string Standard { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int ValidityPeriodMonths { get; set; }
        public bool IsAccredited { get; set; }
    }

    public class UpdateCertificateTypeInput
    {
        public int Id { get; set; }
        public string? TypeName { get; set; }
        public string? Standard { get; set; }
        public string? Category { get; set; }
        public string? Description { get; set; }
        public int? ValidityPeriodMonths { get; set; }
        public bool? IsAccredited { get; set; }
    }

    public class CreateCompanyInput
    {
        public string CompanyCode { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        public string? ContactPerson { get; set; }
        public string? Address { get; set; }
    }

    public class UpdateCompanyInput
    {
        public int Id { get; set; }
        public string? CompanyCode { get; set; }
        public string? CompanyName { get; set; }
        public string? ContactPerson { get; set; }
        public string? Address { get; set; }
    }

    public class CreateUserInput
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string? Department { get; set; }
    }

    public class UpdateUserInput
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Role { get; set; }
        public string? Department { get; set; }
    }
}