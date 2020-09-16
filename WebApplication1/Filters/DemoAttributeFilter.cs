using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.CodeAnalysis.Options;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.ConfigModels;

namespace WebApplication1.Filters
{
    public class DemoAttributeFilter : ActionFilterAttribute
    {
        private ConfigInfo ConfigInfo;
        public DemoAttributeFilter(IOptions<ConfigInfo> option)
        {
            this.ConfigInfo =  option.Value;
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            Console.WriteLine($"{context.ActionDescriptor.DisplayName} begin {this.ConfigInfo.Title} in filter attribute");
            base.OnActionExecuting(context);
        }
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            Console.WriteLine($"{context.ActionDescriptor.DisplayName} end {this.ConfigInfo.Title} in filter attribute");
            base.OnActionExecuted(context);
        }
    }
}
