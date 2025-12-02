using System.ComponentModel.DataAnnotations.Schema;

namespace PMSBackend.Domain.Entities
{
    [Table("Configuration_DoctorChamber")]
    public class Configuration_DoctorChamberEntity : BaseEntity
    {       
        public long DoctorId { get; set; }
        [ForeignKey("DoctorId")]
        public virtual Configuration_DoctorEntity Doctor { get; set; }
        public long HospitalId { get; set; }
        [ForeignKey("HospitalId")]
        public virtual Configuration_HospitalEntity? Hospital { get; set; }
        public long? DepartmentSectionId { get; set; }
        [ForeignKey("DepartmentSectionId")]
        public virtual Configuration_DepartmentSectionEntity? DepartmentSection { get; set; }
        public long DepartmentId { get; set; }

        [ForeignKey("DepartmentId")]
        public virtual Configuration_DepartmentEntity? Department { get; set; }
        public bool IsMainChamber { get; set; }
        public string ChamberType { get; set; } // personal, hospital, diagnostic center
      
        [Column(TypeName = "nvarchar(500)")]
        public string ChamberName { get; set; }

        [Column(TypeName = "nvarchar(1500)")]
        public string ChamberAddress { get; set; }
        public long ChamberCityId { get; set; }
        [ForeignKey("CityId")]
        public virtual Configuration_CityEntity City { get; set; }    // get city, P.S, District, Country from City table   
        [Column(TypeName = "nvarchar(20)")]
        public string ChamberPostalCode { get; set; } = string.Empty;
        [Column(TypeName = "nvarchar(500)")]
        public string ChamberDescription { get; set; }

        [Column(TypeName = "nvarchar(2000)")]
        public string ChamberGoogleAddress { get; set; }
        [Column(TypeName = "nvarchar(2000)")]
        public string ChamberGoogleLocationLink { get; set; }
        [Column(TypeName = "nvarchar(5)")]
        public string? ChamberGoogleRating { get; set; }
        [Column(TypeName = "nvarchar(200)")]
        public string? DoctorBookingMobileNos { get; set; }
        [Column(TypeName = "nvarchar(15)")]
        public string? Helpline_CallCenter { get; set; }
        
        [Column(TypeName = "nvarchar(200)")]
        public string? ChamberEmail { get; set; }
        [Column(TypeName = "nvarchar(50)")]
        public string? ChamberOverseasCaller { get; set; }
        [Column(TypeName = "nvarchar(10)")]
        public string? ChamberVisitingHours { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string? ChamberClosedOnDay { get; set; }
        [Column(TypeName = "nvarchar(25)")]
        public string? ChamberWhatsAppNumber { get; set; }
        [Column(TypeName = "nvarchar(6)")]
        public string? ChamberStartTime { get; set; }
        [Column(TypeName = "nvarchar(6)")]
        public string? ChamberEndTime { get; set; }
        [Column(TypeName = "nvarchar(50)")]
        public string? ChamberOtherDoctorsId { get; set; }
        [Column(TypeName = "nvarchar(500)")]
        public string? Remarks { get; set; }  

        public long? DoctorDesignationInChamberId { get; set; }

        [ForeignKey("DoctorDesignationInChamberId")]
        public virtual Configuration_DesignationEntity? DoctorDesignationInChamber { get; set; }
        [Column(TypeName = "nvarchar(10)")]
        public string VisitingHour { get; set; }

        [Column(TypeName = "nvarchar(400)")]
        public string DoctorVisitingDaysInChamber { get; set; }


        [Column(TypeName = "nvarchar(500)")]
        public string DoctorSpecialization { get; set; }
        public bool IsActive { get; set; }
    }
}
