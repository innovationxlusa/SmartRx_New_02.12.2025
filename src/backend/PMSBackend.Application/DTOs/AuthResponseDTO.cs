using PMSBackend.Application.CommonServices;

namespace PMSBackend.Application.DTOs
{
    public class AuthResponseDTO
    {
        public long UserId { get; set; }
        //public string UserName { get; set; }
        //public string UserCode { get; set; }
        //public string FirstName { get; set; }
        //public string LastName { get; set; }
        public string otp { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime? AccessTokenExpiry { get; set; }
        public DateTime? RefreshTokenExpiry { get; set; }
        public string? TokenType { get; set; }
        public bool IsExistAnyFile { get; set; }
        public UserWiseFolderDTO userPrimaryFolder { get; set; }
        public ApiResponseResult? ApiResponseResult { get; set; }
    }
}
