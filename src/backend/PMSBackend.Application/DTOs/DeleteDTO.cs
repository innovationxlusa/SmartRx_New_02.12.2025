using PMSBackend.Application.CommonServices;

namespace PMSBackend.Application.DTOs
{
    public class DeleteDTO
    {
        public bool IsDeleted { get; set; }
        public ApiResponseResult ApiResponseResult { get; set; }
    }
}
