using PMSBackend.Application.CommonServices;

namespace PMSBackend.Application.DTOs
{
    public class InvestigationFAQListDTO
    {
        public List<InvestigationFAQDTO> investigationFAQs { get; set; }
        public ApiResponseResult ApiResponseResult { get; set; }
    }
}
