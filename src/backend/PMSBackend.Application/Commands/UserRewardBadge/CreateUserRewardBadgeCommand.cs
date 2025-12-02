using MediatR;
using Microsoft.AspNetCore.Http;
using PMSBackend.Application.CommonServices;
using PMSBackend.Application.DTOs;
using PMSBackend.Domain.Entities;
using PMSBackend.Domain.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PMSBackend.Application.Commands.UserRewardBadge
{
    public class CreateUserRewardBadgeCommand : IRequest<UserRewardBadgesDTO>
    {
        public int RecipientUserId { get; set; }
        public int BadgeId { get; set; }
        public DateTime? EarnedDate { get; set; }
        public long UserId { get; set; }
    }

    public class CreateUserRewardBadgeCommandHandler : IRequestHandler<CreateUserRewardBadgeCommand, UserRewardBadgesDTO>
    {
        private readonly IUserRewardBadgeRepository _userRewardBadgeRepository;
        private readonly IRewardBadgeRepository _rewardBadgeRepository;

        public CreateUserRewardBadgeCommandHandler(
            IUserRewardBadgeRepository userRewardBadgeRepository,
            IRewardBadgeRepository rewardBadgeRepository)
        {
            _userRewardBadgeRepository = userRewardBadgeRepository;
            _rewardBadgeRepository = rewardBadgeRepository;
        }

        public async Task<UserRewardBadgesDTO> Handle(CreateUserRewardBadgeCommand request, CancellationToken cancellationToken)
        {
            var responseResult = new UserRewardBadgesDTO();

            if (request.RecipientUserId <= 0)
            {
                responseResult.ApiResponseResult = new ApiResponseResult
                {
                    Data = null,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Status = "Failed",
                    Message = "Recipient user ID not found"
                };
                return responseResult;
            }

            if (request.BadgeId <= 0)
            {
                responseResult.ApiResponseResult = new ApiResponseResult
                {
                    Data = null,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Status = "Failed",
                    Message = "Badge ID must be not found"
                };
                return responseResult;
            }

            var badge = await _rewardBadgeRepository.GetRewardBadgeByIdAsync(request.BadgeId, cancellationToken);
            if (badge == null)
            {
                responseResult.ApiResponseResult = new ApiResponseResult
                {
                    Data = null,
                    StatusCode = StatusCodes.Status404NotFound,
                    Status = "Failed",
                    Message = $"Badge with ID {badge.Name} not found"
                };
                return responseResult;
            }

            if (!badge.IsActive.GetValueOrDefault())
            {
                responseResult.ApiResponseResult = new ApiResponseResult
                {
                    Data = null,
                    StatusCode = StatusCodes.Status409Conflict,
                    Status = "Failed",
                    Message = "Badge is inactive and cannot be assigned"
                };
                return responseResult;
            }

            if (await _userRewardBadgeRepository.IsBadgeAlreadyAssignedAsync(
                request.RecipientUserId,
                request.BadgeId,
                null,
                cancellationToken))
            {
                responseResult.ApiResponseResult = new ApiResponseResult
                {
                    Data = null,
                    StatusCode = StatusCodes.Status409Conflict,
                    Status = "Failed",
                    Message = "This badge has already been assigned to the user"
                };
                return responseResult;
            }

            var userRewardBadge = new SmartRx_UserRewardBadgeEntity
            {
                UserId = request.RecipientUserId,
                BadgeId = request.BadgeId,
                
                EarnedDate = request.EarnedDate ?? DateTime.UtcNow,
                CreatedById = request.UserId,
                CreatedDate = DateTime.UtcNow
            };

            var createdBadge = await _userRewardBadgeRepository.CreateUserRewardBadgeAsync(userRewardBadge, cancellationToken);

            var dto = createdBadge.ToDto();
            responseResult.UserRewardBadge = dto;
            responseResult.ApiResponseResult = new ApiResponseResult
            {
                Data = dto,
                StatusCode = StatusCodes.Status200OK,
                Status = "Success",
                Message = "User reward badge created successfully"
            };

            return responseResult;
        }
    }
}

