using AutoMapper;
using CustomerPortal.ContractsService.Entities;
using CustomerPortal.ContractsService.GraphQL;

namespace CustomerPortal.ContractsService.Mappings;

public class ContractMappingProfile : Profile
{
    public ContractMappingProfile()
    {
        // Entity to GraphQL Type mappings
        CreateMap<Contract, ContractGraphQLType>()
            .ForMember(dest => dest.ContractType, opt => opt.MapFrom(src => src.ContractType));

        CreateMap<Company, CompanyGraphQLType>();

        CreateMap<Service, ServiceGraphQLType>();

        CreateMap<Site, SiteGraphQLType>();

        CreateMap<ContractService, ContractServiceGraphQLType>();

        CreateMap<ContractSite, ContractSiteGraphQLType>();

        CreateMap<ContractTerm, ContractTermGraphQLType>();

        CreateMap<ContractAmendment, ContractAmendmentGraphQLType>();

        CreateMap<ContractRenewal, ContractRenewalGraphQLType>();

        // Input to Entity mappings
        CreateMap<CreateContractInput, Contract>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ContractNumber, opt => opt.Ignore())
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => "DRAFT"))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
            .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
            .ForMember(dest => dest.ModifiedDate, opt => opt.Ignore())
            .ForMember(dest => dest.Company, opt => opt.Ignore())
            .ForMember(dest => dest.Services, opt => opt.Ignore())
            .ForMember(dest => dest.Sites, opt => opt.Ignore())
            .ForMember(dest => dest.Terms, opt => opt.Ignore())
            .ForMember(dest => dest.Amendments, opt => opt.Ignore())
            .ForMember(dest => dest.Renewals, opt => opt.Ignore());

        CreateMap<CreateCompanyInput, Company>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
            .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
            .ForMember(dest => dest.Contracts, opt => opt.Ignore())
            .ForMember(dest => dest.Sites, opt => opt.Ignore());

        CreateMap<CreateServiceInput, Service>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
            .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
            .ForMember(dest => dest.ContractServices, opt => opt.Ignore());

        CreateMap<CreateSiteInput, Site>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
            .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
            .ForMember(dest => dest.Company, opt => opt.Ignore())
            .ForMember(dest => dest.ContractSites, opt => opt.Ignore());

        CreateMap<CreateContractTermInput, ContractTerm>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ContractId, opt => opt.Ignore())
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
            .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
            .ForMember(dest => dest.Contract, opt => opt.Ignore());

        CreateMap<AddContractTermInput, ContractTerm>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
            .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
            .ForMember(dest => dest.Contract, opt => opt.Ignore());

        CreateMap<CreateAmendmentInput, ContractAmendment>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.AmendmentNumber, opt => opt.Ignore())
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => "PENDING"))
            .ForMember(dest => dest.ApprovedBy, opt => opt.Ignore())
            .ForMember(dest => dest.ApprovedDate, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
            .ForMember(dest => dest.Contract, opt => opt.Ignore());

        CreateMap<StartRenewalInput, ContractRenewal>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ContractId, opt => opt.Ignore())
            .ForMember(dest => dest.RenewalNumber, opt => opt.Ignore())
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => "INITIATED"))
            .ForMember(dest => dest.NotificationSentDate, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
            .ForMember(dest => dest.CompletedDate, opt => opt.Ignore())
            .ForMember(dest => dest.ProcessedBy, opt => opt.Ignore())
            .ForMember(dest => dest.Contract, opt => opt.Ignore());
    }
}