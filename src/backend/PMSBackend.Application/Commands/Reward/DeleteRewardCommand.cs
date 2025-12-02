using MediatR;
using PMSBackend.Domain.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PMSBackend.Application.Commands.Reward
{
    public class DeleteRewardCommand : IRequest<bool>
    {
        public long Id { get; set; }
    }

    public class DeleteRewardCommandHandler : IRequestHandler<DeleteRewardCommand, bool>
    {
        private readonly IRewardRepository _rewardRepository;

        public DeleteRewardCommandHandler(IRewardRepository rewardRepository)
        {
            _rewardRepository = rewardRepository;
        }

        public async Task<bool> Handle(DeleteRewardCommand request, CancellationToken cancellationToken)
        {
            return await _rewardRepository.DeleteRewardAsync(request.Id, cancellationToken);
        }
    }
}

