using IdentityService.Application.Interfaces.Services;
using IdentityService.Application.Interfaces.UnitOfWork;
using IdentityService.Domain.Entities;
using Microsoft.IdentityModel.Tokens;

namespace IdentityService.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _uow;
        private readonly ITokenService _tokenService;

        public AuthService(IUnitOfWork uow, ITokenService tokenService)
        {
            _uow = uow;
            _tokenService = tokenService;
        }

        public async Task<(string accessToken, string refreshToken)> LoginAsync(string username, string password)
        {
            var user = await _uow.Users.GetByUsernameAsync(username);
            if (user == null || !VerifyPassword(password, user.PasswordHash))
                throw new UnauthorizedAccessException("Invalid credentials");

            var accessToken = _tokenService.GenerateAccessToken(user);
            var refreshToken = _tokenService.GenerateRefreshToken(user.Id);

            await _uow.RefreshTokens.AddAsync(refreshToken);
            await _uow.SaveChangesAsync();

            return (accessToken, refreshToken.Token);
        }

        public async Task<(string accessToken, string refreshToken)> RefreshAsync(string oldRefreshToken)
        {
            var token = await _uow.RefreshTokens.GetByTokenAsync(oldRefreshToken);
            if (token == null || token.ExpiryDate < DateTime.UtcNow || token.RevokedDate != null)
                throw new SecurityTokenException("Invalid refresh token");

            // Thu hồi
            token.RevokedDate = DateTime.UtcNow;
            token.ReplacedByToken = "replaced";

            // Sinh mới
            var newRefresh = _tokenService.GenerateRefreshToken(token.UserId);
            await _uow.RefreshTokens.AddAsync(newRefresh);

            var accessToken = _tokenService.GenerateAccessToken(token.User);
            await _uow.SaveChangesAsync();

            return (accessToken, newRefresh.Token);
        }

        public async Task RegisterAsync(string username, string password)
        {
            var existing = await _uow.Users.GetByUsernameAsync(username);
            if (existing != null)
                throw new Exception("Username đã tồn tại");

            var user = new User
            {
                Id = Guid.NewGuid(),
                Username = username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
                Role = "User"
            };

            await _uow.Users.AddAsync(user);
            await _uow.SaveChangesAsync();
        }

        private bool VerifyPassword(string input, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(input, hashedPassword);
        }
    }
}
