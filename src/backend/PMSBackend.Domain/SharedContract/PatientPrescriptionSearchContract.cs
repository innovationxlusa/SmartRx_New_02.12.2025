using PMSBackend.Domain.CommonDTO;

namespace PMSBackend.Domain.SharedContract
{
    public class PatientPrescriptionSearchContract
    {
        public long UserId { get; set; }
        public long? PatientId { get; set; }
        public string? SearchKeyword { get; set; } = null; // Optional - only search when provided
        public string? SearchColumn { get; set; } = "all"; // Default to search all columns
        public PagingSortingParams? PagingSorting { get; set; } = new PagingSortingParams(); // Default initialization for Swagger
    }
}