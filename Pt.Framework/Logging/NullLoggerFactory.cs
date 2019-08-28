using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pkpm.Framework.Logging
{
    public class NullLoggerFactory : ILoggerFactory
    {
        public ILogger CreateLogger(Type type)
        {
            return NullLogger.Instance;
        }

        public ILogger CreateLogger(string name)
        {
            return NullLogger.Instance;
        }
    }
}
