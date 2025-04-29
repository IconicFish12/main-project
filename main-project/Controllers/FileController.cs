using main_project.Model;
using main_project.Services;
using Microsoft.AspNetCore.Mvc;

namespace main_project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {

        private readonly FileService _fileService;

        public FileController(FileService fileService)
        {
            _fileService = fileService;
        }

        [HttpGet("getFile")]
        public ActionResult getFile()
        {
            return Ok(new { message = "Hello World" });
        }

        [HttpPost("createFile")]
        public async Task<IActionResult> UploadFile([FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("File tidak valid atau kosong.");
            }

            try
            {
                var result = await _fileService.uploadFIle(file);
                return Ok( new 
                { 
                    message = "Data berhasil ditambahkan",
                    data = result 
                });
            }
            catch (Exception ex)
            {
                return StatusCode(400, new { 
                    message = ex.Message
                });
            }
        }

        [HttpPut("updateFile")]
        public ActionResult updateFile([FromForm] string request)
        {
            string nama = request;

            return Ok($"Halo Nama Saya : {nama}");
        }

        [HttpPut("deleteFile")]
        public ActionResult deleteFile()
        {
            return Ok("Hello World");
        }

    }
}
