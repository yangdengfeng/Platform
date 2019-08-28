using CacheManager.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pkpm.Framework.Cache
{
   public static class CacheUtility
    {
        public static ExpirationMode ConvertFromCustomExpiredMode(ExpiredMode mode)
        {
            switch (mode)
            {
                case ExpiredMode.Default:
                    return ExpirationMode.Default;
                case ExpiredMode.None:
                    return ExpirationMode.None;
                case ExpiredMode.Sliding:
                    return ExpirationMode.Sliding;
                case ExpiredMode.Absolute:
                    return ExpirationMode.Absolute;
                default:
                    return ExpirationMode.None;
            }
        }
    }
}
