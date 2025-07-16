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
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(IdentityDbContext context) : base(context)
        { 
            
        }
        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await _dbSet
                .Include(u => u.RefreshTokens)
                .FirstOrDefaultAsync(u => u.Username == username);
        }
    }
}
