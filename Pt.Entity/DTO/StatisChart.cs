using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pkpm.Entity.DTO
{
    public class StatisChart
    {
        public StatisChart()
        {
            this.StatisChartItems = new List<DTO.StatisChartItem>();
        }

        public List<StatisChartItem> StatisChartItems { get; set; }
    }

    public class StatisChartItem
    {
        public string StatisKey { get; set; }

        public string StatisName { get; set; }

        public long DocCount { get; set; }
    }
}
