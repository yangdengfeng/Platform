using Elmah;
using PkpmGX.Architecture;
using StackExchange.Profiling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace PkpmGX
{
    public class MvcApplication : PkpmApplication
    { 

        protected void Application_Error(Object sender, EventArgs e)
        {
            var lastError = Server.GetLastError();
            if (lastError != null)
            {
                var httpError = lastError as HttpException;
                if (httpError != null)
                {
                    //ASP.NET的400与404错误不记录日志，并都以自定义404页面响应
                    var httpCode = httpError.GetHttpCode();
                    if (httpCode == 400 || httpCode == 404)
                    {
                        Response.StatusCode = 404;//在IIS中配置自定义404页面
                        Server.ClearError();
                        return;
                    }

                    ErrorSignal.FromCurrentContext().Raise(httpError);
                }

                //对于路径错误不记录日志，并都以自定义404页面响应
                if (lastError.TargetSite.ReflectedType == typeof(System.IO.Path))
                {
                    Response.StatusCode = 404;
                    Server.ClearError();
                    return;
                }

                ErrorSignal.FromCurrentContext().Raise(lastError);
                Response.StatusCode = 500;
                Server.ClearError();
            }
        }

        protected void Application_BeginRequest(object src, EventArgs e)
        {
            if (Request.IsLocal)
                MiniProfiler.Start();
        }

        protected void Application_EndRequest(object src, EventArgs e)
        {
            MiniProfiler.Stop();
        }
    }
}
 