using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Supabase;

public record AuthToken(
    string? id,
    string? accessToken,
    string? refreshToken,
    string? expiresIn,
    string? expiresAt,
    string? tokenType
);

public class LoginModel
{
    public string Username { get; set; } = String.Empty;
    public string Password { get; set; } = String.Empty;
}

namespace ReUnited_Backend.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;
        public AuthController(IConfiguration config)
        {
            _config = config;
        }

        [HttpPost("token")]
        public async Task<IActionResult> GenerateToken([FromBody] LoginModel login)
        {
            //if (login.Username != "user@example.com" || login.Password != "password123") return Unauthorized();


            var supabase = new Client(
                _config["Supabase:URL"],
                _config["Supabase:Key"],
                new SupabaseOptions()
            );

            
            


            //await supabase.InitializeAsync();

            var session = await supabase.Auth.SignIn(login.Username, login.Password);
            /*session.ProviderRefreshToken
                session.RefreshToken
                session.ProviderToken*/

            /*var claims = new[]
            {
                new Claim(ClaimTypes.Name, login.Username),
                new Claim(ClaimTypes.Role, "admin")
            };*/

            var authToken = new AuthToken
            (
                id: session?.User?.Id,
                accessToken: session?.AccessToken,
                refreshToken: session?.RefreshToken,
                expiresIn: session?.ExpiresIn.ToString(),
                expiresAt: session?.ExpiresAt().ToString(),
                tokenType: session?.TokenType
            );

            return Ok(authToken);
        }

        [HttpGet("debug-auth")]
        public IActionResult DebugAuth()
        {
            var authHeader = Request.Headers["Authorization"].ToString();
            return Ok(new { received = authHeader });
        }
    }
}
