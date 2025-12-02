using MediatR;
using PMSBackend.Application.DTOs;
using PMSBackend.Domain.CommonDTO;
using PMSBackend.Domain.Entities;
using PMSBackend.Domain.Repositories;
using PMSBackend.Domain.SharedContract;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PMSBackend.Application.Queries.Reward
{
    public class GetAllRewardsQuery : IRequest<PaginatedResult<RewardDTO>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? SortBy { get; set; }
        public string? SortDirection { get; set; }
    }

    public class GetAllRewardsQueryHandler : IRequestHandler<GetAllRewardsQuery, PaginatedResult<RewardDTO>>
    {
        private readonly IRewardRepository _rewardRepository;

        public GetAllRewardsQueryHandler(IRewardRepository rewardRepository)
        {
            _rewardRepository = rewardRepository;
        }

        public async Task<PaginatedResult<RewardDTO>> Handle(GetAllRewardsQuery request, CancellationToken cancellationToken)
        {
            var pagingParams = new PagingSortingParams
            {
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                SortBy = request.SortBy ?? "CreatedDate",
                SortDirection = request.SortDirection ?? "desc"
            };

            var result = await _rewardRepository.GetAllRewardsAsync(pagingParams, cancellationToken);

            // Map to DTOs
            var dtos = result.Data.Select(r => new RewardDTO
            {
                Id = r.Id,
                UserActivityId = r.UserActivityId,                
                RewardCode = r.RewardCode,
                Title = r.Title,
                Details = r.Details,
                IsDeduction = r.IsDeduction,
                NonCashablePoints = r.NonCashablePoints,
                IsCashable = r.IsCashable,
                CashablePoints = r.CashablePoints,
                IsCashedMoney = r.IsCashedMoney,
                CashedMoney = r.CashedMoney,
                IsVisibleToUser = r.IsVisibleToUser,
                CreatedById = r.CreatedById ?? 0,
                CreatedDate = r.CreatedDate,
                ModifiedById = r.ModifiedById,
                ModifiedDate = r.ModifiedDate,
                IsActive = r.IsActive
            }).ToList();

            return new PaginatedResult<RewardDTO>(
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


