using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pkpm.Framework.Logging
{
    public class NLogLogger : ILogger
    {
        private readonly NLog.ILogger nlogLogger;

        public NLogLogger(string name)
        {
            this.nlogLogger = NLog.LogManager.GetLogger(name);
        }

        public bool IsEnabled(LogLevel level)
        {
            switch (level)
            {
                case LogLevel.Debug:
                    return nlogLogger.IsDebugEnabled;
                case LogLevel.Info:
                    return nlogLogger.IsInfoEnabled;
                case LogLevel.Warning:
                    return nlogLogger.IsWarnEnabled;
                case LogLevel.Error:
                    return nlogLogger.IsErrorEnabled;
                case LogLevel.Fatal:
                    return nlogLogger.IsFatalEnabled;
                default:
                    break;
            }

            return false;
        }

        public void Log(LogLevel level, Exception exception, string format, params object[] args)
        {
            if (args == null)
            {
                switch (level)
                {
                    case LogLevel.Debug:
                        nlogLogger.Debug(exception, format);
                        break;
                    case LogLevel.Info:
                        nlogLogger.Info(exception, format);
                        break;
                    case LogLevel.Warning:
                        nlogLogger.Warn(exception, format);
                        break;
                    case LogLevel.Error:
                        nlogLogger.Error(exception, format);
                        break;
                    case LogLevel.Fatal:
                        nlogLogger.Fatal(exception, format);
                        break;
                    default:
                        break;
                }

            }
            else
            {
                switch (level)
                {
                    case LogLevel.Debug:
                        nlogLogger.Debug(exception, format, args);
                        break;
                    case LogLevel.Info:
                        nlogLogger.Info(exception, format, args);
                        break;
                    case LogLevel.Warning:
                        nlogLogger.Warn(exception, format, args);
                        break;
                    case LogLevel.Error:
                        nlogLogger.Error(exception, format, args);
                        break;
                    case LogLevel.Fatal:
                        nlogLogger.Fatal(exception, format, args);
                        break;
                    default:
                        break;
                }
            }

        }
    }
}
