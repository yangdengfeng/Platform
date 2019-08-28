using Pkpm.Contract.FileTransfer;
using Pkpm.Contract.PRKReport;
using Serilog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pkpm.PKRReport
{
    public class PKRReport : IPKRReport
    {
        private static string userName = ConfigurationManager.AppSettings["cellUserName"];
        private static string productId = ConfigurationManager.AppSettings["cellProductId"];
        private static string pwd = ConfigurationManager.AppSettings["cellPassword"];
        static readonly string cloudFilePathPrefix = ConfigurationManager.AppSettings["CloudPathPrefix"];
        static readonly bool IsWcfEnabled = string.IsNullOrEmpty(ConfigurationManager.AppSettings["IsWcfEnabled"]) ? true : ConfigurationManager.AppSettings["IsWcfEnabled"] == "1";
        private static string emptyPdfFilePath = ConfigurationManager.AppSettings["emptyPdfPath"];
        private static string emptyImageFilePath = ConfigurationManager.AppSettings["emptyImagePath"];

        public byte[] BuildImageFormPkrMerge(string pkrPath)
        {
            string imageFiles = string.Format("{0}.jpg", Guid.NewGuid());
            try
            {
                var pkrImageList = printOneCell(pkrPath);
                MergeImages(pkrImageList, imageFiles);
                return System.IO.File.ReadAllBytes(imageFiles);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public string BuildHtmlImageFromPKR(string pkrPath)
        {
            return getHtmlFile(pkrPath, "");
        }

        public string BuildHtmlImageFromPKRUrl(string pkrPath, string url)
        {
            return getHtmlFile(pkrPath, url);
        }

        public byte[] BuildImageFromPKR(string reportPath)
        {
            try
            {
                var pkrFolder = ConfigurationManager.AppSettings["PKRTempFolder"];
                string fileName = System.IO.Path.GetFileName(reportPath);
                string filePath = System.IO.Path.Combine(pkrFolder, fileName);

                var data = GetStoreFile(reportPath);
                System.IO.File.WriteAllBytes(filePath, data);  

                Log.Debug("start building Image from PKR with pkrPath: {0}", filePath); 

                string imageFiles = string.Format("{0}.jpg", Path.GetFileNameWithoutExtension(filePath)); 
                if (File.Exists(imageFiles))
                {
                    return System.IO.File.ReadAllBytes(imageFiles);
                }

                try
                {
                    var pkrImageList = printOneCell(filePath);
                    MergeImages(pkrImageList, imageFiles);
                    return System.IO.File.ReadAllBytes(imageFiles);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "BuildImageFromPKR");
                    return System.IO.File.ReadAllBytes(emptyImageFilePath);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "BuildImageFromPKR");
                return System.IO.File.ReadAllBytes(emptyImageFilePath);
            }
        }

        public byte[] BuildPdfFromPKR(string reportPath)
        {

            try
            {
                var pkrFolder = ConfigurationManager.AppSettings["PKRTempFolder"];
                string fileName = System.IO.Path.GetFileName(reportPath);
                string filePath = System.IO.Path.Combine(pkrFolder, fileName);
              
                var data = GetStoreFile(reportPath);
                System.IO.File.WriteAllBytes(filePath, data);

                //Log.Debug("start building pdf from PKR with pkrPath: {0}", filePath);

                string pdfFile = string.Format("{0}.pdf", Path.GetFileNameWithoutExtension(filePath));
                if (File.Exists(pdfFile))
                {
                    return System.IO.File.ReadAllBytes(pdfFile);
                }

                CELL50LibU.CellClass cell = new CELL50LibU.CellClass();
                cell.OpenFile(filePath, "");

                //Log.Debug("OpenFile pdf from PKR with pkrPath: {0}", filePath);

                //cell.Login("长沙建研信息技术有限公司", "11100101825", "5560-1724-7721-6005");
                cell.Login(userName, productId, pwd);
                cell.LocalizeControl(2052);
                var pages = cell.PrintGetPages(0);
                cell.SetWorkbookXLineEnable(false);
                var result = cell.ExportPdfFile(pdfFile, 0, 0, pages);

                cell.closefile();

                //Log.Debug("Expot pdf and close file pdf from PKR with pkrPath: {0}", filePath); 

                return System.IO.File.ReadAllBytes(pdfFile);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Build Pdf From PKR");
                return System.IO.File.ReadAllBytes(emptyPdfFilePath);
            }
        }

        private static void MergeImages(IEnumerable<string> images, string mergeImageFiles)
        {
            var enumerable = images as IList<string> ?? images.ToList();

            var width = 0;
            var height = 0;

            foreach (var imageFile in enumerable)
            {
                System.Drawing.Image image = System.Drawing.Image.FromFile(imageFile);
                height += image.Height;
                width = Math.Max(image.Width, width);
            }

            var bitmap = new Bitmap(width, height);
            using (var g = Graphics.FromImage(bitmap))
            {
                var localHeight = 0;
                foreach (var imageFile in enumerable)
                {
                    System.Drawing.Image image = System.Drawing.Image.FromFile(imageFile);
                    g.DrawImage(image, 0, localHeight);
                    localHeight += image.Height;
                }
            }

            bitmap.Save(mergeImageFiles);
        }

        private static List<string> SplitCellToMultiFiles(string cellFilePath)
        {

            CELL50LibU.CellClass cell = new CELL50LibU.CellClass();

            cell.OpenFile(cellFilePath, "");
            //pageOritation = cell.PrintGetOrient();
            int ll_cellTopMargin = cell.PrintGetMargin(1);
            int ll_cellBottomMargin = cell.PrintGetMargin(3);
            int ll_pageHeight = cell.PrintGetPaperHeight(0) - ll_cellTopMargin - ll_cellBottomMargin;
            int ll_sumRowHeight = 0;
            int ll_startRow = 1;
            List<int> startRowList = new List<int>();
            List<int> endRowList = new List<int>();
            for (int row = 1; row <= cell.GetRows(0) - 1; row++)
            {
                ll_sumRowHeight += cell.GetRowHeight(0, row, 0);
                //if cell.IsRowPageBreak(row) then
                if (ll_sumRowHeight > ll_pageHeight)
                {
                    ll_sumRowHeight = cell.GetRowHeight(0, row, 0);
                    if (row == 1)
                    {
                        continue;
                    }
                    startRowList.Add(ll_startRow);
                    endRowList.Add(row - 1);
                    ll_startRow = row;
                }
            }
            if (startRowList.Count == 0)
            {
                startRowList.Add(ll_startRow);
                endRowList.Add(cell.GetRows(0) - 1);
            }
            if (endRowList[endRowList.Count - 1] != cell.GetRows(0) - 1)
            {
                startRowList.Add(ll_startRow);
                endRowList.Add(cell.GetRows(0) - 1);
            }
            cell.closefile();
            List<string> multiCellFilePaths = new List<string>();
            for (int i = 0; i < startRowList.Count; i++)
            {
                cell.OpenFile(cellFilePath, "");
                int cellRowsOriginal = cell.GetRows(0);
                if ((cell.GetRows(0) - 1) > endRowList[i])
                {
                    cell.DeleteRow(endRowList[i] + 1, (cell.GetRows(0) - 1) - (endRowList[i] + 1) + 1, 0);
                }
                if (startRowList[i] > 1)
                {
                    cell.DeleteRow(1, startRowList[i] - 1, 0);
                }
                cell.SaveFile(cellFilePath + "." + i + ".cll", 0);
                cell.closefile();
                multiCellFilePaths.Add(cellFilePath + "." + i + ".cll");
            }
            return multiCellFilePaths;
        }

        private static string GetSingleCellImageFile(string cellFile, out float dpiX, out float dpiY)
        {


            dpiX = dpiY = 0f;

            CELL50LibU.CellClass cell = new CELL50LibU.CellClass();
            cell.OpenFile(cellFile, "");

            //cell.Login("长沙建研信息技术有限公司", "11100101825", "5560-1724-7721-6005");
            cell.Login(userName, productId, pwd);
            cell.LocalizeControl(2052);

            int width = cell.PrintGetPaperWidth(0) / 10;     //获取宽度

            int height = cell.PrintGetPaperHeight(0) / 10;     //获取高度  

            using (Graphics gfx = Graphics.FromHwnd(IntPtr.Zero))
            {
                dpiX = gfx.DpiX;
                dpiY = gfx.DpiY;
                width = (int)Math.Round((gfx.DpiX * (1 / 25.4f)) * width);
                height = (int)Math.Round((gfx.DpiY * (1 / 25.4f)) * height);
            }

            string imageFilePath = string.Format("{0}.png", cellFile);

            if (File.Exists(imageFilePath))
            {
                File.Delete(imageFilePath);
            }


            using (Bitmap bmSave = new Bitmap(width, height))
            {
                Graphics gSave = Graphics.FromImage(bmSave);

                cell.PrintToDC((int)gSave.GetHdc(), 0);
                gSave.ReleaseHdc();
                bmSave.Save(imageFilePath);
                gSave.Dispose();
            }
            cell.closefile();
            File.Delete(cellFile);

            return imageFilePath;
        }

        public string getHtmlFile(string cellFile, string url)
        {
            List<string> bmpFileList = printOneCell(cellFile);
            string s = "<html><body>";
            string s1 = "";
            for (int i = 0; i < bmpFileList.Count; i++)
            {
                string fileName = Path.GetFileName(bmpFileList[i]);
                string imageUrl = "/tempData/" + fileName;
                s += "<img src='" + url + imageUrl + "' border=0 style='clear:both;'/>";
                s1 += fileName + "|";
            }
            s1 = "<!--" + s1.Substring(0, s1.Length - 1) + "-->";
            return s + s1 + "</body></html>";
        }

        private List<string> printOneCell(string cellFile)
        {
            float dpiX = 0f;
            float dpiY = 0f;
            List<string> imageFileList = new List<string>();
            CELL50LibU.CellClass cell = new CELL50LibU.CellClass();
            //File.AppendAllText(@"E:\Web\4.0pkpm\log\reportPrint_getpkr.txt", "CELL50LibU " + cellFile);
            try
            {
                cell.OpenFile(cellFile, "");
                //File.AppendAllText(@"E:\Web\4.0pkpm\log\reportPrint_getpkr.txt", "CELL50LibU 进入");
                //ReportComman.setCellDocProp(ref cell);

                //cell.Login("长沙建研信息技术有限公司", "11100101825", "5560-1724-7721-6005");
                //cell.Login("湖南建研信息技术股份有限公司", "11100101825", "8716ca174e-a12613-fe3bc6a098f1a596");
                cell.Login(userName, productId, pwd);
                cell.LocalizeControl(2052);

                //ReportComman.setCellDocProp(ref cell);

                int width = cell.PrintGetPaperWidth(0) / 10;
                int height = cell.PrintGetPaperHeight(0) / 10;

                using (Graphics gfx = Graphics.FromHwnd(IntPtr.Zero))
                {
                    dpiX = gfx.DpiX;
                    dpiY = gfx.DpiY;
                    width = (int)Math.Round((gfx.DpiX * (1 / 25.4f)) * width);
                    height = (int)Math.Round((gfx.DpiY * (1 / 25.4f)) * height);
                }


                for (int page = 1; page <= cell.PrintGetPages(0); page++)
                {
                    cell.PrintSetPrintRange(3, page.ToString());
                    cell.PrintSetScale(1);
                    using (Bitmap bmSave = new Bitmap(width, height))
                    {
                        using (Graphics gSave = Graphics.FromImage(bmSave))
                        {
                            cell.PrintToDC((int)gSave.GetHdc(), 0);
                            gSave.ReleaseHdc();

                            string bmpFile = cellFile + "." + page + ".jpg";
                            imageFileList.Add(bmpFile);
                            bmSave.Save(bmpFile);
                            gSave.Dispose();
                        }
                        bmSave.Dispose();
                    }
                }
                cell.closefile();
            }
            catch (Exception ex)
            {
                File.AppendAllText(@"E:\Web\4.0pkpm\log\reportPrint_getpkr.txt", "Exception:  " + ex.Message);
            }

            return imageFileList;
        }

        private byte[] GetStoreFile(string filePath)
        {
            byte[] data = null;
            if (IsWcfEnabled)
            {
                using (var proxy = new WcfProxy<IFileTransferService>())
                {
                    data = proxy.Service.GetStoreFile(filePath);
                }
            }
            else
            {
                data = System.IO.File.ReadAllBytes(filePath);
            }

            return data;
        }
    }
}
