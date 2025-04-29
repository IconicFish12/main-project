using main_project.Model;
using System.Text.Json;

namespace main_project.Services
{
    public class FileHelper
    {
        public static readonly Dictionary<string, string> ExtensionFolder = new()
        {
            { ".jpg", "image" },
            { ".png", "image" },
            { ".jpeg", "image" },
            { ".pdf", "document/pdf" },
            { ".docx", "document/docx" },
            { ".pptx", "document/ppt" },
            { ".xlsx", "document/xlsx_csv" },
            { ".csv", "document/xlsx_csv" },
            { ".mp4", "video" }
        };


        public static string GetFolderExtension(string extension)
        {
            return ExtensionFolder.GetValueOrDefault(extension.ToLower(), "others");
        }

        private class DataWrapper<T>
        {
            public List<T> Data { get; set; }
        } 

        public static void WriteJson<T>(List<T> data, string filePath)
        {
            var wrapper = new DataWrapper<T> { Data = data };
            var options = new JsonSerializerOptions { WriteIndented = true };
            var json = JsonSerializer.Serialize(wrapper, options);
            File.WriteAllText(filePath, json);
        }

        public static List<T> ReadJson<T>(string filePath)
        {
            if (!File.Exists(filePath)) return new List<T>();

            var json = File.ReadAllText(filePath);
            if (string.IsNullOrWhiteSpace(json))
                return new List<T>();

            var wrapper = JsonSerializer.Deserialize<DataWrapper<T>>(json);
            return wrapper?.Data ?? new List<T>();
        }
    }
}
