using Microsoft.AspNet.Identity;
using Pkpm.Entity;
using Pkpm.Framework.Repsitory;
using Pkpm.Framework.Validation;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspNet.Identity.ServiceStack
{
    public class RoleStore : IRoleStore<Role, int>,
        IQueryableRoleStore<Role,int>
    {
        private readonly IDbConnectionFactory dbFactory;

        public RoleStore(IDbConnectionFactory dbFactory)
        {
            this.dbFactory = dbFactory;
        }

        public IQueryable<Role> Roles
        {
            get
            {
                using (var db = dbFactory.Open())
                {
                    return db.Select<Role>().AsQueryable();
                }
            }
        }

        public Task CreateAsync(Role role)
        {
            Argument.ThrowIfNull(role, "role");

            using (var db = dbFactory.Open())
            {
                var result = db.Insert(role);
                return Task.FromResult(result);
            }
        }

        public Task DeleteAsync(Role role)
        {
            Argument.ThrowIfNull(role, "role");

            using (var db = dbFactory.Open())
            {
                var result = db.DeleteById<Role>(role.Id);
                return Task.FromResult(result);
            }
        }

        public void Dispose()
        {
             
        }

        public Task<Role> FindByIdAsync(int roleId)
        {
            Argument.Validate(roleId > 0, "roleId");

            using (var db=dbFactory.Open())
            {
                var result = db.SingleById<Role>(roleId);
                return Task.FromResult(result);
            }
        }

        public Task<Role> FindByNameAsync(string roleName)
        {
            Argument.ThrowIfNullOrEmpty(roleName, "roleName");

            using (var db = dbFactory.Open())
            {
                var result = db.SingleWhere<Role>("Name", roleName);
                return Task.FromResult(result);
            }
        }

        public Task UpdateAsync(Role role)
        {
            Argument.ThrowIfNull(role, "role");

            using (var db = dbFactory.Open())
            {
                var result = db.Update(role);
                return Task.FromResult(result);
            }
        }
    }
}
