using CacheManager.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pkpm.Framework.Cache
{
    public class MemoryCache<TCacheValue> : ICache<TCacheValue>
    {
        ICacheManager<TCacheValue> cacheManger;

        public MemoryCache()
        {
            cacheManger = CacheFactory.Build<TCacheValue>("MemoryCache", setting =>
            {
                setting
                .WithUpdateMode(CacheUpdateMode.Up)
                .WithSystemRuntimeCacheHandle()
                .WithExpiration(ExpirationMode.Sliding, TimeSpan.FromMinutes(10));
            });

            
           
        }

        public bool Add(string key, TCacheValue value)
        {
            return cacheManger.Add(key, value);
        }

        public bool Add(string key, TCacheValue value, ExpiredMode mode, TimeSpan timespan)
        {
            ExpirationMode expiretionMode = CacheUtility.ConvertFromCustomExpiredMode(mode);
            CacheItem<TCacheValue> item = new CacheItem<TCacheValue>(key, value, expiretionMode, timespan);
            return cacheManger.Add(item);
        }

        public void Clear()
        {
            cacheManger.Clear();
        }

        public TCacheValue Get(string key)
        {
            return cacheManger.Get(key);
        }

        public void Put(string key, TCacheValue value)
        {
            cacheManger.Put(key, value);
        }

        public void Put(string key, TCacheValue value, ExpiredMode mode, TimeSpan timespan)
        {
            ExpirationMode expiretionMode = CacheUtility.ConvertFromCustomExpiredMode(mode);
            CacheItem<TCacheValue> item = new CacheItem<TCacheValue>(key, value, expiretionMode, timespan);
            cacheManger.Put(item);
        }

        public bool Remove(string key)
        {
            return cacheManger.Remove(key);
        }
    }
}
