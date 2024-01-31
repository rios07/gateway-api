using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Infraestructure.Cache;
using Microsoft.Extensions.Caching.Memory;
 

namespace Infraestructure.Cache
{
    public class CacheServices : ICacheServices
    {
        public const int DEFAULT_CACHE_LIFE_SPAN = 99999999;

        public Dictionary<string, CacheData<object>> _cache;
        public static CacheServices instancia { get; set; }
        public static CacheServices GetInstance()
        {
            if (instancia == null)
            {
                instancia = new CacheServices();
                instancia._cache = new Dictionary<string, CacheData<object>>();
            }
            return instancia;
        }
        public CacheServices()
        {
            _cache = new Dictionary<string, CacheData<object>>();
        }
        public void Add<T>(T data, string key, int lifespan = DEFAULT_CACHE_LIFE_SPAN)
        {
            CacheData<object> cacheEntry;
            CacheData<object> entry = new CacheData<object>() { Data = data, StampLife = DateTime.Now.AddMinutes(lifespan) };
            try
            {
                if (instancia._cache.TryGetValue(key, out cacheEntry))
                {
                    if (cacheEntry.StampLife.CompareTo(DateTime.Now) >= 0)
                    {
                        return;
                    }

                    _cache.Remove(key);
                }

                _cache.Add(key, entry);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error en cache al agregar el parametro: '{key}' en diccionario ... Reintentar operacion");
            }
           
        }
        public object Get(string key)
        {
            CacheData<object> cacheEntry;
            if (instancia._cache.TryGetValue(key, out cacheEntry))
            {
                if(cacheEntry.StampLife.CompareTo(DateTime.Now) >= 0)
                    return cacheEntry.Data;
            }

            return new CacheData<object>();
        }
        public void Remove(string key)
        {
            _cache.Remove(key);
        }
        public bool Exist(string key, int lifespan = DEFAULT_CACHE_LIFE_SPAN)
        {
            CacheData<object> cacheEntry;
            if (instancia._cache.TryGetValue(key, out cacheEntry))
            {
                if (cacheEntry.StampLife.CompareTo(DateTime.Now) >= 0)
                {
                    return true;
                }
            }
            return false;
        }
        public Dictionary<string, CacheData<object>> GetAll()
        {
            return _cache;
        }

        public bool Renew(string key, int lifespan = DEFAULT_CACHE_LIFE_SPAN)
        {
            bool exists = false;
            CacheData<object> cacheEntry;
            if (instancia._cache.TryGetValue(key, out cacheEntry))
            {
                if (cacheEntry.StampLife.CompareTo(DateTime.Now) >= 0)
                {
                    exists = true;
                    cacheEntry.StampLife = DateTime.Now.AddMinutes(lifespan);
                }
            }
            return exists;
        }
    }

    public class CacheData<T>
    {
        public T Data { get; set; }
        public DateTime StampLife { get; set; }


    }
}
