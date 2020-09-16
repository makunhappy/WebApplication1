using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Filters
{
    public class DemoFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            Console.WriteLine($"{context.ActionDescriptor.DisplayName} end");
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            Console.WriteLine($"{context.ActionDescriptor.DisplayName} begin");
        }
    }
}
