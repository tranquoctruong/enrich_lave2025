using IdentityService.Application.Interfaces.Services;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.API.Controllers
{
    using IdentityService.DTO;
    using Microsoft.AspNetCore.Authorization;

    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet("api-check")]
        [Authorize]
        public IActionResult HelpCheck()
        {
            return Ok("Very good");
        }

        /// <summary>
        /// Đăng nhập
        /// </summary>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                var result = await _authService.LoginAsync(request.Username, request.Password);
                return Ok(new TokenResponse
                {
                    AccessToken = result.accessToken,
                    RefreshToken = result.refreshToken
                });
            }
            catch (Exception ex)
            {
                return StatusCode(403, new { message = ex.Message });
            }
            
        }

        /// <summary>
        /// Làm mới access token từ refresh token
        /// </summary>
        [HttpPost("refresh-token")]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request)
        {
            var result = await _authService.RefreshAsync(request.RefreshToken);
            return Ok(new TokenResponse
            {
                AccessToken = result.accessToken,
                RefreshToken = result.refreshToken
            });
        }

        /// <summary>
        /// Đăng ký tài khoản mới
        /// </summary>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            await _authService.RegisterAsync(request.Username, request.Password);
            return Ok("Đăng ký thành công");
        }
    }
}