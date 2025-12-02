using System;

namespace PMSBackend.Application.DTOs
{
    public class RewardDTO
    {
        public long Id { get; set; }
        public long UserActivityId { get; set; }
        public string RewardCode { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string? Details { get; set; }
        public bool IsDeduction { get; set; }
        public double? NonCashablePoints { get; set; }
        public bool IsCashable { get; set; }
        public double? CashablePoints { get; set; }
        public bool? IsCashedMoney { get; set; }
        public double? CashedMoney { get; set; }
        public bool IsVisibleToUser { get; set; }
        public long CreatedById { get; set; }
        public DateTime? CreatedDate { get; set; }
        public long? ModifiedById { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool? IsActive { get; set; }
    }
}

