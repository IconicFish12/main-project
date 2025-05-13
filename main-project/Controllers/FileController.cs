using FileValidation;
using main_project.Model;
using main_project.Services;
using Microsoft.AspNetCore.Mvc;
using System.IO;

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

            if (allData.Count == 0)
            {
                return Ok(new
                {
                    message = "Data is empty"
                });
            }

            return Ok(new
            {
                message = "All File MetaData",
                allData = allData
            });
        }

        [HttpGet("getFile/{id}")]
        public ActionResult getFileByID(string id)
        {
            try
            {
                var file = _fileService.getFileByID(id);

                return Ok(new
                {
                    message = "File metadata found",
                    data = file
                });
            }
            catch (FileNotFoundException ex)
            {
                return NotFound(new
                {
                    message = ex.Message
                });
            }
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
                state = FileState.Updated;

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

        [HttpPut("updateMetadata/{id}")]
        public IActionResult UpdateMetadata(string id, [FromBody] FileMetaData updatedMeta)
        {
            try
            {
                _fileService.UpdateFileMetadata(id, updatedMeta);
                return Ok(new { message = "Metadata updated successfully!" });
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }


        [HttpDelete("deleteFile/{id}")]
        public ActionResult deleteFile(string id)
        {
            FileState state = FileState.Uploaded;

            try
            {
                _fileService.deleteFile(id);
                state = FileState.Deleted;

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
