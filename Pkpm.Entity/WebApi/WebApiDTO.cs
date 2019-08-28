using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Pkpm.Entity
{
    [KnownType(typeof(AjaxResult))]
    public class AjaxResult
    {
        public bool ok { get; set; }
        public string err { get; set; }

        public static AjaxResult Success = new AjaxResult() { ok = true };

        public static AjaxResult Failure(string errMsg)
        {
            return new AjaxResult() { ok = false, err = errMsg };
        }

        public static AjaxResult WithData(object data)
        {
            return new DataResult() { ok = true, data = data };
        }

        public static DataResult<List<T>> WithListOf<T>(List<T> data)
        {
            return new DataResult<List<T>>() { ok = true, data = data };
        }
    }

    public class LoginResult : AjaxResult
    {
        public string token { get; set; }
        public string redirectUrl { get; set; }
    }

    [KnownType(typeof(DataResult))]
    public class DataResult : AjaxResult
    {
        public object data { get; set; }
    }

    [KnownType(typeof(DataResult<>))]
    public class DataResult<T> : AjaxResult
    {
        public T data { get; set; }
    }

    public class TBpAcsViewModel
    {

        public string PK { get; set; }

        public string ACSTIME { get; set; }

        public string MAXVALUE { get; set; }
        /// <summary>
        /// 单位  N，KN
        /// </summary>
        public string UnitName { get; set; }

    }
}
