using Dapper;
using Microsoft.Extensions.Configuration;
using Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL
{
    public class SchoolDal
    {
        private string connectionString;
        private string redisConf;
        private string schoolKey = "school:{0}";
        private RedisHelper.RedisHelper RedisHelper;
        public SchoolDal(IConfiguration configuration, RedisHelper.RedisHelper redisHelper)
        {
            this.connectionString = configuration.GetSection("connectionString").Value;
            this.redisConf = configuration.GetSection("redisConf").Value;
            this.RedisHelper = redisHelper;
        }
        public School QueryById(int id)
        {
            var data = this.RedisHelper.Get<School>(string.Format(schoolKey, id));
            if (data == null)
            {
                using (var conn = new MySqlConnection(this.connectionString))
                {
                    var schools = conn.Query<School>("select * from school where id = @Id ",  new { Id = id });
                    if (schools.Count() > 0)
                    {
                        this.RedisHelper.Set(string.Format(schoolKey, id), schools.First());
                        return schools.First();
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            else
            {
                return data;
            }

        }
        public School QueryByName(string name)
        {
            var data = this.RedisHelper.Get<School>(string.Format(schoolKey, name));
            if (data == null)
            {
                using (var conn = new MySqlConnection(this.connectionString))
                {
                    var schools = conn.Query<School>("select * from school where name = @Name ", new { Name = name });
                    if (schools.Count() > 0)
                    {
                        this.RedisHelper.Set(string.Format(schoolKey, name), schools.First());
                        return schools.First();
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            else
            {
                return data;
            }

        }
        public bool Add(School s)
        {
            if (this.RedisHelper.Exist(string.Format(schoolKey, s.Id)))
            {
                this.RedisHelper.Delete(string.Format(schoolKey, s.Id));
            }
            using (var conn = new MySqlConnection(this.connectionString))
            {
                var res = conn.Execute("insert into school(name) values(@Name) ", s);
                return res > 0;
            }
        }
    }
}
