using System.ComponentModel.DataAnnotations.Schema;

namespace PMSBackend.Domain.Entities
{
    [Table("Configuration_PoliceStation")]
    public class Configuration_PoliceStationEntity : BaseEntity
    {
        [Column(TypeName = "nchar(10)")]
        public string Code { get; set; }
        [Column(TypeName = "nvarchar(500)")]
        public string Name { get; set; }
        public long CityId { get; set; }
        [ForeignKey("CityId")]
        public virtual Configuration_CityEntity City { get; set; }
        public long DistrictId { get; set; }
        [ForeignKey("DistrictId")]
        public virtual Configuration_DistrictEntity District { get; set; }
        public long CountryId { get; set; }
        [ForeignKey("CountryId")]
        public virtual Configuration_CountryEntity Country { get; set; }

    }
}
