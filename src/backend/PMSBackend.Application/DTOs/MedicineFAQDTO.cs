using PMSBackend.Application.CommonServices;

namespace PMSBackend.Application.DTOs
{
    public class MedicineFAQDTO
    {
        public long Id { get; set; }
        public long MedicineId { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public ApiResponseResult ApiResponseResult { get; set; }
    }
}
