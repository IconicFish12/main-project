using main_project.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace main_project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthControllers : ControllerBase
    {
        private readonly JWTService _jwtService;

        public AuthControllers(JWTService jwtService)
        {
            _jwtService = jwtService;
        }

        [HttpPost("LoginHandle")]
        public ActionResult login([FromForm] string request)
        {
            return Ok(new { message = "hello world" });
        }
    }
}
