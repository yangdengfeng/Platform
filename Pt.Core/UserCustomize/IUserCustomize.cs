using Pkpm.Entity;
using Pkpm.Entity.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pkpm.Core.UserCustomize
{
    public interface IUserCustomize
    {
        bool SetUserCustom(int userId, List<UserInCustom> userInCustoms, UserCustomType customType,out string errorMsg);

        bool SetUserItem(int userId, List<UserInItem> userInItems, UserItemType itemType, out string errorMsg);
    }
}
