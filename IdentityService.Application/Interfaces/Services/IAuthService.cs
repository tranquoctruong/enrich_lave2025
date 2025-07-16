using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityService.Application.Interfaces.Services
{
    public interface IAuthService
    {
        Task<(string accessToken, string refreshToken)> LoginAsync(string username, string password);
        Task<(string accessToken, string refreshToken)> RefreshAsync(string oldRefreshToken);
        Task RegisterAsync(string username, string password);
    }
}
