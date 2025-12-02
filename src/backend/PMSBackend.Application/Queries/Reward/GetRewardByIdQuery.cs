using MediatR;
using PMSBackend.Application.DTOs;
using PMSBackend.Domain.Entities;
using PMSBackend.Domain.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace PMSBackend.Application.Queries.Reward
{
    public class GetRewardByIdQuery : IRequest<RewardDTO?>
    {
        public long Id { get; set; }
    }

    public class GetRewardByIdQueryHandler : IRequestHandler<GetRewardByIdQuery, RewardDTO?>
    {
        private readonly IRewardRepository _rewardRepository;

        public GetRewardByIdQueryHandler(IRewardRepository rewardRepository)
        {
            _rewardRepository = rewardRepository;
        }

        public async Task<RewardDTO?> Handle(GetRewardByIdQuery request, CancellationToken cancellationToken)
        {
            var result = await _rewardRepository.GetRewardByIdAsync(request.Id, cancellationToken);

            if (result == null)
            {
                return null;
            }

            return new RewardDTO
            {
                Id = result.Id,
                UserActivityId = result.UserActivityId,             
                RewardCode = result.RewardCode,
                Title = result.Title,
                Details = result.Details,
                IsDeduction = result.IsDeduction,
                NonCashablePoints = result.NonCashablePoints,
                IsCashable = result.IsCashable,
                CashablePoints = result.CashablePoints,
                IsCashedMoney = result.IsCashedMoney,
                CashedMoney = result.CashedMoney,
                IsVisibleToUser = result.IsVisibleToUser,
                CreatedById = result.CreatedById ?? 0,
                CreatedDate = result.CreatedDate,
                ModifiedById = result.ModifiedById,
                ModifiedDate = result.ModifiedDate,
                IsActive = result.IsActive
            };
        }
    }
}


