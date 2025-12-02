using MediatR;
using Microsoft.AspNetCore.Http;
using PMSBackend.Application.CommonServices;
using PMSBackend.Domain.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace PMSBackend.Application.Commands.UserRewardBadge
{
    public class DeleteUserRewardBadgeCommand : IRequest<ApiResponseResult>
    {
        public long Id { get; set; }
    }

    public class DeleteUserRewardBadgeCommandHandler : IRequestHandler<DeleteUserRewardBadgeCommand, ApiResponseResult>
    {
        private readonly IUserRewardBadgeRepository _userRewardBadgeRepository;

        public DeleteUserRewardBadgeCommandHandler(IUserRewardBadgeRepository userRewardBadgeRepository)
        {
            _userRewardBadgeRepository = userRewardBadgeRepository;
        }

        public async Task<ApiResponseResult> Handle(DeleteUserRewardBadgeCommand request, CancellationToken cancellationToken)
        {
            if (request.Id <= 0)
            {
                return new ApiResponseResult
                {
                    Data = null,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Status = "Failed",
                    Message = "User reward badge ID must be greater than zero"
                };
            }

            var result = await _userRewardBadgeRepository.DeleteUserRewardBadgeAsync(request.Id, cancellationToken);

            if (!result)
            {
                return new ApiResponseResult
                {
                    Data = null,
                    StatusCode = StatusCodes.Status404NotFound,
                    Status = "Failed",
                    Message = $"User reward badge with ID {request.Id} not found"
                };
            }

            return new ApiResponseResult
            {
                Data = result,
                StatusCode = StatusCodes.Status200OK,
                Status = "Success",
                Message = "User reward badge deleted successfully"
            };
        }
    }
}

