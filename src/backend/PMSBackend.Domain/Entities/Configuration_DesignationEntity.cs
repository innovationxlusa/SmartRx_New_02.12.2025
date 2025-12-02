using System.ComponentModel.DataAnnotations.Schema;

namespace PMSBackend.Domain.Entities
{
    [Table("Configuration_Designation")]
    public class Configuration_DesignationEntity : BaseEntity
    {
        [Column(TypeName = "nvarchar(5)")]
        public string Code { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string Name { get; set; }
    }
}
