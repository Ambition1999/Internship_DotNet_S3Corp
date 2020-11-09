using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Web;

namespace UILayer.CacheItem
{
    public class CacheItem<T>
    {
        public CacheItem(T value, TimeSpan expiresAfter) 
        {
            Value = value;
            ExpiresAfter = expiresAfter;
        }

        public T Value { get; }
        internal DateTimeOffset Created { get; } = DateTimeOffset.Now;
        internal TimeSpan ExpiresAfter { get; }
    }
}