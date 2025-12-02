using PMSBackend.Domain.Entities;

namespace PMSBackend.Domain.Repositories
{
    public interface IRefreshTokenRepository
    {
        Task<RefreshTokenEntity?> GetByTokenAsync(string token, CancellationToken cancellationToken = default);
        Task<RefreshTokenEntity> AddAsync(RefreshTokenEntity refreshToken, CancellationToken cancellationToken = default);
        Task UpdateAsync(RefreshTokenEntity refreshToken, CancellationToken cancellationToken = default);
        Task<bool> RevokeAllUserTokensAsync(long userId, CancellationToken cancellationToken = default);
        Task<List<RefreshTokenEntity>> GetActiveTokensByUserIdAsync(long userId, CancellationToken cancellationToken = default);
    }
}

