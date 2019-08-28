using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PkpmGX.Models
{
    public class SysViewModels
    {
        public Dictionary<string, string> CheckInsts { get; set; }
    }

    public class SysViewSearchModels
    {
        public string CheckUnitName { get; set; }
        public string UserName { get; set; }
        public string UserDisplayName { get; set; }
        public string RoleType { get; set; }
        public int? posStart { get; set; }
        public int? count { get; set; }
        public string RoleNames { get; set; }
        public string CheckStatus { get; set; }
        public string Valie { get; set; }
    }

    public class SysEditViewModel
    {
        public string InstId { get; set; }
        public string DisplayName { get; set; }
        public string UserName { get; set; }
        public string UserSex { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Status { get; set; }
        public string Grade { get; set; }
        public int UserId { get; set; }
        public int? RoleId { get; set; }
        public string UserCode { get; set; }
        public string Mobile { get; set; }
        public string Valie { get; set; }
    }

    public class SysInsertViewModel : SysEditViewModel
    {
        public string UserPwd { get; set; }
    }

    public class SysResetPwdViewModel
    {
        public int UserId { get; set; }
        public string ResetPwd { get; set; }
        public string PwdConfirm { get; set; }
    }

    public class SysChangedPwdViewModel
    {
        public int UserId { get; set; }
        public string UserCode { get; set; }
        public string ResetPwd { get; set; }
        public string PwdConfirm { get; set; }
    }

    public class UserInfoDetailsModel
    {
        public string IDCard { get; set; }
        public string UnitName { get; set; }
        public string Name { get; set; }
        public string Sex { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Status { get; set; }
        public string Photopath { get; set; }
        public string Cardphotopath { get; set; }
        public string UserCardNum { get; set; }
        public List<UserInfoUpdateRecord> UpdateRecord { get; set; }
    }

    public class UserInfoUpdateRecord
    {
        public int Number { get; set; }
        public string Field { get; set; }
        public string UnitName { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }

        public string UpdateDate { get; set; }

    }
}