using FBM.Event.Client.interfaces;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Text;

namespace FBM.Event.DefaultManager
{
    /// <summary>
    /// default cache manager for event client.Its uses memory caching.
    /// <para>If you have distributed cache like redis or you just want to use another cache system,
    /// Override this or crate one which inheritanced from ICacheManager and use your own one </para>
    /// </summary>
    public class CacheManager : ICacheManager
    {
        IMemoryCache _memoryCache;
        public CacheManager(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public TItem Set<TItem>(object key, TItem value, TimeSpan absoluteExpirationRelativeToNow)
        {
            return _memoryCache.Set(key, value, absoluteExpirationRelativeToNow);
        }

        public bool TryGetValue<TItem>(object key, out TItem value)
        {
            return _memoryCache.TryGetValue(key, out value);
        }
    }
}
