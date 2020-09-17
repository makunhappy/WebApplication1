using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace RedisHelper
{
    public class RedisHelper
    {
        private string redisConf;
        public RedisHelper(IConfiguration conf)
        {
            redisConf = conf.GetSection("redisConf").Value;
        }
        public T Get<T>(string key) where T : class
        {
            using (var con = ConnectionMultiplexer.Connect(this.redisConf))
            {
                var db = con.GetDatabase();
                if (db.KeyExists(key))
                {
                    var value = db.StringGet(key);
                    var data = JsonConvert.DeserializeObject<T>(value.ToString());
                    return data;
                }
                return null;
            }
        }
        public bool Set(string key, object obj)
        {
            using (var con = ConnectionMultiplexer.Connect(this.redisConf))
            {
                var data = JsonConvert.SerializeObject(obj);
                return con.GetDatabase().StringSet(key, data);
            }
        }
        public bool Exist(string key)
        {
                using (var con = ConnectionMultiplexer.Connect(this.redisConf))
                {
                    return con.GetDatabase().KeyExists(key);
                }
        }
        public bool Delete(string key)
        {
            using (var con = ConnectionMultiplexer.Connect(this.redisConf))
            {
                return con.GetDatabase().KeyDelete(key);
            }
        }
    }
}
