using Pkpm.Entity; 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pkpm.Core.ItemNameCore
{
    public interface IItemNameService
    {
        ItemShortInfos GetAllItemName();

        string GetItemCNNameFromAll(Dictionary<string, string> data, string typecode, string itemCode);


        bool IsAcsReport(string itemName);

        List<string> GetAcsItemNames();

        List<string> GetFixDtItems();   
       
        List<string> GetAcsPicItemType();

        List<string> GetReCheckItemType();

        bool IsAcsPicItemType(string dataType);

        string GetAcsAxisLabel(string itemName); 

        ItemShortInfos GetAllCategoryName();

        bool IsCReport(string typecode ,string itemcode);

        List<CheckItemType> GetAllTypeCodes();

        List<totalitems> GetAllItemCodes();

        List<totalitems> GetItemCodesByTypeCode(string typeCode);

        bool NewCheckItemType(CheckItemType item, out string errorMsg);

        bool NewTotalItem(totalitems item, out string errorMsg);

        bool EditTotalItem(totalitems item, out string errorMsg);

        bool GetCheckItemChange(string value, out Dictionary<string, string> dicData);

        bool DelTotalItemById(int id, out string errorMsg);
    }
}
