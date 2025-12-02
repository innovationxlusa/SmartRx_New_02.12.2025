using PMSBackend.Domain.Entities;

namespace PMSBackend.Domain.SharedContract
{
    public class DoctorProfileContract
    {
        public long DoctorId { get; set; }
        public string DoctorCode { get; set; }
        public string DoctorTitle { get; set; }
        public string DoctorFirstName { get; set; }
        public string DoctorLastName { get; set; }
        public string? DoctorEducationDegreesStr { get; set; }
        public IList<Configuration_EducationEntity>? DoctorEducationDegrees { get; set; }
        public string? DoctorSpecializedArea { get; set; }
        public string? ProfilePhotoName { get; set; }
        public string? ProfilePhotoPath { get; set; }
        public string? DoctorChambersStr { get; set; }
        public List<DoctorChamberContract>? DoctorChambers { get; set; }
        public int DoctorYearOfExperiences { get; set; }
        public string? DoctorExperiencesStr { get; set; }
        public IList<DoctorChamberContract>? DoctorExperiences { get; set; }
        public string? DoctorBMDCRegNo { get; set; }
        public string? DoctorProfessionalSummary { get; set; }
        public decimal? DoctorRating { get; set; }
        public string? Comments { get; set; }
    }
}
