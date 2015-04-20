using Enyim.Caching;
using Enyim.Caching.Memcached;
using System;
using System.Collections.Generic;

using System.Linq;

using System.Web.Http;

namespace JsonPersister.Controllers
{
    public class ValuesController : ApiController
    {
        private static Lazy<MemcachedClient> cache = new Lazy<MemcachedClient>(() => new MemcachedClient());

        private static MemcachedClient Cache
        {
            get
            {
                return cache.Value;
            }
        }

        public IEnumerable<dynamic> Get(string appId)
        {
            return new string[] { "value1", "value2" };
        }

        public dynamic Get(string appId, string resourceId)
        {
            var id = GetId(appId, resourceId);

            return Cache.Get(id);
        }

        public void Post([FromBody]dynamic value)
        {
        }

        public void Put(string appId, string resourceId, [FromBody]dynamic resource)
        {
            var id = GetId(appId, resourceId);

            Cache.Store(StoreMode.Set, id, resource);
        }

        public void Delete(string id)
        {
        }

        private static string GetId(string appId, string id)
        {
            return string.Format("{0}/{1}", appId, id);
        }
    }
}
