using System.Collections.Generic;

namespace PMSBackend.Domain.SharedContract
{
    public class PatientRewardSplitResult
    {
        public PaginatedResult<PatientRewardContract> All { get; set; }
        public PaginatedResult<PatientRewardContract> Earned { get; set; }
        public PaginatedResult<PatientRewardContract> Consumed { get; set; }
    }
}


