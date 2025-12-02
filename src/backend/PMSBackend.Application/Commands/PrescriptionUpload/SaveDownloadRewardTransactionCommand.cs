using MediatR;
using PMSBackend.Domain.Repositories;
using PMSBackend.Domain.SharedContract;

namespace PMSBackend.Application.Commands.PrescriptionUpload
{
    public class SaveDownloadRewardTransactionCommand : IRequest<RewardTransactionResult?>
    {
        public long PrescriptionId { get; set; }
    }

    public class SaveDownloadRewardTransactionCommandHandler : IRequestHandler<SaveDownloadRewardTransactionCommand, RewardTransactionResult?>
    {
        private readonly IPrescriptionUploadRepository _prescriptionUploadRepository;
        private readonly IRewardTransactionRepository _rewardTransactionRepository;

        public SaveDownloadRewardTransactionCommandHandler(
            IPrescriptionUploadRepository prescriptionUploadRepository,
            IRewardTransactionRepository rewardTransactionRepository)
        {
            _prescriptionUploadRepository = prescriptionUploadRepository;
            _rewardTransactionRepository = rewardTransactionRepository;
        }

        public async Task<RewardTransactionResult?> Handle(SaveDownloadRewardTransactionCommand request, CancellationToken cancellationToken)
        {
            var prescription = await _prescriptionUploadRepository.GetDetailsByIdAsync(request.PrescriptionId);

            if (prescription is null || prescription.Id <= 0)
            {
                return null;
            }

            // Use the prescription owner as the reward receiver
            var userId = prescription.UserId;

            var rewardResult = await _rewardTransactionRepository.CreateRewardTransactionForPrescriptionAsync(
                userId,
                prescription.Id,
                prescription.SmartRxId,
                prescription.PatientId,
                "DOWNLOAD_PRESCRIPTION",
                "downloading a prescription",
                cancellationToken);

            return rewardResult;
        }
    }
}


