namespace PMSBackend.Domain.Entities
{
    public class Configuration_InvestigationFAQEntitiy : BaseEntity
    {
        public long InvestigationId { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
    }
}
