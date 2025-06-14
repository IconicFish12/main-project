using FileValidation;
using main_project.Config;
using main_project.Model;
using main_project.Services;
using main_project.Services.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.IO;

namespace main_project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FileController : ControllerBase
    {
        private readonly ILogger<FileController> _logger;
        private readonly DefaultFileValidation _fileValidation;
        private readonly FileService _fileService;
        private readonly AppSettings _settings;

        public FileController(DefaultFileValidation fileValidation, FileService fileService, ILogger<FileController> logger, IOptions<AppSettings> settings)
        {
            _fileValidation = fileValidation;
            _fileService = fileService;
            _logger = logger;
            _settings = settings.Value;
        }


        [HttpGet("getFile")]
        public ActionResult GetFile()
        {
            FileState state = FileState.inProgress;
            _logger.LogInformation($"DEBUG: Memulai pengambilan semua file - State: {state}", state.ToString().ToUpper());

            var allData = _fileService.getAllFile();

            if (allData.Count == 0)
            {
                state = FileState.Failed;
                _logger.LogWarning($"DEBUG: Data kosong saat mengambil file - State: {state}", state.ToString().ToUpper());
                return Ok(new { message = "Data is empty" });
            }

            state = FileState.isCompleted;
            _logger.LogInformation($"DEBUG: Pengambilan semua file berhasil - State: {state}", state.ToString().ToUpper());
            return Ok(new { message = "All File MetaData", allData = allData });
        }


        [HttpGet("getFile/{id}")]
        public ActionResult GetFileByID(string id)
        {
            FileState state = FileState.inProgress;
            _logger.LogInformation($"DEBUG: Memulai pengambilan file dengan ID {id} - State: {state}", id, state.ToString().ToUpper());

            try
            {
                var file = _fileService.getFileByID(id);

                state = FileState.isCompleted;
                _logger.LogInformation($"DEBUG: File dengan ID {id} ditemukan - State: {state}", id, state.ToString().ToUpper());

                return Ok(new { message = "File metadata found", data = file });
            }
            catch (FileNotFoundException ex)
            {
                state = FileState.Failed;
                _logger.LogWarning($"DEBUG: File dengan ID {id} tidak ditemukan - State: {state} - Error: {ex.Message}", id, state.ToString().ToUpper(), ex.Message);

                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPost("createFile")]
        public async Task<ActionResult> UploadFile([FromForm] IFormFile file)
        {
            FileState state = FileState.Idle;
            _logger.LogInformation("DEBUG: Memulai proses upload file - State: {State}", state.ToString().ToUpper());


            try
            {
                state = FileState.inProgress;
                _logger.LogInformation($"DEBUG: Validasi file - State: {state}", state.ToString().ToUpper());

                if (_settings.Security.EnableFileValidation)
                {
                    _fileValidation.validate(file);
                }

                state = FileState.isCompleted;
                _logger.LogInformation($"DEBUG: File tervalidasi - State: {state}", state.ToString().ToUpper());

                var result = await _fileService.uploadFIle(file);

                state = FileState.isSaved;
                _logger.LogInformation($"DEBUG: File berhasil disimpan - State: {state}", state.ToString().ToUpper());

                return Ok(new
                {
                    message = $"Data berhasil ditambahkan - State: {state}",
                    data = result
                });
            }
            catch (Exception ex)
            {
                state = FileState.Failed;
                _logger.LogError($"DEBUG: Gagal menambahkan file - State: {state} - Error: {ex.Message}", state.ToString().ToUpper(), ex.Message);

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
            FileState state = FileState.Idle;
            _logger.LogInformation($"DEBUG: Memulai update file ID {id} - State: {state}", id, state.ToString().ToUpper());

            try
            {
                state = FileState.inProgress;
                _logger.LogInformation($"DEBUG: Validasi file - State: {state}", state.ToString().ToUpper());

                _fileValidation.validate(file);
                state = FileState.isCompleted;
                _logger.LogInformation($"DEBUG: File tervalidasi - State: {state}", state.ToString().ToUpper());

                var updatedData = await _fileService.updateFile(id, file);

                state = FileState.isUpdated;
                _logger.LogInformation($"DEBUG: File berhasil diupdate - ID: {id} - State: {state}", id, state.ToString().ToUpper());

                return Ok(new
                {
                    message = $"Data berhasil diubah - State: {state}",
                    data = updatedData
                });
            }
            catch (Exception ex)
            {
                state = FileState.Failed;
                _logger.LogError($"DEBUG: Gagal mengubah file - ID: {id} - State: {state} - Error: {ex.Message}", id, state.ToString().ToUpper(), ex.Message);

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
            FileState state = FileState.inProgress;
            _logger.LogInformation($"DEBUG: Memulai update metadata file ID {id} - State:{state}", id, state.ToString().ToUpper());

            try
            {
                _fileService.UpdateFileMetadata(id, updatedMeta);

                state = FileState.isUpdated;
                _logger.LogInformation($"DEBUG: Metadata berhasil diupdate - ID:{id} - State: {state}", id, state.ToString().ToUpper());

                return Ok(new { message = "Metadata updated successfully!" });
            }
            catch (Exception ex)
            {
                state = FileState.Failed;
                _logger.LogError($"DEBUG: Gagal update metadata - ID: {id} - State: {state} - Error: {ex.Message}", id, state.ToString().ToUpper(), ex.Message);

                return NotFound(new { message = ex.Message });
            }
        }

        [HttpDelete("deleteFile/{id}")]
        public ActionResult DeleteFile(string id)
        {
            FileState state = FileState.Idle;
            _logger.LogInformation($"DEBUG: Memulai penghapusan file ID {id} - State: {state}", id, state.ToString().ToUpper());

            try
            {
                _fileService.deleteFile(id);

                state = FileState.isDeleted;
                _logger.LogInformation($"DEBUG: File berhasil dihapus - ID: {id} - State: {state}", id, state.ToString().ToUpper());

                return Ok(new
                {
                    message = $"Data berhasil dihapus - State: {state}"
                });
            }
            catch (Exception ex)
            {
                state = FileState.Failed;
                _logger.LogError($"DEBUG: Gagal menghapus file - ID: {id} - State: {state} - Error: {ex.Message}", id, state.ToString().ToUpper(), ex.Message);

                return StatusCode(400, new
                {
                    message = $"Gagal menghapus data - State: {state}",
                    error = ex.Message
                });
            }
        }
    }
}
