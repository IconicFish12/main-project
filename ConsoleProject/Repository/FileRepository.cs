using ConsoleProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleProject.Repository
{
    internal class FileRepository
    {

        private readonly HttpClient _HttpClient;

        public FileRepository(HttpClient httpClient)
        {
            _HttpClient = httpClient;
        }

        public async Task GetData()
        {
            await Task.FromResult(0);
        }

        public async Task UploadFileAsync(FileMetaData file) { }

        public async Task UpdateFileAsync(string id, FileMetaData updated) { }

        public async Task DeleteFileAsync(string id) { }

    }
}
