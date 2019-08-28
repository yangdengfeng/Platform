using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pkpm.Framework.Common
{
    public class QrCodeGenerator
    {
        public static byte[] Generate(int pixelsPerModule,string strCode)
        {
            byte[] result = null;
            QRCodeGenerator qrGenerator = new QRCoder.QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(strCode, QRCodeGenerator.ECCLevel.M);
            QRCode qrcode = new QRCode(qrCodeData);
            Bitmap qrCodeImage = qrcode.GetGraphic(pixelsPerModule, Color.Black, Color.White, null, 15, 6, false);
            
            using (MemoryStream ms = new MemoryStream())
            {
                qrCodeImage.Save(ms, ImageFormat.Jpeg);
                result = ms.ToArray();
            }
            return result;
        }
    }
}