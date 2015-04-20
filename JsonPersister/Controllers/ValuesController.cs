using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace JsonPersister.Controllers
{
    public class ValuesController : ApiController
    {
        public IEnumerable<dynamic> Get()
        {
            return new string[] { "value1", "value2" };
        }

        public dynamic Get(string appId, string resourceId)
        {
            var id = GetId(appId, resourceId);
            return Cache[id];
        }

        public void Post([FromBody]dynamic value)
        {

        }

        public void Put(string appId, string resourceId, [FromBody]dynamic resource)
        {
            var id = GetId(appId, resourceId);
            Cache[id] = resource;
        }

        public void Delete(string id)
        {
        }

        private static string GetId(string appId, string id)
        {
            return string.Format("{0}/{1}", appId, id);
        }

        private System.Web.Caching.Cache Cache
        {
            get
            {
                return HttpContext.Current.Cache;
            }
        }
    }
}