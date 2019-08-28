﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pkpm.Framework.Logging
{
    public interface ILoggerFactory
    {
        ILogger CreateLogger(Type type);
        ILogger CreateLogger(string name);
    }
}
