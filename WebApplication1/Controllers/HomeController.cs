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
using DAL;
using Models;
using WebApplication1.Filters;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : Controller
    {
        private StudentDal StudentService;
        private SchoolDal SchoolService;
        public HomeController(StudentDal studentDal, SchoolDal schoolDal)
        {
            this.StudentService = studentDal;
            this.SchoolService = schoolDal;
        }
        /// <summary>
        ///     get a student
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ServiceFilter(typeof(DemoAttributeFilter))]
        [HttpGet("{id=0}")]
        public IActionResult Index(int id)
        {
            if (id == 0 || id < 0)
            {
                return Json("error!");
            }
            var data = this.StudentService.QueryById(id);
            return Json(data);

        }
        /// <summary>
        ///     add a student 
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AddStudent(Student s)
        {
            if (s == null || s.SchoolInfo == null)
                return BadRequest();
            if(s.SchoolInfo.Id<1)
            {
                var school = SchoolService.QueryByName(s.SchoolInfo.Name);
                if(school!= null)
                {
                    s.SchoolInfo.Id = school.Id;
                }
                else
                {
                    SchoolService.Add(s.SchoolInfo);
                    var schoolAdd = SchoolService.QueryByName(s.SchoolInfo.Name);
                    s.SchoolInfo.Id = schoolAdd.Id;
                }
            }
            var res = this.StudentService.Add(s);
            return Json(res);

        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            Console.WriteLine("HomeController.OnActionExecuting");
            base.OnActionExecuting(context);
        }
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            Console.WriteLine("HomeController.OnActionExecuted");
            base.OnActionExecuted(context);
        }
    }


}