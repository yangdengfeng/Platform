using Pkpm.Core.ItemNameCore;
using Pkpm.Entity.DTO;
using Pkpm.Entity.ElasticSearch;
using Pkpm.Framework.Common;
using Pkpm.Framework.Repsitory;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pkpm.Core.QrCodeCore
{
    public interface IReportQrCode
    {
        ReportInfo Decode(string qrinfo);

        byte[] Encode(string sysPrimaryKey, string qrInfo);
    }
    public class ReportQrCode : IReportQrCode
    {
        IItemNameService itemNameService;
        IESRepsitory<es_t_bp_item> reportRep;
        string defalutStr;

        public ReportQrCode(IItemNameService itemNameService,
          IESRepsitory<es_t_bp_item> reportRep)
        {
            this.itemNameService = itemNameService;
            this.reportRep = reportRep;
            this.defalutStr = "(空)";
        }
        public ReportInfo Decode(string qrinfo)
        {
            int key = 0;
            try
            {
                key = Convert.ToInt32(qrinfo.Substring(qrinfo.Length - 3, 3));
                key = key - 256;
            }
            catch (Exception)
            {
                return null;
            }
            string values = qrinfo.Substring(0, qrinfo.Length - 3);
            values = Reverse(values);
            string info = UnicodeToString(StringToUnicode(values, key));
            ReportInfo result = null;
            if (info.IndexOf("|") != -1)
            {
                result = new ReportInfo();
                string[] str = info.Split('|');
                for (int i = 0; i < str.Length; i++)
                {
                    switch (i)
                    {
                        case 0:
                            result.ReportNum = str[i];
                            break;
                        case 1:
                            result.ProjectName = str[i];
                            break;
                        case 2:
                            result.StructPart = str[i];
                            break;
                        case 3:
                            result.CheckItem = str[i];
                            break;
                        case 4:
                            result.CheckParam = str[i];
                            break;
                        case 5:
                            if (!str[i].Equals("C") && !str[i].Equals(""))
                            {
                                result.Conclusion = "详见" + result.ReportNum + "报告";
                            }
                            else
                            {
                                result.Conclusion = "详见" + str[i] + "报告";
                            }
                            break;
                        case 6:
                            result.ReportDate = str[i];
                            break;
                        case 7:
                            result.AntiFakeLabel = str[i];
                            break;
                        case 8:
                            result.ItemCode = str[i];
                            break;
                        case 9:
                            result.UnitCode = str[i];
                            break;
                        default:
                            break;
                    }
                }
            }
            return result;
        }

        public byte[] Encode(string sysPrimaryKey, string qrInfo)
        {
            List<string> encodes = new List<string>();
            var reportResponse = reportRep.Get(new Nest.DocumentPath<es_t_bp_item>(sysPrimaryKey));
            string result = string.Empty;
            string reportNum = string.Empty;
            if (reportResponse.IsValid && reportResponse.Source != null)
            {
                var reportItem = reportResponse.Source;

                reportNum = reportItem.REPORTNUM;

                encodes.Add(GetEncodeString(reportItem.REPORTNUM));    //报告编号
                encodes.Add(GetEncodeString(reportItem.PROJECTNAME));  //工程名称
                encodes.Add(GetEncodeString(reportItem.STRUCTPART));  //工程部位
                string itemName = reportItem.ITEMCHNAME;
                string parmName = string.Empty;
                //var subItem = itemNameService.GetItemByItemCode(reportItem.SUBITEMCODE);
                //if (subItem != null)
                //{
                //    itemName = subItem.itemname;
                //}
                //if (itemName.IsNullOrEmpty())
                //{
                //    itemName = reportItem.SUBITEMCODE;
                //}

                //var parmItem = itemNameService.GetItemByParamCode(reportItem.PARMCODE);
                //if (parmItem != null)
                //{
                //    parmName = parmItem.parmname;
                //}
                //if (parmName.IsNullOrEmpty())
                //{
                //    parmName = reportItem.PARMCODE;
                //}

                encodes.Add(GetEncodeString(itemName));    //CheckItem
                encodes.Add(GetEncodeString(parmName));            //CheckParam
                encodes.Add(GetConclusion(reportItem.CONCLUSIONCODE));                //检测结论
                //报告日期
                if (reportItem.PRINTDATE.HasValue)
                {
                    encodes.Add(reportItem.PRINTDATE.Value.ToString("yyyy年MM月dd日"));
                }
                else
                {
                    encodes.Add(defalutStr);
                }
                //encodes.Add(GetEncodeString(reportItem.QRCODEBAR)); //见证取样二维码
                //sencodes.Add(GetEncodeString(reportItem.ITEMNAME));  //检测项目代号
                if (reportItem.CODEBAR.IsNullOrEmpty())
                {
                    encodes.Add(GetEncodeString("委托送检")); //防伪标记 
                }
                else
                {
                    encodes.Add(GetEncodeString("见证取样")); //防伪标记 
                }

            }

            string encodeStr = string.Join("|", encodes) + "|";
            result = ConvertToUnicode(encodeStr);
            string content = Of_stringToHex(result, ToInt(reportNum)) + (ToInt(reportNum) + 256).ToString();

            return QrCodeGenerator.Generate(3, content);
        }
 
        private string GetConclusion(string conclusionCode)
        {
            string r = "";
            switch (conclusionCode)
            {
                case "Y":
                    r = "合格";
                    break;
                case "N":
                    r = "不合格";
                    break;
                case "X":
                    r = "未下结论";
                    break;
            }
            return r;
        }

        private int ToInt(string s)
        {
            try
            {
                return Convert.ToInt16(s);
            }
            catch
            {
            }
            return 0;
        }

        private string GetEncodeString(string s)
        {
            if (s.IsNullOrEmpty())
            {
                return defalutStr;
            }
            else
            {
                return s;
            }
        }

        private string StringToUnicode(string s, int inttmp)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i + 4 <= s.Length; i = i + 4)
            {
                string ss = s.Substring(i, 4);
                int intcode = Convert.ToInt32(ss, 16);
                if (intcode != 65535)
                {
                    int unicode = intcode - inttmp;
                    String tmp = "\\u" + unicode.ToString("X4");
                    sb.Append(tmp);
                }
            }
            return sb.ToString();
        }

        private string UnicodeToString(string str)
        {
            string outStr = "";
            if (!string.IsNullOrEmpty(str))
            {
                string[] strlist = str.Replace("\\", "").Split('u');
                try
                {
                    for (int i = 1; i < strlist.Length; i++)
                    {
                        //将unicode字符转为10进制整数，然后转为char中文字符  
                        outStr += (char)int.Parse(strlist[i], System.Globalization.NumberStyles.HexNumber);
                    }
                }
                catch (FormatException ex)
                {
                    outStr = ex.Message;
                }
            }
            return outStr;
        }

        private string ConvertToUnicode(string utf8String)
        {
            byte[] buffer1 = Encoding.UTF8.GetBytes(utf8String);
            byte[] buffer2 = Encoding.Convert(Encoding.UTF8, Encoding.Unicode, buffer1, 0, buffer1.Length);
            string strBuffer = Encoding.Unicode.GetString(buffer2, 0, buffer2.Length);
            return strBuffer;
        }

        private string Of_stringToHex(string s, int add)
        {
            string str = "";
            for (int i = 0; i < s.Length; i++)
            {
                int temp = Asc(s[i].ToString()) + add;
                str += temp.ToString("X4");
            }
            return Reverse(str);
        }

        private int Asc(string character)
        {
            char[] chars = character.ToCharArray();
            return Convert.ToInt32(chars[0]);
        }

        private string Reverse(string original)
        {
            char[] charArray = original.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }
    }
}
