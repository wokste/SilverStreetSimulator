using System;
using System.Collections.Generic;

namespace CitySimulator
{
    
    public class Cache<TKey,TRef> where TRef : class
    {
        private readonly Dictionary<TKey, WeakReference<TRef>> _cache = new Dictionary<TKey, WeakReference<TRef>>();
        private readonly Func<TKey, TRef> _loadFunc;

        public Cache(Func<TKey, TRef> loadFunc)
        {
            _loadFunc = loadFunc;
        }

        // Retrieve a data object from the cache.
        public TRef this[TKey key] {
            get {
                TRef val;
                WeakReference<TRef> cacheRecord;
                
                // Try to get thing from cache
                if (_cache.TryGetValue(key, out cacheRecord) && cacheRecord.TryGetTarget(out val))
                {
                    return val;
                }
                
                // Failed getting thing from cache loading it
                val = _loadFunc(key);

                if (cacheRecord != null)
                {
                    // existing cache record
                    cacheRecord.SetTarget(val);
                }
                else
                {
                    // New cache record
                    _cache.Add(key, new WeakReference<TRef>(val));
                }

                return val;
            }
        }
    }
}
