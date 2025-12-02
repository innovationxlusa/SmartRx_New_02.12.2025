using MediatR;
using PMSBackend.Application.DTOs;
using PMSBackend.Domain.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace PMSBackend.Application.Queries.RewardRule
{
    public class GetRewardRuleByIdQuery : IRequest<RewardRuleDTO?>
    {
        public long Id { get; set; }
    }

    public class GetRewardRuleByIdQueryHandler : IRequestHandler<GetRewardRuleByIdQuery, RewardRuleDTO?>
    {
        private readonly IRewardRuleRepository _rewardRuleRepository;

        public GetRewardRuleByIdQueryHandler(IRewardRuleRepository rewardRuleRepository)
        {
            _rewardRuleRepository = rewardRuleRepository;
        }

        public async Task<RewardRuleDTO?> Handle(GetRewardRuleByIdQuery request, CancellationToken cancellationToken)
        {
            var result = await _rewardRuleRepository.GetRewardRuleByIdAsync(request.Id, cancellationToken);

            if (result == null)
            {
                return null;
            }

            return new RewardRuleDTO
            {
                Id = result.Id,
                ActivityCode = result.ActivityCode,
                ActivityName = result.ActivityName,
                ActivityTaken = result.ActivityTaken,
                RewardType = result.RewardType,
                RewardDetails = result.RewardDetails,
                IsDeductible = result.IsDeductible,
                IsVisibleBenifit = result.IsVisibleBenifit,
                Points = result.Points,
                IsActive = result.IsActive,
                CreatedById = result.CreatedById,
                CreatedDate = result.CreatedDate,
                ModifiedById = result.ModifiedById,
                ModifiedDate = result.ModifiedDate
            };
        }
    }
}

