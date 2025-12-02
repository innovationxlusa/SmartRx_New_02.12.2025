using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMSBackend.Domain.Entities
{
    [Table("Configuration_Settings")]
    public class Configuration_Settings:BaseEntity
    {
        //public bool HasSmartRxMedicineOtherConfiguration { get; set; }

        public bool IsRewardNegetivePointAllowed { get; set; }
        public long PresentRewardBadgeId { get; set; }  
        //public string RewardRemarks { get; set; }

    }
}
