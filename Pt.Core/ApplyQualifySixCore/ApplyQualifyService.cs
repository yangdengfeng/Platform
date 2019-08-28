using Pkpm.Entity;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pkpm.Core.ApplyQualifySixCore
{
    public class ApplyQualifyService : IApplyQualifyService
    {
        IDbConnectionFactory dbFactory;
        public ApplyQualifyService(IDbConnectionFactory dbFactory)
        {
            this.dbFactory = dbFactory;
        }


        public bool ImportPeople(List<string> strs, string unitcode, string pid,out string errorMsg)
        {
            errorMsg = string.Empty;
            using (var db = dbFactory.Open())
            {
                using (var dbTran = db.OpenTransaction())
                {
                    try
                    {
                        foreach (var item in strs)
                        {
                            t_D_UserTableSix six = new t_D_UserTableSix()
                            {
                                gzjl = item,
                                staitc = 0,
                                unitcode = unitcode,
                                pid = pid
                            };
                            db.Insert(six);
                        }
                        dbTran.Commit();
                        return true;
                    }
                    catch(Exception ex)
                    {
                        dbTran.Rollback();
                        errorMsg = ex.Message;
                        return false;
                    }
                }
            }
              
        }


        public bool Delete(string id ,out string errorMsg)
        {
            errorMsg = string.Empty;
            using (var db = dbFactory.Open())
            {
                try
                {
                    db.DeleteById<t_D_UserTableSix>(id);
                    return true;
                }catch(Exception ex)
                {
                    errorMsg = ex.Message;
                    return false;
                }
            }
        }
    }
}
