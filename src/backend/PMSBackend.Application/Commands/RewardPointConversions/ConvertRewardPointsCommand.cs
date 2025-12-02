using MediatR;
using PMSBackend.Application.CommonServices;
using PMSBackend.Application.DTOs;
using PMSBackend.Domain.Entities;
using PMSBackend.Domain.Repositories;
using System.ComponentModel.DataAnnotations;

namespace PMSBackend.Application.Commands.RewardPointConversions
{
    public class ConvertRewardPointsCommand : IRequest<ApiResponseResult>
    {
        [Required]
        public long UserId { get; set; }

        [Required]
        [Range(1, 3, ErrorMessage = "FromType must be 1 (Noncashable), 2 (Cashable), or 3 (Money)")]
        public RewardType FromType { get; set; }

        [Required]
        [Range(1, 3, ErrorMessage = "ToType must be 1 (Noncashable), 2 (Cashable), or 3 (Money)")]
        public RewardType ToType { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
        public double Amount { get; set; }

        public double? Rate { get; set; }

        public double? ConvertedPoints { get; set; }
    }

    public class ConvertRewardPointsCommandHandler : IRequestHandler<ConvertRewardPointsCommand, ApiResponseResult>
    {
        private readonly IRewardPointConversionsRepository _repository;

        public ConvertRewardPointsCommandHandler(IRewardPointConversionsRepository repository)
        {
            _repository = repository;
        }

        public async Task<ApiResponseResult> Handle(ConvertRewardPointsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Validate conversion types
                if (request.FromType == request.ToType)
                {
                    return new ApiResponseResult
                    {
                        Data = null,
                        StatusCode = 400,
                        Status = "Failed",
                        Message = "FromType and ToType cannot be the same"
                    };
                }

                // Validate conversion types are valid enum values
                if (!Enum.IsDefined(typeof(RewardType), request.FromType) ||
                    !Enum.IsDefined(typeof(RewardType), request.ToType))
                {
                    return new ApiResponseResult
                    {
                        Data = null,
                        StatusCode = 400,
                        Status = "Failed",
                        Message = "Invalid conversion type. Must be amonth Noncashable, Cashable, Money"
                    };
                }

                // Calculate or use provided Rate and ConvertedPoints
                // If not provided, default Rate to 1.0 and ConvertedPoints to Amount
                var rate = request.Rate ?? 1.0;
                var convertedPoints = request.ConvertedPoints ?? request.Amount;

                // Create the conversion entity
                var entity = new SmartRx_RewardPointConversionsEntity
                {
                    UserId = request.UserId,
                    FromType = request.FromType,
                    ToType = request.ToType,
                    Amount = request.Amount,
                    Rate = rate,
                    ConvertedPoints = convertedPoints,
                    CreatedById = request.UserId,
                };

                var createdEntity = await _repository.CreateAsync(entity);

                // Create response DTO
                var responseDTO = new RewardPointConversionResponseDTO
                {
                    Id = createdEntity.Id,
                    UserId = createdEntity.UserId,
                    FromType = createdEntity.FromType,
                    FromTypeName = ((RewardType)createdEntity.FromType).ToString(),
                    ToType = createdEntity.ToType,
                    ToTypeName = ((RewardType)createdEntity.ToType).ToString(),
                    Amount = createdEntity.Amount,
                    ConvertedPoints = createdEntity.ConvertedPoints,
                    Rate = createdEntity.Rate,
                    CreatedDate = createdEntity.CreatedDate
                };

                return new ApiResponseResult
                {
                    Data = responseDTO,
                    StatusCode = 200,
                    Status = "Success",
                    Message = $"Reward points converted successfully from {responseDTO.FromTypeName} to {responseDTO.ToTypeName}"
                };
            }
            catch (Exception ex)
            {
                return new ApiResponseResult
                {
                    Data = null,
                    StatusCode = 500,
                    Status = "Error",
                    Message = $"An error occurred while converting reward points: {ex.Message}"
                };
            }
        }
    }
}
