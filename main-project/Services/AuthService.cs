using main_project.Config;
using main_project.Interfaces;
using main_project.Model;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace main_project.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppSettings? _appSettings;
        private readonly ICrudService<User> _userService;

        public AuthService(IOptions<AppSettings> appSettings, ICrudService<User> userService)
        {
            _appSettings = appSettings.Value;
            _userService = userService;
        }
        public async Task<AuthResponse?> Authenticate(LoginRequest model)
        {
            var user = _userService?.GetData().FirstOrDefault(u => u.Email == model.Email && u.Password == model.Password);

            // return null if user not found
            if (user == null) return null;

            // authentication successful so generate jwt token
            var token = await generateJwtToken(user);

            return new AuthResponse(user, token);
        }

        private async Task<string> generateJwtToken(User user)
        {
            //Generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = await Task.Run(() =>
            {

                var key = Encoding.ASCII.GetBytes(_appSettings.API.Key);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
                    Expires = DateTime.UtcNow.AddDays(7),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                return tokenHandler.CreateToken(tokenDescriptor);
            });

            return tokenHandler.WriteToken(token);
        }
    }
}
