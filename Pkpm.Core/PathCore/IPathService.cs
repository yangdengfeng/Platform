using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pkpm.Core.PathCore
{
    public interface IPathService
    {
        bool AddPath(Entity.Path path, out string errorMsg);
        bool EditPath(Entity.Path path, out string errorMsg);
        bool DeltePath(int pathId, out string errorMsg);

        bool AddPathAction(Entity.Action action, out string errorMsg);
        bool EditPathAction(Entity.Action action, out string errorMsg);
        bool DeletePathAction(int actionId, out string errorMsg);
    }
}
