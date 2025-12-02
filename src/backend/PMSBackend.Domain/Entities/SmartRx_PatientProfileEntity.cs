using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PMSBackend.Domain.Entities
{
    [Table("SmartRx_PatientProfile")]
    public class SmartRx_PatientProfileEntity : BaseEntity
    {
        [Column(TypeName = "nchar(10)")]
        public string PatientCode { get; set; }
        [Column(TypeName = "nvarchar(300)")]
        public string FirstName { get; set; }
        [Column(TypeName = "nvarchar(300)")]
        public string LastName { get; set; }
        [Column(TypeName = "nvarchar(200)")]
        public string? NickName { get; set; }

        public decimal? Age { get; set; }
        public int? AgeYear { get; set; }
        public int? AgeMonth { get; set; }

        public DateTime? DateOfBirth { get; set; }
        public int Gender { get; set; }//enum
        public int? BloodGroup { get; set; }//enum  
        [Column(TypeName = "nvarchar(10)")]
        public string Height { get; set; }
        public int? HeightFeet { get; set; }
        public decimal? HeightInches { get; set; }
        public long? HeightMeasurementUnitId { get; set; }
        [ForeignKey("HeightMeasurementUnitId")]
        public virtual Configuration_UnitEntity HeightUnit { get; set; }
        [NotMapped]
        public string HeightDisplay
        {
            get
            {
                if (HeightFeet.HasValue || HeightInches.HasValue)
                {
                    return $"{HeightFeet ?? 0} ft {HeightInches ?? 0} in";
                }
                return string.Empty;
            }
        }

        public decimal Weight { get; set; }

        public long? WeightMeasurementUnitId { get; set; }
        [ForeignKey("WeightMeasurementUnitId")]
        public virtual Configuration_UnitEntity WeightUnit {get; set;}
      
        [Column(TypeName = "nvarchar(40)")]
        public string PhoneNumber { get; set; }
        [Column(TypeName = "nvarchar(200)")]
        public string Email { get; set; }
        [Column(TypeName = "nvarchar(200)")]
        public string? ProfilePhotoName { get; set; }
        [Column(TypeName = "nvarchar(2000)")]
        public string? ProfilePhotoPath { get; set; }
        [Column(TypeName = "nvarchar(2000)")]
        public string? Address { get; set; }
        public long? PoliceStationId { get; set; }
        [ForeignKey("PoliceStationId")]
        public virtual Configuration_PoliceStationEntity? PoliceStation { get; set; }
        public long? CityId { get; set; }
        [ForeignKey("CityId")]
        public virtual Configuration_CityEntity? City { get; set; }

        [MaxLength(20)]
        public string? PostalCode { get; set; }
        public string? EmergencyContact { get; set; }
        public int? MaritalStatus { get; set; } //enum
        public string? Profession { get; set; }
        public bool IsExistingPatient { get; set; }
        public long? ExistingPatientId { get; set; }
        [ForeignKey("ExistingPatientId")]
        public virtual SmartRx_PatientProfileEntity? ExistingPatientProfile { get; set; }

        public bool IsRelative { get; set; }
        public string? RelationToPatient { get; set; }

        public long? RelatedToPatientId { get; set; }

        [ForeignKey("RelatedToPatientId")]
        public virtual SmartRx_PatientProfileEntity? RelatedToPatientProfile { get; set; }

        public int ProfileProgress { get; set; }

        public bool IsActive { get; set; }
        public long UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual SmartRxUserEntity User { get; set; }

        public bool IsDeleted { get; set; } = false;


        //public virtual IList<Prescription_UploadEntity> Prescriptions { get; set; }
        //public virtual IList<SmartRx_PatientDoctorEntity> PatientDoctors { get; set; }      
        //public virtual IList<SmartRx_PatientVitalsEntity> PatientVitals { get;set; }
        //public virtual IList<SmartRx_PatientChiefComplaintEntity> PatientChiefComplaints { get; set; }


    }
}
