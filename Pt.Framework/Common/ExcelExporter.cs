using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;

using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System.IO;
using NPOI.XSSF.UserModel;

namespace Pkpm.Framework.Common
{
    public enum CellStyle
    {
        Default,
        Header,
        Date,
        DateTime,
        Integer,
        Float,
        Currency
    }

    public enum DateTimeFormat
    {
        Default,
        Date,
        Time,
        Full
    }

    public class ExcelRow
    {
        private IRow m_row;
        private int m_column = 0;

        public ExcelRow(IRow row)
        {
            m_row = row;
        }

        private ICellStyle CreateCellStyle()
        {
            return m_row.Sheet.Workbook.CreateCellStyle();
        }
        private IDataFormat CreateDataFormat()
        {
            return m_row.Sheet.Workbook.CreateDataFormat();
        }
        private ICellStyle CreateCellStyle(string format)
        {
            ICellStyle result = CreateCellStyle();
            result.DataFormat = CreateDataFormat().GetFormat(format);
            return result;
        }

        /// <summary>
        /// 添加文本（左对齐）
        /// </summary>
        /// <param name="text">待显示的文本</param>
        public void AddCell(string text)
        {
            ICell cell = m_row.CreateCell(m_column++);
            cell.SetCellValue(text);
        }

        /// <summary>
        /// 添加整数（右对齐）
        /// </summary>
        /// <param name="value">整数原值</param>
        public void AddCell(int value)
        {
            ICell cell = m_row.CreateCell(m_column++, CellType.Numeric);
            cell.SetCellValue(value);
        }

        /// <summary>
        /// 添加浮点数（右对齐）
        /// </summary>
        /// <param name="value">浮点数原值</param>
        /// <param name="decimalDigits">保留小数位数（四舍五入），默认2位小数</param>
        public void AddCell(double value, int decimalDigits = 2)
        {
            ICell cell = m_row.CreateCell(m_column++, CellType.Numeric);
            cell.SetCellValue(value);

            string pattern = "0.";
            for (var i = 0; i < decimalDigits; ++i)
                pattern += '0';
            cell.CellStyle = CreateCellStyle(pattern);
        }

        /// <summary>
        /// 添加日期时间（右对齐）
        /// </summary>
        /// <param name="value">日期时间原值</param>
        /// <param name="format">显示格式类型，有默认、日期、时间、完整三种</param>
        /// <remarks>
        /// 格式举例：
        ///     默认：2017-06-23 11:29      （没有秒数）
        ///     日期：2017-06-23            （只有日期部分）
        ///     时间：11:29:35              （只有时间部分）
        ///     完整：2017-06-23 11:29:35
        /// </remarks>
        public void AddCell(DateTime value, DateTimeFormat format = DateTimeFormat.Default)
        {
            ICell cell = m_row.CreateCell(m_column++, CellType.Numeric);
            cell.SetCellValue(value);
            switch (format)
            {
                case DateTimeFormat.Date:
                    cell.CellStyle = CreateCellStyle("yyyy-MM-dd");
                    break;
                case DateTimeFormat.Time:
                    cell.CellStyle = CreateCellStyle("HH:mm:ss");
                    break;
                case DateTimeFormat.Full:
                    cell.CellStyle = CreateCellStyle("yyyy-MM-dd HH:mm:ss");
                    break;
                default:
                    cell.CellStyle = CreateCellStyle("yyyy-MM-dd HH:mm");
                    break;
            }
        }

        public string this[int i]
        {
            get
            {
                return m_row.Cells[i].StringCellValue;
            }
        }

        public int Count
        {
            get
            {
                return m_column;
            }
        }
    }

    public class ExcelExporter
    {
        private const string FileExtXls = ".xls";
        private const string FileExtXlsx = ".xlsx";
        private const string mime2003 = "application/msexcel";
        private const string mime2007 = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        private bool m_xlsx;
        private string m_title;
        private IWorkbook m_workbook;
        private ISheet m_sheet;
        private int m_rowNo = 0;
        private int m_colCount = 0;

        private static ICellStyle m_styleTitle;    // 加粗
        //private static ICellStyle m_styleInteger;  // 右对齐

        private void CreatePredefinedStyles()
        {
            ICellStyle style = m_workbook.CreateCellStyle();
            IFont font = m_workbook.CreateFont();

        }

        public ExcelExporter(string sheetName, bool xlsx = true)
        {
            m_xlsx = xlsx;
            m_title = sheetName;
            if (xlsx)
            {
                m_workbook = new XSSFWorkbook();
            }
            else
            {
                m_workbook = new HSSFWorkbook();
            }
            m_sheet = m_workbook.CreateSheet(sheetName);
        }

        public void SetColumnTitles(string[] titles)
        {
            IFont boldFont;
            m_styleTitle = m_workbook.CreateCellStyle();
            boldFont = m_workbook.CreateFont();
            boldFont.Boldweight = (short)FontBoldWeight.Bold;
            m_styleTitle.SetFont(boldFont);

            IRow row = m_sheet.CreateRow(0);
            for (var i = 0; i < titles.Length; ++i)
            {
                ICell cell = row.CreateCell(i);
                cell.SetCellValue(titles[i]);
                cell.CellStyle = m_styleTitle;
            }

            m_colCount = titles.Length;

            // 还没有输出过记录？
            if (m_rowNo == 0)
                ++m_rowNo;
        }

        public void SetColumnTitles(string titles)
        {
            if (string.IsNullOrEmpty(titles))
                return;
            string[] list = titles.Split(',');
            for (var i = 0; i < list.Length; ++i)
            {
                list[i] = list[i].Trim();
            }

            SetColumnTitles(list);
        }

        public void AddRow<T>(T o)
        {
            Type theType = typeof(T);
            IRow row;
            try
            {
                PropertyInfo[] properties = theType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

                // 需要自动输出标题行吗？
                if (m_rowNo == 0)
                {
                    row = m_sheet.CreateRow(m_rowNo);
                    for (var i = 0; i < properties.Length; ++i)
                    {
                        ICell cell = row.CreateCell(i);
                        cell.SetCellValue(properties[i].Name);
                    }
                    ++m_rowNo; // 数据行起始
                }

                row = m_sheet.CreateRow(m_rowNo);
                for (var i = 0; i < properties.Length; ++i)
                {
                    ICell cell = row.CreateCell(i);
                    PropertyInfo pi = properties[i];
                    string val = pi.GetValue(o).ToString();
                    cell.SetCellValue(val);
                }
                m_colCount = properties.Length;
                ++m_rowNo;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception : {0}", ex.Message);
            }
        }

        public ExcelRow AddRow()
        {
            IRow row = m_sheet.CreateRow(m_rowNo);
            ++m_rowNo;
            return new ExcelRow(row);
        }

        public string MIME
        {
            get
            {
                return m_xlsx ? mime2007 : mime2007;
            }
        }


        public byte[] GetAsBytes()
        {
            AdjustColumnWidths();
            // 再导出
            using (MemoryStream ms = new MemoryStream())
            {
                Write(ms);
                return ms.ToArray();
            }
        }

        // 自动调整各列宽度
        private void AdjustColumnWidths()
        {
            for (var i = 0; i < m_colCount; ++i)
            {
                m_sheet.AutoSizeColumn(i);
            }
        }

        public void SaveToFile(string fileName)
        {
            AdjustColumnWidths();
            if (!fileName.EndsWith(FileExtXls) && !fileName.EndsWith(FileExtXlsx))
            {
                fileName += m_xlsx ? FileExtXlsx : FileExtXls;
            }
            using (FileStream fs = File.OpenWrite(fileName))
            {
                Write(fs);
            }
        }

        public string FileName
        {
            get
            {
                return m_title + "_" + DateTime.Now.ToString("yyy-MM-dd_HHmmsss") + (m_xlsx ? FileExtXlsx : FileExtXls);
            }
        }

        private void Write(Stream stream)
        {
            if(m_workbook != null)
            {
                m_workbook.Write(stream);
            }
            else
            {
                throw new Exception("奇怪，居然没有初始化Workbook!");
            }
        }
    }
}