using MediatR;
using Microsoft.Extensions.Logging;
using PMSBackend.Application.DTOs;
using PMSBackend.Domain.Entities;
using PMSBackend.Domain.Repositories;
using System.ComponentModel.DataAnnotations.Schema;



namespace PMSBackend.Application.Commands.PrescriptionUpload
{
    public class InsertPrescriptionUploadCommand : IRequest<PrescriptionUploadDTO>
    {
        //[AllowedFileExtensions(new[] { ".jpg", ".jpeg", ".png", ".gif", ".pdf" })]

        //[Required]
        //[MinLength(1, ErrorMessage = "At least one file is required.")]
        // public IList<FileUploadDto> Files { get; set; }
        public string? FileName { get; set; } = string.Empty;
        public string? FilePath { get; set; }
        public long? FileId { get; set; }
        public long FolderId { get; set; }
        public long UserId { get; set; }
        public string? FolderName { get; set; } = default!;
        public string? SeqNo { get; set; }
        public int? FileCount { get; set; } = 0;
        public long LoginUserId { get; set; }
        public string FileExtension { get; set; }
        public bool IsCaputred { get; set; }
        public bool IsScanned { get; set; }
        public bool IsUploaded { get; set; }

    }
    public class InsertPrescriptionUploadCommandHandler : IRequestHandler<InsertPrescriptionUploadCommand, PrescriptionUploadDTO>
    {

        private readonly IPrescriptionUploadRepository _repository;
        private readonly ILogger<InsertPrescriptionUploadCommandHandler> _logger;
        private readonly IUserWiseFolderRepository _userWiseFolderRepository;
        private readonly IRewardTransactionRepository _rewardTransactionRepository;
        
        public InsertPrescriptionUploadCommandHandler(
            IPrescriptionUploadRepository repository,
            ILogger<InsertPrescriptionUploadCommandHandler> logger,
            IUserWiseFolderRepository userWiseFolderRepository,
            IRewardTransactionRepository rewardTransactionRepository)
        {
            _repository = repository;
            _logger = logger;
            _userWiseFolderRepository = userWiseFolderRepository;
            _rewardTransactionRepository = rewardTransactionRepository;
        }

        public async Task<PrescriptionUploadDTO> Handle(InsertPrescriptionUploadCommand request, CancellationToken cancellationtoken)
        {
            try
            {
                var folderInfo = await _userWiseFolderRepository.GetDetailsByUserIdAsync(request.LoginUserId,request.FolderId);

                Prescription_UploadEntity entity = new Prescription_UploadEntity()
                {
                    PrescriptionCode = request.SeqNo!,
                    FilePath = request.FilePath!,
                    FileName = request.FileName!,
                    FolderId = (request.FolderId == 0) ? folderInfo!.Id : request.FolderId,
                    UserId = request.LoginUserId,
                    NumberOfFilesStoredForThisPrescription = request.FileCount ?? 0,
                    FileExtension = request.FileExtension,
                    CreatedDate = DateTime.Now,
                    CreatedById = request.LoginUserId
                };
                var result = await _repository.AddAsync(entity);

                PrescriptionUploadDTO patientDto = new PrescriptionUploadDTO()
                {
                    Id = result!.Id,
                    PrescriptionCode = entity.PrescriptionCode,
                    FilePath = entity.FilePath,
                    FileName = entity.FileName,
                    FolderId = entity.FolderId,
                    UserId = request.LoginUserId,
                    IsExistingPatient = false,
                    PatientId = entity.PatientId,
                    HasExistingRelative = false,
                    RelativePatientIds = "",
                    IsSmartRxRequested = false
                };

                if (result!=null)
                {
                    string captured_uploaded_scanned = string.Empty;
                    if (request.IsUploaded) captured_uploaded_scanned = "PRESCRIPTION_UPLOAD";
                    if (request.IsCaputred) captured_uploaded_scanned = "PRESCRIPTION_CAPTURE";
                    if (request.IsScanned) captured_uploaded_scanned = "PRESCRIPTION_SCAN";

                    var rewardResult = await _rewardTransactionRepository.CreateRewardTransactionForPrescriptionAsync(
                        request.LoginUserId,
                        result.Id,
                        result.SmartRxId,
                        result.PatientId,
                        captured_uploaded_scanned,
                        "uploading a prescription",
                        cancellationtoken);

                    if (rewardResult is not null && rewardResult.IsRewardUpdated)
                    {
                        patientDto.IsRewardUpdated = true;
                        patientDto.RewardTitle = rewardResult.RewardTitle;
                        patientDto.TotalRewardPoints = rewardResult.Points;
                        patientDto.RewardMessage = rewardResult.RewardMessage;
                    }
                }
                //var sequenceResult = await _repository.GenerateFileSequenceAsync(request.UniqueFileId);
                _logger.LogInformation($"New prescription uploaded. file name: {request.FileName}");

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
