using Pkpm.Entity.DTO;
using Pkpm.Entity.ElasticSearch;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pkpm.Core
{
    public interface IAcsChartService
    {
        AcsChart BuildAcsChart(es_t_bp_acs acsItem, Stream s);
    }
    public class AcsChartService : IAcsChartService
    {
        public AcsChart BuildAcsChart(es_t_bp_acs acsItem, Stream s)
        {
            if (acsItem == null || string.IsNullOrWhiteSpace(acsItem.ACSDATAPATH))
                return new AcsChart();

            if (string.IsNullOrWhiteSpace(acsItem.DATATYPES))
            {
                //Remark : 根据阿莫，没有默认为A
                acsItem.DATATYPES = "A";
            }

            byte[] inputData = StreamToBytes(s);

            if (acsItem.SYSPRIMARYKEY.Substring(2, 1) == "1")
            {
                return ChartForSpecial(acsItem, inputData);
            }
            else if (Encoding.Default.GetString(inputData).StartsWith("Create"))
            {
                return ChartForXinGao(acsItem, inputData);
            }
            else if (acsItem.DATATYPES == "B")
            {
                return ChartForTypeB(acsItem, inputData);
            }
            else if (acsItem.DATATYPES == "A")
            {
                return ChartForTypeA(acsItem, inputData);
            }
            else if (acsItem.DATATYPES == "I" || acsItem.DATATYPES == "D")
            {
                return ChartForTypeIAndD(acsItem, inputData);
            }
            else
            {
                return new AcsChart();
            }
        }

        private AcsChart ChartForSpecial(es_t_bp_acs acsItem, byte[] inputData)
        {
            if (acsItem == null || inputData == null)
            {
                return new AcsChart();
            }

            AcsChart acsChart = new AcsChart();
            acsChart.ChartItems = new List<AcsChartItem>();
            string inputStr = Encoding.Default.GetString(inputData);
            var allRecords = inputStr.Split(new string[] { ";", "\0;" }, StringSplitOptions.RemoveEmptyEntries);
            int idInt = 1;

            //js version
            //for (var i = 1; i < strAr.length; i = i + 2)
            //{
            //    var a = strAr[i].split(",");
            //    if (a[0] == "" || a[1] == "" || isNaN(a[0]) || isNaN(a[1])) continue;
            //    timeAr.push(a[0]);
            //    valueAr.push(a[1]);
            //}

            for (int i = 1; i < allRecords.Length; i = i + 2)
            {
                var oneRecord = allRecords[i].Split(new string[] { ",", "\0," }, StringSplitOptions.RemoveEmptyEntries);
                if (oneRecord.Length > 2)
                {
                    double xTime = 0;
                    double yValue = 0;
                    if (double.TryParse(oneRecord[0].Replace("\0", string.Empty), out xTime)
                        && double.TryParse(oneRecord[1].Replace("\0", string.Empty), out yValue))
                    {
                        acsChart.ChartItems.Add(new AcsChartItem() { Id = idInt, xTime = Math.Round(xTime, 2), yValue = yValue });
                        idInt++;
                    }
                }
            }
            return acsChart;
        }

        private AcsChart ChartForXinGao(es_t_bp_acs acsItem, byte[] inputData)
        {
            if (acsItem == null || inputData == null)
            {
                return new AcsChart();
            }

            AcsChart acsChart = new AcsChart();
            acsChart.ChartItems = new List<AcsChartItem>();
            string inputStr = Encoding.Default.GetString(inputData);
            var allRecords = inputStr.Split(new string[] { "\0\r\0\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            int idInt = 1;

            //js version
            //for (var i = 0; i < strAr.length; i++)
            //{
            //    if (i < 4) continue;
            //    if (strAr[i].indexOf(" ") == -1) continue;
            //    if (strAr[i].toLowerCase().indexOf("stop") != -1)
            //    {
            //        break;
            //    }
            //    var a = strAr[i].split(" ");
            //    timeAr.push(a[0]);
            //    valueAr.push(a[1]);

            for (int i = 0; i < allRecords.Length; i++)
            {
                if (i < 4)
                {
                    continue;
                }
                if (allRecords[i].Contains("stop"))
                {
                    break;
                }

                var oneRecord = allRecords[i].Split(new string[] { " ", "\0 " }, StringSplitOptions.RemoveEmptyEntries);
                if (oneRecord.Length >= 2)
                {
                    double xTime = 0;
                    double yValue = 0;
                    if (double.TryParse(oneRecord[0].Replace("\0", string.Empty), out xTime)
                        && double.TryParse(oneRecord[1].Replace("\0", string.Empty), out yValue))
                    {
                        acsChart.ChartItems.Add(new AcsChartItem() { Id = idInt, xTime = Math.Round(xTime, 2), yValue = yValue });
                        idInt++;
                    }
                }
            }

            return acsChart;


            //}
        }

        private AcsChart ChartForTypeIAndD(es_t_bp_acs acsItem, byte[] inputData)
        {
            if (acsItem == null || inputData == null)
            {
                return new AcsChart();
            }

            //ASCII的UITF16编码，高位为0

            AcsChart acsChart = new AcsChart();
            acsChart.ChartItems = new List<AcsChartItem>();
            string inputStr = Encoding.Default.GetString(inputData);
            var allRecords = inputStr.Split(new string[] { "\0\r\0\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            int idInt = 1;

            for (int i = 0; i < allRecords.Length - 1; i++)
            {
                var oneRecord = allRecords[i].Split(new string[] { "\0#" }, StringSplitOptions.RemoveEmptyEntries);
                if (oneRecord.Length < 2)
                {
                    oneRecord = allRecords[i].Split(new string[] { "#" }, StringSplitOptions.RemoveEmptyEntries);
                }
                if (oneRecord.Length >= 2)
                {
                    double xTime = 0;
                    double yValue = 0;
                    if (double.TryParse(oneRecord[0].Replace("\0", string.Empty), out xTime)
                        && double.TryParse(oneRecord[1].Replace("\0", string.Empty), out yValue))
                    {
                        acsChart.ChartItems.Add(new AcsChartItem() { Id = idInt, xTime = Math.Round(xTime, 2), yValue = yValue });
                        idInt++;
                    }

                }

            }



            return acsChart;
        }

        private AcsChart ChartForTypeB(es_t_bp_acs acsItem, byte[] inputData)
        {
            if (acsItem == null || inputData == null)
            {
                return new AcsChart();
            }

            AcsChart acsChart = new AcsChart();
            acsChart.ChartItems = new List<AcsChartItem>();


            // js version
            //for (var i = 2; i < strAr.length - 1; i++)
            //{
            //    var a = strAr[i].split("\t");
            //    timeAr.push(a[3]);
            //    valueAr.push(a[0]);
            //}
            string inputStr = Encoding.Default.GetString(inputData);
            var allRecords = inputStr.Split(new string[] { "\0\r\0\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            int idInt = 1;
            if (allRecords.Length >= 2)
            {
                for (int i = 2; i < allRecords.Length - 1; i++)
                {
                    var oneRecord = allRecords[i].Split(new string[] { "\t", "\0\t" }, StringSplitOptions.RemoveEmptyEntries);
                    if (oneRecord.Length > 4)
                    {
                        double xTime = 0;
                        double yValue = 0;
                        if (double.TryParse(oneRecord[3].Replace("\0", string.Empty), out xTime)
                            && double.TryParse(oneRecord[0].Replace("\0", string.Empty), out yValue))
                        {
                            acsChart.ChartItems.Add(new AcsChartItem() { Id = idInt, xTime = Math.Round(xTime, 2), yValue = yValue });
                            idInt++;
                        }

                    }

                }
            }


            return acsChart;
        }

        private byte[] StreamToBytes(Stream stream)
        {
            MemoryStream ms = new MemoryStream();
            stream.CopyTo(ms);

            return ms.ToArray();
        }

        private string StreamToStr(Stream stream)
        {
            if (stream == null)
            {
                return string.Empty;
            }

            MemoryStream ms = new MemoryStream();
            stream.CopyTo(ms);

            string str = Encoding.Default.GetString(ms.ToArray());
            return str;
        }


        private AcsChart ChartForTypeA(es_t_bp_acs acsitem, byte[] item)
        {
            double a = 0;
            double b = 0;
            if (!double.TryParse(acsitem.A, out a) || !double.TryParse(acsitem.B, out b))
            {
                return new AcsChart();
            }

            double uplaodMaxY = 0;
            if (!double.TryParse(acsitem.MAXVALUE, out uplaodMaxY))
            {
                uplaodMaxY = 0;
            }

            AcsChart acsChart = new AcsChart();
            acsChart.ChartItems = new List<AcsChartItem>();

            int dataLength = item.Length;
            int bc = 4;
            int idInt = 1;
            for (int k = 0; k < dataLength; k = k + bc)
            {

                string t = string.Empty;
                try
                {
                    t = Encoding.Default.GetString(item).Substring(k, 4);
                }
                catch (Exception) { }

                if (string.IsNullOrWhiteSpace(t))
                {
                    break;
                }

                double xValue = (k / bc * (bc / 4) * 0.1);
                double yValue = a + b * Microsoft.JScript.GlobalObject.parseInt(t, 16);
                if (yValue < 0)
                {
                    yValue = 0;
                }

                acsChart.ChartItems.Add(new AcsChartItem() { Id = idInt, xTime = Math.Round(xValue, 2), yValue = yValue });
                idInt++;
            }

            double chartMaxY = acsChart.ChartItems.Max(cf => cf.yValue);
            if (uplaodMaxY > chartMaxY)
            {
                FixChartYMaxLessThanUplaodMax(uplaodMaxY, acsChart);
            }

            return acsChart;
        }

        private static void FixChartYMaxLessThanUplaodMax(double uplaodMaxY, AcsChart acsChart)
        {
            double chartMaxY = acsChart.ChartItems.Max(cf => cf.yValue);

            AcsChartItem maxItem = acsChart.ChartItems.Where(c => c.yValue == chartMaxY).FirstOrDefault();
            List<AcsChartItem> incrCharts = acsChart.ChartItems.Where(c => c.Id <= maxItem.Id).ToList();
            List<AcsChartItem> descCharts = acsChart.ChartItems.Where(c => c.Id > maxItem.Id).ToList();

            AcsChartItem beforeMaxItem = acsChart.ChartItems.Where(c => c.Id == (maxItem.Id - 1)).FirstOrDefault();

            double incrValue = 0.5;
            if (beforeMaxItem != null)
            {
                incrValue = maxItem.yValue - beforeMaxItem.yValue;
            }

            double incrTime = 0.1;
            if (beforeMaxItem != null)
            {
                incrTime = maxItem.xTime - beforeMaxItem.xTime;
            }

            double fixChartMaxY = chartMaxY;

            while (fixChartMaxY + incrValue < uplaodMaxY)
            {
                var lastChartItem = incrCharts.Last();

                incrCharts.Add(new AcsChartItem()
                {
                    Id = lastChartItem.Id + 1,
                    xTime = Math.Round(lastChartItem.xTime + incrTime),
                    yValue = lastChartItem.yValue + incrValue
                });

                fixChartMaxY += incrValue;
            }

            incrCharts.Add(new AcsChartItem()
            {
                Id = incrCharts.Last().Id + 1,
                yValue = uplaodMaxY,
                xTime = Math.Round(incrCharts.Last().xTime + incrTime)
            });

            double totalAddXTime = incrCharts.Last().xTime - maxItem.xTime;
            int totalAddIdInt = incrCharts.Last().Id - maxItem.Id;

            for (int i = 0; i < descCharts.Count; i++)
            {
                descCharts[i].Id = descCharts[i].Id + totalAddIdInt;
                descCharts[i].xTime = Math.Round(descCharts[i].xTime + totalAddXTime);
            }

            incrCharts.AddRange(descCharts);
            acsChart.ChartItems = incrCharts;
        }

    }
}
