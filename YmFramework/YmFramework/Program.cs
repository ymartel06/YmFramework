using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YmFramework
{
    class Program
    {
        static void Main(string[] args)
        {
            CacheAutoLockerTest test = new CacheAutoLockerTest();

            // first call will add the value in cache and retrieve it from the get function
            int? result = CacheAutoLocker.Get<int?>(test.Get, test.GetFromCache, test.SetInCache, "test");
            // second call will get from the cache
            result = CacheAutoLocker.Get<int?>(test.Get, test.GetFromCache, test.SetInCache, "test");
        }
    }

    // dummy class to be able to test the cache autolocker
    public class CacheAutoLockerTest
    {
        private Dictionary<string, int?> fakeCache = new Dictionary<string, int?>();

        public int? Get(string key)
        {
            return 1;
        }

        public int? GetFromCache(string key)
        {
            if (fakeCache.ContainsKey(key))
                return fakeCache[key];
            else
                return null;
        }

        public void SetInCache(string key, int? number)
        {
            fakeCache[key] = number;
        }
    }
}
