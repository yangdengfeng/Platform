using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pkpm.Framework.FileHandler
{
    public interface IFileHandler
    {
        void UploadFile(Stream stream, string fileSystem, string path);

        Stream LoadFile(string fileSystem, string path);

        void DeleteFile(string fileSystem, string path);

    }
}
