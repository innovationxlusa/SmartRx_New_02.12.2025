using System.ComponentModel.DataAnnotations.Schema;

namespace PMSBackend.Domain.Entities
{
    [Table("Configuration_MedicineBrand")]
    public class Configuration_MedicineBrandEntity : BaseEntity
    {
        public long ManufacturerId { get; set; }
        [ForeignKey("ManufacturerId")]
        public virtual Configuration_MedicineManufactureInfoEntity Manufacturer { get; set; }
        [Column(TypeName = "nvarchar(5)")]
        public string BrandCode { get; set; }
        public long BrandPublicId { get; set; }
        [Column(TypeName = "nvarchar(300)")]
        public string Name { get; set; }
        [Column(TypeName = "nvarchar(4000)")]
        public string? Description { get; set; }


    }
}
