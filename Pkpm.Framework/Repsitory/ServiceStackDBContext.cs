using ServiceStack.Data;
using ServiceStack.OrmLite;
using StackExchange.Profiling;
using StackExchange.Profiling.Data;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Pkpm.Framework.Common;

namespace Pkpm.Framework.Repsitory
{
    public static class ServiceStackDBContext
    {
        public static IDbConnectionFactory GetDBFactory(string connectStr)
        {
            IDbConnectionFactory factory = new OrmLiteConnectionFactory(
                       connectStr,
                       SqlServerDialect.Provider)
            {
                ConnectionFilter = x => new ProfiledDbConnection((DbConnection)x, MiniProfiler.Current)
            };
            OrmLiteConfig.DialectProvider.GetStringConverter().UseUnicode = true;
            return factory;
        }

        private static Lazy<IDbConnectionFactory> dbFactory =
            new Lazy<IDbConnectionFactory>(() =>
            {
                IDbConnectionFactory factory = new OrmLiteConnectionFactory(
                      SymCryptoUtility.Decrypt(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString),
                       SqlServerDialect.Provider)
                {
                    ConnectionFilter = x => new ProfiledDbConnection((DbConnection)x, MiniProfiler.Current)
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

        private static Lazy<IDbConnectionFactory> qrDbFactory =
            new Lazy<IDbConnectionFactory>(() =>
            {
                IDbConnectionFactory factory = new OrmLiteConnectionFactory(
                       SymCryptoUtility.Decrypt(System.Configuration.ConfigurationManager.ConnectionStrings["QRBarConnection"].ConnectionString),
                       SqlServerDialect.Provider)
                {
                    ConnectionFilter = x => new ProfiledDbConnection((DbConnection)x, MiniProfiler.Current)
                };
                OrmLiteConfig.DialectProvider.GetStringConverter().UseUnicode = true;
                return factory;
            }, true);

        public static IDbConnectionFactory QRDbFactory
        {
            get
            {
                return qrDbFactory.Value;
            }
        }
        private static Lazy<IDbConnectionFactory> bhDbFactory =
            new Lazy<IDbConnectionFactory>(() =>
            {
                IDbConnectionFactory factory = new OrmLiteConnectionFactory(
                       SymCryptoUtility.Decrypt(System.Configuration.ConfigurationManager.ConnectionStrings["BHConnection"].ConnectionString),
                       SqlServerDialect.Provider)
                {
                    ConnectionFilter = x => new ProfiledDbConnection((DbConnection)x, MiniProfiler.Current)
                };
                OrmLiteConfig.DialectProvider.GetStringConverter().UseUnicode = true;
                return factory;
            }, true);

        public static IDbConnectionFactory BHDbFactory
        {
            get
            {
                return bhDbFactory.Value;
            }
        }
    }
}
