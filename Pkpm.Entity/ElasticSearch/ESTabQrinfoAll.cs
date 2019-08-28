using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pkpm.Entity.ElasticSearch
{
    [Serializable]
    public class es_tab_qrinfo_all
    {
        public string QRINFO { get; set; }
        public string AREAINFO { get; set; }
        public int PCID { get; set; }
        public int NUM { get; set; }
        public int STATUS { get; set; }
    }

    [Serializable]
    public class es_exists_qrinfo
    {
        public string QRINFO { get; set; }
    }
}
