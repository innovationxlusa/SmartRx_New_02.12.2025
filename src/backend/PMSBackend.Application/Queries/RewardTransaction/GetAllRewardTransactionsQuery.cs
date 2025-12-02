using MediatR;
using PMSBackend.Application.DTOs;
using PMSBackend.Domain.CommonDTO;
using PMSBackend.Domain.Repositories;
using PMSBackend.Domain.SharedContract;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PMSBackend.Application.Queries.RewardTransaction
{
    public class GetAllRewardTransactionsQuery : IRequest<PaginatedResult<RewardTransactionDTO>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? SortBy { get; set; } = "CreatedDate";
        public string? SortDirection { get; set; } = "desc";
    }

    public class GetAllRewardTransactionsQueryHandler : IRequestHandler<GetAllRewardTransactionsQuery, PaginatedResult<RewardTransactionDTO>>
    {
        private readonly IRewardTransactionRepository _rewardTransactionRepository;

        public GetAllRewardTransactionsQueryHandler(IRewardTransactionRepository rewardTransactionRepository)
        {
            _rewardTransactionRepository = rewardTransactionRepository;
        }

        public async Task<PaginatedResult<RewardTransactionDTO>> Handle(GetAllRewardTransactionsQuery request, CancellationToken cancellationToken)
        {
            var pagingParams = new PagingSortingParams
            {
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                SortBy = request.SortBy ?? "CreatedDate",
                SortDirection = request.SortDirection ?? "desc"
            };

            var result = await _rewardTransactionRepository.GetAllRewardTransactionsAsync(pagingParams, cancellationToken);

            var dtos = result.Data.Select(rt => rt.ToDto()).ToList();

            return new PaginatedResult<RewardTransactionDTO>(
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

