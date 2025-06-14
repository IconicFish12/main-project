using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebFrontend.Repository.Model
{
    public class FileMetaData
    {
        public string? Id { get; set; }
        public string? Filename { get; set; }
        public string? File_type { get; set; }
        public int? Size { get; set; }
        public string? Created_at { get; set; }
        public string? Modified_at { get; set; }
        public string? Deleted_at { get; set; }
    }
}
