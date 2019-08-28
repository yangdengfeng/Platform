
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNet.Identity;
using Pkpm.Entity;
using ServiceStack.OrmLite;
using ServiceStack.Data;
using Pkpm.Framework.Validation;
using Pkpm.Framework.Repsitory;

namespace AspNet.Identity.ServiceStack
{
    public class UserStore : 
        IUserStore<User, Int32>,
        IUserClaimStore<User, Int32>,
        IUserLoginStore<User, Int32>,
        IUserRoleStore<User, Int32>,
        IUserPasswordStore<User, Int32>,
        IUserSecurityStampStore<User, Int32>,
        IUserTwoFactorStore<User, Int32>,
        IUserPhoneNumberStore<User, Int32>,
        IUserEmailStore<User, Int32>,
        IUserLockoutStore<User, Int32>,
        IQueryableUserStore<User, Int32>
    {
        private readonly IDbConnectionFactory dbFactory;

        public UserStore(IDbConnectionFactory dbFactory)
        {
            this.dbFactory = dbFactory;
        }

        public IQueryable<User> Users
        {
            get
            {
                using (var db = dbFactory.Open())
                {
                    return db.Select<User>().AsQueryable();
                }
            }
        }

        #region IUserStore
        public Task CreateAsync(User user)
        {
            using (var db = dbFactory.Open())
            {
                db.Save<User>(user,references:true);
                return Task.FromResult(0);
                //return db.SaveAsync<User>(user, references: true);
            }
        }

        public Task DeleteAsync(User user)
        {
            Argument.ThrowIfNull(user, "user");

            using (var db = dbFactory.Open())
            {
                var result = db.DeleteById<User>(user.Id);
                return Task.FromResult(0);
                //return db.DeleteByIdAsync<User>(user.Id);
            }
        }

        public void Dispose()
        {

        }

        public Task<User> FindByIdAsync(int userId)
        {
            using (var db = dbFactory.Open())
            {
                User user = db.SingleById<User>(userId);
                return Task.FromResult(user);
            }
        }

        public Task<User> FindByNameAsync(string userName)
        {
            Argument.ThrowIfNullOrEmpty(userName, "userName");

            using (var db = dbFactory.Open())
             {
                List<User> users = db.Select<User>(u => u.UserName == userName);//已审核的才能登陆
                return Task.FromResult(users.FirstOrDefault());
            }
        }

        public Task UpdateAsync(User user)
        {
            Argument.ThrowIfNull(user, "user");

            using (var db = dbFactory.Open())
            {
                db.Update<User>(user);
                return Task.FromResult(0);
            }
        }
        #endregion

        #region IUserClaimStore
        public Task AddClaimAsync(User user, Claim claim)
        {
            Argument.ThrowIfNull(user, "user");
            Argument.ThrowIfNull(claim, "claim");

            UserClaim userClaim = new UserClaim()
            {
                UserId = user.Id,
                ClaimType = claim.Type,
                ClaimValue = claim.Value
            };

            user.UserClaims.Add(userClaim);
            using (var db = dbFactory.Open())
            {
                db.Insert(userClaim);
                return Task.FromResult(0);
            }
        }

        public Task<IList<Claim>> GetClaimsAsync(User user)
        {
            Argument.ThrowIfNull(user, "user");

            IList<Claim> claims = new List<Claim>();
            if (user.UserClaims != null)
            {
                claims = user.UserClaims.Select(c => new Claim(c.ClaimType, c.ClaimValue)).ToList();
            }
             

            
            return Task.FromResult(claims);
        }

        public Task RemoveClaimAsync(User user, Claim claim)
        {
            Argument.ThrowIfNull(user, "user");

            using (var db = dbFactory.Open())
            {
                Int32 result = db.Delete<UserClaim>(uc => uc.UserId == user.Id && uc.ClaimType == claim.Type && uc.ClaimValue == claim.Value);
                return Task.FromResult(result);
            }
        }

        #endregion

        #region IUserLoginStore
        public Task AddLoginAsync(User user, UserLoginInfo login)
        {
            Argument.ThrowIfNull(user, "user");
            Argument.ThrowIfNull(login, "login");


            UserLogin userLogin = new UserLogin()
            {
                UserId = user.Id,
                LoginProvider = login.LoginProvider,
                ProviderKey = login.ProviderKey
            };

            user.UserLogins.Add(userLogin);
            using (var db = dbFactory.Open())
            {
                var result = db.Insert(userLogin);
                return Task.FromResult(result);
            }
        }

        public Task RemoveLoginAsync(User user, UserLoginInfo login)
        {
            Argument.ThrowIfNull(user, "user");
            Argument.ThrowIfNull(login, "login");

            using (var db = dbFactory.Open())
            {
                var result=db.Delete<UserLogin>(ul => ul.UserId == user.Id && ul.LoginProvider == login.LoginProvider && ul.ProviderKey == login.ProviderKey);
                return Task.FromResult(result);
            }
        }

        public Task<IList<UserLoginInfo>> GetLoginsAsync(User user)
        {
            Argument.ThrowIfNull(user, "user");

            IList<UserLoginInfo> userLoginInfos = user.UserLogins.Select(l => new UserLoginInfo(l.LoginProvider, l.ProviderKey)).ToList();
            return Task.FromResult(userLoginInfos);
        }

        public Task<User> FindAsync(UserLoginInfo login)
        {
            Argument.ThrowIfNull(login, "login");

            using (var db = dbFactory.Open())
            {
                SqlExpression<User> sqlExp = db.From<User>()
                                              .Join<UserLogin>((u, ul) => u.Id == ul.UserId)
                                              .Where<UserLogin>(ul => ul.LoginProvider == login.LoginProvider && ul.ProviderKey == login.ProviderKey)
                                              .SelectDistinct();
                List<User> users = db.LoadSelect(sqlExp);
                return Task.FromResult(users.FirstOrDefault());
            }
        }
        #endregion

        #region IUserRoleStore
        public Task AddToRoleAsync(User user, string roleName)
        {
            Argument.ThrowIfNull(user, "user");
            Argument.ThrowIfNullOrEmpty(roleName, "roleName");

            using (var db = dbFactory.Open())
            {
                Role role = db.Single<Role>(r => r.Name == roleName);
                if (role != null)
                {
                    UserInRole userRole = new UserInRole() { UserId = user.Id, RoleId = role.Id };
                    user.UserRoles.Add(userRole);
                    var result = db.Insert(userRole);
                    return Task.FromResult(result);
                }
                else
                {
                    Int64 result = -1;
                    return Task.FromResult(result);
                }
            }
        }

        public Task RemoveFromRoleAsync(User user, string roleName)
        {
            Argument.ThrowIfNull(user, "user");
            Argument.ThrowIfNullOrEmpty(roleName, "roleName");

            using (var db = dbFactory.Open())
            {
                Role role = db.Single<Role>(r => r.Name == roleName);
                if (role != null)
                {
                    var result=db.Delete<UserInRole>(ur => ur.UserId == user.Id && ur.RoleId == role.Id);
                    return Task.FromResult(result);
                }
                else
                {
                    return Task.FromResult(-1);
                }
            }
        }

        public Task<IList<string>> GetRolesAsync(User user)
        {
            Argument.ThrowIfNull(user, "user");

            using (var db = dbFactory.Open())
            {
                SqlExpression<Role> sqlExp = db.From<Role>()
                                            .Join<UserInRole>((r, ur) => r.Id == ur.RoleId)
                                            .Where<UserInRole>(ur => ur.UserId == user.Id)
                                            .SelectDistinct();

                List<Role> roles = db.Select<Role>(sqlExp);
                IList<string> roleNames = roles.Select(r => r.Name).ToList();
                return Task.FromResult(roleNames);
            }
        }

        public Task<bool> IsInRoleAsync(User user, string roleName)
        {
            Argument.ThrowIfNull(user, "user");
            Argument.ThrowIfNullOrEmpty(roleName, "roleName");

            using (var db = dbFactory.Open())
            {
                SqlExpression<UserInRole> sqlExp = db.From<UserInRole>()
                                            .Join<User>((ur, u) => ur.UserId == u.Id)
                                            .Join<Role>((ur, r) => ur.RoleId == r.Id)
                                            .Where<Role>(r => r.Name == roleName)
                                            .Select();
                var result= db.Exists<UserInRole>(sqlExp);
                return Task.FromResult(result);
            }
        }


        #endregion

        #region IUserPasswordStore
        public Task SetPasswordHashAsync(User user, string passwordHash)
        {
            Argument.ThrowIfNull(user, "user");
            Argument.ThrowIfNullOrEmpty(passwordHash, "passwordHash");

            user.PasswordHash = passwordHash;
            return Task.FromResult(0);
        }

        public Task<string> GetPasswordHashAsync(User user)
        {
            Argument.ThrowIfNull(user, "user");
            return Task.FromResult(user.PasswordHash);
        }

        public Task<bool> HasPasswordAsync(User user)
        {
            Argument.ThrowIfNull(user, "user");

            return Task.FromResult(user.PasswordHash != null);
        }


        #endregion

        #region IUserSecurityStampStore
        public Task SetSecurityStampAsync(User user, string stamp)
        {
            Argument.ThrowIfNull(user, "user");
            Argument.ThrowIfNullOrEmpty(stamp, "stamp");

            user.SecurityStamp = stamp;
            return Task.FromResult(0);
        }

        public Task<string> GetSecurityStampAsync(User user)
        {
            Argument.ThrowIfNull(user, "user");

            return Task.FromResult(user.SecurityStamp);
        }


        #endregion

        #region IUserTwoFactorStore
        public Task SetTwoFactorEnabledAsync(User user, bool enabled)
        {
            Argument.ThrowIfNull(user, "user");

            user.TwoFactorEnabled = enabled;
            return Task.FromResult(0);
        }

        public Task<bool> GetTwoFactorEnabledAsync(User user)
        {
            Argument.ThrowIfNull(user, "user");

            return Task.FromResult(user.TwoFactorEnabled);
        }


        #endregion

        #region IUserPhoneNumberStore
        public Task SetPhoneNumberAsync(User user, string phoneNumber)
        {
            Argument.ThrowIfNull(user, "user");
            Argument.ThrowIfNullOrEmpty(phoneNumber, "phoneNumber");

            user.PhoneNumber = phoneNumber;
            return Task.FromResult(0);
        }

        public Task<string> GetPhoneNumberAsync(User user)
        {
            Argument.ThrowIfNull(user, "user");

            return Task.FromResult(user.PhoneNumber);
        }

        public Task<bool> GetPhoneNumberConfirmedAsync(User user)
        {
            Argument.ThrowIfNull(user, "user");

            return Task.FromResult(user.PhoneNumberConfirmed);
        }

        public Task SetPhoneNumberConfirmedAsync(User user, bool confirmed)
        {
            Argument.ThrowIfNull(user, "user");

            user.PhoneNumberConfirmed = confirmed;
            return Task.FromResult(0);
        }


        #endregion

        #region IUserEmailStore
        public Task SetEmailAsync(User user, string email)
        {
            Argument.ThrowIfNull(user, "user");
            Argument.ThrowIfNullOrEmpty(email, "email");

            user.Email = email;
            return Task.FromResult(0);
        }

        public Task<string> GetEmailAsync(User user)
        {
            Argument.ThrowIfNull(user, "user");

            return Task.FromResult(user.Email);
        }

        public Task<bool> GetEmailConfirmedAsync(User user)
        {
            Argument.ThrowIfNull(user, "user");

            return Task.FromResult(user.EmailConfirmed);
        }

        public Task SetEmailConfirmedAsync(User user, bool confirmed)
        {
            Argument.ThrowIfNull(user, "user");

            user.EmailConfirmed = confirmed;
            return Task.FromResult(0);
        }

        public Task<User> FindByEmailAsync(string email)
        {
            Argument.ThrowIfNullOrEmpty(email, "email");

            using (var db = dbFactory.Open())
            {
                List<User> users = db.LoadSelect<User>(u => u.Email == email);
                return Task.FromResult(users.FirstOrDefault());
            }
        }


        #endregion

        #region IUserLockoutStore
        public Task<DateTimeOffset> GetLockoutEndDateAsync(User user)
        {
            Argument.ThrowIfNull(user, "user");

            return
                Task.FromResult(user.LockoutEndDateUtc.HasValue
                    ? new DateTimeOffset(DateTime.SpecifyKind(user.LockoutEndDateUtc.Value, DateTimeKind.Utc))
                    : new DateTimeOffset());
        }

        public Task SetLockoutEndDateAsync(User user, DateTimeOffset lockoutEnd)
        {
            Argument.ThrowIfNull(user, "user");

            user.LockoutEndDateUtc = lockoutEnd == DateTimeOffset.MinValue ? (DateTime?)null : lockoutEnd.UtcDateTime;
            return Task.FromResult(0);
        }

        public Task<int> IncrementAccessFailedCountAsync(User user)
        {
            Argument.ThrowIfNull(user, "user");

            user.AccessFailedCount++;
            return Task.FromResult(user.AccessFailedCount);
        }

        public Task ResetAccessFailedCountAsync(User user)
        {
            Argument.ThrowIfNull(user, "user");

            user.AccessFailedCount = 0;
            return Task.FromResult(0);
        }

        public Task<int> GetAccessFailedCountAsync(User user)
        {
            Argument.ThrowIfNull(user, "user");

            return Task.FromResult(user.AccessFailedCount);
        }

        public Task<bool> GetLockoutEnabledAsync(User user)
        {
            Argument.ThrowIfNull(user, "user");

            return Task.FromResult(user.LockoutEnabled);
        }

        public Task SetLockoutEnabledAsync(User user, bool enabled)
        {
            Argument.ThrowIfNull(user, "user");

            user.LockoutEnabled = enabled;
            return Task.FromResult(0);
        } 
        #endregion
    }
}
