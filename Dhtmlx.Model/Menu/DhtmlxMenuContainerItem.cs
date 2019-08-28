using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Dhtmlx.Model.Menu
{
    public class DhtmlxMenuContainerItem : DhtmlxMenuItem
    {
        List<DhtmlxMenuItem> items;
        Dictionary<string, string> userDatas;
        DhtmlxMenuIthemLink menuLink;
        string id;
        string text;
        string img;
        string imgdis;
        string hotKey;
        bool enable;

        public DhtmlxMenuContainerItem(string id,string text)
        {
            this.id = id;
            this.text = text;
            this.hotKey = this.img = this.imgdis = string.Empty;
            enable = true;
            items = new List<Menu.DhtmlxMenuItem>();
            userDatas = new Dictionary<string, string>();
        }

        public string HotKey
        {
            set
            {
                hotKey = value;
            }
        }


        public string Img
        {
            set
            {
                img = value;
            }
        }

        public string Imgdis
        {
            set
            {
                imgdis = value;
            }
        }

        public bool Enable
        {

            set
            {
                enable = value;
            }
        }

        public void AddMenuItem(DhtmlxMenuItem item)
        {
            items.Add(item);
        }

        public void AddUserData(string name,string value)
        {
            userDatas[name] = value;
        }

        public void AddLinke(string link,string target="blank")
        {
            if(this.menuLink == null)
            {
                menuLink = new Menu.DhtmlxMenuIthemLink(target, link);
            }
            else
            {
                menuLink.Target = target;
                menuLink.Link = link;
            }
        }


        public override XElement BuildDhtmlXml()
        {
            return new XElement("item",
                new XAttribute("id", id),
                new XAttribute("text", text),
                string.IsNullOrWhiteSpace(img) ? null : new XAttribute("img", img),
                string.IsNullOrWhiteSpace(imgdis) ? null : new XAttribute("imgdis", imgdis),
                enable ? null : new XAttribute("enabled", enable),
                string.IsNullOrWhiteSpace(hotKey) ? null : new XElement("hotkey", hotKey),
                menuLink == null ? null : menuLink.BuildDhtmlXml(),
                from uKv in userDatas
                select new XElement("userdata",
                           new XAttribute("name", uKv.Key),
                           new XText(uKv.Value)),
                from i in items
                select i.BuildDhtmlXml());
        }
    }


    public class DhtmlxMenuIthemLink
    {
        string target;
        string link;

        public string Target
        {
            set
            {
                target = value;
            }
        }

        public string Link
        {
            set
            {
                link = value;
            }
        }

        public DhtmlxMenuIthemLink(string target,string link)
        {
            this.target = target;
            this.link = link;
        }

        public  XElement BuildDhtmlXml()
        {
            return new XElement("href",
                new XAttribute("target", target),
                new XCData(link));
        }

    }
}
