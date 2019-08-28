using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pkpm.Framework.Logging
{
    public enum LogLevel
    {
        Debug,
        Info,
        Warning,
        Error,
        Fatal
    }

    public interface ILogger
    {
        bool IsEnabled(LogLevel level);
        void Log(LogLevel level, Exception exception, string format, params object[] args);
    }
}
