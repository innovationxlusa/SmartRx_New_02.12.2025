using MediatR;
using Microsoft.AspNetCore.Http;
using PMSBackend.Application.CommonServices;
using PMSBackend.Application.CommonServices.Validation;
using PMSBackend.Application.DTOs;
using PMSBackend.Domain.Entities;
using PMSBackend.Domain.Repositories;
using System.ComponentModel.DataAnnotations;


namespace PMSBackend.Application.Commands.User
{
    public class CreateUserCommand : IRequest<UserDetailsResponseDTO>
    {
        public string? UserCode { get; set; }
        [Required(ErrorMessage = "User name is required.")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Password is required.")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*[\W_]).{8,}$",
        ErrorMessage = "Password must be at least 8 characters long and contain at least one uppercase letter, one lowercase letter, and one special character.")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Mobile No is required.")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Mobile number must contain digits only.")]
        [MobileNumberLength]
        //[InternationalMobileValidator]
        public string MobileNo { get; set; } = string.Empty!;
        public string GoogleId { get; set; } = string.Empty!;
        public string FacebookId { get; set; } = string.Empty!;
        public string TwitterId { get; set; } = string.Empty!;
        [OptionalEmailAddress(ErrorMessage = "Email must be a valid email address.")]
        public string Email { get; set; } = string.Empty!;
        [Required(ErrorMessage = "First Name is required.")]
        public string FirstName { get; set; } = string.Empty!;
        [Required(ErrorMessage = "Last Name is required.")]
        public string LastName { get; set; } = string.Empty!;
        [Required(ErrorMessage = "Authentication Method is required.")]
        public int AuthMethod { get; set; } = 2;
        [Required(ErrorMessage = "Gender is required.")]
        public int Gender { get; set; }
        [Required(ErrorMessage = "Date of birth is required.")]
        [DataType(DataType.Date)]
        [PastDate(ErrorMessage = "Date of Birth must be a past date.")]
        [MinimumAge(18, ErrorMessage = "User must be at least 18 years old.")]
        public DateTime DateOfBirth { get; set; }
        [Required(ErrorMessage = "Status is required.")]
        public int Status { get; set; }

    }

    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserDetailsResponseDTO>
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IUserWiseFolderRepository _userWiseFolderRepository;
        private readonly IRewardTransactionRepository _rewardTransactionRepository;

        public CreateUserCommandHandler(
            IUserRepository userRepository,
            IRoleRepository roleRepository,
            IUserRoleRepository userRoleRepository,
            IUserWiseFolderRepository userWiseFolderRepository,
            IRewardTransactionRepository rewardTransactionRepository)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _userRoleRepository = userRoleRepository;
            _userWiseFolderRepository = userWiseFolderRepository;
            _rewardTransactionRepository = rewardTransactionRepository;
        }
        public async Task<UserDetailsResponseDTO> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var responseResult = new UserDetailsResponseDTO();
                //var user = await _userRepository.GetUserDetailsByUserNameAsync(request.UserName.Trim());
                var isUserUnique = await _userRepository.IsUniqueUserName(request.UserName);
                if (!isUserUnique)
                {
                    responseResult.ApiResponseResult = new ApiResponseResult
                    {
                        Data = null,
                        StatusCode = StatusCodes.Status400BadRequest,
                        Status = "Failed",
                        Message = "Duplicate User Found!"
                    };
                    return responseResult;
                }

                //int authType = request.AuthMethod ==(int)LoginType.Mobile ? (int)LoginType.Mobile : request.AuthMethod == (int)LoginType.Google ? (int)LoginType.Google : request.AuthMethod == (int)LoginType.Twitter ? (int)LoginType.Twitter : request.AuthMethod == (int)LoginType.Facebook ? (int)LoginType.Facebook : request.AuthMethod == (int)LoginType.Email ? (int)LoginType.Email : 0;       
                request.AuthMethod = (int)LoginType.Mobile;
                string newUserCode = await _userRepository.GetNextProductCodeAsync();
                var entity = new SmartRxUserEntity()
                {
                    UserCode = newUserCode,
                    UserName = request.UserName,
                    Password = request.Password,
                    MobileNo = request.MobileNo,
                    AuthMethod = request.AuthMethod,
                    Email = request.Email,
                    FacebookId = request.FacebookId,
                    GoogleId = request.GoogleId,
                    TwitterId = request.TwitterId,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Gender = request.Gender,
                    DateOfBirth = request.DateOfBirth,
                    EmployeeId = 0,
                    EmployeeCode = string.Empty,
                    CreatedById = 6,
                    CreatedDate = DateTime.Now,
                    Status = (int)Status.Active,
                };
                var result = await _userRepository.AddAsync(entity);
                if (result != null)
                {
                    Prescription_UserWiseFolderEntity userWiseFolderEntity = new Prescription_UserWiseFolderEntity();
                    userWiseFolderEntity = await _userWiseFolderRepository.IsExistAnyParentFolderForThisUserAsync(result.Id);
                    if (userWiseFolderEntity is null)
                    {
                        var folderModel = new Prescription_UserWiseFolderEntity();
                        folderModel.FolderName = "Primary";
                        folderModel.Description = "Default Folder";
                        folderModel.FolderHierarchy = 0;
                        folderModel.UserId = result.Id;
                        folderModel.ParentFolderId = null;
                        folderModel.PatientId = null;
                        folderModel.CreatedById = result.Id;
                        folderModel.CreatedDate = DateTime.Now;
                        userWiseFolderEntity = await _userWiseFolderRepository.AddAsync(folderModel);
                    }

                    responseResult = new UserDetailsResponseDTO()
                    {
                        Id = result.Id,
                        UserCode = result.UserCode,
                        UserName = result.UserName,
                        Password = result.Password!,
                        MobileNo = result.MobileNo,
                        AuthMethod = result.AuthMethod,
                        Email = result.Email,
                        FacebookId = result.FacebookId,
                        GoogleId = result.GoogleId,
                        TwitterId = result.TwitterId,
                        FirstName = result.FirstName,
                        LastName = result.LastName,
                        Gender = result.Gender,
                        DateOfBirth = result.DateOfBirth,
                        EmployeeId = result.EmployeeId,
                        EmployeeCode = result.EmployeeCode,
                        Status = result.Status,
                        UserFolderDetails = new UserWiseFolderDTO()
                        {
                            Id = userWiseFolderEntity.Id,
                            ParentFolderId = userWiseFolderEntity.ParentFolderId,
                            FolderName = userWiseFolderEntity.FolderName,
                            Description = userWiseFolderEntity.Description,
                            FolderHierarchy = userWiseFolderEntity.FolderHierarchy,
                            PatientId = userWiseFolderEntity.PatientId,
                            UserId = userWiseFolderEntity.UserId
                        },
                        ApiResponseResult = null
                    };
                    var roleDescription = Common.GetEnumDescription(Roles.externaluser);
                    var role = await _roleRepository.GetDetailsByRoleNameAsync(roleDescription);
                    var roles = new List<long>();
                    roles.Add(role!.Id);
                    var roleResult = await _userRoleRepository.AssignUserToRoleByUserId(entity.Id, roles);

                    // Save reward transaction for creating user
                    var rewardResult = await _rewardTransactionRepository.CreateRewardTransactionForPrescriptionAsync(
                        result.Id,              // new user as reward owner
                        0,                      // no prescription
                        null,                   // no SmartRx master
                        null,                   // no patient
                        "USER_REGISTRATION",
                        "creating a user",
                        cancellationToken);

                    if (rewardResult is not null && rewardResult.IsRewardUpdated)
                    {
                        responseResult.IsRewardUpdated = true;
                        responseResult.RewardTitle = rewardResult.RewardTitle;
                        responseResult.TotalRewardPoints = rewardResult.Points;
                        responseResult.RewardMessage = rewardResult.RewardMessage;
                    }
                }

                return responseResult!;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
