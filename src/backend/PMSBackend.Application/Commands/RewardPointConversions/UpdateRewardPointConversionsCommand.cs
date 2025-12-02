using MediatR;
using PMSBackend.Application.DTOs;
using PMSBackend.Application.CommonServices;
using PMSBackend.Domain.Entities;
using PMSBackend.Domain.Repositories;

namespace PMSBackend.Application.Commands.RewardPointConversions
{
    public class UpdateRewardPointConversionsCommand : IRequest<ApiResponseResult>
    {
        public RewardPointConversionsUpdateDTO dto { get; set; } = null!;
    }

    public class UpdateRewardPointConversionsCommandHandler : IRequestHandler<UpdateRewardPointConversionsCommand, ApiResponseResult>
    {
        private readonly IRewardPointConversionsRepository _repository;

        public UpdateRewardPointConversionsCommandHandler(IRewardPointConversionsRepository repository)
        {
            _repository = repository;
        }

        public async Task<ApiResponseResult> Handle(UpdateRewardPointConversionsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var existingEntity = await _repository.GetByIdAsync(request.dto.Id);
                if (existingEntity == null)
                {
                    return new ApiResponseResult
                    {
                        Data = null,
                        StatusCode = 404,
                        Status = "Failed",
                        Message = "Reward point conversion not found"
                    };
                }

                existingEntity.FromType = request.dto.FromType;
                existingEntity.ToType = request.dto.ToType;
                existingEntity.Amount = request.dto.Amount;

                var updatedEntity = await _repository.UpdateAsync(existingEntity);

                var responseDTO = new RewardPointConversionsResponseDTO
                {
                    Id = updatedEntity.Id,
                    UserId = updatedEntity.UserId,
                    FromType = updatedEntity.FromType,
                    ToType = updatedEntity.ToType,
                    Amount = updatedEntity.Amount,
                    ConvertedPoints = updatedEntity.ConvertedPoints,
                    Rate = updatedEntity.Rate,
                };

                return new ApiResponseResult
                {
                    Data = responseDTO,
                    StatusCode = 200,
                    Status = "Success",
                    Message = "Reward point conversion updated successfully"
                };
            }
            catch (Exception ex)
            {
                return new ApiResponseResult
                {
                    Data = null,
                    StatusCode = 500,
                    Status = "Error",
                    Message = $"An error occurred while updating reward point conversion: {ex.Message}"
                };
            }
        }
    }
}
