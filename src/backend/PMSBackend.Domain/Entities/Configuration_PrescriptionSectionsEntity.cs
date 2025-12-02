using System.ComponentModel.DataAnnotations.Schema;

namespace PMSBackend.Domain.Entities
{
    [Table("Configuration_PrescriptionSection")]
    public class Configuration_PrescriptionSectionsEntity : BaseEntity
    {
        [Column(TypeName = "nvarchar(2)")]
        public string Code { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string Name { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string HeadlineText { get; set; }


    }
}
