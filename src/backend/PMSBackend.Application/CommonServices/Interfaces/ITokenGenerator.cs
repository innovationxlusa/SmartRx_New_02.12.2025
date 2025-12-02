using PMSBackend.Application.DTOs;

namespace PMSBackend.Application.CommonServices.Interfaces
{
    public interface ITokenGenerator
    {
        Task<string> GenerateJWTToken(long userId);
        Task<AuthTokenResponseDTO> GenerateTokensAsync(long userId, CancellationToken cancellationToken = default);
        Task<AuthTokenResponseDTO?> RefreshTokenAsync(string accessToken, string refreshToken, CancellationToken cancellationToken = default);
        Task<bool> RevokeTokenAsync(string refreshToken, CancellationToken cancellationToken = default);
        Task<bool> RevokeAllUserTokensAsync(long userId, CancellationToken cancellationToken = default);
    }
}
