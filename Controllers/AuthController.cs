using EcommerceApp.Application.Features.Auth.Login;
using EcommerceApp.Application.Features.Auth.RefreshToken;
using EcommerceApp.Application.Features.Auth.Register;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EcommerceApp.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : Controller
    {
        [HttpPost("register")]
        public async Task<IActionResult> Register(
            [FromServices] RegisterHandler handler,
            [FromBody] RegisterRequest request,
            CancellationToken cancellationToken)
        {
            var result = await handler.HandleAsync(request, cancellationToken);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(
            [FromServices] LoginHandler handler,
            [FromBody] LoginRequest request,
            CancellationToken cancellationToken)
        {
            var result = await handler.HandleAsync(request, cancellationToken);

            if (!result.Success)
                return Unauthorized(result);

            return Ok(result);
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken(
            [FromServices] RefreshTokenHandler handler,
            [FromBody] RefreshTokenRequest request,
            CancellationToken cancellationToken)
        {
            var result = await handler.HandleAsync(request, cancellationToken);

            if (!result.Success)
                return Unauthorized(result);

            return Ok(result);
        }

        [Authorize]
        [HttpGet("me")]
        public IActionResult Me()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            var roles = User.FindAll(ClaimTypes.Role);

            return Ok(new
            {
                UserId = userId,
                Email = email,
                Roles = roles
            });
        }
    }
}
