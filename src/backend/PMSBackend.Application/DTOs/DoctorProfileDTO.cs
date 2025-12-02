using PMSBackend.Application.CommonServices;
using PMSBackend.Domain.SharedContract;

namespace PMSBackend.Application.DTOs
{
    public class DoctorProfileDTO
    {
        public long DoctorId { get; set; }
        public string DoctorCode { get; set; }
        public string DoctorTitle { get; set; }
        public string DoctorFirstName { get; set; }
        public string DoctorLastName { get; set; }
        public IList<EducationDTO>? DoctorEducationDegrees { get; set; }
        public string? DoctorSpecializedArea { get; set; }
        public string? ProfilePhotoName { get; set; }
        public string? ProfilePhotoPath { get; set; }
        public List<DoctorChamberContract>? DoctorChambers { get; set; }
        public int DoctorYearOfExperiences { get; set; }
        public IList<DoctorChamberContract>? DoctorExperiences { get; set; }
        public string? DoctorBMDCRegNo { get; set; }
        public string? DoctorProfessionalSummary { get; set; }
        public decimal? DoctorRating { get; set; }
        public string? Comments { get; set; }
        public ApiResponseResult ApiResponseResult { get; set; }
    }
}
