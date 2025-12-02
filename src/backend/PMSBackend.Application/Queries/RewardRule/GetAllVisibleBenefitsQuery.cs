using MediatR;
using PMSBackend.Application.CommonServices;
using PMSBackend.Application.DTOs;
using PMSBackend.Domain.Repositories;
using PMSBackend.Domain.SharedContract;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PMSBackend.Application.Queries.RewardRule
{
    public class GetAllVisibleBenefitsQuery : IRequest<ApiResponseResult>
    {
    }

    public class GetAllVisibleBenefitsQueryHandler : IRequestHandler<GetAllVisibleBenefitsQuery, ApiResponseResult>
    {
        private readonly IRewardRuleRepository _rewardRuleRepository;

        public GetAllVisibleBenefitsQueryHandler(IRewardRuleRepository rewardRuleRepository)
        {
            _rewardRuleRepository = rewardRuleRepository;
        }

        public async Task<ApiResponseResult> Handle(GetAllVisibleBenefitsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                // Get all reward rules with pagination (we'll filter by IsVisibleBenifit in the query)
                var pagingParams = new PMSBackend.Domain.CommonDTO.PagingSortingParams
                {
                    PageNumber = 1,
                    PageSize = 1000, // Large page size to get all visible benefits
                    SortBy = "CreatedDate",
                    SortDirection = "desc"
                };

                var result = await _rewardRuleRepository.GetAllRewardRulesAsync(pagingParams, cancellationToken);

                // Filter to only include rules where IsVisibleBenifit is true
                var visibleBenefits = result.Data
                    .Where(r => r.IsVisibleBenifit == true && r.IsActive == true)
                    .Select(r => new RewardRuleDTO
                    {
                        Id = r.Id,
                        ActivityCode = r.ActivityCode,
                        ActivityName = r.ActivityName,
                        ActivityHeader = r.ActivityHeader,
                        ActivityTaken = r.ActivityTaken,
                        RewardType = r.RewardType,
                        RewardDetails = r.RewardDetails,
                        IsDeductible = r.IsDeductible,
                        IsVisibleBenifit = r.IsVisibleBenifit,
                        Points = r.Points,
                        IsActive = r.IsActive,
                        CreatedById = r.CreatedById,
                        CreatedDate = r.CreatedDate,
                        ModifiedById = r.ModifiedById,
                        ModifiedDate = r.ModifiedDate
                    })
                    .ToList();

                if (visibleBenefits == null || visibleBenefits.Count == 0)
                {
                    return new ApiResponseResult
                    {
                        Data = null,
                        StatusCode = 404,
                        Status = "Failed",
                        Message = "No visible benefits found"
                    };
                }

                return new ApiResponseResult
                {
                    Data = visibleBenefits,
                    StatusCode = 200,
                    Status = "Success",
                    Message = "Visible benefits retrieved successfully"
                };
            }
            catch (System.Exception ex)
            {
                return new ApiResponseResult
                {
                    Data = null,
                    StatusCode = 500,
                    Status = "Error",
                    Message = $"An error occurred while retrieving visible benefits: {ex.Message}"
                };
            }
        }
    }
}

