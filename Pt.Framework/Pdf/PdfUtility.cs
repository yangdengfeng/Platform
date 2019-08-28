using iTextSharp.text.io;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Pkpm.Framework.Pdf
{
    public static class PdfUtility
    {
        
        public static void FillInPdfTemplateFields(string templatPath, Stream pdfOut, Dictionary<string,string> hashTable, byte[] qrCode, bool isEncrypt)
        {
            PdfReader pdfReader = new PdfReader(templatPath);
            PdfStamper pdfStamper = new PdfStamper(pdfReader, pdfOut);
            try
            {
                if (isEncrypt)
                {
                    pdfStamper.SetEncryption(PdfWriter.STRENGTH40BITS, null, null, PdfWriter.AllowPrinting);
                }

                StreamUtil.AddToResourceSearch(Assembly.Load("iTextAsian"));
                BaseFont font = BaseFont.CreateFont("STSong-Light", "UniGB-UCS2-H", BaseFont.NOT_EMBEDDED);

                AcroFields pdfFormFields = pdfStamper.AcroFields; 
                foreach (KeyValuePair<string, AcroFields.Item> de in pdfFormFields.Fields)
                {
                    string fieldKey = de.Key.ToString();
                    if(string.IsNullOrWhiteSpace(fieldKey))
                    {
                        continue;
                    } 

                    if (fieldKey.Contains("img_"))
                    { 
                        int page = Convert.ToInt32(fieldKey.Split('_')[1]);
                        string hasKey = fieldKey.Split('_')[2];
                        //if (hashTable.ContainsKey(hasKey))
                        //{
                            IList<AcroFields.FieldPosition> fieldPositions = pdfFormFields.GetFieldPositions(fieldKey);
                            AcroFields.FieldPosition fieldPosition = fieldPositions[0];
                            iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(qrCode);
                            image.ScaleToFit(fieldPosition.position.Width, fieldPosition.position.Height);
                            float x = fieldPosition.position.Left;
                            float y = fieldPosition.position.Top - image.ScaledHeight;
                            image.SetAbsolutePosition(x, y);
                            var pdfContentByte = pdfStamper.GetOverContent(page);
                            pdfContentByte.AddImage(image);
                        //}

                    }
                    else if (hashTable.ContainsKey(fieldKey))
                    {  
                        pdfFormFields.SetFieldProperty(fieldKey, "textfont", font, null);
                        string fieldValue = hashTable[fieldKey];
                        if(string.IsNullOrWhiteSpace(fieldValue))
                        {
                            fieldValue = string.Empty;
                        }
                        pdfFormFields.SetField(fieldKey, fieldValue);
                    }
                }

                pdfStamper.FormFlattening = true;
              
            }
            catch (Exception)
            {
                
            }
            finally
            {
                pdfStamper.Close();
                pdfReader.Close();
            }
        }
    }
}
