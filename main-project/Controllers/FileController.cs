using main_project.Model;
using main_project.utility;
using Microsoft.AspNetCore.Mvc;

namespace main_project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {

        private readonly FileService __fileService;

        public FileController(FileService fileService)
        {
            __fileService = fileService;
        }

        [HttpGet("getFile")]
        public ActionResult<IEnumerable<FileMetaData>> getFile()
        {
            return Ok("Hello World");
        }

        [HttpPost("createFile")]
        public ActionResult createFile([FromForm] string request)
        {
            string nama = request;

            return Ok($"Halo Nama Saya : {nama}" );
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
