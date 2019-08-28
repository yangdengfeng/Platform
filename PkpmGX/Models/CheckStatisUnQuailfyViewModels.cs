using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PkpmGX.Models
{
    public class AreaResultModel
    {
        public string Name { get; set; }
        public string AreaCode { get; set; }
        public int TotalCount { get; set; }
        public int UnqualifuCount { get; set; }
        public int FactoryCount { get; set; } //生产厂家总数
        public int UnqualifyFactoryCount { get; set; }
        public string ParentCode { get; set; }
    }
}