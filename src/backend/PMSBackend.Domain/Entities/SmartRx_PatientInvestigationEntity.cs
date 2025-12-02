using System.ComponentModel.DataAnnotations.Schema;

namespace PMSBackend.Domain.Entities
{
    [Table("SmartRx_PatientInvestigation")]
    public class SmartRx_PatientInvestigationEntity : BaseEntity
    {
        public long SmartRxMasterId { get; set; }
        [ForeignKey("SmartRxMasterId")]
        public virtual SmartRx_MasterEntity SmartRxMaster { get; set; }
        public long PrescriptionId { get; set; }
        [ForeignKey("PrescriptionId")]
        public virtual Prescription_UploadEntity Prescription { get; set; }

        public long TestId { get; set; }
        [ForeignKey("TestId")]
        public virtual Configuration_InvestigationEntity InvestigationTest { get; set; }

        public long? DiagnosticCenterWiseTestId { get; set; }
        [ForeignKey("DiagnosticCenterWiseTestId")]
        public virtual Configuration_DiagnosisCenterWiseTestEntity DiagnosticCenterWiseTest { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string? UserSelectedTestCenterIds { get; set; }
        [NotMapped]
        public List<Configuration_HospitalEntity> UserSelectedTestCenters { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal DiscountByAuthority { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public decimal? TestPrice { get; set; }
        public long? PriceUnitId { get; set; }
        [ForeignKey("PriceUnitId")]
        public virtual Configuration_UnitEntity? PriceUnit { get; set; }
        public DateTime TestDate { get; set; }
        [Column(TypeName = "nvarchar(1000)")]
        public string? Result { get; set; } // Optional: test result
        [Column(TypeName = "nvarchar(1000)")]
        public string? Remarks { get; set; }

        public bool IsCompleted { get; set; } = false;
        //public bool IsDoctorRecommendedTestCenter { get; set; } = false;
        public string? DoctorRecommendedTestCenterIds { get; set; }
        [NotMapped]
        public List<Configuration_HospitalEntity> DoctorRecommendedTestCenters { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string? Wishlist { get; set; }
    }
}
