using ServiceStack.Data;
using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QZWebService.ServiceInterface.Repsitory
{
    public static class ServiceStackDBContext
    {
        public static IDbConnectionFactory GetDBFactory(string connectStr)
        {
            IDbConnectionFactory factory = new OrmLiteConnectionFactory(
                       connectStr,
                       SqlServerDialect.Provider)
            {
            };
            OrmLiteConfig.DialectProvider.GetStringConverter().UseUnicode = true;
            return factory;
        }

        private static Lazy<IDbConnectionFactory> dbFactory =
            new Lazy<IDbConnectionFactory>(() =>
            {
                IDbConnectionFactory factory = new OrmLiteConnectionFactory(
                      System.Configuration.ConfigurationManager.ConnectionStrings["QZQrCodeConnection"].ConnectionString,
                       SqlServerDialect.Provider)
                {
                };
                OrmLiteConfig.DialectProvider.GetStringConverter().UseUnicode = true;
                return factory;
            }, true);

        public static IDbConnectionFactory DbFactory
        {
            get
            {
                return dbFactory.Value;
            }
        }

       
    }
}
