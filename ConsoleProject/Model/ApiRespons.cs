using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ConsoleProject.Model
{
    public class ApiRespons
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonPropertyName("allData")]
        public List<FileMetaData> AllData { get; set; }

        [JsonPropertyName("data")]
        public FileMetaData Data { get; set; }
    }
}
