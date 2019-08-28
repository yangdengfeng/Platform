using Pkpm.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PkpmGX.Models
{
    public class InstBaseRecordModels
    {
    }

    public class ZTJGCreateModel
    {
        public string UnitName { get; set; }
        public string BGSCS { get; set; }
        public string BGSCQ { get; set; }
        public string CYSLMZGFYQS { get; set; }
        public string CYSLMZGFYQQ { get; set; }
        public string BGWZZQS { get; set; }
        public string BGWZZQQ { get; set; }
        public string SYYQSBZYXQS { get; set; }
        public string SYYQSBZYXQQ { get; set; }
        public string BGGGFHCXS { get; set; }
        public string BGGGFHCXQ { get; set; }
        public string BHGTZS { get; set; }
        public string BHGTZQ { get; set; }
        public string Remark { get; set; }
        public string CheckPeople { get; set; }
        public DateTime CheckDate { get; set; }
    }

    public class ZTJGEditModel
    {
        public int Id { get; set; }
        public string UnitName { get; set; }
        public string BGSCS { get; set; }
        public string BGSCQ { get; set; }
        public string CYSLMZGFYQS { get; set; }
        public string CYSLMZGFYQQ { get; set; }
        public string BGWZZQS { get; set; }
        public string BGWZZQQ { get; set; }
        public string SYYQSBZYXQS { get; set; }
        public string SYYQSBZYXQQ { get; set; }
        public string BGGGFHCXS { get; set; }
        public string BGGGFHCXQ { get; set; }
        public string BHGTZS { get; set; }
        public string BHGTZQ { get; set; }
        public string Remark { get; set; }
        public string CheckPeople { get; set; }
        public DateTime CheckDate { get; set; }
    }







    public class JbxxDetailsModel
    {
        public InstShortInfos allUnit { get; set; }
        public t_instcheck jbxxCheck { get; set; }
    }

    public class JZJNDetailsModel 
    {
        public InstShortInfos allUnit { get; set; }
        public t_bp_JZJNCheck jzjnCheck { get; set; }
    }


    public class SZDLDetailsModel
    {
        public InstShortInfos allUnit { get; set; }
        public t_bp_SZDLCheck szdlCheck { get; set; }
    }

    public class SLKQDetailsModel
    {
        public InstShortInfos allUnit { get; set; }
        public t_bp_SLKQCheck slkqCheck { get; set; }
    }

    public class ZTJGEditViewModel
    {
        public CheckUnitRecord_MajorStructure Major { get; set; }
        public Dictionary<string, string> allUnit { get; set; }
    }
}