using MediatR;
using Microsoft.AspNetCore.Http;
using PMSBackend.Application.CommonServices;
using PMSBackend.Domain.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace PMSBackend.Application.Commands.RewardTransaction
{
    public class DeleteRewardTransactionCommand : IRequest<ApiResponseResult>
    {
        public long Id { get; set; }
    }

    public class DeleteRewardTransactionCommandHandler : IRequestHandler<DeleteRewardTransactionCommand, ApiResponseResult>
    {
        private readonly IRewardTransactionRepository _rewardTransactionRepository;

        public DeleteRewardTransactionCommandHandler(IRewardTransactionRepository rewardTransactionRepository)
        {
            _rewardTransactionRepository = rewardTransactionRepository;
        }

        public async Task<ApiResponseResult> Handle(DeleteRewardTransactionCommand request, CancellationToken cancellationToken)
        {
            if (request.Id <= 0)
            {
                return new ApiResponseResult
                {
                    Data = null,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Status = "Failed",
                    Message = "Reward transaction ID must be greater than zero"
                };
            }

            var result = await _rewardTransactionRepository.DeleteRewardTransactionAsync(request.Id, cancellationToken);

            if (!result)
            {
                return new ApiResponseResult
                {
                    Data = null,
                    StatusCode = StatusCodes.Status404NotFound,
                    Status = "Failed",
                    Message = $"Reward transaction with ID {request.Id} not found"
                };
            }

            return new ApiResponseResult
            {
                Data = result,
                StatusCode = StatusCodes.Status200OK,
                Status = "Success",
                Message = "Reward transaction deleted successfully"
            };
        }
    }
}

