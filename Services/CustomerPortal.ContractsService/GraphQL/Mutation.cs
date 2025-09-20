using AutoMapper;
using CustomerPortal.ContractsService.Entities;
using CustomerPortal.ContractsService.Repositories;

namespace CustomerPortal.ContractsService.GraphQL;

public class Mutation
{
    public async Task<ContractGraphQLType> CreateContract(
        CreateContractInput input,
        [Service] IContractRepository contractRepository,
        [Service] IServiceRepository serviceRepository,
        [Service] ISiteRepository siteRepository,
        [Service] IContractTermRepository termRepository,
        [Service] IMapper mapper)
    {
        // Generate contract number
        var contractNumber = $"CNT-{DateTime.UtcNow.Year}-{Random.Shared.Next(1000, 9999)}";

        var contract = new Contract
        {
            ContractNumber = contractNumber,
            CompanyId = input.CompanyId,
            ContractType = input.ContractType,
            Title = input.Title,
            Description = input.Description,
            StartDate = input.StartDate,
            EndDate = input.EndDate,
            Value = input.Value,
            Currency = input.Currency,
            PaymentTerms = input.PaymentTerms,
            Status = "DRAFT",
            IsActive = true
        };

        var createdContract = await contractRepository.CreateAsync(contract);

        // Add services if provided
        if (input.Services != null && input.Services.Any())
        {
            foreach (var serviceInput in input.Services)
            {
                var contractService = new ContractService
                {
                    ContractId = createdContract.Id,
                    ServiceId = serviceInput.ServiceId,
                    UnitPrice = serviceInput.UnitPrice,
                    Quantity = serviceInput.Quantity,
                    TotalPrice = serviceInput.UnitPrice * serviceInput.Quantity
                };
                
                // Note: You would need a ContractService repository to save this
                // For now, we'll skip this implementation detail
            }
        }

        // Add terms if provided
        if (input.Terms != null && input.Terms.Any())
        {
            foreach (var termInput in input.Terms)
            {
                var term = new ContractTerm
                {
                    ContractId = createdContract.Id,
                    TermType = termInput.TermType,
                    Description = termInput.Description,
                    Value = termInput.Value,
                    Unit = termInput.Unit,
                    IsRequired = termInput.IsRequired,
                    EffectiveDate = termInput.EffectiveDate,
                    ExpiryDate = termInput.ExpiryDate
                };

                await termRepository.CreateAsync(term);
            }
        }

        // Reload contract with all relationships
        var contractWithRelations = await contractRepository.GetByIdAsync(createdContract.Id);
        return mapper.Map<ContractGraphQLType>(contractWithRelations);
    }

    public async Task<ContractGraphQLType?> UpdateContract(
        UpdateContractInput input,
        [Service] IContractRepository contractRepository,
        [Service] IMapper mapper)
    {
        var contract = await contractRepository.GetByIdAsync(input.Id);
        if (contract == null) return null;

        if (!string.IsNullOrEmpty(input.Title))
            contract.Title = input.Title;
        
        if (!string.IsNullOrEmpty(input.Description))
            contract.Description = input.Description;
        
        if (input.StartDate.HasValue)
            contract.StartDate = input.StartDate.Value;
        
        if (input.EndDate.HasValue)
            contract.EndDate = input.EndDate.Value;
        
        if (input.Value.HasValue)
            contract.Value = input.Value.Value;
        
        if (!string.IsNullOrEmpty(input.PaymentTerms))
            contract.PaymentTerms = input.PaymentTerms;

        var updatedContract = await contractRepository.UpdateAsync(contract);
        return mapper.Map<ContractGraphQLType>(updatedContract);
    }

    public async Task<ContractGraphQLType?> UpdateContractStatus(
        int contractId,
        string status,
        string? reason,
        [Service] IContractRepository contractRepository,
        [Service] IMapper mapper)
    {
        var success = await contractRepository.UpdateStatusAsync(contractId, status, reason);
        if (!success) return null;

        var contract = await contractRepository.GetByIdAsync(contractId);
        return contract != null ? mapper.Map<ContractGraphQLType>(contract) : null;
    }

    public async Task<ContractRenewalGraphQLType> StartContractRenewal(
        int contractId,
        StartRenewalInput input,
        [Service] IContractRenewalRepository renewalRepository,
        [Service] IMapper mapper)
    {
        var renewalNumber = $"REN-{DateTime.UtcNow.Year}-{Random.Shared.Next(1000, 9999)}";

        var renewal = new ContractRenewal
        {
            ContractId = contractId,
            RenewalNumber = renewalNumber,
            ProposedStartDate = input.ProposedStartDate,
            ProposedEndDate = input.ProposedEndDate,
            ProposedValue = input.ProposedValue,
            AutoRenewal = input.AutoRenewal,
            Status = "INITIATED"
        };

        var createdRenewal = await renewalRepository.CreateAsync(renewal);
        return mapper.Map<ContractRenewalGraphQLType>(createdRenewal);
    }

    public async Task<ContractAmendmentGraphQLType> CreateContractAmendment(
        CreateAmendmentInput input,
        [Service] IContractAmendmentRepository amendmentRepository,
        [Service] IMapper mapper)
    {
        var amendmentNumber = $"AMD-{DateTime.UtcNow.Year}-{Random.Shared.Next(1000, 9999)}";

        var amendment = new ContractAmendment
        {
            ContractId = input.ContractId,
            AmendmentNumber = amendmentNumber,
            Description = input.Description,
            AmendmentType = input.AmendmentType,
            EffectiveDate = input.EffectiveDate,
            ValueChange = input.ValueChange,
            Status = "PENDING"
        };

        var createdAmendment = await amendmentRepository.CreateAsync(amendment);
        return mapper.Map<ContractAmendmentGraphQLType>(createdAmendment);
    }

    public async Task<ContractTermGraphQLType> AddContractTerm(
        AddContractTermInput input,
        [Service] IContractTermRepository termRepository,
        [Service] IMapper mapper)
    {
        var term = new ContractTerm
        {
            ContractId = input.ContractId,
            TermType = input.TermType,
            Description = input.Description,
            Value = input.Value,
            Unit = input.Unit,
            IsRequired = input.IsRequired,
            EffectiveDate = input.EffectiveDate,
            ExpiryDate = input.ExpiryDate
        };

        var createdTerm = await termRepository.CreateAsync(term);
        return mapper.Map<ContractTermGraphQLType>(createdTerm);
    }

    // Company mutations
    public async Task<CompanyGraphQLType> CreateCompany(
        CreateCompanyInput input,
        [Service] ICompanyRepository companyRepository,
        [Service] IMapper mapper)
    {
        var company = new Company
        {
            CompanyName = input.CompanyName,
            CompanyCode = input.CompanyCode,
            ContactPerson = input.ContactPerson,
            Email = input.Email,
            Phone = input.Phone,
            Address = input.Address
        };

        var createdCompany = await companyRepository.CreateAsync(company);
        return mapper.Map<CompanyGraphQLType>(createdCompany);
    }

    // Service mutations
    public async Task<ServiceGraphQLType> CreateService(
        CreateServiceInput input,
        [Service] IServiceRepository serviceRepository,
        [Service] IMapper mapper)
    {
        var service = new Service
        {
            ServiceName = input.ServiceName,
            ServiceCode = input.ServiceCode,
            Description = input.Description,
            Category = input.Category
        };

        var createdService = await serviceRepository.CreateAsync(service);
        return mapper.Map<ServiceGraphQLType>(createdService);
    }

    // Site mutations
    public async Task<SiteGraphQLType> CreateSite(
        CreateSiteInput input,
        [Service] ISiteRepository siteRepository,
        [Service] IMapper mapper)
    {
        var site = new Site
        {
            CompanyId = input.CompanyId,
            SiteName = input.SiteName,
            SiteCode = input.SiteCode,
            Address = input.Address,
            City = input.City,
            Country = input.Country,
            PostalCode = input.PostalCode
        };

        var createdSite = await siteRepository.CreateAsync(site);
        return mapper.Map<SiteGraphQLType>(createdSite);
    }

    // Delete operations
    public async Task<bool> DeleteContract(
        int id,
        [Service] IContractRepository contractRepository)
    {
        return await contractRepository.DeleteAsync(id);
    }

    public async Task<bool> DeleteCompany(
        int id,
        [Service] ICompanyRepository companyRepository)
    {
        return await companyRepository.DeleteAsync(id);
    }

    public async Task<bool> DeleteService(
        int id,
        [Service] IServiceRepository serviceRepository)
    {
        return await serviceRepository.DeleteAsync(id);
    }

    public async Task<bool> DeleteSite(
        int id,
        [Service] ISiteRepository siteRepository)
    {
        return await siteRepository.DeleteAsync(id);
    }
}