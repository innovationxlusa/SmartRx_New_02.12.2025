namespace PMSBackend.Application.CommonServices.Exceptions
{
    public class ForbiddenAccessException : Exception
    {
        public int ErrorCode { get; }

        public ForbiddenAccessException() : base("Access to this resource is forbidden.")
        {
            ErrorCode = 403; // Default forbidden status code
        }

        public ForbiddenAccessException(string message) : base(message)
        {
            ErrorCode = 403;
        }

        public ForbiddenAccessException(string message, int errorCode) : base(message)
        {
            ErrorCode = errorCode;
        }

        public ForbiddenAccessException(string message, Exception innerException) : base(message, innerException)
        {
            ErrorCode = 403;
        }
    }
}
