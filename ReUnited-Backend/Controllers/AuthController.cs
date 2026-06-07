using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Supabase;


public class LoginModel
{
    public string Username { get; set; } = String.Empty;
    public string Password { get; set; } = String.Empty;
}

public class RegisterRequest
{
    public string Email { get; set; } = "";
    public string Password { get; set; } = "";
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

        [HttpPost("login")]
        public async Task<IActionResult> GenerateToken([FromBody] LoginModel login)
        {
            var supabase = new Client(
                _config["Supabase:Url"],
                _config["Supabase:AnonKey"],
                new SupabaseOptions()
            );

            var session = await supabase.Auth.SignIn(login.Username, login.Password);

            return Ok(session?.AccessToken);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var supabase = new Client(
                _config["Supabase:Url"],
                _config["Supabase:AnonKey"],
                new SupabaseOptions());

            var result =
                await supabase.Auth.SignUp(
                    request.Email,
                    request.Password);

            return Ok();
        }
    }
}
