using Pkpm.Core.SysDictCore;
using Pkpm.Entity;
using Pkpm.Framework.Repsitory;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pkpm.Core.SysInfoCore
{
    public class SysInfoService: ISysInfoService
    {
        IDbConnectionFactory dbFactory;
        IRepsitory<t_bp_PkpmJCRU> rep;
        public SysInfoService(IDbConnectionFactory dbFactory,IRepsitory<t_bp_PkpmJCRU> rep)
        {
            this.dbFactory = dbFactory;
            this.rep = rep;
        }

        public bool EditInfo(int id ,string informationName,string content,string type,out string errorMsg)
        {
            using (var  db = dbFactory.Open())
            {
                using (var dbTrans = db.OpenTransaction())
                {
                    try
                    {
                        errorMsg = string.Empty;
                        db.UpdateOnly(() => new t_bp_PkpmJCRU() { name = informationName, content = content,type=type }, t => t.ID == id);
                        dbTrans.Commit();
                        return true;
                    }catch(Exception ex)
                    {
                        dbTrans.Rollback();
                        errorMsg = ex.Message;
                        return false;
                    }
                }
            }
        }

        public bool DeleteInfo(int id ,out string errorMsg)
        {
            using (var db = dbFactory.Open())
            {
                using (var dbTrans = db.OpenTransaction())
                {
                    try
                    {
                        errorMsg = string.Empty;
                        db.Delete<t_bp_PkpmJCRU>(t => t.ID == id);
                        // db.Delete(new t_bp_PkpmJCRU { ID = id });
                        dbTrans.Commit();
                        return true;
                    }catch(Exception ex)
                    {
                        dbTrans.Rollback();
                        errorMsg = ex.Message;
                        return false;
                    }
                }
            }
        }

        public bool CreateInfo(string name,string content,string type,string addTime,out string errorMsg)
        {
            errorMsg = string.Empty;
            try
            {
                t_bp_PkpmJCRU pkpmJCRU = new t_bp_PkpmJCRU
                {
                    name = name,
                    content = content,
                    type = type,
                    addtime = addTime,

                };
                rep.Insert(pkpmJCRU);
                return true;
            }catch(Exception ex)
            {
                errorMsg = ex.Message;
                return false;
            }
        }
    }
}
