using System.ComponentModel.DataAnnotations.Schema;

namespace PMSBackend.Domain.Entities
{
    [Table("Configuration_UnitConversion")]
    public class Configuration_UnitConversionEntity : BaseEntity
    {
        public string FromUnit { get; set; } = string.Empty;  // e.g. "lb"
        public string ToUnit { get; set; } = string.Empty;    // e.g. "kg"
        public decimal ConversionFactor { get; set; }         // e.g. 0.453592

        public string? Description { get; set; }
        public long UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual SmartRxUserEntity User { get; set; }

    }
}
