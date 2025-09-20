namespace CustomerPortal.Shared.Exceptions
{
    /// <summary>
    /// Base exception for all business logic exceptions
    /// </summary>
    public abstract class BusinessException : Exception
    {
        public string ErrorCode { get; }

        protected BusinessException(string errorCode, string message) : base(message)
        {
            ErrorCode = errorCode;
        }

        protected BusinessException(string errorCode, string message, Exception innerException) 
            : base(message, innerException)
        {
            ErrorCode = errorCode;
        }
    }

    /// <summary>
    /// Exception thrown when an entity is not found
    /// </summary>
    public class EntityNotFoundException : BusinessException
    {
        public EntityNotFoundException(string entityName, object id) 
            : base("ENTITY_NOT_FOUND", $"{entityName} with id {id} was not found")
        {
        }
    }

    /// <summary>
    /// Exception thrown for validation errors
    /// </summary>
    public class ValidationException : BusinessException
    {
        public ValidationException(string message) 
            : base("VALIDATION_ERROR", message)
        {
        }

        public ValidationException(string message, Exception innerException) 
            : base("VALIDATION_ERROR", message, innerException)
        {
        }
    }

    /// <summary>
    /// Exception thrown for database errors
    /// </summary>
    public class DatabaseException : BusinessException
    {
        public DatabaseException(string message) 
            : base("DATABASE_ERROR", message)
        {
        }

        public DatabaseException(string message, Exception innerException) 
            : base("DATABASE_ERROR", message, innerException)
        {
        }
    }

    /// <summary>
    /// Exception thrown for unauthorized access
    /// </summary>
    public class UnauthorizedException : BusinessException
    {
        public UnauthorizedException(string message = "Unauthorized access") 
            : base("UNAUTHORIZED_ACCESS", message)
        {
        }
    }

    /// <summary>
    /// Exception thrown when a service is unavailable
    /// </summary>
    public class ServiceUnavailableException : BusinessException
    {
        public ServiceUnavailableException(string serviceName) 
            : base("SERVICE_UNAVAILABLE", $"Service {serviceName} is currently unavailable")
        {
        }

        public ServiceUnavailableException(string serviceName, Exception innerException) 
            : base("SERVICE_UNAVAILABLE", $"Service {serviceName} is currently unavailable", innerException)
        {
        }
    }
}