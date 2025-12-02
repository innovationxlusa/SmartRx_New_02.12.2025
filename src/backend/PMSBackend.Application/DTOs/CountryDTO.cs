namespace PMSBackend.Application.DTOs
{
    public class CountryDTO
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string CountryCode { get; set; } = string.Empty;
    }
}
