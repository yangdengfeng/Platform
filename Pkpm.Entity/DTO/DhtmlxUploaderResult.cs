using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pkpm.Entity.DTO
{
    public class DhtmlxUploaderResult
    {
        public bool state { get; set; }
        public string name { get; set; }
    }

    public class UMEditorResult
    {
        public string state { get; set; }
        public string url { get; set; }
        public string originalName { get; set; }
        public string name { get; set; }
        public int size { get; set; }
        public string type { get; set; }

    }
}
