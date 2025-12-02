namespace PMSBackend.Application.CommonServices.Exceptions
{
    public class UnprocessableEntityException : Exception
    {
        public int ErrorCode { get; }

        public UnprocessableEntityException(string message) : base(message)
        {
            ErrorCode = 422; // Default unprocessable entity status code
        }

        public UnprocessableEntityException(string message, int errorCode) : base(message)
        {
            ErrorCode = errorCode;
        }

        public UnprocessableEntityException(string message, Exception innerException) : base(message, innerException)
        {
            ErrorCode = 422;
        }
    }
}
