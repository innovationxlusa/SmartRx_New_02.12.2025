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
    public class CreatePatientRewardCommand : IRequest<PatientRewardsDTO>
    {
        public long? SmartRxMasterId { get; set; }
        public long? PrescriptionId { get; set; }
        public long PatientId { get; set; }
        public long BadgeId { get; set; }
        public int EarnedNonCashablePoints { get; set; }
        public int ConsumedNonCashablePoints { get; set; }
        public int EarnedCashablePoints { get; set; }
        public int ConsumedCashablePoints { get; set; }
        public double? EarnedMoney { get; set; }
        public double? ConsumedMoney { get; set; }
        public double? EncashedMoney { get; set; }
        public string? Remarks { get; set; }
        public long UserId { get; set; }
    }

    public class CreatePatientRewardCommandHandler : IRequestHandler<CreatePatientRewardCommand, PatientRewardsDTO>
    {
        private readonly IPatientRewardRepository _patientRewardRepository;

        public CreatePatientRewardCommandHandler(IPatientRewardRepository patientRewardRepository)
        {
            _patientRewardRepository = patientRewardRepository;
        }

        public async Task<PatientRewardsDTO> Handle(CreatePatientRewardCommand request, CancellationToken cancellationToken)
        {
            var responseResult = new PatientRewardsDTO();

            // Validation: Check if patient exists
            if (request.PatientId <= 0)
            {
                responseResult.ApiResponseResult = new ApiResponseResult
                {
                    Data = null,
                    StatusCode = StatusCodes.Status404NotFound,
                    Status = "Failed",
                    Message = "Patient ID is required"
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
            if (request.EarnedMoney < 0 || request.ConsumedMoney < 0 || request.EncashedMoney < 0)
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
                UserId = request.UserId,
                SmartRxMasterId = request.SmartRxMasterId,
                PrescriptionId = request.PrescriptionId,
                PatientId = request.PatientId,
               // BadgeId = request.BadgeId,
                EarnedNonCashablePoints = request.EarnedNonCashablePoints,
                ConsumedNonCashablePoints = request.ConsumedNonCashablePoints,
                EarnedCashablePoints = request.EarnedCashablePoints,
                ConsumedCashablePoints = request.ConsumedCashablePoints,                
                EncashedMoney = request.EncashedMoney,
                Remarks = request.Remarks,
                CreatedById = request.UserId,
                CreatedDate = DateTime.UtcNow,
            };

            var newPatientReward = await _patientRewardRepository.CreatePatientRewardAsync(patientReward, cancellationToken);

            var dto = new PatientRewardDTO
            {
                Id = newPatientReward.Id,
                SmartRxMasterId = newPatientReward.SmartRxMasterId,
                PrescriptionId = newPatientReward.PrescriptionId,
                PatientId = newPatientReward.PatientId,
                BadgeId = newPatientReward.BadgeId,
                BadgeName = newPatientReward.RewardBadge?.Name,
                BadgeDescription = newPatientReward.RewardBadge?.Description,
                EarnedNonCashablePoints = newPatientReward.EarnedNonCashablePoints,
                ConsumedNonCashablePoints = newPatientReward.ConsumedNonCashablePoints,
                TotalNonCashablePoints = newPatientReward.TotalNonCashablePoints,
                EarnedCashablePoints = newPatientReward.EarnedCashablePoints,
                ConsumedCashablePoints = newPatientReward.ConsumedCashablePoints,
                TotalCashablePoints = newPatientReward.TotalCashablePoints,
                EarnedMoney = newPatientReward.ConvertedCashableToMoney,
                ConsumedMoney = newPatientReward.EncashedMoney,
                TotalMoney = newPatientReward.TotalMoney,
                EncashMoney = newPatientReward.EncashedMoney,
                Remarks = newPatientReward.Remarks,
                CreatedById = newPatientReward.CreatedById ?? 0,
                CreatedDate = newPatientReward.CreatedDate,
            };

            responseResult.ApiResponseResult = new ApiResponseResult
            {
                Data = dto,
                StatusCode = StatusCodes.Status200OK,
                Status = "Success",
                Message = "Patient reward created successfully"
            };
            return responseResult;
        }
    }
}
