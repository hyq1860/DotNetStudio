using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using DotNet.Common.Logging;

namespace DotNet.WebSite.MVC.Filters
{
    public class LogExceptionFilterAttribute : IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            Logger.Log("异常信息：",filterContext.Exception);
        }
    }
}
