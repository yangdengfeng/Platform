using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pkpm.Entity.DTO
{
    public class AcsChart
    {
        public AcsChart()
        {
            this.ChartItems = new List<AcsChartItem>();
            xMaxValue = string.Empty;
            yMaxValue = string.Empty;
        }

        public string yMaxValue { get; set; }
        public string xMaxValue { get; set; }
        public string acsTime { get; set; }
        public string yAxis { get; set; }
        public List<AcsChartItem> ChartItems { get; set; }
    }

    public class AcsChartItem
    {
        public int Id { get; set; }
        public double xTime { get; set; }
        public double yValue { get; set; }
    }
}
