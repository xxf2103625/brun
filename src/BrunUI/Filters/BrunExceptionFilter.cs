using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrunUI.Filters
{
    /// <summary>
    /// Brun控制器专用统一返回异常结果
    /// </summary>
    public class BrunExceptionFilter : IExceptionFilter
    {
        ILogger<BrunExceptionFilter> logger;
        public BrunExceptionFilter(ILogger<BrunExceptionFilter> logger)
        {
            this.logger = logger;
        }
        public void OnException(ExceptionContext context)
        {
            logger.LogError(context.Exception, context.Exception.Message);
            var result = new JsonResult(new { Success = false, ErrorMessage = "服务器异常:" + context.Exception.Message });
            context.Result = result;
        }
    }
}
