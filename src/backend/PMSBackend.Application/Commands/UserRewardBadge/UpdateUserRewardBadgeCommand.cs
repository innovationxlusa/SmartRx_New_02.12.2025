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
    public class UpdateUserRewardBadgeCommand : IRequest<UserRewardBadgesDTO>
    {
        public long Id { get; set; }
        public int RecipientUserId { get; set; }
        public int BadgeId { get; set; }
        public DateTime? EarnedDate { get; set; }
        public long UserId { get; set; }
    }

    public class UpdateUserRewardBadgeCommandHandler : IRequestHandler<UpdateUserRewardBadgeCommand, UserRewardBadgesDTO>
    {
        private readonly IUserRewardBadgeRepository _userRewardBadgeRepository;
        private readonly IRewardBadgeRepository _rewardBadgeRepository;

        public UpdateUserRewardBadgeCommandHandler(
            IUserRewardBadgeRepository userRewardBadgeRepository,
            IRewardBadgeRepository rewardBadgeRepository)
        {
            _userRewardBadgeRepository = userRewardBadgeRepository;
            _rewardBadgeRepository = rewardBadgeRepository;
        }

        public async Task<UserRewardBadgesDTO> Handle(UpdateUserRewardBadgeCommand request, CancellationToken cancellationToken)
        {
            var responseResult = new UserRewardBadgesDTO();

            if (request.Id <= 0)
            {
                responseResult.ApiResponseResult = new ApiResponseResult
                {
                    Data = null,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Status = "Failed",
                    Message = "User reward badge ID must be greater than zero"
                };
                return responseResult;
            }

            if (request.RecipientUserId <= 0)
            {
                responseResult.ApiResponseResult = new ApiResponseResult
                {
                    Data = null,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Status = "Failed",
                    Message = "Recipient user ID must be greater than zero"
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
                    Message = "Badge ID must be greater than zero"
                };
                return responseResult;
            }

            var existingBadge = await _userRewardBadgeRepository.GetUserRewardBadgeByIdAsync(request.Id, cancellationToken);
            if (existingBadge == null)
            {
                responseResult.ApiResponseResult = new ApiResponseResult
                {
                    Data = null,
                    StatusCode = StatusCodes.Status404NotFound,
                    Status = "Failed",
                    Message = $"User reward badge with ID {request.Id} not found"
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
                    Message = $"Badge with ID {request.BadgeId} not found"
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
                request.Id,
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

            var badgeToUpdate = new SmartRx_UserRewardBadgeEntity
            {
                Id = request.Id,
                UserId = request.RecipientUserId,
                BadgeId = request.BadgeId,
                EarnedDate = request.EarnedDate ?? existingBadge.EarnedDate,
                ModifiedById = request.UserId
            };

            var updatedBadge = await _userRewardBadgeRepository.UpdateUserRewardBadgeAsync(badgeToUpdate, cancellationToken);

            var dto = updatedBadge.ToDto();
            responseResult.UserRewardBadge = dto;
            responseResult.ApiResponseResult = new ApiResponseResult
            {
                Data = dto,
                StatusCode = StatusCodes.Status200OK,
                Status = "Success",
                Message = "User reward badge updated successfully"
            };

            return responseResult;
        }
    }
}

