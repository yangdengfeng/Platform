using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pkpm.Entity.ElasticSearch
{
    [Serializable]
    public class es_t_bp_item_list
    {
        public string CUSTOMID { get; set; }
        public string UNITCODE { get; set; }
        public string ITEMTABLENAME { get; set; }
        public string ITEMCHNAME { get; set; }
        public string ITEMTYPES { get; set; }
        public string ITEMSTANDARDS { get; set; }
        public int? REPORTCONLINES { get; set; }
        public int? RECORDCONLINES { get; set; }
        public int? ENTRFORMCONLINES { get; set; }
        public int? REPORTORGI { get; set; }
        public int? RECORDORGI { get; set; }
        public string TESTEQUIPMENTNUM { get; set; }
        public string TESTEQUIPMENTNAME { get; set; }
        public int? REPORTCONSENTDAYS { get; set; }
        public int? REPORTCONSENTCWORKDAYS { get; set; }
        public string KEYS { get; set; }
        public int? USEFREQ { get; set; }
        public int? CHECKMANCOUNT { get; set; }
        public string PRINTDATEMODE { get; set; }
        public string STANDARDNAMEMODE { get; set; }
        public int? ISHAVEGJ { get; set; }
        public int? ONLYMANAGEREPORT { get; set; }
        public int? CHECKDATESTYLE { get; set; }
        public string ISUSE { get; set; }
        public int? CHECKDAYS { get; set; }
        public int? REPORTDAYS { get; set; }
        public string JX_LBDM { get; set; }
        public string JX_ITEMCODE { get; set; }
        public string JX_LB { get; set; }
        public string JX_ITEMCHNAME { get; set; }
        public string HEBEI_ITEMCODE { get; set; }
        public string HEBEI_TABLENAME { get; set; }
        public string HEBEI_ITEMCHNAME { get; set; }
        public DateTime? STARTTIME { get; set; }
        public DateTime? VALIDDATE { get; set; }
        public string PDCOLUMNS { get; set; }
        public DateTime? UPLOADTIME { get; set; }
        public DateTime? UPDATETIME { get; set; }
    }
}
