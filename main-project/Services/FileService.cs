using main_project.Model;

namespace main_project.Services
{
    public class FileService
    {

        private readonly string _storageRoot = "storage";
        private readonly string _metadataPath = "file_storage.json";

        public  List<FileMetaData> getAllFile()
        {
            var data = FileHelper.ReadJson<FileMetaData>(_metadataPath);

            return data;
        }

        public async Task<FileMetaData> uploadFIle(IFormFile file)
        {
            var extension = Path.GetExtension(file.FileName).ToLower();
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
                id = Guid.NewGuid().ToString(), 
                filename = Path.GetFileNameWithoutExtension(file.FileName),
                file_type = extension.Replace(".", ""), 
                size = unchecked((int)file.Length / 1024), 
                created_at = DateTime.Now.ToString(),
                modified_at = null,
                deleted_at = null,
            };

            var allFiles = FileHelper.ReadJson<FileMetaData>(_metadataPath);
            allFiles.Add(metaData);
            FileHelper.WriteJson(allFiles, _metadataPath);

            return metaData;
        }

        public async Task<FileMetaData> updateFile(string id, IFormFile newFile)
        {

            var allFiles = FileHelper.ReadJson<FileMetaData>(_metadataPath);
            var fileMeta = allFiles.FirstOrDefault(f => f.id == id);

            if (fileMeta == null)
                throw new FileNotFoundException("File metadata not found.");

            var oldExtension = "." + fileMeta.file_type;
            var oldFolder = FileHelper.GetFolderExtension(oldExtension);
            var oldPath = Path.Combine(_storageRoot, oldFolder, fileMeta.filename + oldExtension);

            if (File.Exists(oldPath))
                File.Delete(oldPath);

            var newExtension = Path.GetExtension(newFile.FileName).ToLower();
            var newFolder = FileHelper.GetFolderExtension(newExtension);
            var newPath = Path.Combine(_storageRoot, newFolder);
            Directory.CreateDirectory(newPath);

            var newFilePath = Path.Combine(newPath, newFile.FileName);
            using (var stream = new FileStream(newFilePath, FileMode.Create))
            {
                await newFile.CopyToAsync(stream);
            }

            // Update metadata
            fileMeta.filename = Path.GetFileNameWithoutExtension(newFile.FileName);
            fileMeta.file_type = newExtension.Replace(".", "");
            fileMeta.size = unchecked((int)newFile.Length / 1024);
            fileMeta.modified_at = DateTime.Now.ToString();

            FileHelper.WriteJson(allFiles, _metadataPath);
            return fileMeta;
        }

        public void deleteFile(string id)
        {
            var allFiles = FileHelper.ReadJson<FileMetaData>(_metadataPath);
            var fileMeta = allFiles.FirstOrDefault(f => f.id == id);

            if (fileMeta == null)
                throw new FileNotFoundException("File metadata not found.");

            var extension = "." + fileMeta.file_type;
            var folder = FileHelper.GetFolderExtension(extension);
            var path = Path.Combine(_storageRoot, folder, fileMeta.filename + extension);

            if (File.Exists(path))
                File.Delete(path);

            allFiles.Remove(fileMeta);
            FileHelper.WriteJson(allFiles, _metadataPath);
        }
    }
}
