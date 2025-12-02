namespace PMSBackend.Domain.Entities
{
    public class JwtSettings
    {
        public string SecretKey { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public int ExpiryMinutes { get; set; }
        public int RefreshTokenExpiryDays { get; set; } = 7;
    }
    public static class JwtConfig
    {
        public static JwtSettings Settings { get; private set; }

        public static void Initialize(JwtSettings settings)
        {
            Settings = settings;
        }
    }

}
