using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMSBackend.Domain.Entities
{
  
    [Table("SmartRx_Reward_Transactions")]
    public class SmartRx_RewardTransactionEntity : BaseEntity
    {
        [Required]
        public long UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual SmartRxUserEntity User { get; set; }

        public long? BadgeId { get; set; }
        // --- Navigation Properties ---
        [ForeignKey(nameof(BadgeId))]
        public virtual Configuration_RewardBadgeEntity? RewardBadge { get; set; }

        [Required]
        public long RewardRuleId { get; set; }
        [ForeignKey("RewardRuleId")]
        public virtual Configuration_RewardRuleEntity RewardRule { get; set; }

        [Required]
        public RewardType RewardType { get; set; }   // NEW

        public long? SmartRxMasterId { get; set; }
        [ForeignKey("SmartRxMasterId")]
        public virtual SmartRx_MasterEntity? SmartRxMaster { get; set; }

        public long? PrescriptionId { get; set; }
     

        public long? PatientId { get; set; }

      
        public bool IsDeductPoints { get; set; } = false;

        [Column(TypeName = "decimal(18,2)")]
        public double AmountChanged { get; set; }   // +10, -20

        [Column(TypeName = "decimal(18,2)")]
        public double NonCashableBalance { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public double CashableBalance { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public double CashablePoints { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public double CashedMoneyBalance { get; set; }

        [MaxLength(500)]
        public string? Remarks { get; set; }
    }

}
