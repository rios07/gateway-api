using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Cache
{
    public interface ICacheServices
    {
        public void Add<T>(T data, string key, int lifespan = 960);
        public object Get(string key);
        public void Remove(string key);
        public bool Exist(string key, int lifespan = 960);
        public abstract Dictionary<string, CacheData<object>> GetAll();
    }
}
