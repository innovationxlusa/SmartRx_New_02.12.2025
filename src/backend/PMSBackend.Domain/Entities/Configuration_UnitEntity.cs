using System.ComponentModel.DataAnnotations.Schema;

namespace PMSBackend.Domain.Entities
{
    [Table("Configuration_Unit")]
    public class Configuration_UnitEntity : BaseEntity
    {
        [Column(TypeName = "nchar(4)")]
        public string Code { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string Name { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string MeasurementUnit { get; set; }
        [Column(TypeName = "nvarchar(500)")]
        public string? Details { get; set; }
        [Column(TypeName = "nvarchar(1000)")]

        public string? Description { get; set; }
        [Column(TypeName = "nvarchar(50)")]
        public string Type { get; set; }

        //public virtual IList<Configuration_VitalEntity> Vitals { get; set; }
        //public virtual IList<SmartRx_PatientDoctorEntity> SmartRxDoctor { get; set; }
        //public virtual Configuration_VitalEntity Vital { get; set; }

    }
}
