namespace PMSBackend.Domain.SharedContract
{
    public class RelativeContract
    {
        public long Id { get; set; }
        public string? PatientCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? NickName { get; set; }

        public decimal? Age { get; set; }
        public int? AgeYear { get; set; }
        public int? AgeMonth { get; set; }

        public DateTime? DateOfBirth { get; set; }
        public int Gender { get; set; }//enum
        public int? BloodGroup { get; set; }//enum       

        public string? Height { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? ProfilePhotoName { get; set; }
        public string? ProfilePhotoPath { get; set; }
        public string? Address { get; set; }
        public long? PoliceStationId { get; set; }
        public long? CityId { get; set; }
        public string? PostalCode { get; set; }
        public string? EmergencyContact { get; set; }
        public int? MaritalStatus { get; set; }//enum
        public string? Profession { get; set; }
        public bool? IsExistingPatient { get; set; }
        public long? ExistingPatientId { get; set; }
        public bool IsRelative { get; set; }
        public string? RelationToPatient { get; set; }
        public long? RelatedToPatientId { get; set; }
        public int? ProfileProgress { get; set; }
        public int IsActive { get; set; }
    }
}
