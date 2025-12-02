using MediatR;
using Microsoft.AspNetCore.Http;
using PMSBackend.Application.CommonServices;
using PMSBackend.Application.DTOs;
using PMSBackend.Domain.Repositories;

namespace PMSBackend.Application.Commands.SmartRxInsider
{
    public class EditSmartRxSingleVitalCommand : IRequest<SmartRxVitalDTO>
    {
        public long SmartRxMasterId { get; set; }
        public long PrescriptionId { get; set; }
        public long VitalId { get; set; }
        public decimal VitalValue { get; set; }
        public int? HeightFeet { get; set; }
        public decimal? HeightInches { get; set; }
        public long LoginUserId { get; set; }
    }
    public class EditSmartRxSingleVitalCommandHandler : IRequestHandler<EditSmartRxSingleVitalCommand, SmartRxVitalDTO>
    {
        // private readonly ILogger<InsertPrescriptionUploadCommandHandler> _logger;
        private readonly ISmartRxVitalRepository _smartRxVitalRepository;
        private readonly IPrescriptionUploadRepository _prescriptionUploadRepository;
        private readonly IRewardTransactionRepository _rewardTransactionRepository;

        public EditSmartRxSingleVitalCommandHandler(
            ISmartRxVitalRepository smartRxVitalRepository,
            IPrescriptionUploadRepository prescriptionUploadRepository,
            IRewardTransactionRepository rewardTransactionRepository)
        {
            _smartRxVitalRepository = smartRxVitalRepository;
            _prescriptionUploadRepository = prescriptionUploadRepository;
            _rewardTransactionRepository = rewardTransactionRepository;
        }

        public async Task<SmartRxVitalDTO> Handle(EditSmartRxSingleVitalCommand request, CancellationToken cancellationtoken)
        {
            try
            {
                var responseResult = new SmartRxVitalDTO();

                var vital = await _smartRxVitalRepository.IsExistsVital(request.SmartRxMasterId, request.PrescriptionId, request.VitalId);

                if (vital is null)
                {
                    responseResult.ApiResponseResult = new ApiResponseResult
                    {
                        Data = null,
                        StatusCode = StatusCodes.Status417ExpectationFailed,
                        Status = "Failed",
                        Message = "This vital does not exists."
                    };
                    return responseResult;
                }

                vital.VitalValue = request.VitalValue;
                vital.HeightFeet = request.HeightFeet is not null ? request.HeightFeet : null;
                vital.HeightInches=request.HeightInches is not null ? request.HeightInches : null;
                vital.ModifiedDate = DateTime.Now;
                vital.ModifiedById = request.LoginUserId;

                var result = await _smartRxVitalRepository.UpdateAsync(vital);
                await Task.CompletedTask;

                responseResult = new()
                {
                    Id = result.Id,
                    SmartRxMasterId = result.SmartRxMasterId,
                    PrescriptionId = result.PrescriptionId,
                    VitalId = result.VitalId,
                    VitalValue = result.VitalValue,
                    HeightFeet= result.HeightFeet,
                    HeightInches=result.HeightInches,
                    VitalStatus = null,
                    ApiResponseResult = null
                };

                // Save reward transaction for editing vital (following InsertPrescriptionUploadCommand pattern)
                if (result != null)
                {
                    var prescription = await _prescriptionUploadRepository.GetDetailsByIdAsync(request.PrescriptionId);
                    if (prescription != null)
                    {
                        var rewardResult = await _rewardTransactionRepository.CreateRewardTransactionForPrescriptionAsync(
                            request.LoginUserId,
                            request.PrescriptionId,
                            request.SmartRxMasterId,
                            prescription.PatientId,
                            "EDIT_VITAL_SMARTRX",
                            "editing vital in smartrx",
                            cancellationtoken);

                        if (rewardResult is not null && rewardResult.IsRewardUpdated)
                        {
                            responseResult.IsRewardUpdated = true;
                            responseResult.RewardTitle = rewardResult.RewardTitle;
                            responseResult.TotalRewardPoints = rewardResult.Points;
                            responseResult.RewardMessage = rewardResult.RewardMessage;
                        }
                    }
                }

                return responseResult;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
