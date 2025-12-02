namespace PMSBackend.Domain.SharedContract
{
    public class PatientWithRelativesContract
    {
        public long? Id { get; set; }
        public string? PatientCode { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? NickName { get; set; }

        public decimal? Age { get; set; }
        public int? AgeYear { get; set; }
        public int? AgeMonth { get; set; }

        public DateTime? DateOfBirth { get; set; }
        public int? Gender { get; set; }//enum
        public string? GenderString { get; set; }//enum
        public int? BloodGroup { get; set; }//enum       

        public string? Height { get; set; }
        public int? HeightFeet { get; set; }
        public decimal? HeightInches { get; set; }
        public long? HeightMeasurementUnitId { get; set; } // ftin, cm

        public string? HeightMeasurementUnit { get; set; } // ftin, cm

        public decimal? Weight { get; set; }
        public long? WeightMeasurementUnitId { get; set; } // kg, lb

        public string? WeightMeasurementUnit { get; set; } // kg, lb
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
        public bool? IsRelative { get; set; }
        public string? RelationToPatient { get; set; }

        public long? RelatedToPatientId { get; set; }
        public int? ProfileProgress { get; set; }
        public List<RelativeContract>? Relatives { get; set; }
        public bool? IsActive { get; set; }
        public int TotalPrescriptions { get; set; }
        public string? RxType { get; set; }
        public int? TotalSmartRx { get; set; } = 0;
        public int? TotalWaiting { get; set; } = 0;
        public int? TotalFileOnly { get; set; } = 0;


    }
}
