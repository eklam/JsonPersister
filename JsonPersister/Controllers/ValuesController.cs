using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace JsonPersister.Controllers
{
    public class ValuesController : ApiController
    {
        public HttpResponseMessage Options()
        {
            return new HttpResponseMessage { StatusCode = HttpStatusCode.OK };
        }

        public IEnumerable<Resource> Get(string appId)
        {
            return new ResourceRepository(appId).ListAll().ToList();
        }

        public JObject Get(string appId, string resourceId)
        {
            return new ResourceRepository(appId).GetById(resourceId).Content;
        }

        public string Post(string appId, [FromBody]JObject resource)
        {
            return new ResourceRepository(appId).Add(resource);
        }

        public void Put(string appId, string resourceId, [FromBody]JObject resource)
        {
            new ResourceRepository(appId).Add(resourceId, resource);
        }

        public void Delete(string appId, string resourceId = null)
        {
            if (resourceId == null)
            {
                new ResourceRepository(appId).DeleteAll();
            }
            else
            {
                new ResourceRepository(appId).Delete(resourceId);
            }
        }

        [Serializable]
        public struct Resource
        {
            public Resource(string id, JObject content)
            {
                this.Id = id;
                this.Content = content;
            }

            public string Id;

            public JObject Content;
        }

        public class ResourceRepository
        {
            private readonly string appId;

            public ResourceRepository(string appId)
            {
                this.appId = appId;
            }

            private static ConnectionMultiplexer redis;

            private static ConnectionMultiplexer Redis
            {
                get
                {
                    if (redis == null)
                    {
                        var redisCloudUrl = ConfigurationManager.AppSettings["REDISCLOUD_URL"].ToString();
                        var connectionUri = new Uri(redisCloudUrl);
                        string host = connectionUri.Host;
                        int port = connectionUri.Port;
                        string password = connectionUri.UserInfo.Split(':').LastOrDefault();

                        ConfigurationOptions options = new ConfigurationOptions();
                        options.Password = password;
                        options.EndPoints.Add(host, port);

                        redis = ConnectionMultiplexer.Connect(options);
                    }
                    return redis;
                }
            }

            public IEnumerable<Resource> ListAll()
            {
                var db = Redis.GetDatabase();

                var list = db.ListRange(appId, 0, -1);

                return list.Select(x => JsonConvert.DeserializeObject<Resource>(x));
            }

            public Resource GetById(string id)
            {
                return ListAll().SingleOrDefault(x => x.Id == id);
            }

            public void Add(string id, JObject content)
            {
                var db = Redis.GetDatabase();

                db.ListLeftPush(appId, JsonConvert.SerializeObject(new Resource(id, content)));
            }

            public string Add(JObject content)
            {
                var id = Guid.NewGuid().ToString();
                Add(id, content);
                return id;
            }

            public void DeleteAll()
            {
                var db = Redis.GetDatabase();

                db.KeyDelete(appId);
            }

            public void Delete(string id)
            {
                var db = Redis.GetDatabase();

                db.ListRemove(appId, JsonConvert.SerializeObject(GetById(id)), -1);
            }
        }
    }
}
