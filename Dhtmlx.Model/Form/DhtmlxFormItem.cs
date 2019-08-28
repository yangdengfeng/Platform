using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Dhtmlx.Model.Form
{
   public abstract class DhtmlxFormItem
    {
        protected Dictionary<string, string> strItems;
        protected Dictionary<string, int> intItems;
        protected Dictionary<string, bool> boolItems;
        protected Dictionary<string, string> userDatas;
        protected DhtmlxFormNote note;

        public DhtmlxFormItem()
        {
            strItems = new Dictionary<string, string>();
            intItems = new Dictionary<string, int>();
            boolItems = new Dictionary<string, bool>();
            userDatas = new Dictionary<string, string>();
        }

        public DhtmlxFormItem AddStringItem(string name,string item)
        {
            if(!string.IsNullOrWhiteSpace(name) 
                && !string.IsNullOrWhiteSpace(item))
            {
                strItems[name] = item;
            }
            
            return this;
        }

        public DhtmlxFormItem AddIntItem(string name,int item)
        {
            if(!string.IsNullOrWhiteSpace(name))
            {
                intItems[name] = item;
            }
           
            return this;
        }

        public DhtmlxFormItem AddBoolItem(string name,bool item)
        {
            if (!string.IsNullOrWhiteSpace(name))
            {
                boolItems[name] = item;
            }
            return this;
        }

        public DhtmlxFormItem AddUserData(string name,string value)
        {
            if (!string.IsNullOrWhiteSpace(name))
            {
                userDatas[name] = value;
            }
            return this;
        }

        public DhtmlxFormItem AddUserNote(string value)
        {
            if(note==null)
            {
                note = new DhtmlxFormNote()
                {
                   Value = value
                };
            }
            else
            {
                note.Value = value;
            }

            return this;
        }

        public DhtmlxFormItem AddUserNote(int width,string value)
        {
            if(note==null)
            {
                note = new DhtmlxFormNote()
                {
                    Width = width,
                    Value = value
                };
            }
            else
            {
                note.Width = width;
                note.Value = value;
            }

            return this;
        }

        public abstract string Type { get; }

        public virtual XElement BuildDhtmlXml()
        {
            return new XElement("item",
                new XAttribute("type", Type),
                from strKv in strItems
                select new XAttribute(strKv.Key, strKv.Value),
                from intKv in intItems
                select new XAttribute(intKv.Key, intKv.Value),
                from boolKv in boolItems
                select new XAttribute(boolKv.Key, boolKv.Value),
                from userKv in userDatas
                select new XElement("userdata", new XAttribute("name", userKv.Key), new XText(userKv.Value)),
                note == null ? null : note.BuildDhtmlXml());
        }
    }

    public class DhtmlxFormNote
    {
        public int Width { get; set; }
        public string Value { get; set; }

        public XElement BuildDhtmlXml()
        {
            XElement noteElem = new XElement("note");
            if(Width>0)
            {
                noteElem.Add(new XAttribute("width", Width));
            }
            noteElem.Add(new XText(Value));

            return noteElem;
        }
    }
}
