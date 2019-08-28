using Pkpm.Contract.FileTransfer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Pkpm.FileTransfer
{
    public class FileTransfer : IFileTransferService
    {
        public Stream GetCTypeReport(string filePath)
        {
            if(File.Exists(filePath))
            {
                return File.OpenRead(filePath);
            }
            else
            {
                return new MemoryStream();
            }
        }

        public Stream GetPKRReport(string filePath)
        {
            if(File.Exists(filePath))
            {
                return File.OpenRead(filePath);
            }
            else
            {
                return new MemoryStream();
            }
        }

        public byte[] GetStoreFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                return File.ReadAllBytes(filePath);
            }
            else
            {
                return new MemoryStream().ToArray();
            }
        }

        public bool UploadStoreFile(string filePath, byte[] fileData)
        {
            bool result = false;
            string PathName= Path.GetDirectoryName(filePath);
            if (!Directory.Exists(PathName))
            {
                System.IO.Directory.CreateDirectory(PathName);
            }
            using (FileStream fs = new FileStream(filePath, FileMode.Create))
            {
                fs.Write(fileData, 0, fileData.Length);
                fs.Close();
                result = true;
            }
            return result;
        }

        public string Test()
        {
            return "Hello world";
        }
    }
}
