using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IdentityService.Application.Interfaces.Repository;
using IdentityService.Application.Interfaces.UnitOfWork;
using IdentityService.Infrastructure.DBContext;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IdentityDbContext _context;
        public IUserRepository Users { get; }

        public IRefreshTokenRepository RefreshTokens { get; }

        public UnitOfWork(
            IdentityDbContext context,
            IUserRepository userRepository,
            IRefreshTokenRepository refreshTokenRepository)
        {
            _context = context;
            Users = userRepository;
            RefreshTokens = refreshTokenRepository;
        }

        public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();

        public void Dispose() => _context.Dispose();
    }
}
