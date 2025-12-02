using MediatR;
using PMSBackend.Application.DTOs;
using PMSBackend.Domain.CommonDTO;
using PMSBackend.Domain.Repositories;
using PMSBackend.Domain.SharedContract;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PMSBackend.Application.Queries.UserRewardBadge
{
    public class GetAllUserRewardBadgesQuery : IRequest<PaginatedResult<UserRewardBadgeDTO>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? SortBy { get; set; } = "CreatedDate";
        public string? SortDirection { get; set; } = "desc";
    }

    public class GetAllUserRewardBadgesQueryHandler : IRequestHandler<GetAllUserRewardBadgesQuery, PaginatedResult<UserRewardBadgeDTO>>
    {
        private readonly IUserRewardBadgeRepository _userRewardBadgeRepository;

        public GetAllUserRewardBadgesQueryHandler(IUserRewardBadgeRepository userRewardBadgeRepository)
        {
            _userRewardBadgeRepository = userRewardBadgeRepository;
        }

        public async Task<PaginatedResult<UserRewardBadgeDTO>> Handle(GetAllUserRewardBadgesQuery request, CancellationToken cancellationToken)
        {
            var pagingParams = new PagingSortingParams
            {
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                SortBy = request.SortBy ?? "CreatedDate",
                SortDirection = request.SortDirection ?? "desc"
            };

            var result = await _userRewardBadgeRepository.GetAllUserRewardBadgesAsync(pagingParams, cancellationToken);

            var dtos = result.Data.Select(urb => urb.ToDto()).ToList();

            return new PaginatedResult<UserRewardBadgeDTO>(
                dtos,
                result.TotalRecords,
                result.PageNumber,
                result.PageSize,
                result.SortBy,
                result.SortDirection,
                null, null, null, null, null);
        }
    }
}

