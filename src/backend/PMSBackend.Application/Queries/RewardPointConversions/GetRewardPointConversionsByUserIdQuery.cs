using MediatR;
using PMSBackend.Application.DTOs;
using PMSBackend.Application.CommonServices;
using PMSBackend.Domain.Entities;
using PMSBackend.Domain.Repositories;

namespace PMSBackend.Application.Queries.RewardPointConversions
{
    public class GetRewardPointConversionsByUserIdQuery : IRequest<ApiResponseResult>
    {
        public long UserId { get; set; }
    }

    public class GetRewardPointConversionsByUserIdQueryHandler : IRequestHandler<GetRewardPointConversionsByUserIdQuery, ApiResponseResult>
    {
        private readonly IRewardPointConversionsRepository _repository;

        public GetRewardPointConversionsByUserIdQueryHandler(IRewardPointConversionsRepository repository)
        {
            _repository = repository;
        }

        public async Task<ApiResponseResult> Handle(GetRewardPointConversionsByUserIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var entities = await _repository.GetByUserIdAsync(request.UserId);
                
                var responseDTOs = entities.Select(entity => new RewardPointConversionsResponseDTO
                {
                    Id = entity.Id,
                    UserId = entity.UserId,
                    FromType = entity.FromType,
                    ToType = entity.ToType,
                    Amount = entity.Amount,
                    ConvertedPoints = entity.ConvertedPoints,
                    Rate = entity.Rate,
                });

                return new ApiResponseResult
                {
                    Data = responseDTOs,
                    StatusCode = 200,
                    Status = "Success",
                    Message = $"Reward point conversions for this user retrieved successfully"
                };
            }
            catch (Exception ex)
            {
                return new ApiResponseResult
                {
                    Data = null,
                    StatusCode = 500,
                    Status = "Error",
                    Message = $"An error occurred while retrieving reward point conversions for user {request.UserId}: {ex.Message}"
                };
            }
        }
    }
}
