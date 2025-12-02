using System.ComponentModel.DataAnnotations.Schema;

namespace PMSBackend.Domain.Entities
{
    [Table("Configuration_AdviceFAQ")]
    public class Configuration_AdviceFAQEntity : BaseEntity
    {

        [Column(TypeName = "nvarchar(1000)")]
        public string Question { get; set; }
        [Column(TypeName = "nvarchar(4000)")]
        public string Answer { get; set; }
        [Column(TypeName = "nvarchar(4000)")]
        public string TagSearchKeyword { get; set; }

        [Column(TypeName = "nvarchar(300)")]
        public string? IconFileName { get; set; } = default!;
        [Column(TypeName = "nvarchar(1000)")]
        public string? IconFilePath { get; set; } = default!;
        [Column(TypeName = "nvarchar(10)")]
        public string? IconFileExtension { get; set; }

    }
}
