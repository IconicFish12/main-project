using main_project.Model;

namespace main_project.Services
{
    public class FileService
    {

        private readonly string _storageRoot = "storage";
        private readonly string _metadataPath = "file_storage.json";

        public async Task<FileMetaData> uploadFIle(IFormFile file)
        {
            var extension = Path.GetExtension(file.FileName);
            var targetFolder = FileHelper.GetFolderExtension(extension);
            var fullPath = Path.Combine(_storageRoot, targetFolder);

            if (!Directory.Exists(fullPath))
            {
                Directory.CreateDirectory(fullPath);
            }

            var filePath = Path.Combine(fullPath, file.FileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var metaData = new FileMetaData
            {
                filename = file.FileName,
                file_type = file.ContentType,
                size = unchecked((int)file.Length),
                created_at = DateTime.Now.ToString("{0:r}"),
                modified_at = "",
                deleted_at = "",
            };


            var allFiles = FileHelper.ReadJson<FileMetaData>(_metadataPath);
            allFiles.Add(metaData);
            FileHelper.WriteJson(allFiles, _metadataPath);

            return metaData;
        }

        public void deleteFile(string filename)
        {

        }

        public void renameFile(string oldName, string newName)
        {

        }

    }
}
