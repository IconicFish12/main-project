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
            FileState state = FileState.Uploaded;

            try
            {
                _fileValidation.validate(file);
                state = FileState.Validated;

                var result = await _fileService.uploadFIle(file);
                state = FileState.Saved;

                return Ok(new
                {
                    message = $"Data berhasil ditambahkan - State: {state}",
                    data = result
                });
            }
            catch (Exception ex)
            {
                state = FileState.Failed;

                return StatusCode(400, new
                {
                    message = $"Gagal menambahkan data - State: {state}",
                    error = ex.Message
                });
            }
        }

        [HttpPut("updateFile/{id}")]
        public async Task<ActionResult> UpdateFile([FromForm] IFormFile file, string id)
        {
            FileState state = FileState.Uploaded;

            try
            {
                _fileValidation.validate(file);
                state = FileState.Validated;

                var updatedData = await _fileService.updateFile(id, file);
                state = FileState.Saved;

                return Ok(new
                {
                    message = $"Data berhasil diubah - State: {state}",
                    data = updatedData
                });
            }
            catch (Exception ex)
            {
                state = FileState.Failed;

                return StatusCode(400, new
                {
                    message = $"Gagal mengubah data - State: {state}",
                    error = ex.Message
                });
            }
        }

        [HttpDelete("deleteFile/{id}")]
        public ActionResult deleteFile(string id)
        {
            FileState state = FileState.Uploaded;

            try
            {
                _fileService.deleteFile(id);
                state = FileState.Saved;

                return Ok(new
                {
                    message = $"Data berhasil dihapus - State: {state}"
                });
            }
            catch (Exception ex)
            {
                state = FileState.Failed;

                return StatusCode(400, new
                {
                    message = $"Gagal menghapus data - State: {state}",
                    error = ex.Message
                });
            }
        }
    }
}
