using Microsoft.AspNetCore.Http;

namespace PMSBackend.Application.DTOs
{
    public class FileUploadDto
    {
        //[Required]
        //[MaxFileSize(5 * 1024 * 1024)] // 5 MB
        //[AllowedFileExtensions(new[] { ".jpg", ".jpeg", ".png", ".gif", ".pdf" })]
        public IFormFile File { get; set; }
    }
}
