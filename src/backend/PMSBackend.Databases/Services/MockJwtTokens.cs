using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace PMSBackend.Databases.Services
{
    public static class MockJwtTokens
    {
        public static string Issuer { get; } = Guid.NewGuid().ToString();
        public static SecurityKey SecurityKey { get; }
        public static SigningCredentials SigningCredentials { get; }

        private static readonly JwtSecurityTokenHandler s_tokenHandler = new JwtSecurityTokenHandler();
        private static readonly RandomNumberGenerator s_rng = RandomNumberGenerator.Create();
        private static readonly byte[] s_key = new byte[32];

        static MockJwtTokens()
        {
            s_rng.GetBytes(s_key);
            SecurityKey = new SymmetricSecurityKey(s_key) { KeyId = Guid.NewGuid().ToString() };
            SigningCredentials = new SigningCredentials(SecurityKey, SecurityAlgorithms.RsaSha512);
        }

        public static string GenerateJwtTokenStr(IEnumerable<Claim> claims)
        {
            return s_tokenHandler.WriteToken(new JwtSecurityToken(Issuer, null, claims, null, DateTime.UtcNow.AddMinutes(20), SigningCredentials));
        }
        public static JwtSecurityToken GenerateJwtToken(IEnumerable<Claim> claims)
        {
            var token = new JwtSecurityToken(
                issuer: Issuer,
                audience: null,
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(20)),
                signingCredentials: SigningCredentials
           );
            return token;
        }
    }
}
