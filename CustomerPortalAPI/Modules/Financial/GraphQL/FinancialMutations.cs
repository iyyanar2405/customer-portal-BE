using CustomerPortalAPI.Modules.Financial.Entities;
using CustomerPortalAPI.Modules.Financial.Repositories;
using CustomerPortalAPI.Modules.Financial.GraphQL;
using HotChocolate;
using CustomerPortalAPI.Modules.Shared;

namespace CustomerPortalAPI.Modules.Financial.GraphQL
{
    [ExtendObjectType("Mutation")]
    public class FinancialMutations
    {
        public async Task<CreateFinancialPayload> CreateFinancial(CreateFinancialInput input, [Service] IFinancialRepository repository)
        {
            try
            {
                var financial = new FinancialTransaction
                {
                    TransactionName = input.TransactionName,
                    TransactionType = input.TransactionType,
                    Amount = input.Amount,
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow
                };

                var created = await repository.AddAsync(financial);
                return new CreateFinancialPayload(new FinancialOutput(created.Id, created.TransactionName, created.TransactionType, created.Amount, created.IsActive, created.CreatedDate), null);
            }
            catch (Exception ex)
            {
                return new CreateFinancialPayload(null, ex.Message);
            }
        }

        public async Task<UpdateFinancialPayload> UpdateFinancial(UpdateFinancialInput input, [Service] IFinancialRepository repository)
        {
            try
            {
                var financial = await repository.GetByIdAsync(input.Id);
                if (financial == null) return new UpdateFinancialPayload(null, "Financial transaction not found");

                if (input.TransactionName != null) financial.TransactionName = input.TransactionName;
                if (input.TransactionType != null) financial.TransactionType = input.TransactionType;
                if (input.Amount.HasValue) financial.Amount = input.Amount;
                if (input.IsActive.HasValue) financial.IsActive = input.IsActive.Value;
                financial.ModifiedDate = DateTime.UtcNow;

                await repository.UpdateAsync(financial);
                return new UpdateFinancialPayload(new FinancialOutput(financial.Id, financial.TransactionName, financial.TransactionType, financial.Amount, financial.IsActive, financial.CreatedDate), null);
            }
            catch (Exception ex)
            {
                return new UpdateFinancialPayload(null, ex.Message);
            }
        }

        public async Task<BaseDeletePayload> DeleteFinancial(int id, [Service] IFinancialRepository repository)
        {
            try
            {
                await repository.DeleteAsync(id);
                return new BaseDeletePayload(true, null);
            }
            catch (Exception ex)
            {
                return new BaseDeletePayload(false, ex.Message);
            }
        }

        public async Task<BaseDeletePayload> CreateInvoice(CreateInvoiceInput input, [Service] IInvoiceRepository repository)
        {
            try
            {
                var invoice = new Invoice
                {
                    InvoiceNumber = input.InvoiceNumber,
                    CompanyId = input.CompanyId,
                    TotalAmount = input.TotalAmount,
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow
                };

                await repository.AddAsync(invoice);
                return new BaseDeletePayload(true, null);
            }
            catch (Exception ex)
            {
                return new BaseDeletePayload(false, ex.Message);
            }
        }

        public async Task<BaseDeletePayload> UpdateInvoice(UpdateInvoiceInput input, [Service] IInvoiceRepository repository)
        {
            try
            {
                var invoice = await repository.GetByIdAsync(input.Id);
                if (invoice == null) return new BaseDeletePayload(false, "Invoice not found");

                if (input.InvoiceNumber != null) invoice.InvoiceNumber = input.InvoiceNumber;
                if (input.CompanyId.HasValue) invoice.CompanyId = input.CompanyId.Value;
                if (input.TotalAmount.HasValue) invoice.TotalAmount = input.TotalAmount;
                if (input.IsActive.HasValue) invoice.IsActive = input.IsActive.Value;
                invoice.ModifiedDate = DateTime.UtcNow;

                await repository.UpdateAsync(invoice);
                return new BaseDeletePayload(true, null);
            }
            catch (Exception ex)
            {
                return new BaseDeletePayload(false, ex.Message);
            }
        }

        public async Task<BaseDeletePayload> DeleteInvoice(int id, [Service] IInvoiceRepository repository)
        {
            try
            {
                await repository.DeleteAsync(id);
                return new BaseDeletePayload(true, null);
            }
            catch (Exception ex)
            {
                return new BaseDeletePayload(false, ex.Message);
            }
        }
    }
}
