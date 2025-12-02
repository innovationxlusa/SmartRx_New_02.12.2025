using System.ComponentModel.DataAnnotations.Schema;

namespace PMSBackend.Domain.Entities
{
    [Table("Configuration_DepartmentSection")]
    public class Configuration_DepartmentSectionEntity : BaseEntity
    {
        [Column(TypeName = "nvarchar(5)")]
        public string Code { get; set; }
        [Column(TypeName = "nvarchar(200)")]
        public string Name { get; set; }
        public long HospitalId { get; set; }
        [ForeignKey("HospitalId")]
        public virtual Configuration_HospitalEntity Hospital { get; set; }
        public virtual IList<Configuration_DepartmentEntity> Departments { get; set; }
    }
}
