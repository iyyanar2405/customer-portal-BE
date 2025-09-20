namespace CustomerPortal.ContractsService.GraphQL;

public class ContractGraphQLType
{
    public int Id { get; set; }
    public string ContractNumber { get; set; } = string.Empty;
    public int CompanyId { get; set; }
    public string ContractType { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public DateTime? RenewalDate { get; set; }
    public decimal Value { get; set; }
    public string Currency { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string PaymentTerms { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime ModifiedDate { get; set; }
    public CompanyGraphQLType? Company { get; set; }
    public ICollection<ContractServiceGraphQLType> Services { get; set; } = new List<ContractServiceGraphQLType>();
    public ICollection<ContractSiteGraphQLType> Sites { get; set; } = new List<ContractSiteGraphQLType>();
    public ICollection<ContractTermGraphQLType> Terms { get; set; } = new List<ContractTermGraphQLType>();
    public ICollection<ContractAmendmentGraphQLType> Amendments { get; set; } = new List<ContractAmendmentGraphQLType>();
}

public class CompanyGraphQLType
{
    public int Id { get; set; }
    public string CompanyName { get; set; } = string.Empty;
    public string CompanyCode { get; set; } = string.Empty;
    public string? ContactPerson { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Address { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedDate { get; set; }
}

public class ServiceGraphQLType
{
    public int Id { get; set; }
    public string ServiceName { get; set; } = string.Empty;
    public string ServiceCode { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Category { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime CreatedDate { get; set; }
}

public class SiteGraphQLType
{
    public int Id { get; set; }
    public int CompanyId { get; set; }
    public string SiteName { get; set; } = string.Empty;
    public string SiteCode { get; set; } = string.Empty;
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
    public string? PostalCode { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedDate { get; set; }
}

public class ContractServiceGraphQLType
{
    public int ServiceId { get; set; }
    public ServiceGraphQLType? Service { get; set; }
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }
    public decimal TotalPrice { get; set; }
}

public class ContractSiteGraphQLType
{
    public int SiteId { get; set; }
    public SiteGraphQLType? Site { get; set; }
    public bool IsActive { get; set; }
}

public class ContractTermGraphQLType
{
    public int Id { get; set; }
    public string TermType { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? Value { get; set; }
    public string? Unit { get; set; }
    public bool IsRequired { get; set; }
    public DateTime? EffectiveDate { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public bool IsActive { get; set; }
}

public class ContractAmendmentGraphQLType
{
    public int Id { get; set; }
    public string AmendmentNumber { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime EffectiveDate { get; set; }
    public string AmendmentType { get; set; } = string.Empty;
    public decimal? ValueChange { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? ApprovedBy { get; set; }
    public DateTime? ApprovedDate { get; set; }
    public DateTime CreatedDate { get; set; }
}

public class ContractRenewalGraphQLType
{
    public int Id { get; set; }
    public string RenewalNumber { get; set; } = string.Empty;
    public DateTime ProposedStartDate { get; set; }
    public DateTime ProposedEndDate { get; set; }
    public decimal ProposedValue { get; set; }
    public string Status { get; set; } = string.Empty;
    public bool AutoRenewal { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? CompletedDate { get; set; }
}

// Renewal schedule specific type
public class ContractRenewalScheduleGraphQLType
{
    public int ContractId { get; set; }
    public string ContractNumber { get; set; } = string.Empty;
    public string CompanyName { get; set; } = string.Empty;
    public string ContractType { get; set; } = string.Empty;
    public DateTime CurrentEndDate { get; set; }
    public DateTime? RenewalDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public int DaysUntilRenewal { get; set; }
    public bool AutoRenewal { get; set; }
    public decimal RenewalValue { get; set; }
}

// Expiring contracts specific type
public class ExpiringContractGraphQLType
{
    public int Id { get; set; }
    public string ContractNumber { get; set; } = string.Empty;
    public CompanyGraphQLType? Company { get; set; }
    public string ContractType { get; set; } = string.Empty;
    public DateTime EndDate { get; set; }
    public int DaysUntilExpiry { get; set; }
    public bool RenewalRequired { get; set; }
    public bool AutoRenewal { get; set; }
    public bool RenewalNotificationSent { get; set; }
}