
namespace PMSBackend.Application.CommonServices
{
    public class ApiResponseResult
    {
        public object? Data { get; set; } = default!;
        public int StatusCode { get; set; }
        public string Status { get; set; } = default!;
        public string Message { get; set; } = default!;
        public string? StackTrace { get; set; }
        public int? HRResult { get; set; }
        public Dictionary<string, string[]>? Errors { get; set; }
    }
}
