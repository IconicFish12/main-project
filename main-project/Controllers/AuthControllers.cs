using main_project.Interfaces;
using main_project.Model;
using main_project.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;

namespace main_project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthControllers : ControllerBase
    {
        private readonly JWTService _jwtService;
        private readonly ICrudService<User> _userService;

        public AuthControllers(JWTService jwtService, ICrudService<User> userService)
        {
            _jwtService = jwtService;
            _userService = userService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromForm] string username, [FromForm] string password)
        {
            var users = await Task.FromResult(_userService.GetData());
            var user = users.FirstOrDefault(u =>
                u.Name?.Username?.ToLower() == username.ToLower() &&
                u.Password == HashPassword(password));

            if (user == null)
                return Unauthorized(new { message = "Invalid username or password" });

            var token = _jwtService.GenerateToken(user);
            user.LoginToken = token;

            await _userService.UpdateData(user.Id.ToString(), user); // Simpan token ke file

            return Ok(new
            {
                token,
                user.Id,
                user.Email,
                name = new
                {
                    user.Name?.FirstName,
                    user.Name?.LastName,
                    user.Name?.Username
                }
            });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(
            [FromForm] string firstName,
            [FromForm] string lastName,
            [FromForm] string username,
            [FromForm] string email,
            [FromForm] string password)
        {
            var users = await Task.FromResult(_userService.GetData());

            if (users.Any(u => u.Email?.ToLower() == email.ToLower()))
                return BadRequest(new { message = "Email already registered" });

            if (users.Any(u => u.Name?.Username?.ToLower() == username.ToLower()))
                return BadRequest(new { message = "Username already taken" });

            var newUser = new User
            {
                Name = new Name
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Username = username
                },
                Email = email,
                Password = HashPassword(password),
                EmailVerif = false
            };

            var created = await _userService.CreateData(newUser);
            return Ok(new { message = "Registration successful", userId = created?.Id });
        }

        [HttpGet("logout")]
        public async Task<IActionResult> Logout()
        {
            var user = HttpContext.Items["User"] as User;
            if (user == null)
                return Unauthorized(new { message = "Unauthorized" });

            user.LoginToken = null;
            await _userService.UpdateData(user.Id.ToString(), user);

            return Ok(new { message = "Logged out successfully" });
        }

        private string HashPassword(string password)
        {
            using var sha = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }
}
