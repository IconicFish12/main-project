using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace main_project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthControllers : ControllerBase
    {
        [HttpPost("LoginHandle")]
        public ActionResult login([FromForm] string request)
        {
            return Ok(new { message = "hello world" });
        }
    }
}
