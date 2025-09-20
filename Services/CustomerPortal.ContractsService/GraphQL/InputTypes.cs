namespace CustomerPortal.ContractsService.GraphQL;

public class CreateContractInput
{
    public int CompanyId { get; set; }
    public string ContractType { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal Value { get; set; }
    public string Currency { get; set; } = "USD";
    public string PaymentTerms { get; set; } = "NET_30";
    public List<CreateContractServiceInput>? Services { get; set; }
    public List<int>? Sites { get; set; }
    public List<CreateContractTermInput>? Terms { get; set; }
}

public class CreateContractServiceInput
{
    public int ServiceId { get; set; }
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }
}

public class CreateContractTermInput
{
    public string TermType { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? Value { get; set; }
    public string? Unit { get; set; }
    public bool IsRequired { get; set; } = false;
    public DateTime? EffectiveDate { get; set; }
    public DateTime? ExpiryDate { get; set; }
}

public class UpdateContractInput
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public decimal? Value { get; set; }
    public string? PaymentTerms { get; set; }
}

public class StartRenewalInput
{
    public DateTime ProposedStartDate { get; set; }
    public DateTime ProposedEndDate { get; set; }
    public decimal ProposedValue { get; set; }
    public bool AutoRenewal { get; set; } = false;
}

public class CreateAmendmentInput
{
    public int ContractId { get; set; }
    public string Description { get; set; } = string.Empty;
    public string AmendmentType { get; set; } = string.Empty;
    public DateTime EffectiveDate { get; set; }
    public decimal? ValueChange { get; set; }
}

public class AddContractTermInput
{
    public int ContractId { get; set; }
    public string TermType { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? Value { get; set; }
    public string? Unit { get; set; }
    public bool IsRequired { get; set; } = false;
    public DateTime? EffectiveDate { get; set; }
    public DateTime? ExpiryDate { get; set; }
}

public class ReportComplianceIssueInput
{
    public int ContractId { get; set; }
    public string IssueType { get; set; } = string.Empty;
    public string Severity { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime DueDate { get; set; }
    public string? AssignedTo { get; set; }
}

public class CreateCompanyInput
{
    public string CompanyName { get; set; } = string.Empty;
    public string CompanyCode { get; set; } = string.Empty;
    public string? ContactPerson { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Address { get; set; }
}

public class CreateServiceInput
{
    public string ServiceName { get; set; } = string.Empty;
    public string ServiceCode { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Category { get; set; } = string.Empty;
}

public class CreateSiteInput
{
    public int CompanyId { get; set; }
    public string SiteName { get; set; } = string.Empty;
    public string SiteCode { get; set; } = string.Empty;
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
    public string? PostalCode { get; set; }
}