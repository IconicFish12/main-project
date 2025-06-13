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
        private readonly ILogger<FileController> _logger;
        private readonly DefaultFileValidation _fileValidation;
        private readonly FileService _fileService;

        public FileController(DefaultFileValidation fileValidation, FileService fileService, ILogger<FileController> logger)
        {
            _fileValidation = fileValidation;
            _fileService = fileService;
            _logger = logger;
        }


        [HttpGet("getFile")]
        public ActionResult GetFile()
        {
            FileState state = FileState.inProgress;
            _logger.LogInformation("DEBUG: Memulai pengambilan semua file - State: {State}", state.ToString().ToUpper());

            var allData = _fileService.getAllFile();

            if (allData.Count == 0)
            {
                state = FileState.Failed;
                _logger.LogWarning("DEBUG: Data kosong saat mengambil file - State: {State}", state.ToString().ToUpper());
                return Ok(new { message = "Data is empty" });
            }

            state = FileState.isCompleted;
            _logger.LogInformation("DEBUG: Pengambilan semua file berhasil - State: {State}", state.ToString().ToUpper());
            return Ok(new { message = "All File MetaData", allData = allData });
        }


        [HttpGet("getFile/{id}")]
        public ActionResult GetFileByID(string id)
        {
            FileState state = FileState.inProgress;
            _logger.LogInformation("DEBUG: Memulai pengambilan file dengan ID {ID} - State: {State}", id, state.ToString().ToUpper());

            try
            {
                var file = _fileService.getFileByID(id);

                state = FileState.isCompleted;
                _logger.LogInformation("DEBUG: File dengan ID {ID} ditemukan - State: {State}", id, state.ToString().ToUpper());

                return Ok(new { message = "File metadata found", data = file });
            }
            catch (FileNotFoundException ex)
            {
                state = FileState.Failed;
                _logger.LogWarning("DEBUG: File dengan ID {ID} tidak ditemukan - State: {State} - Error: {Error}", id, state.ToString().ToUpper(), ex.Message);

                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPost("createFile")]
        public async Task<ActionResult> UploadFile([FromForm] IFormFile file)
        {
<<<<<<< HEAD
            FileState state = FileState.Idle;
            _logger.LogInformation("DEBUG: Memulai proses upload file - State: {State}", state.ToString().ToUpper());
=======
            FileState state = FileState.Uploaded;
>>>>>>> main

            try
            {
                state = FileState.inProgress;
                _logger.LogInformation("DEBUG: Validasi file - State: {State}", state.ToString().ToUpper());

                _fileValidation.validate(file);
<<<<<<< HEAD
                state = FileState.isCompleted;
                _logger.LogInformation("DEBUG: File tervalidasi - State: {State}", state.ToString().ToUpper());

                var result = await _fileService.uploadFIle(file);

                state = FileState.isSaved;
                _logger.LogInformation("DEBUG: File berhasil disimpan - State: {State}", state.ToString().ToUpper());
=======
                state = FileState.Validated;

                var result = await _fileService.uploadFIle(file);
                state = FileState.Saved;
>>>>>>> main

                return Ok(new
                {
                    message = $"Data berhasil ditambahkan - State: {state}",
                    data = result
                });
            }
            catch (Exception ex)
            {
                state = FileState.Failed;
<<<<<<< HEAD
                _logger.LogError("DEBUG: Gagal menambahkan file - State: {State} - Error: {Error}", state.ToString().ToUpper(), ex.Message);
=======
>>>>>>> main

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
<<<<<<< HEAD
            FileState state = FileState.Idle;
            _logger.LogInformation("DEBUG: Memulai update file ID {ID} - State: {State}", id, state.ToString().ToUpper());
=======
            FileState state = FileState.Uploaded;
>>>>>>> main

            try
            {
                state = FileState.inProgress;
                _logger.LogInformation("DEBUG: Validasi file - State: {State}", state.ToString().ToUpper());

                _fileValidation.validate(file);
<<<<<<< HEAD
                state = FileState.isCompleted;
                _logger.LogInformation("DEBUG: File tervalidasi - State: {State}", state.ToString().ToUpper());
=======
                state = FileState.Validated;
>>>>>>> main

                var updatedData = await _fileService.updateFile(id, file);
                state = FileState.Saved;

                state = FileState.isUpdated;
                _logger.LogInformation("DEBUG: File berhasil diupdate - ID: {ID} - State: {State}", id, state.ToString().ToUpper());

                return Ok(new
                {
                    message = $"Data berhasil diubah - State: {state}",
                    data = updatedData
                });
            }
            catch (Exception ex)
            {
                state = FileState.Failed;
<<<<<<< HEAD
                _logger.LogError("DEBUG: Gagal mengubah file - ID: {ID} - State: {State} - Error: {Error}", id, state.ToString().ToUpper(), ex.Message);
=======
>>>>>>> main

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
            _logger.LogInformation("DEBUG: Memulai update metadata file ID {ID} - State: {State}", id, state.ToString().ToUpper());

            try
            {
                _fileService.UpdateFileMetadata(id, updatedMeta);

                state = FileState.isUpdated;
                _logger.LogInformation("DEBUG: Metadata berhasil diupdate - ID: {ID} - State: {State}", id, state.ToString().ToUpper());

                return Ok(new { message = "Metadata updated successfully!" });
            }
            catch (Exception ex)
            {
                state = FileState.Failed;
                _logger.LogError("DEBUG: Gagal update metadata - ID: {ID} - State: {State} - Error: {Error}", id, state.ToString().ToUpper(), ex.Message);

                return NotFound(new { message = ex.Message });
            }
        }

        [HttpDelete("deleteFile/{id}")]
        public ActionResult DeleteFile(string id)
        {
<<<<<<< HEAD
            FileState state = FileState.Idle;
            _logger.LogInformation("DEBUG: Memulai penghapusan file ID {ID} - State: {State}", id, state.ToString().ToUpper());
=======
            FileState state = FileState.Uploaded;
>>>>>>> main

            try
            {
                _fileService.deleteFile(id);
<<<<<<< HEAD

                state = FileState.isDeleted;
                _logger.LogInformation("DEBUG: File berhasil dihapus - ID: {ID} - State: {State}", id, state.ToString().ToUpper());
=======
                state = FileState.Saved;
>>>>>>> main

                return Ok(new
                {
                    message = $"Data berhasil dihapus - State: {state}"
                });
            }
            catch (Exception ex)
            {
                state = FileState.Failed;
<<<<<<< HEAD
                _logger.LogError("DEBUG: Gagal menghapus file - ID: {ID} - State: {State} - Error: {Error}", id, state.ToString().ToUpper(), ex.Message);
=======
>>>>>>> main

                return StatusCode(400, new
                {
                    message = $"Gagal menghapus data - State: {state}",
                    error = ex.Message
                });
            }
        }
    }
}
