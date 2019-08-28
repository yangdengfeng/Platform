using Pkpm.Entity;
using Pkpm.Framework.Repsitory;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pkpm.Core.SoftwareVersService
{
    public class SoftwareVersService: ISoftwareVersService
    {
        IDbConnectionFactory dbFactory;
        IRepsitory<t_sys_Version> rep;
        public SoftwareVersService(IDbConnectionFactory dbFactory, IRepsitory<t_sys_Version> rep)
        {
            this.dbFactory = dbFactory;
            this.rep = rep;
        }
        public bool EditSoftwareVers(int id, string userCode, string userName, string FileVersionDate, string FileVersion, string EndDate, out string errorMsg)
        {

            using (var db = dbFactory.Open())
            {
                using (var dbTrans = db.OpenTransaction())
                {
                    try
                    {
                        errorMsg = string.Empty;
                        db.UpdateOnly(() => new t_sys_Version() { name = userName,
                            usercode = userCode,
                            FileVersion =FileVersion,
                            FileVersionDate=FileVersionDate,
                            EndDate=EndDate}, t => t.id == id);
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
