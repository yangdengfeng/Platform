using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pkpm.Framework.FileHandler
{
    public class LocalFileHandler : IFileHandler
    {
        private static string dPath = ConfigurationManager.AppSettings["FileHandlerPath"] == null ? "E:\\" : ConfigurationManager.AppSettings["FileHandlerPath"];
        private static string noImagePath = ConfigurationManager.AppSettings["NoImageFolder"];

        public void DeleteFile(string fileSystem, string path)
        {
            string fsPath = Path.Combine(dPath, fileSystem);
            if (path.StartsWith("/"))
            {
                path = path.TrimStart('/');
            }
            string filePath = Path.Combine(fsPath, path.Replace('/', '\\'));
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
             
        }

        private byte[] GetNoImage()
        {
            return File.ReadAllBytes(noImagePath);
        }

        public  Stream LoadFile(string fileSystem, string path)
        {

            if (string.IsNullOrWhiteSpace(fileSystem))
            {
                byte[] fbData = null;
                if (string.IsNullOrWhiteSpace(path) || !File.Exists(path))
                {
                    fbData = GetNoImage();
                }
                else
                {
                    fbData = File.ReadAllBytes(path);
                }

                MemoryStream pms = new MemoryStream(fbData);

                return pms;
            }
            else
            {
                string fsPath = Path.Combine(dPath, fileSystem);
                if (path.StartsWith("/"))
                {
                    path = path.TrimStart('/');
                }
                if (path.Contains('|'))
                {
                    path = path.Split('|')[0];
                }
                string filePath = Path.Combine(fsPath, path.Replace('/', '\\'));

                byte[] bData = null;
                if (!File.Exists(filePath))
                {
                    bData = GetNoImage();
                }
                else
                {
                    bData = File.ReadAllBytes(filePath);
                }
                MemoryStream ms = new MemoryStream(bData);

                return ms;
            }
        }

        public void UploadFile(Stream stream, string fileSystem, string path)
        {
            string fsPath = Path.Combine(dPath, fileSystem);
            if (path.StartsWith("/"))
            {
                path = path.TrimStart('/');
            }
            string filePath = Path.Combine(fsPath, path.Replace('/', '\\'));
            string fileDir = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(fileDir))
            {
                Directory.CreateDirectory(fileDir);
            }

            using (var fs = new FileStream(filePath, FileMode.Create))
            {
                stream.CopyTo(fs);
            }

            return;
        }
    }
}
