using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Filters
{
    public class MyResourceFilter : IResourceFilter
    {
        public void OnResourceExecuted(ResourceExecutedContext context)
        {
            Console.WriteLine("resource filter end");
        }

        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            Console.WriteLine("resource filter start");
        }
    }
}
