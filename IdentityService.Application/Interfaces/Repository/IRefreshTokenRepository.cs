using IdentityService.Domain.Entities;

namespace IdentityService.Application.Interfaces.Repository
{
    public interface IRefreshTokenRepository : IBaseRepository<RefreshToken>
    {
        Task<RefreshToken?> GetByTokenAsync(string token);
        Task<IEnumerable<RefreshToken>> GetValidTokensByUserIdAsync(Guid userId);
    }
}
