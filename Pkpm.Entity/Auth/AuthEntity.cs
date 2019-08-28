using Microsoft.AspNet.Identity;
using ServiceStack.DataAnnotations;
using ServiceStack.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pkpm.Entity
{
    [Serializable]
    [Alias("Role")]
    public partial class Role : IRole<int>, IHasId<int>
    {
        [Alias("Id")]
        [AutoIncrement]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
    }

    [Serializable]
    [Alias("User")]
    public partial class User : IUser<Int32>, IHasId<int>
    {
        [Alias("Id")]
        [AutoIncrement]
        public int Id { get; set; }
        public string Email { get; set; }
        [Required]
        public bool EmailConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public string PhoneNumber { get; set; }
        [Required]
        public bool PhoneNumberConfirmed { get; set; }
        [Required]
        public bool TwoFactorEnabled { get; set; }
        public DateTime? LockoutEndDateUtc { get; set; }
        [Required]
        public bool LockoutEnabled { get; set; }
        [Required]
        public int AccessFailedCount { get; set; }
        [Required]
        public string UserName { get; set; }
        public string Mobile { get; set; }
        public string Sex { get; set; }
        public string Status { get; set; }
        public string Grade { get; set; }
        public string CustomId { get; set; }
        public string UserDisplayName { get; set; }
        public string StationId { get; set; }
        public string OpenId { get; set; }
        public string UserCode { get; set; }
        public string photopath { get; set; }
        public string CheckStatus { get; set; }
        public string UnitName { get; set; }
        public string Valie { get; set; }
        public DateTime? CreateTime { get; set; }
        [Reference]
        public List<UserClaim> UserClaims { get; set; }

        [Reference]
        public List<UserLogin> UserLogins { get; set; }

        [Reference]
        public List<UserInRole> UserRoles { get; set; }
    }

    [Serializable]
    [Alias("UserClaim")]
    public partial class UserClaim : IHasId<int>
    {
        [Alias("Id")]
        [AutoIncrement]
        public int Id { get; set; }
        [Required]
        public int UserId { get; set; }
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }
    }

    [Serializable]
    [Alias("UserInRole")]
    public partial class UserInRole : IHasId<int>
    {
        [Alias("Id")]
        [AutoIncrement]
        public int Id { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public int RoleId { get; set; }
    }

    [Serializable]
    [Alias("UserLogin")]
    public partial class UserLogin : IHasId<int>
    {
        [Alias("Id")]
        [AutoIncrement]
        public int Id { get; set; }
        [Required]
        public string LoginProvider { get; set; }
        [Required]
        public string ProviderKey { get; set; }
        [Required]
        public int UserId { get; set; }
    }

    [Serializable]
    public class UserAsRole : IUser<Int32>, IHasId<Int32>
    {

        [AutoIncrement]
        public int Id { get; set; }

        public string Email { get; set; }

        [Required]
        public bool EmailConfirmed { get; set; }

        public string PasswordHash { get; set; }

        public string SecurityStamp { get; set; }

        public string PhoneNumber { get; set; }

        [Required]
        public bool PhoneNumberConfirmed { get; set; }

        [Required]
        public bool TwoFactorEnabled { get; set; }

        public DateTime? LockoutEndDateUtc { get; set; }

        [Required]
        public bool LockoutEnabled { get; set; }

        [Required]
        public int AccessFailedCount { get; set; }

        [Required]
        public string UserName { get; set; }

        public string Mobile { get; set; }

        public string Sex { get; set; }

        public string Status { get; set; }

        public string Grade { get; set; }

        public string CustomId { get; set; }

        public string UserDisplayName { get; set; }

        public string StationId { get; set; }

        public string OpenId { get; set; }

        public string UserCode { get; set; }
        /// <summary>
        /// 审核状态，1已审核，0未审核，检测人员注册时未1，见证、取样、监督人员注册时为0需要管理员审核后为1才能登陆
        /// </summary>
        public string CheckStatus { get; set; }

        [Reference]
        public List<UserClaim> UserClaims { get; set; }

        [Reference]
        public List<UserLogin> UserLogins { get; set; }

        [Reference]
        public List<UserInRole> UserRoles { get; set; }
        public string UnitName { get; set; }
        public string photopath { get; set; }
        public string cardphotopath { get; set; }
        public int RoleId { get; set; }

        public string Valie { get; set; }
    }


}
