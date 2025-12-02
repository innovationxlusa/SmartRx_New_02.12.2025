using PMSBackend.Application.CommonServices;

namespace PMSBackend.Application.DTOs
{
    public class PatientProfileListDTO
    {
        public IList<PatientWithRelativesDTO> PatientProfiles { get; set; }
        public ApiResponseResult ApiResponseResult { get; set; }
    }
}
