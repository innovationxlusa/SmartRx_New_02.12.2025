using MediatR;
using Microsoft.AspNetCore.Http;
using PMSBackend.Application.CommonServices;
using PMSBackend.Application.DTOs;
using PMSBackend.Domain.Entities;
using PMSBackend.Domain.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PMSBackend.Application.Commands.PatientReward
{
    public class UpdatePatientRewardCommand : IRequest<PatientRewardsDTO>
    {
        public long Id { get; set; }

        public long BadgeId { get; set; }
        public int EarnedNonCashablePoints { get; set; }
        public int ConsumedNonCashablePoints { get; set; }
        public int EarnedCashablePoints { get; set; }
        public int ConsumedCashablePoints { get; set; }
        public double? EarnedMoney { get; set; }
        public double? ConsumedMoney { get; set; }
        public double? EncashMoney { get; set; }
        public string? Remarks { get; set; }
        public long UserId { get; set; }
    }

    public class UpdatePatientRewardCommandHandler : IRequestHandler<UpdatePatientRewardCommand, PatientRewardsDTO>
    {
        private readonly IPatientRewardRepository _patientRewardRepository;

        public UpdatePatientRewardCommandHandler(IPatientRewardRepository patientRewardRepository)
        {
            _patientRewardRepository = patientRewardRepository;
        }

        public async Task<PatientRewardsDTO> Handle(UpdatePatientRewardCommand request, CancellationToken cancellationToken)
        {
            var responseResult = new PatientRewardsDTO();

            // Validation: Check if patient reward exists
            var existingReward = await _patientRewardRepository.GetPatientRewardByIdAsync(request.Id, cancellationToken);
            
            if (existingReward == null)
            {
                responseResult.ApiResponseResult = new ApiResponseResult
                {
                    Data = null,
                    StatusCode = StatusCodes.Status404NotFound,
                    Status = "Failed",
                    Message = $"Patient reward with ID {request.Id} not found"
                };
                return responseResult;
            }

            // Validation: Check if badge exists
            if (request.BadgeId <= 0)
            {
                responseResult.ApiResponseResult = new ApiResponseResult
                {
                    Data = null,
                    StatusCode = StatusCodes.Status404NotFound,
                    Status = "Failed",
                    Message = "Badge ID is required"
                };
                return responseResult;
            }

            // Validation: Check points are non-negative
            if (request.EarnedNonCashablePoints < 0 || request.ConsumedNonCashablePoints < 0)
            {
                responseResult.ApiResponseResult = new ApiResponseResult
                {
                    Data = null,
                    StatusCode = StatusCodes.Status417ExpectationFailed,
                    Status = "Failed",
                    Message = "Non-cashable points cannot be negative"
                };
                return responseResult;
            }

            if (request.EarnedCashablePoints < 0 || request.ConsumedCashablePoints < 0)
            {
                responseResult.ApiResponseResult = new ApiResponseResult
                {
                    Data = null,
                    StatusCode = StatusCodes.Status417ExpectationFailed,
                    Status = "Failed",
                    Message = "Cashable points cannot be negative"
                };
                return responseResult;
            }

            // Validation: Check money values are non-negative
            if (request.EarnedMoney < 0 || request.ConsumedMoney < 0 || request.EncashMoney < 0)
            {
                responseResult.ApiResponseResult = new ApiResponseResult
                {
                    Data = null,
                    StatusCode = StatusCodes.Status417ExpectationFailed,
                    Status = "Failed",
                    Message = "Money values cannot be negative"
                };
                return responseResult;
            }

            var patientReward = new SmartRx_PatientReward
            {
                Id = request.Id,
                PatientId = existingReward.PatientId,
                SmartRxMasterId = existingReward.SmartRxMasterId,
                PrescriptionId = existingReward.PrescriptionId,
               // BadgeId = request.BadgeId,
                EarnedNonCashablePoints = request.EarnedNonCashablePoints,
                ConsumedNonCashablePoints = request.ConsumedNonCashablePoints,
                EarnedCashablePoints = request.EarnedCashablePoints,
                ConsumedCashablePoints = request.ConsumedCashablePoints,
                EncashedMoney = request.EncashMoney??0,
                Remarks = request.Remarks,
                ModifiedById = request.UserId
            };

            var updatedPatientReward = await _patientRewardRepository.UpdatePatientRewardAsync(patientReward, cancellationToken);

            var dto = new PatientRewardDTO
            {
                Id = updatedPatientReward.Id,
                SmartRxMasterId = updatedPatientReward.SmartRxMasterId,
                PrescriptionId = updatedPatientReward.PrescriptionId,
                PatientId = updatedPatientReward.PatientId,
                BadgeId = updatedPatientReward.BadgeId,
                BadgeName = updatedPatientReward?.RewardBadge?.Name,
                BadgeDescription = updatedPatientReward?.RewardBadge?.Description,
                EarnedNonCashablePoints = updatedPatientReward.EarnedNonCashablePoints,
                ConsumedNonCashablePoints = updatedPatientReward.ConsumedNonCashablePoints,
                TotalNonCashablePoints = updatedPatientReward.TotalNonCashablePoints,
                EarnedCashablePoints = updatedPatientReward.EarnedCashablePoints,
                ConsumedCashablePoints = updatedPatientReward.ConsumedCashablePoints,
                TotalCashablePoints = updatedPatientReward.TotalCashablePoints,
                TotalMoney = updatedPatientReward.TotalMoney,
                EncashMoney = updatedPatientReward.EncashedMoney,
                Remarks = updatedPatientReward.Remarks,
                CreatedById = updatedPatientReward.CreatedById ?? 0,
                CreatedDate = updatedPatientReward.CreatedDate,
                ModifiedById = updatedPatientReward.ModifiedById,
                ModifiedDate = updatedPatientReward.ModifiedDate,
            };

            responseResult.ApiResponseResult = new ApiResponseResult
            {
                Data = dto,
                StatusCode = StatusCodes.Status200OK,
                Status = "Success",
                Message = "Patient reward updated successfully"
            };
            return responseResult;
        }
    }
}
