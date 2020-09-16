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
        public HomeController(StudentDal studentDal)
        {
            this.StudentService = studentDal;
        }
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