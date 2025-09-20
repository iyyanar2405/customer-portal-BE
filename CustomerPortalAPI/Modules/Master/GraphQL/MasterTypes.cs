using HotChocolate;

namespace CustomerPortalAPI.Modules.Master.GraphQL
{
    // Input Types
    public record CreateCountryInput(string CountryName, string? CountryCode, string? CurrencyCode, string? Region);
    public record UpdateCountryInput(int Id, string? CountryName, string? CountryCode, string? CurrencyCode, string? Region, bool? IsActive);
    
    public record CreateCityInput(string CityName, int CountryId, string? StateProvince, string? PostalCode);
    public record UpdateCityInput(int Id, string? CityName, int? CountryId, string? StateProvince, string? PostalCode, bool? IsActive);
    
    public record CreateCompanyInput(string CompanyName, string? CompanyCode, string? Address, int? CountryId, string? Phone, string? Email, string? Website, string? ContactPerson, string? CompanyType);
    public record UpdateCompanyInput(int Id, string? CompanyName, string? CompanyCode, string? Address, int? CountryId, string? Phone, string? Email, string? Website, string? ContactPerson, string? CompanyType, bool? IsActive);
    
    public record CreateSiteInput(string SiteName, string? SiteCode, int CompanyId, int? CityId, string? Address, string? Phone, string? Email, string? ContactPerson, string? SiteType);
    public record UpdateSiteInput(int Id, string? SiteName, string? SiteCode, int? CompanyId, int? CityId, string? Address, string? Phone, string? Email, string? ContactPerson, string? SiteType, bool? IsActive);
    
    public record CreateServiceInput(string ServiceName, string? ServiceCode, string? Description, string? ServiceCategory);
    public record UpdateServiceInput(int Id, string? ServiceName, string? ServiceCode, string? Description, string? ServiceCategory, bool? IsActive);

    // Output Types
    public record CountryOutput(int Id, string CountryName, string? CountryCode, string? CurrencyCode, string? Region, bool IsActive, DateTime CreatedDate);
    public record CityOutput(int Id, string CityName, int CountryId, string? StateProvince, string? PostalCode, bool IsActive, DateTime CreatedDate);
    public record CompanyOutput(int Id, string CompanyName, string? CompanyCode, string? Address, int? CountryId, string? Phone, string? Email, string? Website, string? ContactPerson, string? CompanyType, bool IsActive, DateTime CreatedDate);
    public record SiteOutput(int Id, string SiteName, string? SiteCode, int CompanyId, int? CityId, string? Address, string? Phone, string? Email, string? ContactPerson, string? SiteType, bool IsActive, DateTime CreatedDate);
    public record ServiceOutput(int Id, string ServiceName, string? ServiceCode, string? Description, string? ServiceCategory, bool IsActive, DateTime CreatedDate);

    // Payload Types
    public record CreateCountryPayload(CountryOutput? Country, string? Error);
    public record UpdateCountryPayload(CountryOutput? Country, string? Error);
    // Filter Input
    public record MasterFilterInput(string? Name, bool? IsActive);
}
