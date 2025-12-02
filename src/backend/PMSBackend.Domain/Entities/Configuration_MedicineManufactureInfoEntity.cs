using System.ComponentModel.DataAnnotations.Schema;

namespace PMSBackend.Domain.Entities
{
    [Table("Configuration_MedicineManufactureInfo")]
    public class Configuration_MedicineManufactureInfoEntity : BaseEntity
    {

        [Column(TypeName = "nvarchar(1000)")]
        public string Name { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string? OriginRegion { get; set; }
        [Column(TypeName = "nvarchar(200)")]
        public string? Importer { get; set; }
        public DateTime? EstablishedDate { get; set; }
        [Column(TypeName = "nvarchar(4000)")]
        public string? Products { get; set; }
        //public bool IsActive { get; set; }
        [Column(TypeName = "nvarchar(200)")]
        public string? CompanyUrl { get; set; }
        [Column(TypeName = "nvarchar(300)")]
        public string? FileName { get; set; } = default!;
        [Column(TypeName = "nvarchar(1000)")]
        public string? FilePath { get; set; } = default!;
        [Column(TypeName = "nvarchar(10)")]
        public string? FileExtension { get; set; }

    }
}
