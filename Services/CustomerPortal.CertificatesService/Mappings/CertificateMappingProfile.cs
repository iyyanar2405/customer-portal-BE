using AutoMapper;
using CustomerPortal.CertificatesService.Entities;
using CustomerPortal.CertificatesService.GraphQL.Types;

namespace CustomerPortal.CertificatesService.Mappings
{
    public class CertificateMappingProfile : Profile
    {
        public CertificateMappingProfile()
        {
            // Basic entity to GraphQL type mappings - only for existing properties
            CreateMap<Certificate, CertificateGraphQLType>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.CertificateNumber, opt => opt.MapFrom(src => src.CertificateNumber))
                .ForMember(dest => dest.CompanyId, opt => opt.MapFrom(src => src.CompanyId))
                .ForMember(dest => dest.CertificateTypeId, opt => opt.MapFrom(src => src.CertificateTypeId))
                .ForMember(dest => dest.AuditId, opt => opt.MapFrom(src => src.AuditId))
                .ForMember(dest => dest.IssueDate, opt => opt.MapFrom(src => src.IssueDate))
                .ForMember(dest => dest.ExpiryDate, opt => opt.MapFrom(src => src.ExpiryDate))
                .ForMember(dest => dest.RenewalDate, opt => opt.MapFrom(src => src.RenewalDate))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive));

            CreateMap<CertificateType, CertificateTypeGraphQLType>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.TypeName, opt => opt.MapFrom(src => src.TypeName))
                .ForMember(dest => dest.Standard, opt => opt.MapFrom(src => src.Standard))
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.ValidityPeriodMonths, opt => opt.MapFrom(src => src.ValidityPeriodMonths))
                .ForMember(dest => dest.IsAccredited, opt => opt.MapFrom(src => src.IsAccredited))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive));

            CreateMap<Company, CompanyGraphQLType>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.CompanyCode, opt => opt.MapFrom(src => src.CompanyCode))
                .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.CompanyName))
                .ForMember(dest => dest.ContactPerson, opt => opt.MapFrom(src => src.ContactPerson))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive));

            CreateMap<Site, SiteGraphQLType>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.SiteCode, opt => opt.MapFrom(src => src.SiteCode))
                .ForMember(dest => dest.SiteName, opt => opt.MapFrom(src => src.SiteName))
                .ForMember(dest => dest.CompanyId, opt => opt.MapFrom(src => src.CompanyId))
                .ForMember(dest => dest.CityId, opt => opt.MapFrom(src => src.CityId))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive));

            CreateMap<Service, ServiceGraphQLType>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.ServiceCode, opt => opt.MapFrom(src => src.ServiceCode))
                .ForMember(dest => dest.ServiceName, opt => opt.MapFrom(src => src.ServiceName))
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive));

            CreateMap<User, UserGraphQLType>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role))
                .ForMember(dest => dest.Department, opt => opt.MapFrom(src => src.Department))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive));

            CreateMap<Audit, AuditGraphQLType>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.AuditNumber, opt => opt.MapFrom(src => src.AuditNumber))
                .ForMember(dest => dest.LeadAuditorId, opt => opt.MapFrom(src => src.LeadAuditorId))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));

            CreateMap<Country, CountryGraphQLType>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.CountryName, opt => opt.MapFrom(src => src.CountryName))
                .ForMember(dest => dest.CountryCode, opt => opt.MapFrom(src => src.CountryCode));

            CreateMap<City, CityGraphQLType>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.CityName, opt => opt.MapFrom(src => src.CityName))
                .ForMember(dest => dest.CountryId, opt => opt.MapFrom(src => src.CountryId));
        }
    }
}