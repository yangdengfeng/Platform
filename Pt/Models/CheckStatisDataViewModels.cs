using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PkpmGX.Models
{
    public class CheckStatisDataViewModels
    {
    }

    public class CheckStatisDataSearchModel
    {
        public string Type { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public int? posStart { get; set; }
        public int? count { get; set; }
    }

    public class CheckStatisDataSearchResultModel
    {
        public string CustomId { get; set; }
        public int YearCount { get; set; }
        public int QuarterCount { get; set; }
        public int MonthCount { get; set; }
        public int WeekCount { get; set; }
        public int DayCount { get; set; }
        /// <summary>
        /// 总上传
        /// </summary>
        public int TotalCount { get; set; }
    }

    public class CheckStatisDataGridSearchModel
    {
        public string Type { get; set; }
        public string CustomId { get; set; }
        public string SearchType { get; set; }
        public int? posStart { get; set; }
        public int? count { get; set; }
    }


}