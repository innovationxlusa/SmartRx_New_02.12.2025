using MediatR;
using Microsoft.AspNetCore.Http;
using PMSBackend.Application.CommonServices;
using PMSBackend.Application.DTOs;
using PMSBackend.Domain.Entities;
using PMSBackend.Domain.Repositories;

namespace PMSBackend.Application.Commands.Folders
{
    public class CreateFolderCommand : IRequest<UserWiseFolderDTO>
    {
        public long ParentFolderId { get; set; }
        public long? PreviousFolderHierarchy { get; set; }
        public long FolderHierarchy { get; set; }
        public string FolderName { get; set; }
        public string Description { get; set; }
        public long? PatientId { get; set; }
        public long LoginUserId { get; set; }
    }
    public class FolderCreateCommandHandler : IRequestHandler<CreateFolderCommand, UserWiseFolderDTO>
    {
        private readonly IUserWiseFolderRepository _userWiseFolderRepository;
        private readonly IRewardTransactionRepository _rewardTransactionRepository;

        
        public FolderCreateCommandHandler(IUserWiseFolderRepository userWiseFolderRepository, IRewardTransactionRepository rewardTransactionRepository)
        {
            _userWiseFolderRepository = userWiseFolderRepository;
            _rewardTransactionRepository = rewardTransactionRepository;
        }

        public async Task<UserWiseFolderDTO> Handle(CreateFolderCommand request, CancellationToken cancellationtoken)
        {
            try
            {
                var responseResult = new UserWiseFolderDTO();
                var IsExistsParentFolder = await _userWiseFolderRepository.IsExistAnyParentFolderForThisUserAsync(request.LoginUserId);
                if (IsExistsParentFolder is null)
                {
                    responseResult.ApiResponseResult = new ApiResponseResult
                    {
                        Data = null,
                        StatusCode = StatusCodes.Status417ExpectationFailed,
                        Status = "Failed",
                        Message = "User information not created properly. Please contact with the system administrator."
                    };
                    return responseResult;
                }
                var IsExistsFolderName = await _userWiseFolderRepository.IsExistsFolderName(request.FolderName, request.LoginUserId, request.ParentFolderId);
                if (IsExistsFolderName)
                {
                    responseResult.ApiResponseResult = new ApiResponseResult
                    {
                        Data = null,
                        StatusCode = StatusCodes.Status400BadRequest,
                        Status = "Failed",
                        Message = "Folder name already exists!"
                    };
                    return responseResult;
                }

                Prescription_UserWiseFolderEntity entity = new()
                {
                    FolderName = request.FolderName,
                    Description = request.Description,
                    PatientId = request.PatientId,
                    FolderHierarchy = request.FolderHierarchy,
                    ParentFolderId = request.ParentFolderId,
                    UserId = request.LoginUserId,
                    CreatedDate = DateTime.Now,
                    CreatedById = request.LoginUserId
                };
                var result = await _userWiseFolderRepository.AddAsync(entity);
                
                UserWiseFolderDTO patientDto = new()
                {
                    Id = result.Id,
                    FolderName = request.FolderName,
                    Description = request.Description,
                    PatientId = request.PatientId,
                    FolderHierarchy = request.FolderHierarchy,
                    UserId = result.UserId,
                    ParentFolderId = request.ParentFolderId,
                    ApiResponseResult = null
                };

                if (result != null)
                {
                    var rewardResult = await _rewardTransactionRepository.CreateRewardTransactionForPrescriptionAsync(
                        request.LoginUserId,
                        result.Id,
                        null,
                        result.PatientId,
                        "ADD_FOLDER",
                        "adding a folder",
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