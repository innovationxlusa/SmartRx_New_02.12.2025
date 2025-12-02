using MediatR;
using PMSBackend.Application.DTOs;
using PMSBackend.Application.CommonServices;
using PMSBackend.Domain.Entities;
using PMSBackend.Domain.Repositories;

namespace PMSBackend.Application.Commands.RewardPointConversions
{
    public class CreateRewardPointConversionsCommand : IRequest<ApiResponseResult>
    {
        public RewardPointConversionsCreateDTO dto { get; set; } = null!;
    }

    public class CreateRewardPointConversionsCommandHandler : IRequestHandler<CreateRewardPointConversionsCommand, ApiResponseResult>
    {
        private readonly IRewardPointConversionsRepository _repository;

        public CreateRewardPointConversionsCommandHandler(IRewardPointConversionsRepository repository)
        {
            _repository = repository;
        }

        public async Task<ApiResponseResult> Handle(CreateRewardPointConversionsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = new SmartRx_RewardPointConversionsEntity
                {                   
                    UserId = request.dto.UserId,
                    FromType = request.dto.FromType,
                    ToType = request.dto.ToType,
                    Amount = request.dto.Amount,
                    CreatedById = request.dto.UserId
                };

                var createdEntity = await _repository.CreateAsync(entity);

                var responseDTO = new RewardPointConversionsResponseDTO
                {
                    Id = createdEntity.Id,
                    UserId = createdEntity.UserId,
                    FromType = createdEntity.FromType,
                    ToType = createdEntity.ToType,
                    Amount = createdEntity.Amount,
                    ConvertedPoints = createdEntity.ConvertedPoints,
                    Rate = createdEntity.Rate,
                };

                return new ApiResponseResult
                {
                    Data = responseDTO,
                    StatusCode = 200,
                    Status = "Success",
                    Message = "Reward point conversion created successfully"
                };
            }
            catch (Exception ex)
            {
                return new ApiResponseResult
                {
                    Data = null,
                    StatusCode = 500,
                    Status = "Error",
                    Message = $"An error occurred while creating reward point conversion: {ex.Message}"
                };
            }
        }
    }
}
