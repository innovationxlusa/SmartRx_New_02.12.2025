using PMSBackend.Application.CommonServices;
using PMSBackend.Domain.SharedContract;

namespace PMSBackend.Application.DTOs
{
    public class PatientDropdownDTO
    {
        public IList<PatientDropdown> patientDropdowns { get; set; }
        public ApiResponseResult ApiResponseResult { get; set; }
    }
}
