using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pkpm.Entity;
using Pkpm.Framework.Repsitory;
using Pkpm.Framework.Cache; 

namespace Pkpm.Core.PathCore
{
    public class PathService : IPathService
    {
        IRepsitory<Path> pathRep;
        IRepsitory<Entity.Action> actionRep;
        ICache<List<Path>> pathCache;

        public PathService(IRepsitory<Path> pathRep,
            IRepsitory<Entity.Action> actionRep,
            ICache<List<Path>> pathCache)
        {
            this.pathRep = pathRep;
            this.actionRep = actionRep;
            this.pathCache = pathCache;
        }

        public bool AddPath(Path path, out string errorMsg)
        {
            errorMsg = string.Empty;

            try
            {
                pathRep.Insert(path);
                if (path.IsCategory)
                {
                    pathCache.Remove(PkPmCacheKeys.CategoryPaths);
                }
                else
                {
                    pathCache.Clear();
                }
                return true;
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                return false;
            }
        }

        public bool EditPath(Path path, out string errorMsg)
        {
            errorMsg = string.Empty;

            try
            {
                int updateCount = pathRep.Update(path);
                if (path.IsCategory)
                {
                    pathCache.Remove(PkPmCacheKeys.CategoryPaths);
                }
                else
                {
                    pathCache.Clear();
                }
                return true;
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                return false;
            }
        }

        public bool DeltePath(int pathId, out string errorMsg)
        {
            errorMsg = string.Empty;

            try
            {
                var existPath = pathRep.GetById(pathId);
                bool isCategoryPath = false;
                if(existPath!=null && existPath.IsCategory)
                {
                    isCategoryPath = true;
                }
                int delCount = pathRep.DeleteById(pathId);
                if(isCategoryPath)
                {
                    pathCache.Remove(PkPmCacheKeys.CategoryPaths);
                }
                else
                {
                    pathCache.Clear();
                }
                return true;
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                return false;
            }
        }

        public bool AddPathAction(Entity.Action action, out string errorMsg)
        {
            errorMsg = string.Empty;

            try
            {
                actionRep.Insert(action);
                return true;
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                return false;
            }
        }

        public bool DeletePathAction(int actionId, out string errorMsg)
        {
            errorMsg = string.Empty;

            try
            {
                int delCount = actionRep.DeleteById(actionId);
                return true;
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                return false;
            }
        }

        public bool EditPathAction(Entity.Action action, out string errorMsg)
        {
            errorMsg = string.Empty;

            try
            {
                int delCount = actionRep.Update(action);
                return true;
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                return false;
            }
        }
    }
}
