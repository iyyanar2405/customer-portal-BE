using CustomerPortal.AuditsService.Entities;

namespace CustomerPortal.AuditsService.GraphQL.Types
{
    public class AuditType : ObjectType<Audit>
    {
        protected override void Configure(IObjectTypeDescriptor<Audit> descriptor)
        {
            descriptor.Field(a => a.Id);
            descriptor.Field(a => a.AuditNumber);
            descriptor.Field(a => a.AuditTitle);
            descriptor.Field(a => a.CompanyId);
            descriptor.Field(a => a.AuditTypeId);
            descriptor.Field(a => a.StartDate);
            descriptor.Field(a => a.EndDate);
            descriptor.Field(a => a.Status);
            descriptor.Field(a => a.LeadAuditorId);
            descriptor.Field(a => a.IsActive);
            descriptor.Field(a => a.CreatedDate);
            descriptor.Field(a => a.ModifiedDate);
            
            // Navigation properties
            descriptor.Field(a => a.Company).Type<CompanyType>();
            descriptor.Field(a => a.AuditType).Type<AuditTypeType>();
            descriptor.Field(a => a.LeadAuditor).Type<UserType>();
            descriptor.Field(a => a.AuditSites).Type<ListType<AuditSiteType>>();
            descriptor.Field(a => a.AuditTeamMembers).Type<ListType<AuditTeamMemberType>>();
            descriptor.Field(a => a.AuditServices).Type<ListType<AuditServiceType>>();
        }
    }

    public class CompanyType : ObjectType<Company>
    {
        protected override void Configure(IObjectTypeDescriptor<Company> descriptor)
        {
            descriptor.Field(c => c.Id);
            descriptor.Field(c => c.CompanyName);
            descriptor.Field(c => c.CompanyCode);
            descriptor.Field(c => c.Address);
            descriptor.Field(c => c.ContactPerson);
            descriptor.Field(c => c.ContactEmail);
            descriptor.Field(c => c.ContactPhone);
            descriptor.Field(c => c.IsActive);
            descriptor.Field(c => c.CreatedDate);
            descriptor.Field(c => c.ModifiedDate);
        }
    }

    public class UserType : ObjectType<User>
    {
        protected override void Configure(IObjectTypeDescriptor<User> descriptor)
        {
            descriptor.Field(u => u.Id);
            descriptor.Field(u => u.FirstName);
            descriptor.Field(u => u.LastName);
            descriptor.Field(u => u.Email);
            descriptor.Field(u => u.Phone);
            descriptor.Field(u => u.Qualifications);
            descriptor.Field(u => u.Role);
            descriptor.Field(u => u.IsActive);
            descriptor.Field(u => u.CreatedDate);
            descriptor.Field(u => u.ModifiedDate);
        }
    }

    public class SiteType : ObjectType<Site>
    {
        protected override void Configure(IObjectTypeDescriptor<Site> descriptor)
        {
            descriptor.Field(s => s.Id);
            descriptor.Field(s => s.SiteName);
            descriptor.Field(s => s.SiteCode);
            descriptor.Field(s => s.Address);
            descriptor.Field(s => s.ContactPerson);
            descriptor.Field(s => s.ContactEmail);
            descriptor.Field(s => s.ContactPhone);
            descriptor.Field(s => s.CompanyId);
            descriptor.Field(s => s.IsActive);
            descriptor.Field(s => s.CreatedDate);
            descriptor.Field(s => s.ModifiedDate);
            
            descriptor.Field(s => s.Company).Type<CompanyType>();
        }
    }

    public class ServiceType : ObjectType<Service>
    {
        protected override void Configure(IObjectTypeDescriptor<Service> descriptor)
        {
            descriptor.Field(s => s.Id);
            descriptor.Field(s => s.ServiceName);
            descriptor.Field(s => s.ServiceCode);
            descriptor.Field(s => s.Description);
            descriptor.Field(s => s.IsActive);
            descriptor.Field(s => s.CreatedDate);
            descriptor.Field(s => s.ModifiedDate);
        }
    }

    public class AuditTypeType : ObjectType<CustomerPortal.AuditsService.Entities.AuditType>
    {
        protected override void Configure(IObjectTypeDescriptor<CustomerPortal.AuditsService.Entities.AuditType> descriptor)
        {
            descriptor.Field(at => at.Id);
            descriptor.Field(at => at.AuditTypeName);
            descriptor.Field(at => at.Description);
            descriptor.Field(at => at.EstimatedDurationDays);
            descriptor.Field(at => at.IsActive);
            descriptor.Field(at => at.CreatedDate);
            descriptor.Field(at => at.ModifiedDate);
        }
    }

    public class AuditSiteType : ObjectType<AuditSite>
    {
        protected override void Configure(IObjectTypeDescriptor<AuditSite> descriptor)
        {
            descriptor.Field(as_ => as_.Id);
            descriptor.Field(as_ => as_.AuditId);
            descriptor.Field(as_ => as_.SiteId);
            descriptor.Field(as_ => as_.ScheduledDate);
            descriptor.Field(as_ => as_.Status);
            descriptor.Field(as_ => as_.Notes);
            descriptor.Field(as_ => as_.IsActive);
            descriptor.Field(as_ => as_.CreatedDate);
            descriptor.Field(as_ => as_.ModifiedDate);
            
            descriptor.Field(as_ => as_.Site).Type<SiteType>();
        }
    }

    public class AuditServiceType : ObjectType<AuditService>
    {
        protected override void Configure(IObjectTypeDescriptor<AuditService> descriptor)
        {
            descriptor.Field(as_ => as_.Id);
            descriptor.Field(as_ => as_.AuditId);
            descriptor.Field(as_ => as_.ServiceId);
            descriptor.Field(as_ => as_.Status);
            descriptor.Field(as_ => as_.Notes);
            descriptor.Field(as_ => as_.IsActive);
            descriptor.Field(as_ => as_.CreatedDate);
            descriptor.Field(as_ => as_.ModifiedDate);
            
            descriptor.Field(as_ => as_.Service).Type<ServiceType>();
        }
    }

    public class AuditTeamMemberType : ObjectType<AuditTeamMember>
    {
        protected override void Configure(IObjectTypeDescriptor<AuditTeamMember> descriptor)
        {
            descriptor.Field(atm => atm.Id);
            descriptor.Field(atm => atm.AuditId);
            descriptor.Field(atm => atm.UserId);
            descriptor.Field(atm => atm.Role);
            descriptor.Field(atm => atm.AssignedDate);
            descriptor.Field(atm => atm.Notes);
            descriptor.Field(atm => atm.IsActive);
            descriptor.Field(atm => atm.CreatedDate);
            descriptor.Field(atm => atm.ModifiedDate);
            
            descriptor.Field(atm => atm.User).Type<UserType>();
        }
    }

    public class AuditLogType : ObjectType<AuditLog>
    {
        protected override void Configure(IObjectTypeDescriptor<AuditLog> descriptor)
        {
            descriptor.Field(al => al.Id);
            descriptor.Field(al => al.AuditId);
            descriptor.Field(al => al.Action);
            descriptor.Field(al => al.Description);
            descriptor.Field(al => al.UserId);
            descriptor.Field(al => al.ActionDate);
            descriptor.Field(al => al.IpAddress);
            descriptor.Field(al => al.IsActive);
            descriptor.Field(al => al.CreatedDate);
            descriptor.Field(al => al.ModifiedDate);
            
            descriptor.Field(al => al.User).Type<UserType>();
        }
    }

    public class AuditSiteAuditType : ObjectType<AuditSiteAudit>
    {
        protected override void Configure(IObjectTypeDescriptor<AuditSiteAudit> descriptor)
        {
            descriptor.Field(asa => asa.Id);
            descriptor.Field(asa => asa.AuditId);
            descriptor.Field(asa => asa.SiteId);
            descriptor.Field(asa => asa.AuditDate);
            descriptor.Field(asa => asa.Status);
            descriptor.Field(asa => asa.LeadAuditorId);
            descriptor.Field(asa => asa.Notes);
            descriptor.Field(asa => asa.ComplianceScore);
            descriptor.Field(asa => asa.FindingsCount);
            descriptor.Field(asa => asa.CriticalFindingsCount);
            descriptor.Field(asa => asa.MajorFindingsCount);
            descriptor.Field(asa => asa.MinorFindingsCount);
            descriptor.Field(asa => asa.ObservationsCount);
            descriptor.Field(asa => asa.IsActive);
            descriptor.Field(asa => asa.CreatedDate);
            descriptor.Field(asa => asa.ModifiedDate);
            
            descriptor.Field(asa => asa.Site).Type<SiteType>();
            descriptor.Field(asa => asa.LeadAuditor).Type<UserType>();
        }
    }
}