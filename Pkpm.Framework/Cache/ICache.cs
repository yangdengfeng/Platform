using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pkpm.Framework.Cache
{
    public interface ICache<TCacheValue>
    {
        TCacheValue Get(string key);
        bool Add(string key, TCacheValue value);
        bool Add(string key, TCacheValue value, ExpiredMode mode, TimeSpan timespan);
        void Put(string key, TCacheValue value);
        void Put(string key, TCacheValue value, ExpiredMode mode, TimeSpan timespan);
        bool Remove(string key);
        void Clear();
    }


    public enum ExpiredMode
    {
        Default = 0,
        None = 1,
        Sliding = 2,
        Absolute = 3
    }

}
