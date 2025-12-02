namespace PMSBackend.Application.DTOs
{
    public class TestCenterDTO
    {
        public long TestCenterId { get; set; }
        public string TestCenterName { get; set; }
        public string? TestCenterBranch { get; set; }
        public string? TestCenterLocation { get; set; }
        public string? TestCenterIcon { get; set; }
        public decimal? TestUnitPrice { get; set; }
        public string? TestPriceMeasurementUnit { get; set; }

    }
}
