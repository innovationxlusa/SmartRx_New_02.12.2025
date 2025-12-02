using PMSBackend.Application.CommonServices;

namespace PMSBackend.Application.DTOs
{
    public class MedicineFAQListDTO
    {
        public List<MedicineFAQDTO> medicineFAQList { get; set; }
        public ApiResponseResult ApiResponseResult { get; set; }
    }
}
