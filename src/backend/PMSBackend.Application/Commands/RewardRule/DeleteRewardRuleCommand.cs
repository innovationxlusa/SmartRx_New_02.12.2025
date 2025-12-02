using MediatR;
using PMSBackend.Domain.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace PMSBackend.Application.Commands.RewardRule
{
    public class DeleteRewardRuleCommand : IRequest<bool>
    {
        public long Id { get; set; }
    }

    public class DeleteRewardRuleCommandHandler : IRequestHandler<DeleteRewardRuleCommand, bool>
    {
        private readonly IRewardRuleRepository _rewardRuleRepository;

        public DeleteRewardRuleCommandHandler(IRewardRuleRepository rewardRuleRepository)
        {
            _rewardRuleRepository = rewardRuleRepository;
        }

        public async Task<bool> Handle(DeleteRewardRuleCommand request, CancellationToken cancellationToken)
        {
            return await _rewardRuleRepository.DeleteRewardRuleAsync(request.Id, cancellationToken);
        }
    }
}

