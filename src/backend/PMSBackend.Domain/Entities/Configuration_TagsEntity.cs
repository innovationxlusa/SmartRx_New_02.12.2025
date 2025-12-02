using System.ComponentModel.DataAnnotations.Schema;

namespace PMSBackend.Domain.Entities
{
    [Table("Configuration_Tags")]
    public class Configuration_TagsEntity : BaseEntity
    {
        [Column(TypeName = "nvarchar(200)")]
        public string TagShortName { get; set; }
        [Column(TypeName = "nvarchar(1000)")]
        public string? TagDescription { get; set; }
        [Column(TypeName = "nvarchar(1000)")]
        public string? TagPrescriptionSection { get; set; }

    }
}
