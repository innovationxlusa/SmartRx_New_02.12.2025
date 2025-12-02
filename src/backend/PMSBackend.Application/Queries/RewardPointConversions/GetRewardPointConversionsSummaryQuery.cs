using MediatR;
using PMSBackend.Application.DTOs;
using PMSBackend.Application.CommonServices;
using PMSBackend.Domain.Repositories;

namespace PMSBackend.Application.Queries.RewardPointConversions
{
    public class GetRewardPointConversionsSummaryQuery : IRequest<ApiResponseResult>
    {
        public long UserId { get; set; }
    }

    public class GetRewardPointConversionsSummaryQueryHandler : IRequestHandler<GetRewardPointConversionsSummaryQuery, ApiResponseResult>
    {
        private readonly IRewardPointConversionsRepository _repository;

        public GetRewardPointConversionsSummaryQueryHandler(IRewardPointConversionsRepository repository)
        {
            _repository = repository;
        }

        public async Task<ApiResponseResult> Handle(GetRewardPointConversionsSummaryQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var summary = await _repository.GetConversionSummaryByUserIdAsync(request.UserId);

                return new ApiResponseResult
                {
                    Data = summary,
                    StatusCode = 200,
                    Status = "Success",
                    Message = $"Reward point conversions summary for user {request.UserId} retrieved successfully"
                };
            }
            catch (Exception ex)
            {
                return new ApiResponseResult
                {
                    Data = null,
                    StatusCode = 500,
                    Status = "Error",
                    Message = $"An error occurred while retrieving reward point conversions summary for user {request.UserId}: {ex.Message}"
                };
            }
        }
    }
}
