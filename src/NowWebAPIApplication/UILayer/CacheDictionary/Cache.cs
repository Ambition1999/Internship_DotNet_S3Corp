using UILayer.CacheItem;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using UILayer.Models;
using Model.Model_Mapper;

namespace UILayer
{
    public class Cache<TKey, TValue>
    {
        private Dictionary<TKey, CacheItem<TValue>> _cache = new Dictionary<TKey, CacheItem<TValue>>();
        public void Store(TKey key, TValue value, TimeSpan expiresAfter)
        {
            _cache[key] = new CacheItem<TValue>(value, expiresAfter);
        }

        public TValue Get(TKey key)
        {
            if (!_cache.ContainsKey(key)) return default(TValue);
            var cached = _cache[key];
            if(DateTimeOffset.Now - cached.Created >= cached.ExpiresAfter)
            {
                _cache.Remove(key);
                return default(TValue);
            }
            return cached.Value;
        }

        public bool IsNotNull()
        {
            if (_cache == null || _cache.Count == 0)
                return false;
            return true;
        }

        public List<TKey> GetKeysByValue(string value)
        {
            var listId = _cache.Where(t => t.Value.Value.ToString().ToLower().Trim().Contains(value.ToLower())).Select(t => t.Key).ToList();
            return listId;
        }

        public int Count()
        {
            return _cache.Count;
        }


        //public List<TKey> GetKeysByValue(string strInput)
        //{
        //    List<TKey> listTemp = new List<TKey>();
        //    foreach (var item in _cache)
        //    {
        //        String value = item.Value.Value.ToString();
        //        if (value.Contains(strInput))
        //            listTemp.Add(item.Key);
        //    }       
        //    return listTemp;
        //}
    }
}