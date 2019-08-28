using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Dhtmlx.Model.TreeView
{
   public class DhtmlxTreeViewItem
    {
        string id;
        string text;
        bool isOpen;
        bool isCheck;
        DhtmlxTreeViewCheckbox checkbox;

        List<DhtmlxTreeViewItem> items;
        Dictionary<string, string> userDatas;
        DhtmlxTreeViewIcons icon;

        public DhtmlxTreeViewItem(string id, string text) : this(id, text, false, false, DhtmlxTreeViewCheckbox.NotSet)
        {

        }

        public DhtmlxTreeViewItem(string id, string text, bool isOpen, bool isCheck, DhtmlxTreeViewCheckbox checkbox)
        {
            this.id = id;
            this.text = text;
            this.isOpen = isOpen;
            this.isCheck = isCheck;
            this.checkbox = checkbox;

            items = new List<TreeView.DhtmlxTreeViewItem>();
            userDatas = new Dictionary<string, string>();

        }

        public bool IsOpen
        {

            set
            {
                isOpen = value;
            }
        }

        public bool IsCheck
        {
            set
            {
                isCheck = value;
            }
        }

        public DhtmlxTreeViewCheckbox Checkbox
        {
            set
            {
                checkbox = value;
            }
        } 

        public void AddTreeViewItem(DhtmlxTreeViewItem item)
        {
            this.items.Add(item);
        } 

        public void AddTreeViewItem(string id, string text)
        {
            this.items.Add(new TreeView.DhtmlxTreeViewItem(id, text));
        }

        public void AddTreeViewItem(int id, string text)
        {
            this.items.Add(new TreeView.DhtmlxTreeViewItem(id.ToString(), text));
        }

        public void AddTreeViewItem(string id, string text, bool isCheck, DhtmlxTreeViewCheckbox checkbox)
        {
            this.items.Add(new TreeView.DhtmlxTreeViewItem(id, text) { IsCheck = isCheck, Checkbox = checkbox });
        }

        public void AddUserData(string name,string value)
        {
            userDatas[name] = value;
        }

        public void AddCustomIcons(string fold_opened, string fold_closed, string file = "")
        {
            if (icon == null)
            {
                icon = new DhtmlxTreeViewIcons();
            }

            icon.Folder_opened = fold_opened;
            icon.Folder_closed = fold_closed;
            icon.File = file;
        }

        public XElement BuildDhtmlXml()
        {
            return new XElement("item",
                new XAttribute("id", id),
                new XAttribute("text", text),
                isOpen == false ? null : new XAttribute("open", "1"),
                isCheck == false ? null : new XAttribute("checked", "1"),
                checkbox == DhtmlxTreeViewCheckbox.NotSet ? null : new XAttribute("checkbox", checkbox.ToString()),
                icon == null ? null : icon.BuildDhtmlXml(),
                from i in items
                select i.BuildDhtmlXml(),
                userDatas.Count == 0 ? null : new XElement("userdata",
                                            from userKv in userDatas
                                            select new XAttribute(userKv.Key, userKv.Value)));
        }


    }

    public enum DhtmlxTreeViewCheckbox
    {
        NotSet,
        hidden,
        disabled
    }

    public class DhtmlxTreeViewIcons
    {
        public string File { get; set; }
        public string Folder_opened { get; set; }
        public string Folder_closed { get; set; }

        public XElement BuildDhtmlXml()
        {
            return new XElement("icons",
                string.IsNullOrWhiteSpace(File) ? null : new XAttribute("file", File),
                string.IsNullOrWhiteSpace(Folder_opened) ? null : new XAttribute("folder_opened", Folder_opened),
                string.IsNullOrWhiteSpace(Folder_closed) ? null : new XAttribute("folder_closed", Folder_closed));
        }
    }
}
