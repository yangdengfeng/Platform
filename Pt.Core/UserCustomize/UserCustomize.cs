using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pkpm.Entity;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using Pkpm.Framework.Cache;
using ServiceStack;
using Pkpm.Entity.Auth;

namespace Pkpm.Core.UserCustomize
{
    public class UserCustomize : IUserCustomize
    {
        IDbConnectionFactory dbFactory;
        ICache<InstShortInfos> cacheInsts;

        public UserCustomize(IDbConnectionFactory dbFactory, 
            ICache<InstShortInfos> cacheInsts)
        {
            this.dbFactory = dbFactory;
            this.cacheInsts = cacheInsts;
        }

        public bool SetUserCustom(int userId, List<UserInCustom> userInCustoms, UserCustomType customType, out string errorMsg)
        {
            using (var db = dbFactory.Open())
            {
                using (var dbTrans = db.OpenTransaction())
                {
                    try
                    {
                        errorMsg = string.Empty;

                        db.Delete<UserInCustom>(u => u.UserId == userId && u.UserCustomType == customType);
                        foreach (var item in userInCustoms)
                        {
                            db.Insert(item);
                        }

                        dbTrans.Commit();
                        if(customType== UserCustomType.UserLogCustom)
                        {
                            cacheInsts.Remove(PkPmCacheKeys.CustomsByUserIdFmt.Fmt(userId));
                        } 
                        return true;

                    }
                    catch (Exception ex)
                    {
                        dbTrans.Rollback();
                        errorMsg = ex.Message;
                        return false;
                    }
                }
            }
        }

        public bool SetUserItem(int userId, List<UserInItem> userInItems, UserItemType itemType, out string errorMsg)
        {
            using (var db = dbFactory.Open())
            {
                using (var dbTrans = db.OpenTransaction())
                {
                    try
                    {
                        errorMsg = string.Empty;

                        db.Delete<UserInItem>(u => u.UserId == userId && u.UserItemType == itemType);
                        foreach (var item in userInItems)
                        {
                            db.Insert(item);
                        }

                        dbTrans.Commit();
                        return true;

                    }
                    catch (Exception ex)
                    {
                        dbTrans.Rollback();
                        errorMsg = ex.Message;
                        return false;
                    }
                }
            }
        }
    }
}
