using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PMSBackend.Application.DTOs;
using PMSBackend.Domain.Entities;
using PMSBackend.Domain.Repositories;

namespace PMSBackend.Application.Commands.PrescriptionUpload
{
    public class EditForSmartRxRequestCommand : IRequest<PrescriptionUploadDTO>
    {
        public long PrescriptionId { get; set; }
        public long UpdatedBy { get; set; }
    }
    public class UpdateForSmartRxRequestCommandHandler : IRequestHandler<EditForSmartRxRequestCommand, PrescriptionUploadDTO>
    {

        private readonly IPrescriptionUploadRepository _repository;
        private readonly IRewardTransactionRepository _rewardTransactionRepository;

        public UpdateForSmartRxRequestCommandHandler(
            IPrescriptionUploadRepository repository,
            IRewardTransactionRepository rewardTransactionRepository)
        {
            _repository = repository;
            _rewardTransactionRepository = rewardTransactionRepository;
        }

        public async Task<PrescriptionUploadDTO> Handle(EditForSmartRxRequestCommand request, CancellationToken cancellationtoken)
        {
            try
            {
                var prescriptionInfo = await _repository.GetDetailsByIdAsync(request.PrescriptionId);
                prescriptionInfo!.IsSmartRxRequested = true;
                prescriptionInfo.ModifiedById = request.UpdatedBy;
                prescriptionInfo.ModifiedDate = DateTime.Now;

                var result=  await _repository.UpdateAsync(prescriptionInfo);   
                
                PrescriptionUploadDTO patientDto = new PrescriptionUploadDTO()
                {
                    Id = prescriptionInfo.Id,
                    PrescriptionCode = prescriptionInfo.PrescriptionCode,
                    FilePath = prescriptionInfo.FilePath,
                    FileName = prescriptionInfo.FileName,
                    IsExistingPatient = false,
                    PatientId = prescriptionInfo.PatientId,
                    HasExistingRelative = false,
                    RelativePatientIds = "",
                    IsSmartRxRequested = prescriptionInfo.IsSmartRxRequested
                };
                if (result != null)
                {
                    var rewardResult = await _rewardTransactionRepository.CreateRewardTransactionForPrescriptionAsync(
                        request.UpdatedBy,
                        result.Id,
                        result.SmartRxId,
                        result.PatientId,
                        "REQUEST_SMARTRX",
                        "requesting to convert into smartrx",
                        cancellationtoken);

                    if (rewardResult is not null && rewardResult.IsRewardUpdated)
                    {
                        patientDto.IsRewardUpdated = true;
                        patientDto.RewardTitle = rewardResult.RewardTitle;
                        patientDto.TotalRewardPoints = rewardResult.Points;
                        patientDto.RewardMessage = rewardResult.RewardMessage;
                    }
                }
                await Task.CompletedTask;
                return patientDto;
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}

