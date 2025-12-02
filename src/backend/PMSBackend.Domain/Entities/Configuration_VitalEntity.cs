using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PMSBackend.Domain.Entities
{

    [Table("Configuration_Vital")]
    public class Configuration_VitalEntity : BaseEntity
    {
        [Column(TypeName = "nchar(2)")]
        public string Code { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(100)")]
        public string Name { get; set; }
        [Column(TypeName = "nvarchar(500)")]
        public string? Description { get; set; }
        [Column(TypeName = "nvarchar(200)")]
        public string? ApplicableEntity { get; set; }
        [Required]
        public long UnitId { get; set; }
        [ForeignKey("UnitId")]
        public virtual Configuration_UnitEntity Unit { get; set; }
        [Column(TypeName = "decimal(5,2)")]
        public decimal? LowRange { get; set; }
        [Column(TypeName = "nvarchar(15)")]
        public string? LowStatus { get; set; }
        [Column(TypeName = "decimal(5,2)")]
        public decimal? MidRange { get; set; }
        [Column(TypeName = "nvarchar(15)")]
        public string? MidStatus { get; set; }
        [Column(TypeName = "decimal(5,2)")]
        public decimal? MidNextRange { get; set; }
        [Column(TypeName = "nvarchar(15)")]
        public string? MidNextStatus { get; set; }
        [Column(TypeName = "decimal(5,2)")]
        public decimal? HighRange { get; set; }
        [Column(TypeName = "nvarchar(15)")]
        public string? HighStatus { get; set; }
        [Column(TypeName = "decimal(5,2)")]
        public decimal? ExtremeRange { get; set; }
        [Column(TypeName = "nvarchar(15)")]
        public string? ExtremeStatus { get; set; }

        //public virtual IList<SmartRx_PatientVitalsEntity> PatientVitals { get; set; } = new List<SmartRx_PatientVitalsEntity>();
    }
}
