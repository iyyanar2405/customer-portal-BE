namespace CustomerPortal.Shared.Common
{
    /// <summary>
    /// Common constants used across services
    /// </summary>
    public static class Constants
    {
        public static class ServiceNames
        {
            public const string ActionsService = "CustomerPortal.ActionsService";
            public const string AuditsService = "CustomerPortal.AuditsService";
            public const string MasterService = "CustomerPortal.MasterService";
            public const string UsersService = "CustomerPortal.UsersService";
            public const string CertificatesService = "CustomerPortal.CertificatesService";
            public const string ContractsService = "CustomerPortal.ContractsService";
            public const string FinancialService = "CustomerPortal.FinancialService";
            public const string FindingsService = "CustomerPortal.FindingsService";
            public const string NotificationsService = "CustomerPortal.NotificationsService";
            public const string SettingsService = "CustomerPortal.SettingsService";
            public const string OverviewService = "CustomerPortal.OverviewService";
            public const string WidgetsService = "CustomerPortal.WidgetsService";
            public const string Gateway = "CustomerPortal.Gateway";
        }

        public static class ApiRoutes
        {
            public const string ActionsBase = "/api/actions";
            public const string AuditsBase = "/api/audits";
            public const string MasterBase = "/api/master";
            public const string UsersBase = "/api/users";
            public const string CertificatesBase = "/api/certificates";
            public const string ContractsBase = "/api/contracts";
            public const string FinancialBase = "/api/financial";
            public const string FindingsBase = "/api/findings";
            public const string NotificationsBase = "/api/notifications";
            public const string SettingsBase = "/api/settings";
            public const string OverviewBase = "/api/overview";
            public const string WidgetsBase = "/api/widgets";
        }

        public static class GraphQLRoutes
        {
            public const string ActionsGraphQL = "/graphql/actions";
            public const string AuditsGraphQL = "/graphql/audits";
            public const string MasterGraphQL = "/graphql/master";
            public const string UsersGraphQL = "/graphql/users";
            public const string CertificatesGraphQL = "/graphql/certificates";
            public const string ContractsGraphQL = "/graphql/contracts";
            public const string FinancialGraphQL = "/graphql/financial";
            public const string FindingsGraphQL = "/graphql/findings";
            public const string NotificationsGraphQL = "/graphql/notifications";
            public const string SettingsGraphQL = "/graphql/settings";
            public const string OverviewGraphQL = "/graphql/overview";
            public const string WidgetsGraphQL = "/graphql/widgets";
        }

        public static class ErrorCodes
        {
            public const string EntityNotFound = "ENTITY_NOT_FOUND";
            public const string ValidationError = "VALIDATION_ERROR";
            public const string DatabaseError = "DATABASE_ERROR";
            public const string UnauthorizedAccess = "UNAUTHORIZED_ACCESS";
            public const string ServiceUnavailable = "SERVICE_UNAVAILABLE";
        }
    }

    /// <summary>
    /// Service discovery configuration
    /// </summary>
    public class ServiceEndpoints
    {
        public string ActionsService { get; set; } = "http://localhost:5001";
        public string AuditsService { get; set; } = "http://localhost:5002";
        public string MasterService { get; set; } = "http://localhost:5003";
        public string UsersService { get; set; } = "http://localhost:5004";
        public string CertificatesService { get; set; } = "http://localhost:5005";
        public string ContractsService { get; set; } = "http://localhost:5006";
        public string FinancialService { get; set; } = "http://localhost:5007";
        public string FindingsService { get; set; } = "http://localhost:5008";
        public string NotificationsService { get; set; } = "http://localhost:5009";
        public string SettingsService { get; set; } = "http://localhost:5010";
        public string OverviewService { get; set; } = "http://localhost:5011";
        public string WidgetsService { get; set; } = "http://localhost:5012";
    }
}