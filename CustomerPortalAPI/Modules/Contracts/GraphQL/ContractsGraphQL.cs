using CustomerPortalAPI.Modules.Contracts.Entities;
using CustomerPortalAPI.Modules.Contracts.Repositories;
using HotChocolate;

namespace CustomerPortalAPI.Modules.Contracts.GraphQL
{
    public record ContractOutput(int Id, string? ContractNumber, string? ContractName, int CompanyId, DateTime? StartDate, DateTime? EndDate, bool IsActive, DateTime CreatedDate);
    public record CreateContractInput(string? ContractNumber, string? ContractName, int CompanyId, DateTime? StartDate, DateTime? EndDate);
    public record UpdateContractInput(int Id, string? ContractNumber, string? ContractName, DateTime? StartDate, DateTime? EndDate, bool? IsActive);
    public record CreateContractPayload(ContractOutput? Contract, string? Error);
    public record UpdateContractPayload(ContractOutput? Contract, string? Error);
    public record DeletePayload(bool Success, string? Error);

    [ExtendObjectType("Query")]
    public class ContractsQueries
    {
        public async Task<IEnumerable<ContractOutput>> GetContracts([Service] IContractRepository repository)
        {
            var contracts = await repository.GetAllAsync();
            return contracts.Select(c => new ContractOutput(c.Id, c.ContractNumber, c.ContractName, c.CompanyId, c.StartDate, c.EndDate, c.IsActive, c.CreatedDate));
        }

        public async Task<ContractOutput?> GetContractById(int id, [Service] IContractRepository repository)
        {
            var contract = await repository.GetByIdAsync(id);
            return contract == null ? null : new ContractOutput(contract.Id, contract.ContractNumber, contract.ContractName, contract.CompanyId, contract.StartDate, contract.EndDate, contract.IsActive, contract.CreatedDate);
        }

        public async Task<IEnumerable<ContractOutput>> GetContractsByCompany(int companyId, [Service] IContractRepository repository)
        {
            var contracts = await repository.GetAllAsync();
            var companyContracts = contracts.Where(c => c.CompanyId == companyId);
            return companyContracts.Select(c => new ContractOutput(c.Id, c.ContractNumber, c.ContractName, c.CompanyId, c.StartDate, c.EndDate, c.IsActive, c.CreatedDate));
        }
    }

    [ExtendObjectType("Mutation")]
    public class ContractsMutations
    {
        public async Task<CreateContractPayload> CreateContract(CreateContractInput input, [Service] IContractRepository repository)
        {
            try
            {
                var contract = new Contract
                {
                    ContractNumber = input.ContractNumber,
                    ContractName = input.ContractName ?? "",
                    CompanyId = input.CompanyId,
                    StartDate = input.StartDate,
                    EndDate = input.EndDate,
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow
                };

                var created = await repository.AddAsync(contract);
                return new CreateContractPayload(new ContractOutput(created.Id, created.ContractNumber, created.ContractName, created.CompanyId, created.StartDate, created.EndDate, created.IsActive, created.CreatedDate), null);
            }
            catch (Exception ex)
            {
                return new CreateContractPayload(null, ex.Message);
            }
        }

        public async Task<UpdateContractPayload> UpdateContract(UpdateContractInput input, [Service] IContractRepository repository)
        {
            try
            {
                var contract = await repository.GetByIdAsync(input.Id);
                if (contract == null) return new UpdateContractPayload(null, "Contract not found");

                if (input.ContractNumber != null) contract.ContractNumber = input.ContractNumber;
                if (input.ContractName != null) contract.ContractName = input.ContractName;
                if (input.StartDate.HasValue) contract.StartDate = input.StartDate.Value;
                if (input.EndDate.HasValue) contract.EndDate = input.EndDate.Value;
                if (input.IsActive.HasValue) contract.IsActive = input.IsActive.Value;
                contract.ModifiedDate = DateTime.UtcNow;

                await repository.UpdateAsync(contract);
                return new UpdateContractPayload(new ContractOutput(contract.Id, contract.ContractNumber, contract.ContractName, contract.CompanyId, contract.StartDate, contract.EndDate, contract.IsActive, contract.CreatedDate), null);
            }
            catch (Exception ex)
            {
                return new UpdateContractPayload(null, ex.Message);
            }
        }

        public async Task<DeletePayload> DeleteContract(int id, [Service] IContractRepository repository)
        {
            try
            {
                await repository.DeleteAsync(id);
                return new DeletePayload(true, null);
            }
            catch (Exception ex)
            {
                return new DeletePayload(false, ex.Message);
            }
        }
    }
}