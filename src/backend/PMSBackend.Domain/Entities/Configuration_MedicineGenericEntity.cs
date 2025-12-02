using System.ComponentModel.DataAnnotations.Schema;

namespace PMSBackend.Domain.Entities
{
    [Table("Configuration_MedicineGeneric")]
    public class Configuration_MedicineGenericEntity : BaseEntity
    {
        [Column(TypeName = "nvarchar(300)")]
        public string Name { get; set; }
        [Column(TypeName = "nvarchar(4000)")]
        public string? Description { get; set; }
    }
}
