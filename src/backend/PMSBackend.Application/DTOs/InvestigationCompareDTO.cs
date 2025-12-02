using PMSBackend.Application.CommonServices;
using PMSBackend.Domain.SharedContract;

namespace PMSBackend.Application.DTOs
{
    public class InvestigationCompareDTO
    {
        public List<long> SourceTestIds { get; set; }

        public IEnumerable<InvestigationTestDTO> SelectedOrRecommendedTestList { get; set; }
        public PaginatedResult<InvestigationTestDTO> TestCentersListWithBranch { get; set; }
        public PaginatedResult<InvestigationTestDTO> ComparedTestList { get; set; }
        public ApiResponseResult ApiResponseResult { get; set; }

        public bool? IsRewardUpdated { get; set; }
        public string? RewardTitle { get; set; }
        public double? TotalRewardPoints { get; set; }
        public string? RewardMessage { get; set; }
    }
}
