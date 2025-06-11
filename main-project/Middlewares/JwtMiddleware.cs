using main_project.Config;
using main_project.Interfaces;
using main_project.Model;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace main_project.Middlewares
{
    public class JwtMiddleware
    {
        private readonly ILogger<JwtMiddleware> _logger;
        private readonly RequestDelegate _next;
        private readonly AppSettings _appSettings;

        public JwtMiddleware(RequestDelegate next, IOptions<AppSettings> appSettings, ILogger<JwtMiddleware> logger)
        {
            _next = next;
            _appSettings = appSettings.Value;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context, ICrudService<User> userService)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token != null)
            {
                await AttachUserToContext(context, userService, token);
            }

            await _next(context);
        }

        private async Task AttachUserToContext(HttpContext context, ICrudService<User> userService, string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_appSettings.API.Key);

                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = jwtToken.Claims.First(x => x.Type == "id").Value;

                var user = await userService.FindData(userId);

                if (user != null)
                {
                    context.Items["User"] = user;
                }
            }
            catch (Exception ex)
            {
                // Do nothing if JWT validation fails
                // Let request proceed without attaching user to context
                _logger.LogInformation("Terjadi suatu masalah pada Authentication");
                _logger.LogError($"Error info : {ex.Message.ToString()}");
            }
        }
    }

}
