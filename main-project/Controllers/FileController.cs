using FileValidation;
using main_project.Model;
using main_project.Services;
using Microsoft.AspNetCore.Mvc;

namespace main_project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly DefaultFileValidation _fileValidation;
        private readonly FileService _fileService;

        public FileController(DefaultFileValidation fileValidation, FileService fileService)
        {
            _fileValidation = fileValidation;
            _fileService = fileService;
        }

        [HttpGet("getFile")]
        public ActionResult getFile()
        {
            var allData = _fileService.getAllFile();
            return Ok(new
            {
                message = "All File MetaData",
                data = allData
            });
        }

        [HttpPost("createFile")]
        public async Task<ActionResult> UploadFile([FromForm] IFormFile file)
        {
            try
            {
                _fileValidation.validate(file);
                var result = await _fileService.uploadFIle(file);
                return Ok(new
                {
                    message = "Data berhasil ditambahkan",
                    data = result
                });
            }
            catch (Exception ex)
            {
                return StatusCode(400, new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPut("updateFile/{id}")]
        public async Task<ActionResult> UpdateFile([FromForm] IFormFile file, string id)
        {
            try
            {
                _fileValidation.validate(file);
                var updatedData = await _fileService.updateFile(id, file);

                return Ok(new
                {
                    message = "Data berhasil diubah",
                    data = updatedData
                });
            }
            catch (Exception ex)
            {
                return StatusCode(400, new
                {
                    message = ex.Message
                });
            }
        }

        [HttpDelete("deleteFile/{id}")]
        public ActionResult deleteFile(string id)
        {
            try
            {
                _fileService.deleteFile(id);
                return Ok(new
                {
                    message = "Data berhasil dihapus",
                });
            }
            catch (Exception ex)
            {
                return StatusCode(400, new
                {
                    message = ex.Message
                });
            }
        }

    }
}
