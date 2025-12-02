namespace PMSBackend.Domain.Entities
{
    public class PMSLogModel : BaseEntity
    {
        public ExceptionDetails Exception { get; set; }

        public string Level { get; set; }

        public string AppName { get; set; }

        public string Message { get; set; }

        public Dictionary<string, string> Others { get; set; }
    }
}
