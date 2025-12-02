using Microsoft.IdentityModel.Tokens;
using PMSBackend.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace PMSBackend.Databases.Services
{
    public static class TokenHelper
    {
        public static SymmetricSecurityKey GetSigningKey()
        {
            byte[] keyBytes;
            try
            {
                keyBytes = Convert.FromBase64String(JwtConfig.Settings.SecretKey);
            }
            catch
            {
                throw new Exception("JwtSettings.SecretKey must be a valid Base64 string representing at least 256 bits (32 bytes).");
            }

            if (keyBytes.Length < 32)
            {
                throw new Exception("Secret key must be at least 256 bits (32 bytes) long");
            }

            return new SymmetricSecurityKey(keyBytes);
        }

        public static TokenValidationParameters BuildValidationParameters(bool validateLifetime)
        {
            return new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = validateLifetime,
                ValidIssuer = JwtConfig.Settings.Issuer,
                ValidAudience = JwtConfig.Settings.Audience,
                IssuerSigningKey = GetSigningKey(),
                ClockSkew = TimeSpan.Zero
            };
        }

        public static ClaimsPrincipal ValidateToken(string token, out SecurityToken validatedToken, bool validateLifetime = true)
        {
            var handler = new JwtSecurityTokenHandler();
            return handler.ValidateToken(token, BuildValidationParameters(validateLifetime), out validatedToken);
        }

        public static bool TryValidateToken(string token, out ClaimsPrincipal? principal, out SecurityToken? validatedToken, bool validateLifetime = true)
        {
            principal = null;
            validatedToken = null;
            try
            {
                principal = ValidateToken(token, out var vToken, validateLifetime);
                validatedToken = vToken;
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}


