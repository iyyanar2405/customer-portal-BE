using CustomerPortal.MasterService.Entities;

namespace CustomerPortal.MasterService.GraphQL.Types
{
    public class CountryType
    {
        public int Id { get; set; }
        public string CountryName { get; set; } = string.Empty;
        public string? CountryCode { get; set; }
        public string? CurrencyCode { get; set; }
        public string? Region { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public IEnumerable<CityType>? Cities { get; set; }
    }

    public class CityType
    {
        public int Id { get; set; }
        public string CityName { get; set; } = string.Empty;
        public int CountryId { get; set; }
        public string? StateProvince { get; set; }
        public string? PostalCode { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public CountryType? Country { get; set; }
    }

    public class CompanyType
    {
        public int Id { get; set; }
        public string CompanyName { get; set; } = string.Empty;
        public string? CompanyCode { get; set; }
        public string? Address { get; set; }
        public int? CountryId { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Website { get; set; }
        public string? ContactPerson { get; set; }
        public string? CompanyTypeName { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public CountryType? Country { get; set; }
        public IEnumerable<SiteType>? Sites { get; set; }
    }

    public class SiteType
    {
        public int Id { get; set; }
        public string SiteName { get; set; } = string.Empty;
        public string? SiteCode { get; set; }
        public int CompanyId { get; set; }
        public int? CityId { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? ContactPerson { get; set; }
        public string? SiteTypeName { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public CompanyType? Company { get; set; }
        public CityType? City { get; set; }
    }

    public class ServiceType
    {
        public int Id { get; set; }
        public string ServiceName { get; set; } = string.Empty;
        public string? ServiceCode { get; set; }
        public string? Description { get; set; }
        public string? ServiceCategory { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }

    public class RoleType
    {
        public int Id { get; set; }
        public string RoleName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Permissions { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }

    public class ChapterType
    {
        public int Id { get; set; }
        public string ChapterName { get; set; } = string.Empty;
        public string? ChapterNumber { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public IEnumerable<ClauseType>? Clauses { get; set; }
    }

    public class ClauseType
    {
        public int Id { get; set; }
        public string ClauseName { get; set; } = string.Empty;
        public string? ClauseNumber { get; set; }
        public int? ChapterId { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public ChapterType? Chapter { get; set; }
    }

    public class FocusAreaType
    {
        public int Id { get; set; }
        public string FocusAreaName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}