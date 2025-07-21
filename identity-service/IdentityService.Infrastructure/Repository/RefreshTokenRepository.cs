using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IdentityService.Application.Interfaces.Repository;
using IdentityService.Domain.Entities;
using IdentityService.Infrastructure.DBContext;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Infrastructure.Repository
{
    public class RefreshTokenRepository : BaseRepository<RefreshToken>, IRefreshTokenRepository
    {
        public RefreshTokenRepository(IdentityDbContext context) : base(context)
        {

        }
        public async Task<RefreshToken?> GetByTokenAsync(string token)
        {
            return await _dbSet
                .Include(t => t.User)
                .FirstOrDefaultAsync(t => t.Token == token);
        }

        public async Task<IEnumerable<RefreshToken>> GetValidTokensByUserIdAsync(Guid userId)
        {
            return await _dbSet
                .Where(t => t.UserId == userId && t.RevokedDate == null && t.ExpiryDate > DateTime.UtcNow)
                .ToListAsync();
        }
    }
}
