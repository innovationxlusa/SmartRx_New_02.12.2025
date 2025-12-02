using MediatR;
using Microsoft.AspNetCore.Http;
using PMSBackend.Application.CommonServices;
using PMSBackend.Application.DTOs;
using PMSBackend.Domain.Entities;
using PMSBackend.Domain.Repositories;

namespace PMSBackend.Application.Commands.SmartRxInsider
{
    public class AddSmartRxVitalCommand : IRequest<SmartRxVitalDTO>
    {
        public long SmartRxMasterId { get; set; }
        public long PrescriptionId { get; set; }
        public long PatientId { get; set; }
        //public string VitalName { get; set; }
        public long VitalId { get; set; }
        public decimal VitalValue { get; set; }
        public long LoginUserId { get; set; }
    }

    public class CreateVitalCommandHandler : IRequestHandler<AddSmartRxVitalCommand, SmartRxVitalDTO>
    {
        // private readonly ILogger<InsertPrescriptionUploadCommandHandler> _logger;
        private readonly ISmartRxVitalRepository _smartRxVitalRepository;
        private readonly IPrescriptionUploadRepository _prescriptionUploadRepository;
        private readonly IRewardTransactionRepository _rewardTransactionRepository;

        public CreateVitalCommandHandler(
            ISmartRxVitalRepository smartRxVitalRepository,
            IPrescriptionUploadRepository prescriptionUploadRepository,
            IRewardTransactionRepository rewardTransactionRepository)
        {
            _smartRxVitalRepository = smartRxVitalRepository;
            _prescriptionUploadRepository = prescriptionUploadRepository;
            _rewardTransactionRepository = rewardTransactionRepository;
        }

        public async Task<SmartRxVitalDTO> Handle(AddSmartRxVitalCommand request, CancellationToken cancellationtoken)
        {
            try
            {
                var responseResult = new SmartRxVitalDTO();

                var existingVital = await _smartRxVitalRepository.IsExistsVital(request.SmartRxMasterId, request.PrescriptionId, request.VitalId);

                //if (existingVital is not null)
                //{
                //    responseResult.ApiResponseResult = new ApiResponseResult
                //    {
                //        Data = null,
                //        StatusCode = StatusCodes.Status409Conflict,
                //        Status = "Failed",
                //        Message = "This vital is already added in this prescription."
                //    };
                //    return responseResult;
                //}

                SmartRx_PatientVitalsEntity entity = new()
                {
                    SmartRxMasterId = request.SmartRxMasterId,
                    PrescriptionId = request.PrescriptionId,
                    PatientId=request.PatientId,
                    VitalId = request.VitalId,
                    VitalValue = request.VitalValue,
                    VitalStatus = string.Empty,
                    CreatedDate = DateTime.Now,
                    CreatedById = request.LoginUserId
                };
                var result = await _smartRxVitalRepository.AddAsync(entity);
                await Task.CompletedTask;

                SmartRxVitalDTO vitalDto = new()
                {
                    Id = result.Id,
                    SmartRxMasterId = request.SmartRxMasterId,
                    PrescriptionId = request.PrescriptionId,
                    VitalId = request.VitalId,
                    VitalValue = request.VitalValue,
                    VitalStatus = null,
                    ApiResponseResult = null
                };

                // Save reward transaction for adding vital (following InsertPrescriptionUploadCommand pattern)
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
                            "ADD_VITAL_SMARTRX",
                            "adding vital to smartrx",
                            cancellationtoken);

                        if (rewardResult is not null && rewardResult.IsRewardUpdated)
                        {
                            vitalDto.IsRewardUpdated = true;
                            vitalDto.RewardTitle = rewardResult.RewardTitle;
                            vitalDto.TotalRewardPoints = rewardResult.Points;
                            vitalDto.RewardMessage = rewardResult.RewardMessage;
                        }
                    }
                }

                return vitalDto;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
