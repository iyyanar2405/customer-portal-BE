namespace CustomerPortal.ContractsService.Entities;

public enum ContractStatus
{
    DRAFT,
    PENDING_APPROVAL,
    ACTIVE,
    SUSPENDED,
    EXPIRED,
    TERMINATED,
    RENEWED
}

public enum ContractType
{
    CERTIFICATION_SERVICES,
    TRAINING_SERVICES,
    CONSULTING_SERVICES,
    AUDIT_SERVICES,
    SUPPORT_SERVICES
}

public enum AmendmentType
{
    SERVICE_ADDITION,
    SERVICE_REMOVAL,
    PRICE_CHANGE,
    TERM_EXTENSION,
    TERM_MODIFICATION,
    SITE_ADDITION,
    SITE_REMOVAL
}

public enum TermType
{
    PAYMENT_SCHEDULE,
    SERVICE_LEVEL,
    CANCELLATION,
    RENEWAL,
    LIABILITY,
    CONFIDENTIALITY,
    INTELLECTUAL_PROPERTY
}

public enum PaymentTerms
{
    NET_15,
    NET_30,
    NET_45,
    NET_60,
    COD,
    ADVANCE
}