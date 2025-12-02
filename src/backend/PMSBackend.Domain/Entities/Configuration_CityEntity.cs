
using System.ComponentModel.DataAnnotations.Schema;


namespace PMSBackend.Domain.Entities
{
    [Table("Configuration_City")]
    public class Configuration_CityEntity : BaseEntity
    {
        [Column(TypeName = "nvarchar(100)")]
        public string Name { get; set; }
        [Column(TypeName = "nchar(5)")]
        public string Code { get; set; }
        public long? DistrictId { get; set; }
        [ForeignKey("DistrictId")]
        public virtual Configuration_DistrictEntity District { get; set; }
        public long? CountryId { get; set; }
        [ForeignKey("CountryId")]
        public virtual Configuration_CountryEntity Country { get; set; }

    }
}
