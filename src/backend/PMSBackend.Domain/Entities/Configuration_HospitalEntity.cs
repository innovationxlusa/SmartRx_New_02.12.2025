using System.ComponentModel.DataAnnotations.Schema;

namespace PMSBackend.Domain.Entities
{
    [Table("Configuration_Hospital")]
    public class Configuration_HospitalEntity : BaseEntity
    {
        [Column(TypeName = "nvarchar(5)")]
        public string Code { get; set; }
        [Column(TypeName = "nvarchar(500)")]
        public string Name { get; set; }
        [Column(TypeName = "nvarchar(500)")]
        public string Branch { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string? Location { get; set; }
        [Column(TypeName = "nvarchar(4000)")]
        public string Description { get; set; }
        public string DiagnosticCenterIcon { get; set; }
        [Column(TypeName = "nvarchar(2000)")]
        public string? Address { get; set; }

        public long? CityId { get; set; }
        [ForeignKey("CityId")]
        public virtual Configuration_CityEntity City { get; set; }
        [Column(TypeName = "nvarchar(50)")]
        public string? YearEstablished { get; set; }
        [Column(TypeName = "nvarchar(10)")]
        public string? GoogleRating { get; set; }
        [Column(TypeName = "nvarchar(3000)")]
        public string? GoogleLocation { get; set; }
        [Column(TypeName = "nvarchar(20)")]
        public string? OpenTime { get; set; }
        [Column(TypeName = "nvarchar(20)")]
        public string? CloseTime { get; set; }
        [Column(TypeName = "nvarchar(20)")]
        public string? OpenDay { get; set; }
        [Column(TypeName = "nvarchar(20)")]
        public string? CloseDay { get; set; }
        [Column(TypeName = "nvarchar(20)")]
        public string? Weekend { get; set; }
        [Column(TypeName = "nvarchar(300)")]
        public string? Mobile { get; set; }
        [Column(TypeName = "nvarchar(300)")]
        public string? Fax { get; set; }
        [Column(TypeName = "nvarchar(300)")]
        public string? Phone { get; set; }

        [Column(TypeName = "nvarchar(1000)")]
        public string? WebAddress { get; set; }

        [Column(TypeName = "nvarchar(200)")]
        public string? Email { get; set; }
        [Column(TypeName = "nvarchar(300)")]
        public string? FileName { get; set; } = default!;
        [Column(TypeName = "nvarchar(1000)")]
        public string? FilePath { get; set; } = default!;
        [Column(TypeName = "nvarchar(10)")]
        public string? FileExtension { get; set; }

        [Column(TypeName = "nvarchar(1000)")]
        public string? Remarks { get; set; }
        public bool IsMainBranch { get; set; } = false;
        public bool IsActive { get; set; }
    }
}
