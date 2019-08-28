using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pkpm.Core
{
    public class PkPmCacheKeys
    {
        public static string AllSTCheckUnit = "AllSTCheckUnit";
        public static string ActionsByRoleIdFmt = "ActionsByRoleId[{0}]";
        public static string ActionsByUserIdFmt = "ActionsByUserId[{0}]";
        public static string ActionsByRoleIdsFmt = "ActionsByRoleIds[{0}]";
        public static string ActionsByRoleIdPathIdFmt = "ActionsByRoleId[{0}]-PathID:[{1}]";
        public static string ActionsByRoleIdsPathIdFmt = "ActionsByRoleIds[{0}]-PathID:[{1}]";

        public static string CategoryPaths = "CategoryPaths";
        public static string CompanyQualification = "CompanyQualification";
        public static string PathsByRoleIdFmt = "PathsByRoleId[{0}]";
        public static string PathsByUserIdFmt = "PathsByUserId[{0}]";
        public static string PathsByRoleIdsFmt = "PathsByRoleId[{0}]";

        public static string RolesByUserIdFmt = "RolesByUserId[{0}]";
        public static string RoleByUserIdFmt = "RoleByUserId[{0}]";

        public static string AllCheckUnitST = "AllCheckUnitCodeWithNameST";
        public static string AllCheckUnit = "AllCheckUnitCodeWithName";
        public static string AllCheckUnitInArea = "AllCheckUnitInArea";
        public static string AllItemName = "AllItemName";
        public static string AllSHItemName = "AllSHItemName";


        public static string AllCategoryName = "AllCategoryName";
        public static string ItemNameWithCategory = "ItemNameWithCategory[{0}]";
        public static string CReportItemNameCustomIdFmt = "CReportItemNameByCustomId[{0}]";

        public static string CustomsByUserIdFmt = "DTOCustomsByUserId[{0}]";
        public static string CustomsByUserIdSTFmt = "DTOCustomsByUserIdST[{0}]";
        public static string CustomByUserIdFmt = "DTOCustomByUserId[{0}]";

        public static string InspectByUserIdFmt = "InspectIdByUserId[{0}]";

        public static string SysDictByKeyFmt = "SysDict_[{0}]";

        public static string AllAreasFromSysDict = "AllAreasFromSysDict";

        public static string UserInAreByUserIdFmt = "UserInAreaByUserId[{0}]";
        public static string CommonItemNameByItemTableName = "CommonItemNameyBy[{0}]";

        //public static string TypeCodeWithNames = "TypeCodesWithNames";
        // public static string ItemCodeWithNames = "ItemCodesWithNames";
        //public static string TypeCode = "TypeCode";
        public static string CheckItemType = "CheckItemType";
        public static string totalitems = "totalitems";
        public static string ParmCodeWithNames = "ParmCodeWithNames";
        public static string ParmCodeWithNamesByItemCode = "ParmCodeWithNamesByItemCode[{0}]";

        public static string SubItemParams = "SubItemParam";

        public static string CustomDetectNum = "CustomDetectNum";

       

        public static string GetRoleByRoleCode = "RoleByRoleCode[{0}]";

        public static string JsonColumnByItemCodeFmt = "JsonColumnByItemCodeFmt[{0}]";
        public static string NoReportNumCanSearchDateTime = "NoReportNumCanSearchDateTime";
        public static string EsHrItemByKeyFmt = "EsHrItem_[{0}]";

        public static string AllArea = "AllAreaFromRegion";

        public static string AllAreaFromArea = "AllAreaFromArea";
        public static string AllAreaByTree = "AllAreaTree";
        public static string AllCustomST= "AllCustomST";
        public static string AllListArea = "AllListArea";

        public static string CustomsByUserIdEliminateIsUerFmt = "DTOCustomsByUserIdEliminateIsUer[{0}]";
        public static string AllCheckUnitEliminateIsUser = "AllCheckUnitEliminateIsUerFmtCodeWithName";
    }
}
