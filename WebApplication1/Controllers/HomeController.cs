using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using Dapper;
using StackExchange.Redis;
using Newtonsoft.Json;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : Controller
    {
        private IConfiguration configuration;
        private string connectionString;
        private string redisConf;
        private string studentKey = "student:{0}";
        public HomeController(IConfiguration conf)
        {
            this.configuration = conf;
            this.connectionString = this.configuration.GetSection("connectionString").Value;
            this.redisConf = this.configuration.GetSection("redisConf").Value;
        }
        [HttpGet]
        public IActionResult Index(int id)
        {
            if (id == 0 || id < 0)
            {
                return Json("error!");
            }
            using (var ex = ConnectionMultiplexer.Connect(this.redisConf))
            {
                var db = ex.GetDatabase();
                if (db.KeyExists(string.Format(this.studentKey, id)))
                {
                    var value = db.StringGet(string.Format(this.studentKey, id));
                    var data = JsonConvert.DeserializeObject<Student[]>(value.ToString());
                    return Json(data);
                }
            }
            using (var conn = new MySqlConnection(this.connectionString))
            {
                var students = conn.Query<Student, School, Student>("select * from student as stu join school sch on stu.schoolid = sch.id ", (student, school) =>
                {
                    student.SchoolInfo = school;
                    return student;
                });
                using (var ex = ConnectionMultiplexer.Connect(this.redisConf))
                {
                    var db = ex.GetDatabase();
                    db.StringSet(string.Format(this.studentKey, id), JsonConvert.SerializeObject(students));
                }
                return Json(students);
            }
            return Json(new { name = "kun", age = 12 });
        }
    }
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public School SchoolInfo { get; set; }
    }
    public class School
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}