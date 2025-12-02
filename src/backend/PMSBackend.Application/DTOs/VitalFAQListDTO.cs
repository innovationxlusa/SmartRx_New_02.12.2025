using PMSBackend.Application.CommonServices;

namespace PMSBackend.Application.DTOs
{
    public class VitalFAQListDTO
    {
        public List<VitalFAQDTO> vitalFAQDTOs { get; set; }
        public ApiResponseResult ApiResponseResult { get; set; }
    }
}
