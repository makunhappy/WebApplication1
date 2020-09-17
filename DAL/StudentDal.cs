using Dapper;
using Microsoft.Extensions.Configuration;
using Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using StackExchange.Redis;
using Newtonsoft.Json;
using RedisHelper;

namespace DAL
{
    public class StudentDal
    {
        private string connectionString;
        private string redisConf;
        private string studentKey = "student:{0}";
        private RedisHelper.RedisHelper RedisHelper;
        public StudentDal(IConfiguration configuration, RedisHelper.RedisHelper redisHelper)
        {
            this.connectionString = configuration.GetSection("connectionString").Value;
            this.redisConf = configuration.GetSection("redisConf").Value;
            this.RedisHelper = redisHelper;
        }
        public Student QueryById(int id)
        {
            var data = this.RedisHelper.Get<Student>(string.Format(studentKey, id));
            if (data == null)
            {
                using (var conn = new MySqlConnection(this.connectionString))
                {
                    var students = conn.Query<Student, School, Student>("select * from student as stu join school sch on stu.schoolid = sch.id where stu.id = @Id ", (student, school) =>
                    {
                        student.SchoolInfo = school;
                        return student;
                    }, new { Id = id });
                    if (students.Count() > 0)
                    {
                        this.RedisHelper.Set(string.Format(studentKey, id), students.First());
                        return students.First();
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
        public bool Add(Student s)
        {
            if (this.RedisHelper.Exist(string.Format(studentKey, s.Id)))
            {
                this.RedisHelper.Delete(string.Format(studentKey, s.Id));
            }
            using (var conn = new MySqlConnection(this.connectionString))
            {
                var res = conn.Execute("insert into student(name,address,schoolid) values(@Name,@Address,@SchoolId) ", new { Name = s.Name, Address = s.Address, SchoolId = s.SchoolInfo.Id });
                return res > 0;
            }
        }
    }
}
