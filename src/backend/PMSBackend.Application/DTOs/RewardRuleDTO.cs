using PMSBackend.Application.CommonServices;
using PMSBackend.Domain.Entities;
using System;

namespace PMSBackend.Application.DTOs
{
    public class RewardRuleDTO
    {
        public long Id { get; set; }
        public string ActivityCode { get; set; } = string.Empty;
        public string ActivityName { get; set; } = string.Empty;
        public string ActivityHeader { get; set; } = string.Empty;
        public string ActivityTaken { get; set; } = string.Empty;
        public RewardType RewardType { get; set; }
        public string RewardDetails { get; set; } = string.Empty;
        public bool IsDeductible { get; set; }
        public bool IsVisibleBenifit { get; set; }
        public double Points { get; set; }
        public bool IsActive { get; set; }
        public long? CreatedById { get; set; }
        public DateTime? CreatedDate { get; set; }
        public long? ModifiedById { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }

    public class RewardRulesDTO
    {
        public RewardRuleDTO? RewardRule { get; set; }
        public ApiResponseResult? ApiResponseResult { get; set; }
    }
}

