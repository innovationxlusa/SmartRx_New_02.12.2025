using MediatR;
using Microsoft.AspNetCore.Http;
using PMSBackend.Application.CommonServices;
using PMSBackend.Application.DTOs;
using PMSBackend.Domain.Repositories;

namespace PMSBackend.Application.Queries.PrescriptionUpload
{
    public class GetPrescriptionDetailsByIdQuery : IRequest<PrescriptionUploadDTO>
    {
        public long Id { get; set; }
    }

    public class GetPrescriptionDetailsByIdHandler : IRequestHandler<GetPrescriptionDetailsByIdQuery, PrescriptionUploadDTO>
    {
        private readonly IPrescriptionUploadRepository _prescriptionUploadRepository;
        public GetPrescriptionDetailsByIdHandler(IPrescriptionUploadRepository prescriptionUploadRepository)
        {
            _prescriptionUploadRepository = prescriptionUploadRepository;
        }
        public async Task<PrescriptionUploadDTO> Handle(GetPrescriptionDetailsByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                PrescriptionUploadDTO patientDto = new PrescriptionUploadDTO();
                var prescritption = await _prescriptionUploadRepository.GetDetailsByIdAsync(request.Id);

                if (prescritption is null)
                {
                    patientDto.ApiResponseResult = new ApiResponseResult
                    {
                        Data = null,
                        StatusCode = StatusCodes.Status400BadRequest,
                        Status = "Failed",
                        Message = "No prescription details found!"
                    };
                }

                patientDto = new PrescriptionUploadDTO()
                {
                    Id = prescritption!.Id,
                    PrescriptionCode = prescritption.PrescriptionCode,
                    FilePath = prescritption.FilePath,
                    FileName = prescritption.FileName,
                    FolderId = prescritption.FolderId,
                    UserId = prescritption.UserId,
                    IsExistingPatient = prescritption.IsExistingPatient,
                    PatientId = prescritption.PatientId,
                    HasExistingRelative = prescritption.HasExistingRelative,
                    RelativePatientIds = prescritption.RelativePatientIds,
                    IsSmartRxRequested = prescritption.IsSmartRxRequested,
                    IsRecommended = prescritption.IsRecommended,
                    RecommendedBy = prescritption.RecommendedById,
                    RecommendedDate = prescritption.RecommendedDate,
                    IsLocked = prescritption.IsLocked,
                    LockedBy = prescritption.LockedById,
                    LockedDate = prescritption.LockedDate,
                    IsCompleted = prescritption.IsCompleted,
                    CompletedBy = prescritption.CompletedById,
                    CompletedDate = prescritption.CompletedDate,
                    IsApproved = prescritption.IsApproved,
                    ApprovedBy = prescritption.ApprovedById,
                    ApprovedDate = prescritption.ApprovedDate,
                    IsReported = prescritption.IsReported,
                    ReportBy = prescritption.ReportById,
                    ReportDate = prescritption.ReportDate,
                    Tag1 = prescritption.Tag1,
                    Tag2 = prescritption.Tag2,
                    Tag3 = prescritption.Tag3,
                    Tag4 = prescritption.Tag4,
                    Tag5 = prescritption.Tag5,
                    ReportReason = prescritption.ReportReason,
                    ReportDetails = prescritption.ReportDetails,
                };

                patientDto.ApiResponseResult = null;
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