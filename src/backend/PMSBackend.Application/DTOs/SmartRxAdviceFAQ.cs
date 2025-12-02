namespace PMSBackend.Application.DTOs
{
    public class SmartRxAdviceFAQ
    {
        public long Id { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public string SearchKeyword { get; set; }

        public string? IconFileName { get; set; } = default!;
        public string? IconFilePath { get; set; } = default!;
        public string? IconFileExtension { get; set; }
    }
}
