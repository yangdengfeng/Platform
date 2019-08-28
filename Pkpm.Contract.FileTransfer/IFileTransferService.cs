using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Pkpm.Contract.FileTransfer
{
    [ServiceContract]
    public interface IFileTransferService
    {
        /// <summary>
        /// 获取大型C类报告文件
        /// </summary>
        /// <param name="filePath">文件目录</param>
        /// <returns></returns>
        [OperationContract]
        Stream GetCTypeReport(string filePath);

        /// <summary>
        /// 获取大型PKR文件
        /// </summary>
        /// <param name="filePath">文件目录</param>
        /// <returns></returns>
        [OperationContract]
        Stream GetPKRReport(string filePath);

        /// <summary>
        /// 获取小型文件
        /// </summary>
        /// <param name="filePath">文件目录</param>
        /// <returns></returns>
        [OperationContract]
        byte[] GetStoreFile(string filePath);

        /// <summary>
        /// wcf上传文件
        /// </summary>
        /// <param name="filePath">上传文件路径如：E:/UploadFiles/zhaopian1.jpg</param>
        /// <param name="fileData">文件内容byte[]</param>
        /// <returns>tre上传成功，false失败</returns>
        [OperationContract]
        bool UploadStoreFile(string filePath, byte[] fileData);

        /// <summary>
        /// for test purpose
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        string Test();
    }
}
