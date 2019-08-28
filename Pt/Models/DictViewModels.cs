using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PkpmGX.Models
{
    public class SearchDictViewModel
    {
        public string DictName { get; set; }
    }

    public class NewDictViewModel : SearchDictViewModel
    {
        public string KeyValue { get; set; }
        public int DictOrderNo { get; set; }
        public int CategoryId { get; set; }
        public int DictStatus { get; set; }
    }

    public class EditDictViewModel : NewDictViewModel
    {
        public int SysDictId { get; set; }
    }
}