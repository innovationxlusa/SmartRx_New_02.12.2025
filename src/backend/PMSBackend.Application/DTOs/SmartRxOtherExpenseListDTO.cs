using PMSBackend.Application.CommonServices;
using PMSBackend.Domain.SharedContract;

namespace PMSBackend.Application.DTOs
{
    public class SmartRxOtherExpenseListDTO
    {
        public List<SmartRxOtherExpenseDTO>? Data { get; set; }
        public ApiResponseResult? ApiResponseResult { get; set; }
    }
}
