using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pkpm.Framework.Logging
{
    public class NLogLoggerFactory : ILoggerFactory
    {
        public ILogger CreateLogger(Type type)
        {
            return new NLogLogger(type.FullName);
        }

        public ILogger CreateLogger(string name)
        {
            return new NLogLogger(name);
        }
    }
}
