namespace PMSBackend.Domain.Entities
{
    public class ExceptionDetails : BaseEntity
    {
        public string ExceptionSource { get; set; }

        public string Message { get; set; }

        public string StackTrace { get; set; }
    }
}
