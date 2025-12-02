using Microsoft.IdentityModel.Tokens;
using PMSBackend.Application.CommonServices.Interfaces;
using PMSBackend.Domain.Entities;
using PMSBackend.Domain.Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PMSBackend.Infrastructure.Services
{
    public class TokenGenerator// : ITokenGenerator
    {

        private readonly string _key;
        private readonly string _issuer;
        private readonly string _audience;
        private readonly string _expiryMinutes;
        private readonly IUserRepository _userRepository;

        public TokenGenerator(string key, string issueer, string audience, string expiryMinutes,IUserRepository userRepository)
        {
            _key = key;
            _issuer = issueer;
            _audience = audience;
            _expiryMinutes = expiryMinutes;
            _userRepository = userRepository;
        }      

        //public async Task<string> GenerateJWTToken((string userId, string userCode, string userName, IList<long> roles) userDetails)
        //{
        //    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key));
        //    var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

           
        //    long userIdint = Convert.ToInt64(userDetails.userId); 
        //    var user =await _userRepository.GetDetailsByIdAsync(userIdint);
        //    var (userId, userCode, userName, roles) = userDetails;

        //    var claims = new List<Claim>()
        //    {
        //        new Claim(JwtRegisteredClaimNames.Sub, "AUTHENTICATION"),
        //        new Claim(JwtRegisteredClaimNames.Jti, userId),
        //        new Claim(ClaimTypes.Name, userName),
        //        new Claim("UserId", userId),
        //        new Claim("FirstName", user!.FirstName),
        //        new Claim("LastName", user.LastName)
        //    };
        //    claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role.ToString())));


        //    var token = new JwtSecurityToken(
        //        issuer: _issuer,
        //        audience: _audience,
        //        claims: claims,
        //        expires: DateTime.Now.AddMinutes(Convert.ToDouble(_expiryMinutes)),
        //        signingCredentials: signingCredentials
        //   );

        //    var encodedToken = new JwtSecurityTokenHandler().WriteToken(token);
        //    return encodedToken;
        //}
    }
}
