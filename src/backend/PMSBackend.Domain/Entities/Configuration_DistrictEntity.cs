using System.ComponentModel.DataAnnotations.Schema;

namespace PMSBackend.Domain.Entities
{
    [Table("Configuration_District")]
    public class Configuration_DistrictEntity : BaseEntity
    {
        [Column(TypeName = "nchar(2)")]
        public string Code { get; set; }
        [Column(TypeName = "varchar(150)")]
        public string Name { get; set; }
        public int? DivisionId { get; set; }
    }
}
