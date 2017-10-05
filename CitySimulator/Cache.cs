using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CitySimulator
{
    
    public class Cache<K,T> where T : class
    {
        private Dictionary<K, WeakReference<T>> _cache = new Dictionary<K, WeakReference<T>>();
        private Func<K, T> _loadFunc;

        public Cache(Func<K, T> loadFunc)
        {
            _loadFunc = loadFunc;
        }

        // Retrieve a data object from the cache.
        public T this[K key] {
            get {
                T val;
                WeakReference<T> cacheRecord;
                
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
                    _cache.Add(key, new WeakReference<T>(val));
                }

                return val;
            }
        }
    }
}
