using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pkpm.Framework.Logging
{
  public static  class LoggingExtensions
    {
        public static void Debug(this ILogger logger, string message)
        {
            FilteredLog(logger, LogLevel.Debug, null, message, null);
        }
        public static void Information(this ILogger logger, string message)
        {
            FilteredLog(logger, LogLevel.Info, null, message, null);
        }
        public static void Warning(this ILogger logger, string message)
        {
            FilteredLog(logger, LogLevel.Warning, null, message, null);
        }
        public static void Error(this ILogger logger, string message)
        {
            FilteredLog(logger, LogLevel.Error, null, message, null);
        }
        public static void Fatal(this ILogger logger, string message)
        {
            FilteredLog(logger, LogLevel.Fatal, null, message, null);
        }

        public static void Debug(this ILogger logger, Exception exception)
        {
            FilteredLog(logger, LogLevel.Debug, exception, string.Empty, null);
        }
        public static void Information(this ILogger logger, Exception exception)
        {
            FilteredLog(logger, LogLevel.Info, exception, string.Empty, null);
        }
        public static void Warning(this ILogger logger, Exception exception)
        {
            FilteredLog(logger, LogLevel.Warning, exception, string.Empty, null);
        }
        public static void Error(this ILogger logger, Exception exception)
        {
            FilteredLog(logger, LogLevel.Error, exception, string.Empty, null);
        }
        public static void Fatal(this ILogger logger, Exception exception)
        {
            FilteredLog(logger, LogLevel.Fatal, exception, string.Empty, null);
        }

        public static void Debug(this ILogger logger, Exception exception, string message)
        {
            FilteredLog(logger, LogLevel.Debug, exception, message, null);
        }
        public static void Information(this ILogger logger, Exception exception, string message)
        {
            FilteredLog(logger, LogLevel.Info, exception, message, null);
        }
        public static void Warning(this ILogger logger, Exception exception, string message)
        {
            FilteredLog(logger, LogLevel.Warning, exception, message, null);
        }
        public static void Error(this ILogger logger, Exception exception, string message)
        {
            FilteredLog(logger, LogLevel.Error, exception, message, null);
        }
        public static void Fatal(this ILogger logger, Exception exception, string message)
        {
            FilteredLog(logger, LogLevel.Fatal, exception, message, null);
        }

        public static void Debug(this ILogger logger, string format, params object[] args)
        {
            FilteredLog(logger, LogLevel.Debug, null, format, args);
        }
        public static void Information(this ILogger logger, string format, params object[] args)
        {
            FilteredLog(logger, LogLevel.Info, null, format, args);
        }
        public static void Warning(this ILogger logger, string format, params object[] args)
        {
            FilteredLog(logger, LogLevel.Warning, null, format, args);
        }
        public static void Error(this ILogger logger, string format, params object[] args)
        {
            FilteredLog(logger, LogLevel.Error, null, format, args);
        }
        public static void Fatal(this ILogger logger, string format, params object[] args)
        {
            FilteredLog(logger, LogLevel.Fatal, null, format, args);
        }

        public static void Debug(this ILogger logger, Exception exception, string format, params object[] args)
        {
            FilteredLog(logger, LogLevel.Debug, exception, format, args);
        }
        public static void Information(this ILogger logger, Exception exception, string format, params object[] args)
        {
            FilteredLog(logger, LogLevel.Info, exception, format, args);
        }
        public static void Warning(this ILogger logger, Exception exception, string format, params object[] args)
        {
            FilteredLog(logger, LogLevel.Warning, exception, format, args);
        }
        public static void Error(this ILogger logger, Exception exception, string format, params object[] args)
        {
            FilteredLog(logger, LogLevel.Error, exception, format, args);
        }
        public static void Fatal(this ILogger logger, Exception exception, string format, params object[] args)
        {
            FilteredLog(logger, LogLevel.Fatal, exception, format, args);
        }

        private static void FilteredLog(ILogger logger, LogLevel level, Exception exception, string format, object[] objects)
        {
            if (logger.IsEnabled(level))
            {
                logger.Log(level, exception, format, objects);
            }
        }
    }
}
