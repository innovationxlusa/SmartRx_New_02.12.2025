namespace PMSBackend.Domain.CommonDTO
{
    public class PagingSortingParams
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string SortBy { get; set; } = "name";
        public string SortDirection { get; set; } = "asc";

    }
}
