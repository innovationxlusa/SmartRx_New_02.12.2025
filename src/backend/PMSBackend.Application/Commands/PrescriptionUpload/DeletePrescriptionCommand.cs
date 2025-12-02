using MediatR;
using Microsoft.AspNetCore.Http;
using PMSBackend.Application.DTOs;
using PMSBackend.Domain.Entities;
using PMSBackend.Domain.Repositories;

namespace PMSBackend.Application.Commands.PrescriptionUpload
{
    public class DeletePrescriptionCommand : IRequest<DeletePrescriptionDTO>
    {
        public long PrescriptionId { get; set; }
        public long UserId { get; set; }
    }

    public class DeletePrescriptionDTO
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public int StatusCode { get; set; }

        public bool? IsRewardUpdated { get; set; }
        public string? RewardTitle { get; set; }
        public double? TotalRewardPoints { get; set; }
        public string? RewardMessage { get; set; }
    }

    public class DeletePrescriptionCommandHandler : IRequestHandler<DeletePrescriptionCommand, DeletePrescriptionDTO>
    {
        private readonly IPrescriptionUploadRepository _repository;
        private readonly IRewardRuleRepository _rewardRuleRepository;
        private readonly IUserRewardBadgeRepository _userRewardBadgeRepository;
        private readonly IRewardTransactionRepository _rewardTransactionRepository;

        public DeletePrescriptionCommandHandler(IPrescriptionUploadRepository repository, IRewardRuleRepository rewardRuleRepository, IUserRewardBadgeRepository userRewardBadgeRepository, IRewardTransactionRepository rewardTransactionRepository)
        {
            _repository = repository;
            _rewardRuleRepository = rewardRuleRepository;
            _userRewardBadgeRepository = userRewardBadgeRepository;
            _rewardTransactionRepository = rewardTransactionRepository;

        }

        public async Task<DeletePrescriptionDTO> Handle(DeletePrescriptionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var prescription = await _repository.GetDetailsByIdAsync(request.PrescriptionId);

                if (prescription is null || prescription.Id <= 0)
                {
                    return new DeletePrescriptionDTO
                    {
                        Success = false,
                        Message = "Prescription not found",
                        StatusCode = StatusCodes.Status404NotFound
                    };
                }

                if (prescription.IsDeleted)
                {
                    return new DeletePrescriptionDTO
                    {
                        Success = false,
                        Message = "Prescription already deleted",
                        StatusCode = StatusCodes.Status409Conflict
                    };
                }

               
                // Soft delete: mark prescription as deleted instead of physical delete
                prescription.IsDeleted = true;
                prescription.ModifiedById = request.UserId;
                prescription.ModifiedDate = DateTime.Now;

                await _repository.UpdateAsync(prescription);

                var response= new DeletePrescriptionDTO
                {
                    Success = true,
                    Message = "Prescription deleted successfully",
                    StatusCode = StatusCodes.Status200OK
                };

                var rewardResult = await _rewardTransactionRepository.CreateRewardTransactionForPrescriptionAsync(
                    request.UserId,
                    prescription.Id,
                    prescription.SmartRxId,
                    prescription.PatientId,
                    "DELETE_PRESCRIPTION",
                    "deleting a prescription",
                    cancellationToken);

                if (rewardResult is not null && rewardResult.IsRewardUpdated)
                {
                    response.IsRewardUpdated = true;
                    response.RewardTitle = rewardResult.RewardTitle;
                    response.TotalRewardPoints = rewardResult.Points;
                    response.RewardMessage = rewardResult.RewardMessage;
                }

                return response;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        //private async Task RewardTransactionSave(long userId, long prescriptionId, DeletePrescriptionDTO prescriptionDto, CancellationToken cancellationtoken)
        //{
        //    var rewardBadge = await _userRewardBadgeRepository.GetLatestUserRewardBadgeAsync(userId, cancellationtoken);
        //    var rewardPoints = await _rewardRuleRepository.GetRewardRuleByActivityNameAsync("DELETE_PRESCRIPTION", cancellationtoken);
        //    if (rewardPoints != null)
        //    {
        //        var patientReward = new SmartRx_RewardTransactionEntity();
        //        patientReward.BadgeId = rewardBadge != null ? rewardBadge.BadgeId : null;
        //        patientReward.RewardRuleId = rewardPoints.Id;
        //        patientReward.RewardType = RewardType.Noncashable;
        //        patientReward.UserId = userId;
        //        patientReward.PrescriptionId = prescriptionId;
        //        patientReward.SmartRxMasterId = null;
        //        patientReward.PatientId = null;
        //        patientReward.AmountChanged = rewardPoints.Points;
        //        patientReward.NonCashableBalance += rewardPoints.Points;
        //        patientReward.IsDeductPoints = false;
        //        var patientRewardResult = await _rewardTransactionRepository.CreateRewardTransactionAsync(patientReward, cancellationtoken);

        //        prescriptionDto.IsRewardUpdated = true;
        //        prescriptionDto.RewardTitle = rewardPoints.ActivityTaken;
        //        prescriptionDto.TotalRewardPoints = rewardPoints.Points;
        //        prescriptionDto.RewardMessage = $"You have earned {rewardPoints.Points} points for uploading a prescription.";
        //    }
        //}
    }
}