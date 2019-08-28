using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;
using Elmah.Contrib.WebApi;
using System.Web.Http.ExceptionHandling;
using System.Web.Configuration;
using PkpmGx.Webapi.Architecture;
using System.Web.Http.Cors;

namespace PkpmGx.Webapi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Services.Add(typeof(IExceptionLogger), new ElmahExceptionLogger());
            config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Never;


            // Web API 配置和服务
            // 将 Web API 配置为仅使用不记名令牌身份验证。
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            // Enable CORS
            string allowedClients = System.Configuration.ConfigurationManager.AppSettings["AllowedClients"];
            config.EnableCors(new EnableCorsAttribute("*", "*", "*"));


            // Web API 路由
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Filters.Add(new UnhandledExceptionFilter());
            config.Services.Add(typeof(IExceptionLogger), new ElmahExceptionLogger());

            
        }
    }
}
