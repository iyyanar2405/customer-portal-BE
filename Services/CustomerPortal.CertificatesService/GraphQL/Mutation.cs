using CustomerPortal.CertificatesService.Entities;
using CustomerPortal.CertificatesService.GraphQL.Types.Input;
using CustomerPortal.CertificatesService.Repositories;

namespace CustomerPortal.CertificatesService.GraphQL
{
    public class Mutation
    {
        private readonly ICertificateRepository _certificateRepository;
        private readonly ICertificateTypeRepository _certificateTypeRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly IUserRepository _userRepository;

        public Mutation(
            ICertificateRepository certificateRepository,
            ICertificateTypeRepository certificateTypeRepository,
            ICompanyRepository companyRepository,
            IUserRepository userRepository)
        {
            _certificateRepository = certificateRepository;
            _certificateTypeRepository = certificateTypeRepository;
            _companyRepository = companyRepository;
            _userRepository = userRepository;
        }

        // Certificate mutations
        public async Task<Certificate> CreateCertificate(CreateCertificateInput input)
        {
            var certificate = new Certificate
            {
                CertificateNumber = $"CERT-{DateTime.UtcNow:yyyyMMdd}-{Random.Shared.Next(1000, 9999)}",
                CompanyId = input.CompanyId,
                CertificateTypeId = input.CertificateTypeId,
                AuditId = input.AuditId ?? 1, // Default audit
                IssueDate = input.IssueDate,
                ExpiryDate = input.ExpiryDate,
                RenewalDate = input.RenewalDate ?? input.ExpiryDate.AddMonths(-6),
                Scope = "Standard Scope", // Default scope
                Status = "DRAFT",
                CreatedBy = 1, // Default user
                CreatedDate = DateTime.UtcNow,
                IsActive = true
            };

            return await _certificateRepository.AddAsync(certificate);
        }

        public async Task<Certificate?> UpdateCertificate(UpdateCertificateInput input)
        {
            var certificate = await _certificateRepository.GetByIdAsync(input.CertificateId);
            if (certificate == null) return null;

            // Update only the properties that exist
            if (input.IssueDate.HasValue)
                certificate.IssueDate = input.IssueDate.Value;
            
            if (input.ExpiryDate.HasValue)
                certificate.ExpiryDate = input.ExpiryDate.Value;
            
            if (input.RenewalDate.HasValue)
                certificate.RenewalDate = input.RenewalDate.Value;
            
            if (!string.IsNullOrEmpty(input.Status))
                certificate.Status = input.Status;

            certificate.ModifiedBy = 1; // Default user
            certificate.ModifiedDate = DateTime.UtcNow;

            return await _certificateRepository.UpdateAsync(certificate);
        }

        public async Task<bool> DeleteCertificate(int id)
        {
            return await _certificateRepository.DeleteAsync(id);
        }

        // Certificate Type mutations
        public async Task<CertificateType> CreateCertificateType(CreateCertificateTypeInput input)
        {
            var certificateType = new CertificateType
            {
                TypeName = input.TypeName,
                Standard = input.Standard,
                Category = input.Category,
                Description = input.Description,
                ValidityPeriodMonths = input.ValidityPeriodMonths,
                IsAccredited = input.IsAccredited,
                CreatedBy = 1,
                CreatedDate = DateTime.UtcNow,
                IsActive = true
            };

            return await _certificateTypeRepository.AddAsync(certificateType);
        }

        public async Task<bool> DeleteCertificateType(int id)
        {
            return await _certificateTypeRepository.DeleteAsync(id);
        }

        // Company mutations
        public async Task<Company> CreateCompany(CreateCompanyInput input)
        {
            var company = new Company
            {
                CompanyCode = input.CompanyCode,
                CompanyName = input.CompanyName,
                ContactPerson = input.ContactPerson,
                Address = input.Address,
                CreatedBy = 1,
                CreatedDate = DateTime.UtcNow,
                IsActive = true
            };

            return await _companyRepository.AddAsync(company);
        }

        public async Task<bool> DeleteCompany(int id)
        {
            return await _companyRepository.DeleteAsync(id);
        }

        // User mutations
        public async Task<User> CreateUser(CreateUserInput input)
        {
            var user = new User
            {
                FirstName = input.FirstName,
                LastName = input.LastName,
                Email = input.Email,
                Role = input.Role,
                Department = input.Department,
                IsActive = true,
                CreatedBy = 1,
                CreatedDate = DateTime.UtcNow
            };

            return await _userRepository.AddAsync(user);
        }

        public async Task<bool> DeleteUser(int id)
        {
            return await _userRepository.DeleteAsync(id);
        }
    }
}