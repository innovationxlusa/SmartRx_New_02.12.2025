using System.ComponentModel.DataAnnotations.Schema;

namespace PMSBackend.Domain.Entities
{

    [Table("Configuration_DiagnosisCenterWiseTest")]
    public class Configuration_DiagnosisCenterWiseTestEntity : BaseEntity
    {
        public long TestCenterId { get; set; }
        [ForeignKey("TestCenterId")]
        public virtual Configuration_HospitalEntity DiagnosticTestCenter { get; set; }
        public long? TestCenterBranchId { get; set; }
        [ForeignKey("TestCenterBranchId")]
        public long TestId { get; set; }
        [ForeignKey("TestId")]
        public virtual Configuration_InvestigationEntity DiagnosticTest { get; set; }

        [Column(TypeName = "nvarchar(1000)")]
        public string DiagnosticCenterGivenTestName { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public decimal? DiagnosticCenterGivenPrice { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal? DiscountByAuthority { get; set; }
        public long? PriceUnitId { get; set; }
        [ForeignKey("PriceUnitId")]
        public virtual Configuration_UnitEntity? PriceUnit { get; set; }

        [Column(TypeName = "nvarchar(1000)")]
        public string? Schedule { get; set; }

        [Column(TypeName = "nvarchar(1000)")]
        public string? ReportDeliveryTime { get; set; }

        [Column(TypeName = "nvarchar(1000)")]
        public string? SpecialNote { get; set; }

        [Column(TypeName = "nvarchar(1000)")]
        public string? Remarks { get; set; }
    }
}
