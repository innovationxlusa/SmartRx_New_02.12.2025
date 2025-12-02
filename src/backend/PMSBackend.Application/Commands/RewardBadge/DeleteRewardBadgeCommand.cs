using MediatR;
using PMSBackend.Domain.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PMSBackend.Application.Commands.RewardBadge
{
    public class DeleteRewardBadgeCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }

    public class DeleteRewardBadgeCommandHandler : IRequestHandler<DeleteRewardBadgeCommand, bool>
    {
        private readonly IRewardBadgeRepository _rewardBadgeRepository;

        public DeleteRewardBadgeCommandHandler(IRewardBadgeRepository rewardBadgeRepository)
        {
            _rewardBadgeRepository = rewardBadgeRepository;
        }

        public async Task<bool> Handle(DeleteRewardBadgeCommand request, CancellationToken cancellationToken)
        {
            return await _rewardBadgeRepository.DeleteRewardBadgeAsync(request.Id, cancellationToken);
        }
    }
}

