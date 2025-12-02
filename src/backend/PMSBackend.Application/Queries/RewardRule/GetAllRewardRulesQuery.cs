using MediatR;
using PMSBackend.Application.DTOs;
using PMSBackend.Domain.CommonDTO;
using PMSBackend.Domain.Entities;
using PMSBackend.Domain.Repositories;
using PMSBackend.Domain.SharedContract;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PMSBackend.Application.Queries.RewardRule
{
    public class GetAllRewardRulesQuery : IRequest<PaginatedResult<RewardRuleDTO>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? SortBy { get; set; }
        public string? SortDirection { get; set; }
    }

    public class GetAllRewardRulesQueryHandler : IRequestHandler<GetAllRewardRulesQuery, PaginatedResult<RewardRuleDTO>>
    {
        private readonly IRewardRuleRepository _rewardRuleRepository;

        public GetAllRewardRulesQueryHandler(IRewardRuleRepository rewardRuleRepository)
        {
            _rewardRuleRepository = rewardRuleRepository;
        }

        public async Task<PaginatedResult<RewardRuleDTO>> Handle(GetAllRewardRulesQuery request, CancellationToken cancellationToken)
        {
            var pagingParams = new PagingSortingParams
            {
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                SortBy = request.SortBy ?? "CreatedDate",
                SortDirection = request.SortDirection ?? "desc"
            };

            var result = await _rewardRuleRepository.GetAllRewardRulesAsync(pagingParams, cancellationToken);

            // Map to DTOs
            var dtos = result.Data.Select(r => new RewardRuleDTO
            {
                Id = r.Id,
                ActivityCode = r.ActivityCode,
                ActivityName = r.ActivityName,
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
            }).ToList();

            return new PaginatedResult<RewardRuleDTO>(
                dtos,
                result.TotalRecords,
                result.PageNumber,
                result.PageSize,
                result.SortBy,
                result.SortDirection,
                null, null, null, null, null);
        }
    }
}

