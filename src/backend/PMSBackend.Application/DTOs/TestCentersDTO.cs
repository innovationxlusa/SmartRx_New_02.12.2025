using PMSBackend.Application.CommonServices;

namespace PMSBackend.Application.DTOs
{
    public class TestCentersDTO
    {
        public List<TestCenterDTO> TestCenters { get; set; }
        public ApiResponseResult ApiResponseResult { get; set; }
    }
}
