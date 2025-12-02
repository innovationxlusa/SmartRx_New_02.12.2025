


namespace PMSBackend.Application.CommonServices.Exceptions
{
    public class BadRequestException : Exception
    {
        public int ErrorCode { get; }
        public BadRequestException() : base()
        {

        }

        public BadRequestException(string message, int errorCode) : base(message)
        {
            ErrorCode = errorCode;
        }

        public BadRequestException(string message, Exception exp) : base(message, exp)
        {

        }
    }
}
