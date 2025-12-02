using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PMSBackend.Domain.Entities
{
    [Table("SmartRx_RewardPointConversions")]
    public class SmartRx_RewardPointConversionsEntity : BaseEntity
    {
        [Required]
        public long UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual SmartRxUserEntity User { get; set; }

        public RewardType FromType { get; set; }  // NonCashable
        public RewardType ToType { get; set; }    // Cashable

        [Column(TypeName = "decimal(18,2)")]
        public double ConvertedPoints { get; set; }      // 100

        [Column(TypeName = "decimal(18,2)")]
        public double Rate { get; set; }     

        [Column(TypeName = "decimal(18,2)")]
        public double Amount { get; set; }

    }
}
