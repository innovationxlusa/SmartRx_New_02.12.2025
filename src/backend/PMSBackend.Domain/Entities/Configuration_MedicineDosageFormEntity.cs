using System.ComponentModel.DataAnnotations.Schema;

namespace PMSBackend.Domain.Entities
{
    [Table("Configuration_MedicineDosageForm")]
    public class Configuration_MedicineDosageFormEntity : BaseEntity
    {
        [Column(TypeName = "nvarchar(200)")]
        public string Name { get; set; } // Dosage Form
        [Column(TypeName = "nvarchar(15)")]
        public string ShortForm { get; set; }
        [Column(TypeName = "nvarchar(2000)")]
        public string? Description { get; set; }
    }
}
