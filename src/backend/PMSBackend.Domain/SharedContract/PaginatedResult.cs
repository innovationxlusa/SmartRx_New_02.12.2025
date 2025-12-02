namespace PMSBackend.Domain.SharedContract
{
    public class PaginatedResult<T>
    {
        public int TotalRecords { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public List<T> Data { get; set; } = new();
        public string? message { get; set; }
        public string SortBy { get; set; } = "name";
        public string SortDirection { get; set; } = "asc";

        public bool? IsRewardUpdated { get; set; }
        public string? RewardTitle { get; set; }
        public double? TotalRewardPoints { get; set; }
        public string? RewardMessage { get; set; }
        public PaginatedResult() { }

        public PaginatedResult(List<T>? data, int totalRecords, int pageNumber, int pageSize, string? sortBy, string? sortDirection, string? message, bool? isRewardedUpdated, string? rewardTitle, double? totalRewardPoints, string? rewardMessage)
        {
            Data = data;
            TotalRecords = totalRecords;
            PageNumber = pageNumber;
            PageSize = pageSize;
            SortBy = sortBy;
            SortDirection = sortDirection;
            this.message = message;
            IsRewardUpdated = isRewardedUpdated;
            RewardTitle = rewardTitle;
            TotalRewardPoints = totalRewardPoints;
            RewardMessage = rewardMessage;
        }

        public int TotalPages => (int)Math.Ceiling(TotalRecords / (double)PageSize);
        public bool HasPreviousPage => PageNumber > 1;
        public bool HasNextPage => PageNumber < TotalPages;
    }

}
