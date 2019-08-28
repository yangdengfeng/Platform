using CacheManager.Core;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pkpm.Framework.Cache
{
    public class MemoryBackRedisCache<TCacheValue> : ICache<TCacheValue>
    {
        ICacheManager<TCacheValue> cacheManger;

        public MemoryBackRedisCache()
        {
            string redisEndPoint = ConfigurationManager.AppSettings["RedisEndPoint"];
            if(string.IsNullOrWhiteSpace(redisEndPoint))
            {
                redisEndPoint = "127.0.0.1";
            }

            var builder = new ConfigurationBuilder("MemoryBackRedisCache");
            builder
                   .WithRetryTimeout(500)
                   .WithMaxRetries(5);

            builder
                .WithSystemRuntimeCacheHandle()
                .WithExpiration(ExpirationMode.Sliding, TimeSpan.FromMinutes(20))
                .DisableStatistics();

            builder
                .WithRedisCacheHandle("redis", true)
                .WithExpiration(ExpirationMode.Sliding, TimeSpan.FromHours(4))
                .DisableStatistics();

            builder.WithRedisBackplane("redis");

             builder.WithRedisConfiguration("redis", config =>
              {
                  config
                      .WithAllowAdmin()
                      .WithDatabase(0)
                      .WithConnectionTimeout(5000)
                      .WithEndpoint(redisEndPoint, 6379);
              });


            cacheManger = new BaseCacheManager<TCacheValue>(builder.Build());
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
