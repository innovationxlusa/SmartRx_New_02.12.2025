namespace PMSBackend.Application.CommonServices.Exceptions
{
    public class ConflictException : Exception
    {
        public int ErrorCode { get; }

        public ConflictException(string message) : base(message)
        {
            ErrorCode = 409; // Default conflict status code
        }

        public ConflictException(string message, int errorCode) : base(message)
        {
            ErrorCode = errorCode;
        }

        public ConflictException(string message, Exception innerException) : base(message, innerException)
        {
            ErrorCode = 409;
        }
    }
}