
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ninject;
using System.Web.Mvc;
using System.Web.Routing;
using Pkpm.Framework.Logging;
using Pkpm.Framework.Cache;
using ServiceStack.Data;
using Pkpm.Framework.Repsitory;
using Pkpm.Framework.FileHandler;
using Nest;
using System.Web.Optimization;
using Pkpm.Core.UserRoleCore;
using Pkpm.Core.CheckUnitCore;
using Pkpm.Core.SysDictCore;
using Pkpm.Core.PathCore;
using Pkpm.Core.ItemNameCore;
using Pkpm.Framework.PkpmConfigService;
using Pkpm.Core.UserCustomize;
using Pkpm.Core.CheckPeopleManagerCore;
using Pkpm.Core.SysInfoCore;
using Pkpm.Core.SoftwareVersService;
using Ninject.Web.Common.WebHost;
using PkpmGx.Webapi;
using System.Web.Http;
using Pkpm.Core.AreaCore;
using Pkpm.Core;
using Pkpm.Core.ReportCore;
using Pkpm.Core.STCustomCore;

namespace PkpmGX.Architecture
{
    /// <summary>
    /// 
    /// </summary>
    public class PkpmApplication : NinjectHttpApplication
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filters"></param>
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            //filters.Add(new HandleErrorAttribute());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="routes"></param>
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new
                {
                    controller = "Home",
                    action = "Index",
                    id = UrlParameter.Optional
                });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            RegisterServices(kernel);
            return kernel;
        }

        private void RegisterServices(StandardKernel kernel)
        {
            kernel.Bind<ILogger>().To<NLogLogger>().InSingletonScope().WithConstructorArgument("name", x => x.Request.ParentContext.Request.Service.FullName);
            kernel.Bind(typeof(ICache<>)).To(typeof(MemoryBackRedisCache<>)).InSingletonScope();
            //kernel.Bind(typeof(ICache<>)).To(typeof(MemoryCache<>)).InSingletonScope();
            kernel.Bind<IPkpmConfigService>().To<PkpmConfigService>().InSingletonScope();
            //kernel.Bind<IDbConnectionFactory>().ToMethod(x => ServiceStackDBContext.QRDbFactory).InSingletonScope().Named("QR");
            kernel.Bind<IDbConnectionFactory>().ToMethod(x => ServiceStackDBContext.DbFactory).InSingletonScope(); 
            kernel.Bind<IConnectionSettingsValues>().ToMethod(x => ESConnectionSettings.connectionSettings).InSingletonScope();
            kernel.Bind(typeof(IESRepsitory<>)).To(typeof(ESRepsitory<>));
            kernel.Bind<IFileHandler>().To<LocalFileHandler>();
            kernel.Bind(typeof(IRepsitory<>)).To(typeof(ServiceStackRepsitory<>));
            kernel.Bind<IRoleService>().To<RoleService>();
            kernel.Bind<IUserService>().To<UserService>();
            kernel.Bind<IPathService>().To<PathService>();
            kernel.Bind<ISysDictService>().To<SysDictService>();
            kernel.Bind<ICheckUnitService>().To<CheckUnitService>();
            kernel.Bind<ISysInfoService>().To<SysInfoService>();
            kernel.Bind<ISoftwareVersService>().To<SoftwareVersService>();
            kernel.Bind<IItemNameService>().To<ItemNameService>();
            kernel.Bind<ICheckPeopleManagerService>().To<CheckPeopleManagerService>();
            kernel.Bind<IAcsChartService>().To<AcsChartService>();
            kernel.Bind<IUserCustomize>().To<UserCustomize>();
            kernel.Bind<IReportService>().To<ReportService>();
            kernel.Bind<IAreaService>().To<AreaService>();
            kernel.Bind<ISTCustomService>().To<STCustomService>();

        }

        protected override void OnApplicationStarted()
        {
            base.OnApplicationStarted();

           
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

        }
    }
}