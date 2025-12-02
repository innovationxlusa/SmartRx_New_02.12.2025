using MediatR;
using Microsoft.AspNetCore.Http;
using PMSBackend.Application.CommonServices;
using PMSBackend.Application.DTOs;
using PMSBackend.Domain.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace PMSBackend.Application.Queries.RewardTransaction
{
    public class GetRewardTransactionByIdQuery : IRequest<RewardTransactionsDTO?>
    {
        public long Id { get; set; }
    }

    public class GetRewardTransactionByIdQueryHandler : IRequestHandler<GetRewardTransactionByIdQuery, RewardTransactionsDTO?>
    {
        private readonly IRewardTransactionRepository _rewardTransactionRepository;

        public GetRewardTransactionByIdQueryHandler(IRewardTransactionRepository rewardTransactionRepository)
        {
            _rewardTransactionRepository = rewardTransactionRepository;
        }

        public async Task<RewardTransactionsDTO?> Handle(GetRewardTransactionByIdQuery request, CancellationToken cancellationToken)
        {
            var responseResult = new RewardTransactionsDTO();

            var result = await _rewardTransactionRepository.GetRewardTransactionByIdAsync(request.Id, cancellationToken);

            if (result == null)
            {
                responseResult.ApiResponseResult = new ApiResponseResult
                {
                    Data = null,
                    StatusCode = StatusCodes.Status404NotFound,
                    Status = "Failed",
                    Message = $"Reward transaction with ID {request.Id} not found"
                };
                return responseResult;
            }

            var dto = result.ToDto();
            responseResult.RewardTransaction = dto;
            responseResult.ApiResponseResult = new ApiResponseResult
            {
                Data = dto,
                StatusCode = StatusCodes.Status200OK,
                Status = "Success",
                Message = "Reward transaction retrieved successfully"
            };

            return responseResult;
        }
    }
}

