using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PMSBackend.Application.CommonServices;
using PMSBackend.Application.DTOs;
using PMSBackend.Domain.Entities;
using PMSBackend.Domain.Repositories;

namespace PMSBackend.Application.Commands.PrescriptionUpload
{
    public class EditUploadedPrescriptionCommand : IRequest<PrescriptionUploadDTO>
    {
        public string TaskName{ get; set;}
        public long PrescriptionId { get; set; }
        public long? FolderId { get; set; }
        public long UserId { get; set; }
        public bool? IsExistingPatient { get; set; }
        public long? PatientId { get; set; }
        public bool? HasExistingRelative { get; set; }
        public string? RelativePatientIds { get; set; }
        public bool? IsLocked { get; set; }
        public bool? IsReported { get; set; }
        public string? ReportReason { get; set; }
        public string? ReportDetails { get; set; }
        public bool? IsRecommended { get; set; }
        public bool? IsApproved { get; set; }
        public bool? IsCompleted { get; set; }
        public long UpdatedBy { get; set; }
        public string? Tag1 { get; set; }
        public string? Tag2 { get; set; }
        public string? Tag3 { get; set; }
        public string? Tag4 { get; set; }
        public string? Tag5 { get; set; }
        
    }
    public class UpdateUploadedPrescriptionCommandHandler : IRequestHandler<EditUploadedPrescriptionCommand, PrescriptionUploadDTO>
    {

        private readonly IPrescriptionUploadRepository _repository;
        private readonly IRewardTransactionRepository _rewardTransactionRepository;

        public UpdateUploadedPrescriptionCommandHandler(
            IPrescriptionUploadRepository repository,
            IRewardTransactionRepository rewardTransactionRepository)
        {
            _repository = repository;
            _rewardTransactionRepository = rewardTransactionRepository;
        }

        public async Task<PrescriptionUploadDTO> Handle(EditUploadedPrescriptionCommand request, CancellationToken cancellationtoken)
        {
            try
            {
                var responseResult = new PrescriptionUploadDTO();
                var prescriptionInfo = await _repository.GetDetailsByIdAsync(request.PrescriptionId);

                if (prescriptionInfo is null)
                {
                    responseResult.ApiResponseResult = new ApiResponseResult
                    {
                        Data = null,
                        StatusCode = StatusCodes.Status400BadRequest,
                        Status = "Failed",
                        Message = "File not found"
                    };
                    return responseResult;
                }
                if (request.FolderId is not null && request.FolderId > 0) prescriptionInfo!.FolderId = request.FolderId.Value;
                if (!string.IsNullOrWhiteSpace(request.Tag1)) prescriptionInfo!.Tag1 = request.Tag1;
                if (!string.IsNullOrWhiteSpace(request.Tag2)) prescriptionInfo!.Tag2 = request.Tag2;
                if (!string.IsNullOrWhiteSpace(request.Tag3)) prescriptionInfo!.Tag3 = request.Tag3;
                if (!string.IsNullOrWhiteSpace(request.Tag4)) prescriptionInfo!.Tag4 = request.Tag4;
                if (!string.IsNullOrWhiteSpace(request.Tag5)) prescriptionInfo!.Tag5 = request.Tag5;

                if (request.IsExistingPatient.HasValue) prescriptionInfo!.IsExistingPatient = request.IsExistingPatient;
                if (request.IsExistingPatient.HasValue && request.PatientId is not null && request.PatientId > 0) prescriptionInfo!.PatientId = request.PatientId;
                if (request.HasExistingRelative.HasValue) prescriptionInfo!.HasExistingRelative = request.HasExistingRelative;
                if (request.HasExistingRelative.HasValue && !string.IsNullOrWhiteSpace(request.RelativePatientIds)) prescriptionInfo!.RelativePatientIds = request.RelativePatientIds;

                if (request.IsLocked.HasValue) prescriptionInfo!.IsLocked = request.IsLocked;
                if (request.IsLocked.HasValue && request.IsLocked.Value) prescriptionInfo.LockedById = request.UpdatedBy;
                if (request.IsLocked.HasValue && request.IsLocked.Value) prescriptionInfo.LockedDate = DateTime.Now;

                if (request.IsReported.HasValue) prescriptionInfo!.IsReported = request.IsReported;
                if (request.IsReported.HasValue && request.IsReported.Value) prescriptionInfo.ReportById = request.UpdatedBy;
                if (request.IsReported.HasValue && request.IsReported.Value) prescriptionInfo.ReportDate = DateTime.Now;
                if (request.IsReported.HasValue && request.IsReported.Value) prescriptionInfo.ReportDetails = request.ReportDetails;
                if (request.IsReported.HasValue && request.IsReported.Value) prescriptionInfo.ReportReason = request.ReportReason;

                if (request.IsRecommended.HasValue) prescriptionInfo!.IsRecommended = request.IsRecommended;
                if (request.IsRecommended.HasValue && request.IsRecommended.Value) prescriptionInfo.RecommendedById = request.UpdatedBy;
                if (request.IsRecommended.HasValue && request.IsRecommended.Value) prescriptionInfo.RecommendedDate = DateTime.Now;

                if (request.IsApproved.HasValue) prescriptionInfo!.IsApproved = request.IsApproved;
                if (request.IsApproved.HasValue && request.IsApproved.Value) prescriptionInfo.ApprovedById = request.UpdatedBy;
                if (request.IsApproved.HasValue && request.IsApproved.Value) prescriptionInfo.ApprovedDate = DateTime.Now;

                if (request.IsCompleted.HasValue) prescriptionInfo!.IsCompleted = request.IsCompleted;
                if (request.IsCompleted.HasValue && request.IsCompleted.Value) prescriptionInfo.CompletedById = request.UpdatedBy;
                if (request.IsCompleted.HasValue && request.IsCompleted.Value) prescriptionInfo.CompletedDate = DateTime.Now;

                prescriptionInfo!.ModifiedById = request.UpdatedBy;
                prescriptionInfo.ModifiedDate = DateTime.Now;

                var result = await _repository.UpdateAsync(prescriptionInfo);

                PrescriptionUploadDTO patientDto = new PrescriptionUploadDTO()
                {
                    Id = prescriptionInfo.Id,
                    UserId = prescriptionInfo.UserId,
                    FolderId = prescriptionInfo.FolderId,
                    PrescriptionCode = prescriptionInfo.PrescriptionCode,
                    FilePath = prescriptionInfo.FilePath,
                    FileName = prescriptionInfo.FileName,
                    IsExistingPatient = prescriptionInfo.IsExistingPatient,
                    PatientId = prescriptionInfo.PatientId,
                    HasExistingRelative = prescriptionInfo.HasExistingRelative,
                    RelativePatientIds = prescriptionInfo.RelativePatientIds,
                    IsSmartRxRequested = prescriptionInfo.IsSmartRxRequested,
                    IsLocked = prescriptionInfo.IsLocked,
                    LockedBy = prescriptionInfo.LockedById,
                    LockedDate = prescriptionInfo?.LockedDate,
                    IsReported = prescriptionInfo?.IsReported,
                    ReportBy = prescriptionInfo?.ReportById,
                    ReportDate = prescriptionInfo?.ReportDate,
                    ReportDetails = prescriptionInfo?.ReportDetails,
                    ReportReason = prescriptionInfo?.ReportReason,
                    IsRecommended = prescriptionInfo?.IsRecommended,
                    RecommendedBy = prescriptionInfo?.RecommendedById,
                    RecommendedDate = prescriptionInfo?.RecommendedDate,
                    IsApproved = prescriptionInfo?.IsApproved,
                    ApprovedBy = prescriptionInfo?.ApprovedById,
                    ApprovedDate = prescriptionInfo?.ApprovedDate,
                    IsCompleted = prescriptionInfo?.IsCompleted,
                    CompletedBy = prescriptionInfo?.CompletedById,
                    CompletedDate = prescriptionInfo?.CompletedDate,
                    Tag1 = prescriptionInfo?.Tag1,
                    Tag2 = prescriptionInfo?.Tag2,
                    Tag3 = prescriptionInfo?.Tag3,
                    Tag4 = prescriptionInfo?.Tag4,
                    Tag5 = prescriptionInfo?.Tag5,
                    ApiResponseResult = null
                };
                if (result != null)
                {
                    if (!string.IsNullOrWhiteSpace(request.TaskName))
                    {
                        string activityName = request.TaskName switch
                        {
                            "TAG"  => "TAG_PRESCRIPTION",
                            "MOVE" => "MOVE_PRESCRIPTION",
                            _      => string.Empty
                        };

                        if (!string.IsNullOrWhiteSpace(activityName))
                        {
                            string activityFor = request.TaskName switch
                            {
                                "TAG"  => "tagging a prescription",
                                "MOVE" => "moving a prescription",
                                _      => activityName
                            };

                            var rewardResult = await _rewardTransactionRepository.CreateRewardTransactionForPrescriptionAsync(
                                request.UpdatedBy,
                                result.Id,
                                result.SmartRxId,
                                result.PatientId,
                                activityName,
                                activityFor,
                                cancellationtoken);

                            if (rewardResult is not null && rewardResult.IsRewardUpdated)
                            {
                                patientDto.IsRewardUpdated = true;
                                patientDto.RewardTitle = rewardResult.RewardTitle;
                                patientDto.TotalRewardPoints = rewardResult.Points;
                                patientDto.RewardMessage = rewardResult.RewardMessage;
                            }
                        }
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