namespace PMSBackend.Application.DTOs
{
    public class VitalFAQDTO
    {
        public long Id { get; set; }
        public long VitalId { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
    }

}
