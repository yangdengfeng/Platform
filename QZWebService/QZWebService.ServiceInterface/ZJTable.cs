using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QZWebService.ServiceInterface
{
    public class ZJTable
    {
        public DataTable dt = null;
        public StringBuilder sb = null;
        private string colrOne;

        private string trOneClass;
        /// <summary>
        /// 行1颜色
        /// </summary>
        public string TrOneClass
        {
            get { return trOneClass; }
            set { trOneClass = value; }
        }

        private string trTwoClass;
        /// <summary>
        /// 行2颜色
        /// </summary>
        public string TrTwoClass
        {
            get { return trTwoClass; }
            set { trTwoClass = value; }
        }

        private string colorMouse;

        public string ColorMouse
        {
            get { return colorMouse; }
            set { colorMouse = value; }
        }

        private string[] widthString;
        public string[] WidthString
        {
            get { return widthString; }
            set { widthString = value; }
        }

        private string[] alignString;
        public string[] AlignString
        {
            get { return alignString; }
            set { alignString = value; }
        }

        private string TrClass(string classValue)
        {
            return "class=\"" + classValue + "\"";
        }

        private string MouseColor(string color)
        {
            if (color != null)
            {
                return "onmouseover=\"c=this.style.backgroundColor;this.style.backgroundColor='" + color + "'\" onmouseout=\"this.style.backgroundColor=c;\"";
            }
            return "";
        }

        private string AlignStringAdd()
        {
            return "align=\"center\"";
        }

        private string WidthStringAdd(string tdWidth)
        {
            return "width=\"" + tdWidth + "\"";
        }

        public string CreateJyTableColumn(DataTable dt, int n)
        {
            sb = new StringBuilder();
            for (int i = n; i < dt.Columns.Count; i++)
            {
                sb.Append("<td align=\"center\">");
                string cName = "";
                if (dt.Columns[i].ColumnName.IndexOf("通道") > -1)
                {
                    cName = dt.Columns[i].ColumnName + "（mm）";
                }
                else if (dt.Columns[i].ColumnName.IndexOf("沉降") > -1)
                {
                    cName = dt.Columns[i].ColumnName + "（mm）";
                }
                else
                {
                    cName = dt.Columns[i].ColumnName;
                }
                sb.Append(cName);
                sb.Append("</td>");
            }
            return sb.ToString();
        }

        public string CreateJyTable(DataTable dt, int type)
        {
            sb = new StringBuilder();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (trOneClass != null && trTwoClass != null)
                {
                    if (i % 2 == 0)
                    {
                        sb.Append("<tr " + TrClass(trOneClass) + " " + MouseColor(colorMouse) + " >");
                    }
                    else
                    {
                        sb.Append("<tr " + TrClass(trTwoClass) + " " + MouseColor(colorMouse) + " >");
                    }
                }
                if (TrOneClass != null && trTwoClass == null)
                {
                    sb.Append("<tr " + TrClass(trOneClass) + " " + MouseColor(colorMouse) + " >");
                }
                if (TrOneClass == null && TrTwoClass != null)
                {
                    sb.Append("<tr " + TrClass(trTwoClass) + " " + MouseColor(colorMouse) + " >");
                }
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    if (widthString == null)
                    {
                        sb.Append("<td " + AlignStringAdd() + " >");
                    }
                    else
                    {
                        sb.Append("<td " + WidthStringAdd(widthString[j]) + " " + AlignStringAdd() + ">");
                    }
                    if (type == 0)
                    {
                        if (dt.Columns[j].ColumnName == "理论荷载" || dt.Columns[j].ColumnName == "实测荷载")
                        {
                            sb.Append(doubleTwo(dt.Rows[i][j], 0));
                        }
                        else if (dt.Columns[j].ColumnName == "实测油压")
                        {
                            sb.Append(doubleTwo(dt.Rows[i][j], 2));
                        }
                        else if (dt.Columns[j].ColumnName.IndexOf("位移通道") > -1 || dt.Columns[j].ColumnName == "平均沉降" || dt.Columns[j].ColumnName == "本级沉降" || dt.Columns[j].ColumnName == "累计沉降" || dt.Columns[j].ColumnName == "本次沉降")
                        {
                            sb.Append(doubleTwo(dt.Rows[i][j], 2));
                        }
                        else
                        {
                            sb.Append(dt.Rows[i][j].ToString());
                        }
                    }
                    else if (type == 1)
                    {
                        if (dt.Columns[j].ColumnName != "时间")
                        {
                            if (dt.Rows[i][j] == DBNull.Value)
                            {
                                sb.Append("");
                            }
                            else
                            {
                                sb.Append(doubleTwo(dt.Rows[i][j], 2));
                            }
                        }
                        else
                        {
                            sb.Append(dt.Rows[i][j].ToString());
                        }
                    }
                    sb.Append("</td>");
                }
                sb.Append("</tr>\n");
            }
            return sb.ToString();
        }

        private string doubleTwo(object obj, int count)
        {
            string num = "F" + count;
            return string.Format("{0:" + num + "}", Math.Round(Convert.ToDouble(obj), count));
        }

        public string CreateGcTable(DataTable dt)
        {
            sb = new StringBuilder();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (trOneClass != null && trTwoClass != null)
                {
                    if (i % 2 == 0)
                    {
                        sb.Append("<tr " + TrClass(trOneClass) + " " + MouseColor(colorMouse) + " >");
                    }
                    else
                    {
                        sb.Append("<tr " + TrClass(trTwoClass) + " " + MouseColor(colorMouse) + " >");
                    }
                }
                if (TrOneClass != null && trTwoClass == null)
                {
                    sb.Append("<tr " + TrClass(trOneClass) + " " + MouseColor(colorMouse) + " >");
                }
                if (TrOneClass == null && TrTwoClass != null)
                {
                    sb.Append("<tr " + TrClass(trTwoClass) + " " + MouseColor(colorMouse) + " >");
                }
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    if (widthString == null)
                    {
                        sb.Append("<td " + AlignStringAdd() + " >");
                    }
                    else
                    {
                        sb.Append("<td " + WidthStringAdd(widthString[j]) + " " + AlignStringAdd() + ">");
                    }
                    sb.Append(dt.Rows[i][j].ToString());
                    sb.Append("</td>");
                }
                sb.Append("</tr>");
            }
            return sb.ToString();
        }
    }
}
