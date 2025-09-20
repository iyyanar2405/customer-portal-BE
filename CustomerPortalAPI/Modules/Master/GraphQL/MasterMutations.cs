using CustomerPortalAPI.Modules.Master.Entities;
using CustomerPortalAPI.Modules.Master.Repositories;
using CustomerPortalAPI.Modules.Master.GraphQL;
using HotChocolate;

namespace CustomerPortalAPI.Modules.Master.GraphQL
{
    public class MasterMutations
    {
        // Country Mutations
        public async Task<CreateCountryPayload> CreateCountry(
            CreateCountryInput input,
            [Service] ICountryRepository repository)
        {
            try
            {
                var country = new Country
                {
                    CountryName = input.CountryName,
                    CountryCode = input.CountryCode,
                    CurrencyCode = input.CurrencyCode,
                    Region = input.Region,
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow
                };

                var created = await repository.AddAsync(country);
                
                return new CreateCountryPayload(
                    new CountryOutput(
                        created.Id,
                        created.CountryName,
                        created.CountryCode,
                        created.CurrencyCode,
                        created.Region,
                        created.IsActive,
                        created.CreatedDate
                    ),
                    null
                );
            }
            catch (Exception ex)
            {
                return new CreateCountryPayload(null, ex.Message);
            }
        }

        public async Task<UpdateCountryPayload> UpdateCountry(
            UpdateCountryInput input,
            [Service] ICountryRepository repository)
        {
            try
            {
                var country = await repository.GetByIdAsync(input.Id);
                if (country == null)
                    return new UpdateCountryPayload(null, "Country not found");

                if (input.CountryName != null) country.CountryName = input.CountryName;
                if (input.CountryCode != null) country.CountryCode = input.CountryCode;
                if (input.CurrencyCode != null) country.CurrencyCode = input.CurrencyCode;
                if (input.Region != null) country.Region = input.Region;
                if (input.IsActive.HasValue) country.IsActive = input.IsActive.Value;
                country.ModifiedDate = DateTime.UtcNow;

                await repository.UpdateAsync(country);
                
                return new UpdateCountryPayload(
                    new CountryOutput(
                        country.Id,
                        country.CountryName,
                        country.CountryCode,
                        country.CurrencyCode,
                        country.Region,
                        country.IsActive,
                        country.CreatedDate
                    ),
                    null
                );
            }
            catch (Exception ex)
            {
                return new UpdateCountryPayload(null, ex.Message);
            }
        }

        public async Task<DeletePayload> DeleteCountry(
            int id,
            [Service] ICountryRepository repository)
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

        // City Mutations
        public async Task<DeletePayload> CreateCity(
            CreateCityInput input,
            [Service] ICityRepository repository)
        {
            try
            {
                var city = new City
                {
                    CityName = input.CityName,
                    CountryId = input.CountryId,
                    StateProvince = input.StateProvince,
                    PostalCode = input.PostalCode,
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow
                };

                await repository.AddAsync(city);
                return new DeletePayload(true, null);
            }
            catch (Exception ex)
            {
                return new DeletePayload(false, ex.Message);
            }
        }

        public async Task<DeletePayload> UpdateCity(
            UpdateCityInput input,
            [Service] ICityRepository repository)
        {
            try
            {
                var city = await repository.GetByIdAsync(input.Id);
                if (city == null)
                    return new DeletePayload(false, "City not found");

                if (input.CityName != null) city.CityName = input.CityName;
                if (input.CountryId.HasValue) city.CountryId = input.CountryId.Value;
                if (input.StateProvince != null) city.StateProvince = input.StateProvince;
                if (input.PostalCode != null) city.PostalCode = input.PostalCode;
                if (input.IsActive.HasValue) city.IsActive = input.IsActive.Value;
                city.ModifiedDate = DateTime.UtcNow;

                await repository.UpdateAsync(city);
                return new DeletePayload(true, null);
            }
            catch (Exception ex)
            {
                return new DeletePayload(false, ex.Message);
            }
        }

        public async Task<DeletePayload> DeleteCity(
            int id,
            [Service] ICityRepository repository)
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

        // Company Mutations
        public async Task<DeletePayload> CreateCompany(
            CreateCompanyInput input,
            [Service] ICompanyRepository repository)
        {
            try
            {
                var company = new Company
                {
                    CompanyName = input.CompanyName,
                    CompanyCode = input.CompanyCode,
                    Address = input.Address,
                    CountryId = input.CountryId,
                    Phone = input.Phone,
                    Email = input.Email,
                    Website = input.Website,
                    ContactPerson = input.ContactPerson,
                    CompanyType = input.CompanyType,
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow
                };

                await repository.AddAsync(company);
                return new DeletePayload(true, null);
            }
            catch (Exception ex)
            {
                return new DeletePayload(false, ex.Message);
            }
        }

        public async Task<DeletePayload> UpdateCompany(
            UpdateCompanyInput input,
            [Service] ICompanyRepository repository)
        {
            try
            {
                var company = await repository.GetByIdAsync(input.Id);
                if (company == null)
                    return new DeletePayload(false, "Company not found");

                if (input.CompanyName != null) company.CompanyName = input.CompanyName;
                if (input.CompanyCode != null) company.CompanyCode = input.CompanyCode;
                if (input.Address != null) company.Address = input.Address;
                if (input.CountryId.HasValue) company.CountryId = input.CountryId;
                if (input.Phone != null) company.Phone = input.Phone;
                if (input.Email != null) company.Email = input.Email;
                if (input.Website != null) company.Website = input.Website;
                if (input.ContactPerson != null) company.ContactPerson = input.ContactPerson;
                if (input.CompanyType != null) company.CompanyType = input.CompanyType;
                if (input.IsActive.HasValue) company.IsActive = input.IsActive.Value;
                company.ModifiedDate = DateTime.UtcNow;

                await repository.UpdateAsync(company);
                return new DeletePayload(true, null);
            }
            catch (Exception ex)
            {
                return new DeletePayload(false, ex.Message);
            }
        }

        public async Task<DeletePayload> DeleteCompany(
            int id,
            [Service] ICompanyRepository repository)
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

        // Site Mutations
        public async Task<DeletePayload> CreateSite(
            CreateSiteInput input,
            [Service] ISiteRepository repository)
        {
            try
            {
                var site = new Site
                {
                    SiteName = input.SiteName,
                    SiteCode = input.SiteCode,
                    CompanyId = input.CompanyId,
                    CityId = input.CityId,
                    Address = input.Address,
                    Phone = input.Phone,
                    Email = input.Email,
                    ContactPerson = input.ContactPerson,
                    SiteType = input.SiteType,
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow
                };

                await repository.AddAsync(site);
                return new DeletePayload(true, null);
            }
            catch (Exception ex)
            {
                return new DeletePayload(false, ex.Message);
            }
        }

        public async Task<DeletePayload> UpdateSite(
            UpdateSiteInput input,
            [Service] ISiteRepository repository)
        {
            try
            {
                var site = await repository.GetByIdAsync(input.Id);
                if (site == null)
                    return new DeletePayload(false, "Site not found");

                if (input.SiteName != null) site.SiteName = input.SiteName;
                if (input.SiteCode != null) site.SiteCode = input.SiteCode;
                if (input.CompanyId.HasValue) site.CompanyId = input.CompanyId.Value;
                if (input.CityId.HasValue) site.CityId = input.CityId;
                if (input.Address != null) site.Address = input.Address;
                if (input.Phone != null) site.Phone = input.Phone;
                if (input.Email != null) site.Email = input.Email;
                if (input.ContactPerson != null) site.ContactPerson = input.ContactPerson;
                if (input.SiteType != null) site.SiteType = input.SiteType;
                if (input.IsActive.HasValue) site.IsActive = input.IsActive.Value;
                site.ModifiedDate = DateTime.UtcNow;

                await repository.UpdateAsync(site);
                return new DeletePayload(true, null);
            }
            catch (Exception ex)
            {
                return new DeletePayload(false, ex.Message);
            }
        }

        public async Task<DeletePayload> DeleteSite(
            int id,
            [Service] ISiteRepository repository)
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

        // Service Mutations
        public async Task<DeletePayload> CreateService(
            CreateServiceInput input,
            [Service] IServiceRepository repository)
        {
            try
            {
                var service = new Service
                {
                    ServiceName = input.ServiceName,
                    ServiceCode = input.ServiceCode,
                    Description = input.Description,
                    ServiceCategory = input.ServiceCategory,
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow
                };

                await repository.AddAsync(service);
                return new DeletePayload(true, null);
            }
            catch (Exception ex)
            {
                return new DeletePayload(false, ex.Message);
            }
        }

        public async Task<DeletePayload> UpdateService(
            UpdateServiceInput input,
            [Service] IServiceRepository repository)
        {
            try
            {
                var service = await repository.GetByIdAsync(input.Id);
                if (service == null)
                    return new DeletePayload(false, "Service not found");

                if (input.ServiceName != null) service.ServiceName = input.ServiceName;
                if (input.ServiceCode != null) service.ServiceCode = input.ServiceCode;
                if (input.Description != null) service.Description = input.Description;
                if (input.ServiceCategory != null) service.ServiceCategory = input.ServiceCategory;
                if (input.IsActive.HasValue) service.IsActive = input.IsActive.Value;
                service.ModifiedDate = DateTime.UtcNow;

                await repository.UpdateAsync(service);
                return new DeletePayload(true, null);
            }
            catch (Exception ex)
            {
                return new DeletePayload(false, ex.Message);
            }
        }

        public async Task<DeletePayload> DeleteService(
            int id,
            [Service] IServiceRepository repository)
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