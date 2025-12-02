using MediatR;
using Microsoft.AspNetCore.Http;
using PMSBackend.Application.CommonServices;
using PMSBackend.Application.DTOs;
using PMSBackend.Domain.Repositories;

namespace PMSBackend.Application.Commands.Folders
{
    public class DeleteFolderCommand : IRequest<UserWiseFolderDTO>
    {
        public long UserId { get; set; }
        public long FolderId { get; set; }
    }

    public class DeleteFolderCommandHandler : IRequestHandler<DeleteFolderCommand, UserWiseFolderDTO>
    {
        // private readonly ILogger<InsertPrescriptionUploadCommandHandler> _logger;
        private readonly IUserWiseFolderRepository _userWiseFolderRepository;
        private readonly IRewardTransactionRepository _rewardTransactionRepository;

        public DeleteFolderCommandHandler(
            IUserWiseFolderRepository userWiseFolderRepository,
            IRewardTransactionRepository rewardTransactionRepository)
        {
            _userWiseFolderRepository = userWiseFolderRepository;
            _rewardTransactionRepository = rewardTransactionRepository;
        }

        public async Task<UserWiseFolderDTO> Handle(DeleteFolderCommand request, CancellationToken cancellationtoken)
        {
            try
            {
                var responseResult = new UserWiseFolderDTO();
                var IsExistsFolder = await _userWiseFolderRepository.IsExistsThisFolder(request.FolderId);
                if (!IsExistsFolder)
                {
                    responseResult.ApiResponseResult = new ApiResponseResult
                    {
                        Data = null,
                        StatusCode = StatusCodes.Status400BadRequest,
                        Status = "Failed",
                        Message = "Folder does not exists!"
                    };
                    return responseResult;
                }
                var IsExistsFolderName = await _userWiseFolderRepository.IsUploadedAnyFileForThisUser(request.UserId, request.FolderId);
                if (IsExistsFolderName)
                {
                    responseResult.ApiResponseResult = new ApiResponseResult
                    {
                        Data = null,
                        StatusCode = StatusCodes.Status400BadRequest,
                        Status = "Failed",
                        Message = "Files found in this folder. Please remove them first!"
                    };
                    return responseResult;
                }
                // Load folder details before delete (for patientId, etc.)
                var folderEntity = await _userWiseFolderRepository.GetDetailsByIdAsync(request.FolderId);

                await _userWiseFolderRepository.DeleteAsync(request.FolderId);
                await Task.CompletedTask;
                responseResult.IsDeleted = true;

                // Reward for deleting folder
                if (folderEntity is not null)
                {
                    var rewardResult = await _rewardTransactionRepository.CreateRewardTransactionForPrescriptionAsync(
                        request.UserId,
                        folderEntity.Id,
                        null,
                        folderEntity.PatientId,
                        "REMOVE_FOLDER",
                        "deleting a folder",
                        cancellationtoken);

                    if (rewardResult is not null && rewardResult.IsRewardUpdated)
                    {
                        responseResult.IsRewardUpdated = true;
                        responseResult.RewardTitle = rewardResult.RewardTitle;
                        responseResult.TotalRewardPoints = rewardResult.Points;
                        responseResult.RewardMessage = rewardResult.RewardMessage;
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
