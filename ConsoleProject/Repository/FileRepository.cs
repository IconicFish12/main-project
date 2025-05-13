using ConsoleProject.LayoutServices;
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
        private static readonly string apiStoragePath = @"C:\Users\super\OneDrive\Documents\KULIAHHH\SEMESTER 4\SOFTWARE CONSTRUCTION\Personal Assignment (Code Implementaion)\main-project\main-project\storage";
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

                Console.WriteLine($"Current filename: {metadata.Data.filename}.{metadata.Data.file_type}");
                Console.Write("Enter new file name (without extension): ");
                string newName = Console.ReadLine();

                string ext = $".{metadata.Data.file_type}";
                string folder = FileHelper.ExtensionFolder[ext];
                var path = Path.Combine(apiStoragePath, folder);

                string oldPath = Path.Combine(path, metadata.Data.filename + ext);

                //Console.WriteLine(metadata.Data.id, metadata.Data.filename);
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
                    id = id,
                    filename = newName,
                    file_type = ext,
                    size = metadata.Data.size,
                    created_at = metadata.Data.created_at,
                    modified_at = DateTime.Now.ToString(),
                    deleted_at = null,
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
            string baseUrl = $"http://localhost:5144/api/File/delete/{id}";

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


        public static async Task SearchData(string input)
        {
            var allFileData = await FileRepository.GetData();

            if (allFileData == null || allFileData.Count == 0)
            {
                Console.WriteLine("No data available.");
                return;
            }

            var matchItem = allFileData
                .Where(item => item.filename != null && item.filename.ToLower().Contains(input.ToLower()))
                .ToList();

            if (matchItem.Count == 0)
            {
                Console.WriteLine("No matching data found.");
                return;
            }

            Console.WriteLine("Matching File Metadata:");
            Console.WriteLine("==================================================================================================================");
            Console.WriteLine("| No |                ID                |         File Name         | File Type | File Size | Created At |");
            Console.WriteLine("==================================================================================================================");

            for (int i = 0; i < matchItem.Count; i++)
            {
                var item = matchItem[i];
                Console.WriteLine($"| {i + 1,2} | {item.id,-32} | {item.filename,-25} | {item.file_type,-9} | {item.size,9} | {item.created_at,-10} |");
            }

            Console.WriteLine("==================================================================================================================");
        }

        public static async Task<string> SearchDataForId()
        {
            Console.WriteLine("------------------------------");
            Console.Write("Enter keyword to search file: ");
            var keyword = InputStream.GetString(Console.ReadLine()).ToLower();
            Console.WriteLine("------------------------------");

            var allFileData = await FileRepository.GetData();

            if (allFileData == null || allFileData.Count == 0)
            {
                Console.WriteLine("No data available.");
                return null;
            }

            var matchItems = allFileData
                .Where(item => item.filename != null && item.filename.ToLower().Contains(keyword))
                .ToList();

            if (matchItems.Count == 0)
            {
                Console.WriteLine("No matching data found.");
                return null;
            }

            Console.WriteLine("==================================================================================================================");
            Console.WriteLine("| No |                ID                |         File Name         | File Type | File Size | Created At |");
            Console.WriteLine("==================================================================================================================");

            for (int i = 0; i < matchItems.Count; i++)
            {
                var item = matchItems[i];
                Console.WriteLine($"| {i + 1,2} | {item.id,-32} | {item.filename,-25} | {item.file_type,-9} | {item.size,9} | {item.created_at,-10} |");
            }

            Console.WriteLine("==================================================================================================================");

            int selectedIndex = InputStream.GetInt("Select file number to continue (0 to cancel): ");
            if (selectedIndex <= 0 || selectedIndex > matchItems.Count)
            {
                Console.WriteLine("Operation cancelled.");
                return null;
            }

            var selectedItem = matchItems[selectedIndex - 1];
            Console.WriteLine($"You selected: {selectedItem.filename} (ID: {selectedItem.id})");

            return selectedItem.id;
        }

    }
}
