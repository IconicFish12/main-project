using main_project.Services.Helper;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WebFrontend.Repository.Model;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WebFrontend.Repository
{
    public class FileRepository
    {
        private static readonly string apiStoragePath = @"C:\Users\super\OneDrive\Documents\KULIAHHH\SEMESTER 4\SOFTWARE CONSTRUCTION\Personal Assignment (Code Implementaion)\main-project\main-project\storage";
        public static async Task<List<FileMetaData>?> GetData()
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

                    if (jsonRespons?.AllData == null && jsonRespons == null) return null;

                    listData.AddRange(jsonRespons.AllData);
                }
            }
            catch (Exception ex) 
            {
                Console.WriteLine($"Exception: {ex.Message}");
            }

            return listData;
        }

        public static async Task UploadFileAsync(IBrowserFile browserFile)
        {
            string uploadUrl = "http://localhost:5144/api/File/createFile";
            using HttpClient client = new HttpClient();
            using var content = new MultipartFormDataContent();

            if (browserFile != null)
            {
                var stream = browserFile.OpenReadStream(maxAllowedSize: 1024 * 1024 * 10); // 10MB max
                var streamContent = new StreamContent(stream);
                streamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(browserFile.ContentType);
                content.Add(streamContent, "file", browserFile.Name);

                HttpResponseMessage response = await client.PostAsync(uploadUrl, content);
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("File uploaded successfully!");
                }
                else
                {
                    Console.WriteLine($"Failed to upload file. Status: {response.StatusCode}");
                }
            }
            else
            {
                Console.WriteLine("No file selected for upload.");
            }
        }

        public static async Task RenameFileMetadata(string id)
        {
            string updateUrl = $"http://localhost:5144/api/File/updateMetadata/{id}";
            string getFileUrl = $"http://localhost:5144/api/File/getFile/{id}";

            using HttpClient client = new HttpClient();

            try
            {
                HttpResponseMessage res = await client.GetAsync(getFileUrl);
                if (!res.IsSuccessStatusCode)
                {
                    Console.WriteLine("File not found.");
                    return;

                }

                var json = await res.Content.ReadAsStringAsync();
                var metadata = JsonSerializer.Deserialize<ApiRespons>(json);

                Console.WriteLine($"Current filename: {metadata?.Data.Filename}.{metadata?.Data.File_type}");
                Console.Write("Enter new file name (without extension): ");
                string newName = Console.ReadLine();

                string ext = $".{metadata?.Data.File_type}";
                string folder = FileHelper.ExtensionFolder[ext];
                var path = Path.Combine(apiStoragePath, folder);

                string oldPath = Path.Combine(path, metadata?.Data.Filename + ext);

                //Console.WriteLine(metadata?.Data.id, metadata?.Data.filename);
                //return;

                string? newPath = Path.Combine(path, newName + ext);

                if (File.Exists(oldPath))
                {
                    File.Move(oldPath, newPath);
                } 
                else
                {
                    Console.WriteLine("Original file not found in local storage.");
                    return;
                }

                var updatedData = new FileMetaData
                {
                    Id = id,
                    Filename = newName,
                    File_type = metadata?.Data.File_type,
                    Size = metadata?.Data.Size,
                    Created_at = metadata?.Data.Created_at,
                    Modified_at = DateTime.Now.ToString(),
                    Deleted_at = null,
                };

                var updatedJson = new StringContent(
                    JsonSerializer.Serialize(updatedData),
                    Encoding.UTF8,
                    "application/json");

                var putResponse = await client.PutAsync(updateUrl, updatedJson);
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

        public static async Task DeleteFileById(string id)
        {
            string baseUrl = $"http://localhost:5144/api/File/deleteFile/{id}";

            using HttpClient client = new HttpClient();

            try
            {
                HttpResponseMessage res = await client.DeleteAsync(baseUrl);

                if (res.IsSuccessStatusCode)
                {
                    Console.WriteLine("File dan metadata berhasil untuk dihapus.");
                }
                else
                {
                    Console.WriteLine($"gagal untuk menghapus file. Status: {res.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred while deleting: {ex.Message}");
            }
        }

        public static async Task<List<FileMetaData>> SearchData(string input)
        {
            var allFileData = await GetData();

            if (allFileData == null || allFileData.Count == 0)
            {
                return new List<FileMetaData>(); // Return an empty list if no data is available
            }

            var matchItem = allFileData
                .Where(item => item.Filename != null && item.Filename.ToLower().Contains(input.ToLower()))
                .ToList();

            return matchItem; 
        }

        public static async Task<FileMetaData?> SearchDataForId(string input)
        {
            var allFileData = await GetData();

            if (allFileData == null || allFileData.Count == 0)
            {
                return null; 
            }

            var matchItems = allFileData
                .Where(item => item.Filename != null && item.Filename.ToLower().Contains(input.ToLower()))
                .ToList();

            if (matchItems.Count == 0)
            {
                return null; 
            }

            return matchItems.FirstOrDefault(); 
        }

    }
}
