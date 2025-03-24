using AcademyApp.Api.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AcademyApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return BadRequest("Invalid login request.");
            }

            var result = await _authService.AuthenticateAsync(username, password);
            if (result == null)
            {
                return Unauthorized("Invalid credentials.");
            }

            return Ok(result);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return BadRequest("Invalid registration request.");
            }

            var result = await _authService.RegisterAsync(username, password);
            if (result == null)
            {
                return BadRequest("Registration failed.");
            }

            return CreatedAtAction(nameof(Login), new { username }, result);
        }
    }
}
