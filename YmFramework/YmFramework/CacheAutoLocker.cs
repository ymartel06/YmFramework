using System;
using System.Collections.Generic;

namespace YmFramework
{
    public static class CacheAutoLocker
    {
        private static Dictionary<string, object> lockers = new Dictionary<string, object>();
        private static object lockersLock = new object();

        public static T Get<T>(Func<string, T> get, Func<string, T> getFromCache, Action<string, T> setInCache, string key)
        {
            //check arguments
            if (get == null)
                throw new ArgumentNullException("get");

            if (getFromCache == null)
                throw new ArgumentNullException("getFromCache");

            if (setInCache == null)
                throw new ArgumentNullException("set");

            if (String.IsNullOrEmpty(key))
                throw new ArgumentNullException("key");

            //get the lock for this operation
            object operationLock = GetLock(key);

            //get the result
            T result = getFromCache.Invoke(key);
            if (result == null)
            {
                lock (operationLock)
                {
                    result = getFromCache.Invoke(key);
                    if (result == null)
                    {
                        result = get.Invoke(key);
                        setInCache(key, result);
                    }
                }
            }
            return result;

        }

        private static object GetLock(string key)
        {
            // get or generate the locker        
            if (lockers.ContainsKey(key))
            {
                return lockers[key];
            }

            object operationLock = null;
            lock (lockersLock)
            {
                if (lockers.ContainsKey(key))
                {
                    operationLock = lockers[key];
                }
                else
                {
                    operationLock = new object();
                    lockers.Add(key, operationLock);
                }
            }
            //return the operationlock outside of the main lock (avoid double lock)
            return operationLock;
        }
    }
}