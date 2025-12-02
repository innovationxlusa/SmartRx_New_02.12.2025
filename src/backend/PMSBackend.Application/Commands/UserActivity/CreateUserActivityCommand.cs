using MediatR;
using Microsoft.AspNetCore.Http;
using PMSBackend.Application.CommonServices;
using PMSBackend.Application.DTOs;
using PMSBackend.Domain.Entities;
using PMSBackend.Domain.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PMSBackend.Application.Commands.UserActivity
{
    public class CreateUserActivityCommand : IRequest<UserActivitiesDTO>
    {
        public string ActivityName { get; set; } = string.Empty;
        public double ActivityPoint { get; set; }
        public string? Remarks { get; set; }
        public long UserId { get; set; }
    }

    public class CreateUserActivityCommandHandler : IRequestHandler<CreateUserActivityCommand, UserActivitiesDTO>
    {
        private readonly IUserActivityRepository _userActivityRepository;
        private readonly ICodeGenerationService _codeGenerationService;

        public CreateUserActivityCommandHandler(
            IUserActivityRepository userActivityRepository,
            ICodeGenerationService codeGenerationService)
        {
            _userActivityRepository = userActivityRepository;
            _codeGenerationService = codeGenerationService;
        }

        public async Task<UserActivitiesDTO> Handle(CreateUserActivityCommand request, CancellationToken cancellationToken)
        {
            var responseResult = new UserActivitiesDTO();

            // Validation: Check for null or empty ActivityName
            if (string.IsNullOrWhiteSpace(request.ActivityName))
            {
                responseResult.ApiResponseResult = new ApiResponseResult
                {
                    Data = null,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Status = "Failed",
                    Message = "Activity name is required"
                };
                return responseResult;
            }

            // Validation: Check ActivityPoint is positive
            if (request.ActivityPoint < 0)
            {
                responseResult.ApiResponseResult = new ApiResponseResult
                {
                    Data = null,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Status = "Failed",
                    Message = "Activity point must be greater than or equal to zero"
                };
                return responseResult;
            }

            // Validation: Check for duplicate ActivityName
            var nameExists = await _userActivityRepository.IsActivityNameExistsAsync(request.ActivityName, null, cancellationToken);
            if (nameExists)
            {
                responseResult.ApiResponseResult = new ApiResponseResult
                {
                    Data = null,
                    StatusCode = StatusCodes.Status409Conflict,
                    Status = "Failed",
                    Message = $"A user activity with the name '{request.ActivityName}' already exists"
                };
                return responseResult;
            }

            // Generate ActivityCode
            var activityCode = await _codeGenerationService.GenerateUserActivityCodeAsync(cancellationToken);

            var userActivity = new Configuration_UserActivityEntity
            {
                ActivityCode = activityCode,
                ActivityName = request.ActivityName,
                ActivityPoint = request.ActivityPoint,
                Remarks = request.Remarks,
                CreatedById = request.UserId,
                CreatedDate = DateTime.UtcNow
            };

            var newUserActivity = await _userActivityRepository.CreateUserActivityAsync(userActivity, cancellationToken);

            var dto = new UserActivityDTO
            {
                Id = newUserActivity.Id,
                ActivityCode = newUserActivity.ActivityCode,
                ActivityName = newUserActivity.ActivityName,
                ActivityPoint = newUserActivity.ActivityPoint,
                Remarks = newUserActivity.Remarks,
                CreatedDate = newUserActivity.CreatedDate,
                CreatedById = newUserActivity.CreatedById
            };

            responseResult.ApiResponseResult = new ApiResponseResult
            {
                Data = dto,
                StatusCode = StatusCodes.Status200OK,
                Status = "Success",
                Message = "User activity created successfully"
            };

            return responseResult;
        }
    }
}

