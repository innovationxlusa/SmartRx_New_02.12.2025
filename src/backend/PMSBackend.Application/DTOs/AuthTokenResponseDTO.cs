namespace PMSBackend.Application.DTOs
{
    public class AuthTokenResponseDTO
    {
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime AccessTokenExpiry { get; set; }
        public DateTime RefreshTokenExpiry { get; set; }
        public string TokenType { get; set; } = "Bearer";
    }
}

