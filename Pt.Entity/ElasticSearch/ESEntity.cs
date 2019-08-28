using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pkpm.Entity.ElasticSearch
{
    [Serializable]
    public class ESJCVersion
    {
        public string Id { get; set; }
        public string CUSTOMID { get; set; }
        public string FILEVERSION { get; set; }
        public string DATABASEVERSION { get; set; }
        public DateTime? UPDATEATE { get; set; }
    }
}
