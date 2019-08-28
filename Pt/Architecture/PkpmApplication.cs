using Ninject.Web.Common.WebHost;
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
using StackExchange.Profiling.Mvc;
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
using Pkpm.Core.STCustomCore;
using Pkpm.Core.AreaCore;
using Pkpm.Web;
using Pkpm.Core.STCheckEquipCore;
using Pkpm.Core.STCheckPeopleCore;
using Pkpm.Core.CovrliistService;
using Pkpm.Core.ReportCore;
using Pkpm.Core.QrCodeCore;
using Pkpm.Core;
using Pkpm.Core.CheckQualifyCore;
using Pkpm.Core.ApplyQualifySixCore;
using Pkpm.Core.ZJCheckService;
using Pkpm.Core.SHItemNameCore;
using Pkpm.Core.HtyService;

namespace PkpmGX.Architecture
{
    public class PkpmApplication : NinjectHttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            //filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new
                {
                    controller = "Admin",
                    action = "Index",
                    id = UrlParameter.Optional
                });
        }

        protected override IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            RegisterServices(kernel);
            return kernel;
        }

        private void RegisterServices(StandardKernel kernel)
        {
            kernel.Bind<ILogger>().To<NLogLogger>().InSingletonScope().WithConstructorArgument("name", x => x.Request.ParentContext.Request.Service.FullName);
            //kernel.Bind(typeof(ICache<>)).To(typeof(MemoryBackRedisCache<>)).InSingletonScope();
            kernel.Bind(typeof(ICache<>)).To(typeof(MemoryCache<>)).InSingletonScope();
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
            //kernel.Bind<IPeopleChange>().To<PeopleChange>();
            //kernel.Bind<IInstlchange>().To<CheckInstChangeService>();
            kernel.Bind<IItemNameService>().To<ItemNameService>();
            kernel.Bind<ICheckPeopleManagerService>().To<CheckPeopleManagerService>();
            //kernel.Bind<IContractSearchService>().To<ContractSearchService>();
            kernel.Bind<IAcsChartService>().To<AcsChartService>();
            //kernel.Bind<ICreditStandard>().To<CreditStandardService>();
            //kernel.Bind<ICustomCredit>().To<CustomCredit>();
            //kernel.Bind<ICreditLevel>().To<CreditLevelService>();
            //kernel.Bind<ICreditRating>().To<CreditRating>();
            //kernel.Bind<ISoftWareVersionService>().To<SoftWareVersionService>();
            //kernel.Bind<IProjectService>().To<ProjectService>();
            //kernel.Bind<ICheckSuperVisor>().To<CheckSuperVisorService>();
            kernel.Bind<IUserCustomize>().To<UserCustomize>();
            //kernel.Bind<ILiveDetectService>().To<LiveDetectService>();
            ////kernel.Bind<IAreaCodeService>().To<AreaCodeService>();
            //kernel.Bind<IDetectionReportService>().To<DetectionReportService>();
            //kernel.Bind<IInstBasicSituationService>().To<InstBasicSituationService>();
            //kernel.Bind<IPeopleBasicSituationService>().To<PeopleBasicSituationService>();
            //kernel.Bind<ICertPeopleSituationService>().To<CertPeopleSituationService>();
            //kernel.Bind<IDetectBusinessReportService>().To<DetectBusinessReportService>();
            //kernel.Bind<IExamInfoManageService>().To<ExamInfoManageService>();
            //kernel.Bind<IWebContent>().To<WebContentService>();
            //kernel.Bind<IWxScheduleService>().To<WxScheduleService>();
            //kernel.Bind<IEsProjectZJSeqService>().To<ESProjectZJSequence>();
            //kernel.Bind<IEntrustService>().To<EntrustServcie>();
            //kernel.Bind<IQrInfoService>().To<QrInfoService>();
            kernel.Bind<IReportService>().To<ReportService>();
            //kernel.Bind<ICovrlistService>().To<CovrlistService>();
            //kernel.Bind<IDatauploadmonitorService>().To<DatauploadmonitorService>();
            kernel.Bind<IReportQrCode>().To<ReportQrCode>();
            //kernel.Bind<IHrItemService>().To<HrItemService>();
            //kernel.Bind<ICheckTypeConfig>().To<CheckTypeConfig>();
            //kernel.Bind<ICheckPeopleTypeService>().To<CheckPeopleTypeService>();ISTCustomService
            kernel.Bind<IAreaService>().To<AreaService>();
            kernel.Bind<ISTCustomService>().To<STCustomService>(); 
            kernel.Bind<ISTCheckEquipService>().To<STCheckEquipService>();
            kernel.Bind<ISTCheckPeopleService>().To<STCheckPeopleService>();

            kernel.Bind<ICovrlistService>().To<CovrlistService>();
            kernel.Bind<ISHItemNameService>().To<SHItemNameService>();
            kernel.Bind<ICheckQualifyService>().To<CheckQualifyService>();
            kernel.Bind<IApplyQualifyService>().To<ApplyQualifyService>();
            kernel.Bind<IZJCheckService>().To<ZJCheckService>();
            kernel.Bind<IHtyService>().To<HtyService>();
            kernel.Bind<IPileService>().To<PileService>();
        }

        protected override void OnApplicationStarted()
        {
            base.OnApplicationStarted();

            GlobalFilters.Filters.Add(new ProfilingActionFilter());

            AreaRegistration.RegisterAllAreas();
            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);


            var copy = ViewEngines.Engines.ToList();
            ViewEngines.Engines.Clear();
            foreach (var item in copy)
            {
                ViewEngines.Engines.Add(new ProfilingViewEngine(item));
            }

        }
    }
}