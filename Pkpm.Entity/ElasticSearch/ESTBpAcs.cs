using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Pkpm.Entity.ElasticSearch
{
    
    [Serializable]
    public class es_t_bp_acs
    { 
        public string CUSTOMID { get; set; } 
       
        public string UNITCODE { get; set; } 
       
        public string SYSPRIMARYKEY { get; set; } 
      
        public string ITEMTABLENAME { get; set; }

        public string SAMPLENUM { get; set; } 
       
        public int? MAXLC { get; set; }

        public string TIMES { get; set; } 
       
        public string MAXVALUE { get; set; } 
       
        public string QFVALUE { get; set; } 

        public DateTime? ACSTIME { get; set; } 
      
        public string DATATYPES { get; set; } 
      
        public string A { get; set; } 
     
        public string B { get; set; } 
      
        public string A1 { get; set; } 
        
        public string B1 { get; set; } 
       
        public string A2 { get; set; } 
      
        public string B2 { get; set; }
        
        public string YSN { get; set; }

        public string OPERATIONUSERNUM { get; set; }

        public string ISUPLOADED { get; set; }

        public string BZC { get; set; }

        public string BYXS { get; set; }

        public string PJZ { get; set; }

        public string CJSJ { get; set; }

        public string XGXS { get; set; }

        public string DGSJPD { get; set; }

        public string HQFVALUE { get; set; }

        public string DHBJ { get; set; }

        public string MACNUM { get; set; }


        public string CHECKMAN { get; set; }

     
        public string INSTRUMENTNUM { get; set; }

       
        public string INSTRUMENTNAME { get; set; }

        
        public string SPEED { get; set; }

      
        public string ACSDATAPATH { get; set; }

       
        public string PK { get; set; }

      
        public DateTime? UPLOADTIME { get; set; }

       
        public DateTime? UPDATETIME { get; set; }

       
    }
}
