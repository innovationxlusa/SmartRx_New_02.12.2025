namespace PMSBackend.Application.DTOs
{
    public class SmartRxPatientProfile
    {
        public string PatientCode { get; set; }
        public string FirstName { get; set; }
        public string? LastName { get; set; }
        public string? NickName { get; set; }
        public decimal? Age { get; set; }
        public int? AgeYear { get; set; }
        public int? AgeMonth { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int Gender { get; set; }//enum
        public int? BloodGroup { get; set; }//enum
        public string? Height { get; set; }
        public int HeightFeet { get; set; } = 0;
        public decimal HeightInch { get; set; } = 0;
        public string HeightMeasuremetUnit { get; set; }
        public decimal? Weight { get; set; }
        public string? WeightMeasuremetUnit { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? ProfilePhotoName { get; set; }
        public string? ProfilePhotoPath { get; set; }
        public string? Address { get; set; }
        public long? PoliceStationId { get; set; }
        public long? CityId { get; set; }
        public long? DistrictId { get; set; }
        public string? PostalCode { get; set; }
        public long CountryId { get; set; }
        public string? EmergencyContact { get; set; }
        public int? MaritalStatus { get; set; }//enum
        public int ProfileProgress { get; set; }
        public bool IsActive { get; set; }
    }
}
