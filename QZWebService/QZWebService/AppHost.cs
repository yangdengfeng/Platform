using Funq;
using ServiceStack;
using QZWebService.ServiceInterface;
using QZWebService.ServiceInterface.Repsitory;
using ServiceStack.Caching;
using ServiceStack.Data;
using QZWebService.ServiceModel;
using ServiceStack.Api.Swagger;
using ServiceStack.Logging.NLogger;
using ServiceStack.Logging;
using ServiceStack.Validation;

namespace QZWebService
{
    //VS.NET Template Info: https://servicestack.net/vs-templates/EmptyAspNet
    public class AppHost : AppHostBase
    {
        /// <summary>
        /// Base constructor requires a Name and Assembly where web service implementation is located
        /// </summary>
        public AppHost()
            : base("QZWebService", typeof(MyServices).Assembly) { }

        /// <summary>
        /// Application specific configuration
        /// This method should initialize any IoC resources utilized by your web service classes.
        /// </summary>
        public override void Configure(Container container)
        {
            //Config examples
            //this.Plugins.Add(new PostmanFeature());
            //this.Plugins.Add(new CorsFeature());
            //var ClientUrl = AppSettings.Get<string>("ClientUrl");

            LogManager.LogFactory = new NLogFactory();

            this.ServiceExceptionHandlers.Add((httpReq, request, exception) => {
                //log your exceptions here

                //return null; //continue with default Error Handling
                LogManager.GetLogger("QzWebError").Error(exception);

                //return new ErrorDto() { Code = -1, ErrorMsg = exception.Message };

                //or return your own custom response
                return DtoUtils.CreateErrorResponse(request, exception);
            });

            //Handle Unhandled Exceptions occurring outside of Services
            //E.g. Exceptions during Request binding or in filters:
            this.UncaughtExceptionHandlers.Add((req, res, operationName, ex) => {
                res.Write($"Error: {ex.GetType().Name}: {ex.Message}");
                res.EndRequest(skipHeaders: true);
            });

            this.Plugins.Add(new CorsFeature(allowedOrigins: "*",
                allowedMethods: "*",
                allowedHeaders: "Origin, X-Requested-With, Content-Type, Accept,access_token,Authorization,Cache-Control",
                allowCredentials: false));

            Plugins.Add(new ValidationFeature());

            container.RegisterValidators(typeof(MyServices).Assembly);

            container.Register<ICacheClient>(new MemoryCacheClient());
            container.AddSingleton<IDbConnectionFactory>(dbsettings => ServiceStackDBContext.DbFactory);
            container.AddTransient<IRepsitory<tab_qrinfo>, ServiceStackRepsitory<tab_qrinfo>>();
            container.AddTransient<IRepsitory<view_programmeLiftList>, ServiceStackRepsitory<view_programmeLiftList>>();
            container.AddTransient<IRepsitory<view_testingSite>, ServiceStackRepsitory<view_testingSite>>();
            container.AddTransient<IRepsitory<view_programmePileList>, ServiceStackRepsitory<view_programmePileList>>();
            container.AddTransient<IRepsitory<tab_qz_report>, ServiceStackRepsitory<tab_qz_report>>();
            container.AddTransient<IRepsitory<t_bp_item>, ServiceStackRepsitory<t_bp_item>>();
            container.AddTransient<IRepsitory<view_projectinfo>, ServiceStackRepsitory<view_projectinfo>>();
            container.AddTransient<IRepsitory<tab_custominfo>, ServiceStackRepsitory<tab_custominfo>>();
            container.AddTransient<IRepsitory<tab_zj_updatelog>, ServiceStackRepsitory<tab_zj_updatelog>>();
            container.AddTransient<IRepsitory<view_programmeSecneList>, ServiceStackRepsitory<view_programmeSecneList>>();
            container.AddTransient<IRepsitory<view_pileDataList>, ServiceStackRepsitory<view_pileDataList>>();
            container.AddTransient<IRepsitory<Jy_BasicInfo>, ServiceStackRepsitory<Jy_BasicInfo>>();
            container.AddTransient<IRepsitory<Jy_TestingLogInfo>, ServiceStackRepsitory<Jy_TestingLogInfo>>();
            container.AddTransient<IRepsitory<view_pilephoto>, ServiceStackRepsitory<view_pilephoto>>();
            container.AddTransient<IRepsitory<tab_xc_report>, ServiceStackRepsitory<tab_xc_report>>();
            container.AddTransient<IRepsitory<view_GpsPileInfo>, ServiceStackRepsitory<view_GpsPileInfo>>();
            container.AddTransient<IRepsitory<view_testingHis>, ServiceStackRepsitory<view_testingHis>>();
            

            //container.AddTransient<IReportCoreService, ReportCoreService>();

            Plugins.Add(new SwaggerFeature());

            Plugins.Add(new RequestLogsFeature
            {
                RequestLogger = new CsvRequestLogger(),
            });
        }
    } 
}