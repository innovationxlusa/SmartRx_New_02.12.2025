using MediatR;
using PMSBackend.Application.DTOs;
using PMSBackend.Application.CommonServices;
using PMSBackend.Domain.Entities;
using PMSBackend.Domain.Repositories;

namespace PMSBackend.Application.Queries.RewardPointConversions
{
    public class GetAllRewardPointConversionsQuery : IRequest<ApiResponseResult>
    {
    }

    public class GetAllRewardPointConversionsQueryHandler : IRequestHandler<GetAllRewardPointConversionsQuery, ApiResponseResult>
    {
        private readonly IRewardPointConversionsRepository _repository;

        public GetAllRewardPointConversionsQueryHandler(IRewardPointConversionsRepository repository)
        {
            _repository = repository;
        }

        public async Task<ApiResponseResult> Handle(GetAllRewardPointConversionsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var entities = await _repository.GetAllAsync();
                
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
                    Message = "Reward point conversions retrieved successfully"
                };
            }
            catch (Exception ex)
            {
                return new ApiResponseResult
                {
                    Data = null,
                    StatusCode = 500,
                    Status = "Error",
                    Message = $"An error occurred while retrieving reward point conversions: {ex.Message}"
                };
            }
        }
    }
}
