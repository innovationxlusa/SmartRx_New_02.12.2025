using System.ComponentModel.DataAnnotations.Schema;

namespace PMSBackend.Domain.Entities
{
    [Table("Configuration_Department")]
    public class Configuration_DepartmentEntity : BaseEntity
    {
        [Column(TypeName = "nvarchar(5)")]
        public string Code { get; set; }
        [Column(TypeName = "nvarchar(200)")]
        public string Name { get; set; }
        public long? SectionId { get; set; }
        [ForeignKey("SectionId")]
        public virtual Configuration_DepartmentSectionEntity? DepartmentSection { get; set; }

        //public long ChamberId { get; set; }
        //public virtual IList<Configuration_ChamberEntity> Chambers { get; set; }

        public long HospitalId { get; set; }
        [ForeignKey("HospitalId")]
        public virtual Configuration_HospitalEntity Hospital { get; set; }
    }
}
