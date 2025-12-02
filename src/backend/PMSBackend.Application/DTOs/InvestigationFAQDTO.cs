namespace PMSBackend.Application.DTOs
{
    public class InvestigationFAQDTO
    {
        public long Id { get; set; }
        public long InvestigationId { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
    }
}
