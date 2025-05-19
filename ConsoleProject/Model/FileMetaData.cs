using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleProject.Model
{
    public class FileMetaData
    {
        public string id { get; set; }
        public string filename { get; set; }
        public string file_type { get; set; }
        public int size { get; set; }
        public string created_at { get; set; }
        public string modified_at { get; set; }
        public string deleted_at { get; set; }
    }
}
