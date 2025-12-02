using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMSBackend.Domain.Entities
{
    [Table("SmartRx_Reward_UserBalances")]
    public class SmartRx_Reward_UserBalancesEntity : BaseEntity
    {

        [Required]
        public long UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual SmartRxUserEntity User { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal NonCashablePoints { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal CashablePoints { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal CashedMoney { get; set; }
    }
}
