using ConsoleProject.Model;
using main_project.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ConsoleProject.Repository
{
    internal class FileRepository
    {
        public static async Task<List<FileMetaData>> GetData()
        {
            string baseUrl = "http://localhost:5144/api/File/getFile";

            var listData = new List<FileMetaData>();

            using HttpClient client = new HttpClient();

            try
            {
                HttpResponseMessage res = await client.GetAsync(baseUrl);

                if (res.IsSuccessStatusCode)
                {
                    var respons = await res.Content.ReadAsStringAsync();

                    var jsonRespons = JsonSerializer.Deserialize<ApiRespons>(respons, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    if (jsonRespons?.Data == null && jsonRespons == null) return null;

                    listData.AddRange(jsonRespons.Data);
                }
            }
            catch (Exception ex) 
            {
                Console.WriteLine($"Exception: {ex.Message}");
            }

            return listData;
        }

        public static async Task RenameFileMetadata(string id)
        {
            // 1. Fetch the existing metadata
            string baseUrl = $"http://localhost:5144/api/File/updateFile/{id}";

            using HttpClient client = new HttpClient();

            try
            {
                HttpResponseMessage res = await client.GetAsync(baseUrl);
                if (!res.IsSuccessStatusCode)
                {
                    Console.WriteLine("File not found.");
                    return;

                }

                var json = await res.Content.ReadAsStringAsync();
                var metadata = JsonSerializer.Deserialize<FileMetaData>(json);

                Console.WriteLine($"Current filename: {metadata.filename}.{metadata.file_type}");
                Console.Write("Enter new file name (without extension): ");
                string newName = Console.ReadLine();

                // 2. Determine folder path based on extension
                string ext = $".{metadata.file_type}";
                string folder = FileHelper.ExtensionFolder[ext];

                // 3. Create old and new file paths
                string oldPath = Path.Combine(folder, metadata.filename + ext);
                string newPath = Path.Combine(folder, newName + ext);

                // 4. Rename file in local storage
                if (File.Exists(oldPath))
                {
                    File.Move(oldPath, newPath);
                }
                else
                {
                    Console.WriteLine("Original file not found in local storage.");
                    return;
                }

                metadata.filename = newName;
                metadata.modified_at = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");

                var updatedJson = new StringContent(
                    JsonSerializer.Serialize(metadata),
                    Encoding.UTF8,
                    "application/json");

                var putResponse = await client.PutAsync($"http://localhost:5000/api/files/{id}", updatedJson);
                if (putResponse.IsSuccessStatusCode)
                    Console.WriteLine("Filename updated successfully!");
                else
                    Console.WriteLine("Failed to update metadata.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
            }
        }

        public async Task DeleteFileAsync(string id) 
        {
            await Task.FromResult(0);
        }

        public void searchData(string input)
        {
            var allFileData = GetData();
        }
    }
}
