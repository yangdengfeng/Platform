using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PkpmGX.Models
{
    public class FileModel
    {
        public Dictionary<string, int> Paths { get; set; }
        public int Modify { get; set; }
        public int Number { get; set; }
    }
}