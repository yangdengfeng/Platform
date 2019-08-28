using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pkpm.Framework.Common
{
    public class CommonUtils
    {
        public static string GetValueByFieldString<T>(T TObject, string Field) where T : class
        {
            string Value = string.Empty;
            Type CurType = TObject.GetType();
            System.Reflection.PropertyInfo[] CurProperties = CurType.GetProperties();
            foreach (var Prop in CurProperties)
            {
                if (Field.ToUpper() == Prop.Name.ToUpper())
                {
                    Value = Prop.GetValue(TObject, null) == null ? "" : Prop.GetValue(TObject, null).ToString();
                    break;
                }
            }
            return Value;
        }

        public static string GetDateTimeStr(DateTime? dt, string format = "yyyy-MM-dd")
        {
            if (dt.HasValue)
            {
                return dt.Value.ToString(format);
            }
            else
            {
                return string.Empty;
            }
        }

        public static void GetLayuiDateRange(string layUIDateStr, out DateTime? startDt, out DateTime? endDt)
        {
            startDt = endDt = null;

            if (layUIDateStr.IsNullOrEmpty())
            {
                return;
            }

            var dts = layUIDateStr.Trim().Split(new string[] { " - " }, StringSplitOptions.RemoveEmptyEntries);
            if (dts == null || dts.Count() == 0)
            {
                return;
            }

            if (!dts[0].IsNullOrEmpty())
            {
                DateTime dt = DateTime.Now;

                if (DateTime.TryParse(dts[0], out dt))
                {
                    startDt = dt;
                }
                else
                {
                    startDt = null;
                }
            }

            if (dts.Count() >= 2 && !dts[1].IsNullOrEmpty())
            {
                DateTime dt = DateTime.Now;

                if (DateTime.TryParse(dts[1], out dt))
                {
                    endDt = dt;
                }
                else
                {
                    endDt = null;
                }
            }

        }

        public static List<string> SplitStrIntoList(string str, char split = ',')
        {
            if (str.IsNullOrEmpty())
            {
                return new List<string>();
            }

            return str.Split(split).ToList();
        }

        public static string GetSplitFirst(string str, char split = ',')
        {
            if (str.IsNullOrEmpty())
            {
                return string.Empty;
            }

            var splitIndex = str.IndexOf(split);
            if (splitIndex == -1)
            {
                return string.Empty;
            }

            return str.Substring(0, splitIndex);

        }

        public static string GetStrFromDt(DateTime? dt, string format = "yyyy-MM-dd")
        {
            if (dt.HasValue)
            {
                return dt.Value.ToString(format);
            }
            else
            {
                return string.Empty;
            }
        }

        public static string GetStrFromDt(DateTime dt,string format="yyyy-MM-dd")
        {
            return dt.ToString(format);
        }

        public static string GetStrFromDouble(double? dValue)
        {
            if (dValue.HasValue)
            {
                return dValue.ToString();
            }
            else
            {
                return string.Empty;
            }
        }

        public static string GetStrFromInt(int? iValue)
        {
            if (iValue.HasValue)
            {
                return iValue.ToString();
            }
            else
            {
                return string.Empty;
            }
        }

    }
}
