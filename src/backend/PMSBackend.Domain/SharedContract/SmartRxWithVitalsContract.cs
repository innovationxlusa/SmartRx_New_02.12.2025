using PMSBackend.Domain.CommonDTO;

namespace PMSBackend.Domain.SharedContract
{
    public class SmartRxWithVitalsContract
    {
        public long SmartRxId { get; set; }
        public long PatientId { get; set; }
        public long PrescriptionId { get; set; }
        public DateTime? PrescriptionDate { get; set; }
        public string? Remarks { get; set; }
        public bool? IsRecommended { get; set; }
        public bool? IsApproved { get; set; }
        public bool? IsCompleted { get; set; }
        public DateTime? RecommendedDate { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public DateTime? CompletedDate { get; set; }
        public string? Tag1 { get; set; }
        public string? Tag2 { get; set; }
        public string? Tag3 { get; set; }
        public string? Tag4 { get; set; }
        public string? Tag5 { get; set; }
        
        // Patient Details
        public SmartRxPatientProfileWithVitalsContract? PatientInfo { get; set; }
        
        // Vitals
        public IList<SmartRxVitalContract>? Vitals { get; set; }
    }

    public class SmartRxPatientProfileWithVitalsContract
    {
        public long Id { get; set; }
        public string PatientCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? NickName { get; set; }
        public decimal? Age { get; set; }
        public int? AgeYear { get; set; }
        public int? AgeMonth { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int Gender { get; set; }
        public string? GenderString { get; set; }
        public int? BloodGroup { get; set; }
        public string? Height { get; set; }
        public int? HeightFeet { get; set; }
        public decimal? HeightInches { get; set; }
        public string? HeightMeasurementUnit { get; set; }
        public decimal? Weight { get; set; }
        public string? WeightMeasurementUnit { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? ProfilePhotoName { get; set; }
        public string? ProfilePhotoPath { get; set; }
        public string? Address { get; set; }
        public long? PoliceStationId { get; set; }
        public long? CityId { get; set; }
        public string? PostalCode { get; set; }
        public string? EmergencyContact { get; set; }
        public int? MaritalStatus { get; set; }
        public string? Profession { get; set; }
        public bool? IsExistingPatient { get; set; }
        public long? ExistingPatientId { get; set; }
        public int? ProfileProgress { get; set; }
        public bool IsActive { get; set; }
        public int TotalPrescriptions { get; set; }
        public string? RxType { get; set; }
    }

    public class SmartRxVitalContract
    {
        public long Id { get; set; }
        public long SmartRxMasterId { get; set; }
        public long PrescriptionId { get; set; }
        public long VitalId { get; set; }
        public string VitalName { get; set; }
        public string ApplicableEntity { get; set; }
        public decimal VitalValue { get; set; }
        public string VitalUnit { get; set; }
        public string? VitalStatus { get; set; }
        public int? HeightFeet { get; set; }
        public decimal? HeightInches { get; set; }
        public long? CreatedById { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? CreatedDateStr { get; set; }
    }
}
