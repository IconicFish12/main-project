using System.Text.Json;

namespace main_project.Services
{
    public class FileHelper
    {
        public static readonly Dictionary<string, string> ExtensionFolder = new()
        {
            { ".jpg", "image" },
            { ".png", "image" },
            { ".pdf", "document/pdf" },
            { ".docx", "document/docx" },
            { ".pptx", "document/ppt" },
            { ".xlsx", "document/xlsx_csv" },
            { ".csv", "document/xlsx_csv" },
            { ".mp4", "video" }
        };


        public static string GetFolderExtension(string extension)
        {
            return ExtensionFolder.GetValueOrDefault(extension, "others");
        }

        public static void WriteJson<T>(List<T> data, string filePath)
        {
            var json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, json);
        }

        public static List<T> ReadJson<T>(string filePath)
        {
            if (!File.Exists(filePath)) return new List<T>();
            var json = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<List<T>>(json);
        }
    }
}
