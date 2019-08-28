using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dhtmlx.Model.Grid
{
    public class LayUIGridRow
    {
        string id;
        Dictionary<string, string> strCells;
        Dictionary<string, int> intCells;
        Dictionary<string, bool> boolCells;
        Dictionary<string, float> floatCells;
        Dictionary<string, double> doubleCells;
        JObject jObject;
        //List<LayUITableRow> rows;
        public LayUIGridRow(string id)
        {
            this.id = id;
           
            strCells = new Dictionary<string, string>();
            intCells = new Dictionary<string, int>();
            boolCells = new Dictionary<string, bool>();
            floatCells = new Dictionary<string, float>();
            doubleCells = new Dictionary<string, double>();
            jObject = new JObject();
          
        }

        public void AddCell(string field, string text)
        {
            strCells.Add(field, text);
        }

        

        public void AddCell(string field, int text)
        {
            intCells.Add(field, text);
             
        }

        public void AddCell(string field, int? text)
        {
             
            int textValue = text ?? default(int);
            AddCell(field, textValue);
        }

        public void AddCell(string field, float text)
        {
            floatCells.Add(field, text); 
        }

        public void AddCell(string field, float? text)
        {
            float textValue = text ?? default(float);
            floatCells.Add(field, textValue);
        }

        public void AddCell(string field, double text)
        {
            doubleCells.Add(field, text);
           
        }

        public void AddCell(string field, double? text)
        {
            double textValue = text ?? default(double);
            doubleCells.Add(field, textValue);

        }

        public void AddCell(string field, DateTime text, string format = "yyyy-MM-dd")
        {
            strCells.Add(field, text.ToString(format));
           
        }

        public void AddCell(string field, DateTime? text, string format = "yyyy-MM-dd")
        {
            if(text.HasValue)
            {
                 AddCell(field, text.Value, format);
            }
            else
            {
                AddCell(field, string.Empty);
            }
        }

        public void AddCell(string field, bool text)
        {
            boolCells.Add(field, text);
        }

        public void AddCell(string field, bool? text)
        {
            bool textValue = text ?? default(bool);
            boolCells.Add(field, textValue);
        }


        public JObject BuildRowJson()
        {
            jObject = new JObject(
                from cell in strCells
                select new JProperty(cell.Key, cell.Value));
            jObject.Add(
                from cell in intCells
                select new JProperty(cell.Key, cell.Value));
            jObject.Add(
                from cell in floatCells
                select new JProperty(cell.Key, cell.Value));
            jObject.Add(
                from cell in doubleCells
                select new JProperty(cell.Key, cell.Value));
            jObject.Add(
                from cell in boolCells
                select new JProperty(cell.Key, cell.Value));
            return jObject;
        }
    }
}
