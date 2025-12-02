using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using PMSBackend.Application.CommonServices.Interfaces;
using PMSBackend.Application.DTOs;
using PMSBackend.Domain.Entities;
using PMSBackend.Domain.Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Sockets;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace PMSBackend.Databases.Services
{
    public class TokenGenerator : ITokenGenerator
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserWiseFolderRepository _userWiseFolderRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly ILogger<TokenGenerator> _logger;

        public TokenGenerator(
            IUserRepository userRepository, 
            IUserWiseFolderRepository userWiseFolderRepository,
            IRefreshTokenRepository refreshTokenRepository,
            ILogger<TokenGenerator> logger)
        {
            _userRepository = userRepository;
            _userWiseFolderRepository = userWiseFolderRepository;
            _refreshTokenRepository = refreshTokenRepository;
            _logger = logger;
        }

        /// <summary>
        /// Generates a JWT access token only (Legacy method - Use GenerateTokensAsync instead for access + refresh tokens)
        /// </summary>
        [Obsolete("Use GenerateTokensAsync instead to get both access and refresh tokens")]
        public async Task<string> GenerateJWTToken(long userId)
        {
            try
            {
                _logger.LogInformation("Generating JWT token for UserId: {UserId}", userId);
                
                byte[] keyBytes;
               
                try
                {
                    if (JwtConfig.Settings.SecretKey.Contains('-') || JwtConfig.Settings.SecretKey.Contains('_'))
                    {
                        // Base64URL format
                        keyBytes = Base64UrlEncoder.DecodeBytes(JwtConfig.Settings.SecretKey);
                    }
                    else
                    {
                        // Standard Base64 format
                        keyBytes = Convert.FromBase64String(JwtConfig.Settings.SecretKey);
                    }
                   
                }
                catch
                {
                    throw new Exception("JwtSettings.SecretKey must be a valid Base64 string representing at least 256 bits (32 bytes).");
                }
                if (keyBytes.Length < 32)
                {
                    throw new Exception("Secret key must be at least 256 bits (32 bytes) long");
                }

                var securityKey = new SymmetricSecurityKey(keyBytes);
                var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
                var user = await _userRepository.GetDetailsByIdAsync(userId);
                
                if (user == null)
                {
                    _logger.LogWarning("User not found for UserId: {UserId}", userId);
                    throw new InvalidOperationException($"User with ID {userId} not found");
                }
                var jwtId = Guid.NewGuid().ToString();
                var primaryFolderInfo = await _userWiseFolderRepository.GetPrimaryDetailsByIdAsync(user.Id);
                
                var claims = new List<Claim>()
                {
                    new Claim(JwtRegisteredClaimNames.Sub, "AUTHENTICATION"),
                    new Claim(JwtRegisteredClaimNames.Jti, jwtId),
                    new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim("UserId", userId.ToString()),
                    new Claim("UserCode", user.UserCode),
                    new Claim("FirstName", user.FirstName),
                    new Claim("LastName", user.LastName),
                    new Claim("PrimaryFolderId", primaryFolderInfo.Id.ToString()),
                    new Claim("PrimaryFolderHeirarchy", primaryFolderInfo.FolderHierarchy.ToString()),
                    new Claim("PrimaryFolderParentFolderId", primaryFolderInfo.ParentFolderId.ToString()),
                };
                claims.AddRange(user!.UserRoles!.Select(role => new Claim(ClaimTypes.Role, role!.ToString())));

                var token = new JwtSecurityToken(
                    issuer: JwtConfig.Settings.Issuer,
                    audience: JwtConfig.Settings.Audience,
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(JwtConfig.Settings.ExpiryMinutes)),
                    signingCredentials: signingCredentials
               );

                var encodedToken = new JwtSecurityTokenHandler().WriteToken(token);
                _logger.LogInformation("JWT token generated successfully for UserId: {UserId}", userId);
                return encodedToken;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating JWT token for UserId: {UserId}", userId);
                throw;
            }
        }

        /// <summary>
        /// Generates both access token and refresh token pair (Recommended method)
        /// </summary>
        public async Task<AuthTokenResponseDTO> GenerateTokensAsync(long userId, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Generating access and refresh tokens for UserId: {UserId}", userId);
                
                byte[] keyBytes;
                try
                {
                    if (JwtConfig.Settings.SecretKey.Contains('-') || JwtConfig.Settings.SecretKey.Contains('_'))
                    {
                        // Base64URL format
                        keyBytes = Base64UrlEncoder.DecodeBytes(JwtConfig.Settings.SecretKey);
                    }
                    else
                    {
                        // Standard Base64 format
                        keyBytes = Convert.FromBase64String(JwtConfig.Settings.SecretKey);
                    }
                }
                catch
                {
                    throw new Exception("JwtSettings.SecretKey must be a valid Base64 string representing at least 256 bits (32 bytes).");
                }
                if (keyBytes.Length < 32)
                {
                    throw new Exception("Secret key must be at least 256 bits (32 bytes) long");
                }
                var securityKey = new SymmetricSecurityKey(keyBytes);
                var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
                var user = await _userRepository.GetDetailsByIdAsync(userId);
                
                if (user == null)
                {
                    _logger.LogWarning("User not found for UserId: {UserId}", userId);
                    throw new InvalidOperationException($"User with ID {userId} not found");
                }
                
                var primaryFolderInfo = await _userWiseFolderRepository.GetPrimaryDetailsByIdAsync(user.Id);

                var jwtId = Guid.NewGuid().ToString();
                var accessTokenExpiry = DateTime.UtcNow.AddMinutes(Convert.ToDouble(JwtConfig.Settings.ExpiryMinutes));

                var claims = new List<Claim>()
                {
                    new Claim(JwtRegisteredClaimNames.Sub, "AUTHENTICATION"),
                    new Claim(JwtRegisteredClaimNames.Jti, jwtId),
                    new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim("UserId", userId.ToString()),
                    new Claim("UserCode", user.UserCode),
                    new Claim("FirstName", user.FirstName),
                    new Claim("LastName", user.LastName),
                    new Claim("PrimaryFolderId", primaryFolderInfo.Id.ToString()),
                    new Claim("PrimaryFolderHeirarchy", primaryFolderInfo.FolderHierarchy.ToString()),
                    new Claim("PrimaryFolderParentFolderId", primaryFolderInfo.ParentFolderId.ToString()),
                };
                claims.AddRange(user.UserRoles.Select(role => new Claim(ClaimTypes.Role, role.ToString())));

                var token = new JwtSecurityToken(
                    issuer: JwtConfig.Settings.Issuer,
                    audience: JwtConfig.Settings.Audience,
                    claims: claims,
                    notBefore: DateTime.UtcNow,
                    expires: accessTokenExpiry,
                    signingCredentials: signingCredentials
               );

                var accessToken = new JwtSecurityTokenHandler().WriteToken(token);

                // Generate refresh token
                var refreshToken = GenerateRefreshToken();
                var refreshTokenExpiry = DateTime.UtcNow.AddDays(JwtConfig.Settings.RefreshTokenExpiryDays);

                // Store refresh token in database
                var refreshTokenEntity = new RefreshTokenEntity
                {                
                    Token = refreshToken,
                    JwtId = jwtId,
                    UserId = userId,
                    IsUsed = false,
                    IsRevoked = false,
                    CreatedDate = DateTime.UtcNow,
                    ExpiryDate = refreshTokenExpiry
                };           

                await _refreshTokenRepository.AddAsync(refreshTokenEntity, cancellationToken);
                _logger.LogInformation("Tokens generated and stored successfully for UserId: {UserId}", userId);

                return new AuthTokenResponseDTO
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,
                    AccessTokenExpiry = accessTokenExpiry,
                    RefreshTokenExpiry = refreshTokenExpiry,
                    TokenType = "Bearer"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating tokens for UserId: {UserId}", userId);
                throw;
            }
        }

        public async Task<AuthTokenResponseDTO?> RefreshTokenAsync(string accessToken, string refreshToken, CancellationToken cancellationToken = default)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
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
            var securityKey = new SymmetricSecurityKey(keyBytes);

            try
            {
                _logger.LogInformation("Attempting to refresh token");

                // Validate the access token structure (even if expired)
                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = JwtConfig.Settings.Issuer,
                    ValidAudience = JwtConfig.Settings.Audience,
                    IssuerSigningKey = securityKey,
                    ValidateLifetime = false // Don't validate lifetime for refresh
                };

                var principal = tokenHandler.ValidateToken(accessToken, tokenValidationParameters, out var validatedToken);

                // Check if it's a valid JWT token
                if (validatedToken is not JwtSecurityToken jwtToken ||
                    !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                {
                    _logger.LogWarning("Token refresh failed: Invalid JWT algorithm");
                    return null;
                }

                // Get the jti claim from the access token
                var jtiClaim = principal.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti);
                if (jtiClaim == null)
                {
                    _logger.LogWarning("Token refresh failed: Missing JTI claim");
                    return null;
                }

                // Get the userId from the access token
                var claims = principal.Claims.Where(c => c.Type == "UserId").ToList();
                var userIdClaim = principal.Claims.Where(c => c.Type == "UserId").FirstOrDefault();
                if (userIdClaim == null || !long.TryParse(userIdClaim.Value, out var userId))
                {
                    _logger.LogWarning("Token refresh failed: Missing or invalid UserId claim");
                    return null;
                }

                // Verify the refresh token exists and is valid
                var storedRefreshToken = await _refreshTokenRepository.GetByTokenAsync(refreshToken, cancellationToken);
                if (storedRefreshToken == null)
                {
                    _logger.LogWarning("Token refresh failed: Refresh token not found in database");
                    return null;
                }

                // Check if refresh token is expired
                if (storedRefreshToken.ExpiryDate < DateTime.UtcNow)
                {
                    _logger.LogWarning("Token refresh failed: Refresh token expired for user {UserId}", userId);
                    return null;
                }

                // Check if refresh token has been used or revoked
                if (storedRefreshToken.IsUsed || storedRefreshToken.IsRevoked)
                {
                    _logger.LogWarning("Token refresh failed: Refresh token already used or revoked for user {UserId}. Potential security breach!", userId);
                    // Consider revoking all user tokens here as a security measure
                    await RevokeAllUserTokensAsync(userId, cancellationToken);
                    return null;
                }

                // Check if the refresh token's JwtId matches the access token's JwtId
                if (storedRefreshToken.JwtId != jtiClaim.Value)
                {
                    _logger.LogWarning("Token refresh failed: JTI mismatch for user {UserId}. Potential token theft!", userId);
                    // Revoke all tokens as security measure
                    await RevokeAllUserTokensAsync(userId, cancellationToken);
                    return null;
                }

                // Mark the current refresh token as used
                storedRefreshToken.IsUsed = true;
                await _refreshTokenRepository.UpdateAsync(storedRefreshToken, cancellationToken);

                _logger.LogInformation("Token refreshed successfully for user {UserId}", userId);

                // Generate new tokens
                return await GenerateTokensAsync(userId, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during token refresh");
                return null;
            }
        }

        public async Task<bool> RevokeTokenAsync(string refreshToken, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Attempting to revoke refresh token");
                
                var storedToken = await _refreshTokenRepository.GetByTokenAsync(refreshToken, cancellationToken);
                if (storedToken == null)
                {
                    _logger.LogWarning("Refresh token not found for revocation");
                    return false;
                }

                storedToken.IsRevoked = true;
                await _refreshTokenRepository.UpdateAsync(storedToken, cancellationToken);
                _logger.LogInformation("Refresh token revoked successfully for UserId: {UserId}", storedToken.UserId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error revoking refresh token");
                throw;
            }
        }

        public async Task<bool> RevokeAllUserTokensAsync(long userId, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Attempting to revoke all tokens for UserId: {UserId}", userId);
                var result = await _refreshTokenRepository.RevokeAllUserTokensAsync(userId, cancellationToken);
                _logger.LogInformation("Revoke all tokens completed for UserId: {UserId}", userId);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error revoking all tokens for UserId: {UserId}", userId);
                throw;
            }
        }

        private string GenerateRefreshToken()
        {
            try
            {
                var randomNumber = new byte[64];
                using var rng = RandomNumberGenerator.Create();
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating refresh token");
                throw new InvalidOperationException("Failed to generate refresh token", ex);
            }
        }
    }
}
