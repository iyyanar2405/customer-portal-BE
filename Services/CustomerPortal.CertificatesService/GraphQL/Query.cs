using AutoMapper;
using CustomerPortal.CertificatesService.GraphQL.Types;
using CustomerPortal.CertificatesService.Repositories;
using HotChocolate;

namespace CustomerPortal.CertificatesService.GraphQL
{
    /// <summary>
    /// GraphQL Query operations for Certificates service
    /// </summary>
    public class Query
    {
        private readonly IMapper _mapper;

        public Query(IMapper mapper)
        {
            _mapper = mapper;
        }

        // Certificate Queries
        [GraphQLDescription("Get all certificates")]
        public async Task<IEnumerable<CertificateGraphQLType>> GetCertificatesAsync(
            [Service] ICertificateRepository certificateRepository)
        {
            var certificates = await certificateRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<CertificateGraphQLType>>(certificates);
        }

        [GraphQLDescription("Get certificate by ID")]
        public async Task<CertificateGraphQLType?> GetCertificateByIdAsync(
            [Service] ICertificateRepository certificateRepository,
            int id)
        {
            var certificate = await certificateRepository.GetByIdAsync(id);
            return certificate != null ? _mapper.Map<CertificateGraphQLType>(certificate) : null;
        }

        [GraphQLDescription("Get certificates by company ID")]
        public async Task<IEnumerable<CertificateGraphQLType>> GetCertificatesByCompanyAsync(
            [Service] ICertificateRepository certificateRepository,
            int companyId)
        {
            var certificates = await certificateRepository.GetByCompanyIdAsync(companyId);
            return _mapper.Map<IEnumerable<CertificateGraphQLType>>(certificates);
        }

        [GraphQLDescription("Get certificates by status")]
        public async Task<IEnumerable<CertificateGraphQLType>> GetCertificatesByStatusAsync(
            [Service] ICertificateRepository certificateRepository,
            string status)
        {
            var certificates = await certificateRepository.GetByStatusAsync(status);
            return _mapper.Map<IEnumerable<CertificateGraphQLType>>(certificates);
        }

        [GraphQLDescription("Get expiring certificates within specified days")]
        public async Task<IEnumerable<ExpiringCertificateGraphQLType>> GetExpiringCertificatesAsync(
            [Service] ICertificateRepository certificateRepository,
            int withinDays)
        {
            var certificates = await certificateRepository.GetExpiringCertificatesAsync(withinDays);
            var expiringCerts = certificates.Select(c => new ExpiringCertificateGraphQLType
            {
                Id = c.Id,
                CertificateNumber = c.CertificateNumber,
                Company = _mapper.Map<CompanyGraphQLType>(c.Company),
                CertificateType = _mapper.Map<CertificateTypeGraphQLType>(c.CertificateType),
                ExpiryDate = c.ExpiryDate,
                DaysUntilExpiry = (int)(c.ExpiryDate - DateTime.UtcNow).TotalDays,
                RenewalRequired = c.RenewalDate.HasValue && c.RenewalDate <= DateTime.UtcNow.AddDays(90),
                HasActiveRenewalProcess = false // This would need to be calculated based on renewal records
            });
            return expiringCerts;
        }

        [GraphQLDescription("Get renewal schedule for specified date range")]
        public async Task<IEnumerable<RenewalScheduleGraphQLType>> GetRenewalScheduleAsync(
            [Service] ICertificateRepository certificateRepository,
            DateTime startDate,
            DateTime endDate)
        {
            var certificates = await certificateRepository.GetRenewalScheduleAsync(startDate, endDate);
            var renewalSchedule = certificates.Select(c => new RenewalScheduleGraphQLType
            {
                CertificateId = c.Id,
                CertificateNumber = c.CertificateNumber,
                CompanyName = c.Company?.CompanyName ?? "",
                CertificateType = c.CertificateType?.TypeName ?? "",
                CurrentExpiryDate = c.ExpiryDate,
                RenewalDate = c.RenewalDate,
                Status = c.Status,
                DaysUntilRenewal = c.RenewalDate.HasValue ? (int)(c.RenewalDate.Value - DateTime.UtcNow).TotalDays : 0,
                Priority = c.RenewalDate.HasValue && c.RenewalDate <= DateTime.UtcNow.AddDays(30) ? "HIGH" : 
                          c.RenewalDate.HasValue && c.RenewalDate <= DateTime.UtcNow.AddDays(90) ? "MEDIUM" : "LOW"
            });
            return renewalSchedule;
        }

        [GraphQLDescription("Validate certificate by certificate number")]
        public async Task<CertificateValidationResultGraphQLType?> ValidateCertificateAsync(
            [Service] ICertificateRepository certificateRepository,
            string certificateNumber)
        {
            var certificate = await certificateRepository.ValidateCertificateAsync(certificateNumber);
            if (certificate == null) return null;

            return new CertificateValidationResultGraphQLType
            {
                IsValid = certificate.Status == "ACTIVE" && certificate.ExpiryDate > DateTime.UtcNow,
                CertificateNumber = certificate.CertificateNumber,
                CompanyName = certificate.Company?.CompanyName ?? "",
                CertificateType = certificate.CertificateType?.TypeName ?? "",
                Scope = certificate.Scope,
                IssueDate = certificate.IssueDate,
                ExpiryDate = certificate.ExpiryDate,
                Status = certificate.Status,
                Sites = _mapper.Map<IEnumerable<SiteGraphQLType>>(certificate.Sites.Select(cs => cs.Site)),
                ValidationDate = DateTime.UtcNow,
                QrCodeUrl = $"/api/certificates/{certificate.Id}/qr-code"
            };
        }

        // Certificate Types Queries
        [GraphQLDescription("Get all certificate types")]
        public async Task<IEnumerable<CertificateTypeGraphQLType>> GetCertificateTypesAsync(
            [Service] ICertificateTypeRepository certificateTypeRepository)
        {
            var certificateTypes = await certificateTypeRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<CertificateTypeGraphQLType>>(certificateTypes);
        }

        [GraphQLDescription("Get certificate types by standard")]
        public async Task<IEnumerable<CertificateTypeGraphQLType>> GetCertificateTypesByStandardAsync(
            [Service] ICertificateTypeRepository certificateTypeRepository,
            string standard)
        {
            var certificateTypes = await certificateTypeRepository.GetByStandardAsync(standard);
            return _mapper.Map<IEnumerable<CertificateTypeGraphQLType>>(certificateTypes);
        }

        [GraphQLDescription("Get certificate types by category")]
        public async Task<IEnumerable<CertificateTypeGraphQLType>> GetCertificateTypesByCategoryAsync(
            [Service] ICertificateTypeRepository certificateTypeRepository,
            string category)
        {
            var certificateTypes = await certificateTypeRepository.GetByCategoryAsync(category);
            return _mapper.Map<IEnumerable<CertificateTypeGraphQLType>>(certificateTypes);
        }

        // Company Queries
        [GraphQLDescription("Get all companies")]
        public async Task<IEnumerable<CompanyGraphQLType>> GetCompaniesAsync(
            [Service] ICompanyRepository companyRepository)
        {
            var companies = await companyRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<CompanyGraphQLType>>(companies);
        }

        [GraphQLDescription("Get company by ID")]
        public async Task<CompanyGraphQLType?> GetCompanyByIdAsync(
            [Service] ICompanyRepository companyRepository,
            int id)
        {
            var company = await companyRepository.GetByIdAsync(id);
            return company != null ? _mapper.Map<CompanyGraphQLType>(company) : null;
        }

        [GraphQLDescription("Search companies by search term")]
        public async Task<IEnumerable<CompanyGraphQLType>> SearchCompaniesAsync(
            [Service] ICompanyRepository companyRepository,
            string searchTerm)
        {
            var companies = await companyRepository.SearchCompaniesAsync(searchTerm);
            return _mapper.Map<IEnumerable<CompanyGraphQLType>>(companies);
        }

        // User Queries
        [GraphQLDescription("Get all users")]
        public async Task<IEnumerable<UserGraphQLType>> GetUsersAsync(
            [Service] IUserRepository userRepository)
        {
            var users = await userRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<UserGraphQLType>>(users);
        }

        [GraphQLDescription("Get auditors")]
        public async Task<IEnumerable<UserGraphQLType>> GetAuditorsAsync(
            [Service] IUserRepository userRepository)
        {
            var auditors = await userRepository.GetAuditorsAsync();
            return _mapper.Map<IEnumerable<UserGraphQLType>>(auditors);
        }

        [GraphQLDescription("Get users by role")]
        public async Task<IEnumerable<UserGraphQLType>> GetUsersByRoleAsync(
            [Service] IUserRepository userRepository,
            string role)
        {
            var users = await userRepository.GetByRoleAsync(role);
            return _mapper.Map<IEnumerable<UserGraphQLType>>(users);
        }

        // Site Queries - Temporarily disabled
        /*
        [GraphQLDescription("Get all sites")]
        public async Task<IEnumerable<SiteGraphQLType>> GetSitesAsync(
            [Service] ISiteRepository siteRepository)
        {
            var sites = await siteRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<SiteGraphQLType>>(sites);
        }

        [GraphQLDescription("Get sites by company ID")]
        public async Task<IEnumerable<SiteGraphQLType>> GetSitesByCompanyAsync(
            [Service] ISiteRepository siteRepository,
            int companyId)
        {
            var sites = await siteRepository.GetByCompanyIdAsync(companyId);
            return _mapper.Map<IEnumerable<SiteGraphQLType>>(sites);
        }

        // Service Queries - Temporarily disabled
        [GraphQLDescription("Get all services")]
        public async Task<IEnumerable<ServiceGraphQLType>> GetServicesAsync(
            [Service] IServiceRepository serviceRepository)
        {
            var services = await serviceRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<ServiceGraphQLType>>(services);
        }

        [GraphQLDescription("Get services by category")]
        public async Task<IEnumerable<ServiceGraphQLType>> GetServicesByCategoryAsync(
            [Service] IServiceRepository serviceRepository,
            string category)
        {
            var services = await serviceRepository.GetByCategoryAsync(category);
            return _mapper.Map<IEnumerable<ServiceGraphQLType>>(services);
        }
        */

        // Certificate Dashboard and Analytics - Simplified for initial implementation
        [GraphQLDescription("Get basic certificate dashboard data")]
        public async Task<CertificateDashboardGraphQLType> GetCertificateDashboardAsync(
            [Service] ICertificateRepository certificateRepository,
            int? companyId = null,
            string period = "YEAR")
        {
            var totalCertificates = await certificateRepository.GetTotalCountAsync();
            var activeCertificates = (await certificateRepository.GetByStatusAsync("ACTIVE")).Count();
            var expiredCertificates = (await certificateRepository.GetByStatusAsync("EXPIRED")).Count();
            var suspendedCertificates = (await certificateRepository.GetByStatusAsync("SUSPENDED")).Count();
            var expiringWithin30Days = (await certificateRepository.GetExpiringCertificatesAsync(30)).Count();
            var expiringWithin90Days = (await certificateRepository.GetExpiringCertificatesAsync(90)).Count();
            var renewalSuccessRate = await certificateRepository.GetRenewalSuccessRateAsync();
            var averageRenewalTime = await certificateRepository.GetAverageRenewalTimeAsync();

            var certificatesByStatus = await certificateRepository.GetCertificatesByStatusAsync();
            var certificatesByType = await certificateRepository.GetCertificatesByTypeAsync();

            return new CertificateDashboardGraphQLType
            {
                TotalCertificates = totalCertificates,
                ActiveCertificates = activeCertificates,
                ExpiredCertificates = expiredCertificates,
                SuspendedCertificates = suspendedCertificates,
                ExpiringWithin30Days = expiringWithin30Days,
                ExpiringWithin90Days = expiringWithin90Days,
                RenewalSuccessRate = renewalSuccessRate,
                AverageRenewalTime = averageRenewalTime,
                CertificatesByStatus = certificatesByStatus.Select(kvp => new StatusStatsGraphQLType
                {
                    Status = kvp.Key,
                    Count = kvp.Value,
                    Percentage = totalCertificates > 0 ? (decimal)kvp.Value / totalCertificates * 100 : 0
                }),
                CertificatesByType = certificatesByType.Select(kvp => new CertificateTypeStatsGraphQLType
                {
                    TypeName = kvp.Key,
                    Count = kvp.Value,
                    ActiveCount = kvp.Value // This would need more sophisticated calculation
                }),
                MonthlyIssuance = new List<MonthlyIssuanceGraphQLType>() // Would need implementation
            };
        }

        // Temporarily disabled until repositories are implemented
        /*
        // Certificate Sites
        [GraphQLDescription("Get certificate sites by certificate ID")]
        public async Task<IEnumerable<CertificateSiteGraphQLType>> GetCertificateSitesAsync(
            [Service] ICertificateSiteRepository certificateSiteRepository,
            int certificateId)
        {
            var certificateSites = await certificateSiteRepository.GetByCertificateIdAsync(certificateId);
            return _mapper.Map<IEnumerable<CertificateSiteGraphQLType>>(certificateSites);
        }

        // Countries and Cities
        [GraphQLDescription("Get all countries")]
        public async Task<IEnumerable<CountryGraphQLType>> GetCountriesAsync(
            [Service] ICountryRepository countryRepository)
        {
            var countries = await countryRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<CountryGraphQLType>>(countries);
        }

        [GraphQLDescription("Get cities by country ID")]
        public async Task<IEnumerable<CityGraphQLType>> GetCitiesByCountryAsync(
            [Service] ICityRepository cityRepository,
            int countryId)
        {
            var cities = await cityRepository.GetByCountryIdAsync(countryId);
            return _mapper.Map<IEnumerable<CityGraphQLType>>(cities);
        }
        */
    }
}