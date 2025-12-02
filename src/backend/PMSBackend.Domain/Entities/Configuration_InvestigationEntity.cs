using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PMSBackend.Domain.Entities
{
    [Table("Configuration_Investigation")]
    public class Configuration_InvestigationEntity : BaseEntity
    {
        [Column(TypeName = "nchar(5)")]
        public string Code { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(1000)")]
        public string TestName { get; set; }
        [Column(TypeName = "nvarchar(4000)")]
        public string? TestDescription { get; set; }
        [Column(TypeName = "nvarchar(1000)")]
        public string? TestFullName { get; set; }
        [Column(TypeName = "nvarchar(1000)")]
        public string? TestGenericName { get; set; }
        [Column(TypeName = "nvarchar(200)")]
        public string? TestShortName { get; set; }

        [Column(TypeName = "nvarchar(1000)")]
        public string? TestNameByDiagnosticCenter { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public decimal UnitPrice { get; set; } = 0;
        public long? PriceUnitId { get; set; }
        [ForeignKey("PriceUnitId")]
        public virtual Configuration_UnitEntity? PriceUnit { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public decimal NationalUnitPrice { get; set; } = 0;
        public long? NationalPriceUnitId { get; set; }
        [ForeignKey("NationalPriceUnitId")]
        public virtual Configuration_UnitEntity? NationalPriceUnit { get; set; }
        public string NationalPriceReference { get; set; }

        [Column(TypeName = "nvarchar(2000)")]
        public string? Speciality { get; set; }//any special machine/setup/environment etc.
        [Column(TypeName = "nvarchar(500)")]
        public string? Specimen { get; set; }
        [Column(TypeName = "nvarchar(2000)")]
        public string? Comments { get; set; }
        public bool IsActive { get; set; }

    }
}
