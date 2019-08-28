using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Pkpm.Framework.Common
{
    public enum HashMode
    {
        MD5,
        SHA1,
        SHA256,
        SHA384,
        SHA512
    }

    public static class HashUtility
    {
        public static string ComputeHash(string data, HashMode mode = HashMode.MD5)
        {
            byte[] byteData = Encoding.UTF8.GetBytes(data);
            return Convert.ToBase64String(ComputeHash(byteData, mode));
        }

        public static byte[] ComputeHash(byte[] data, HashMode mode = HashMode.MD5)
        {
            byte[] hash = null;
            switch (mode)
            {
                case HashMode.MD5:
                    hash = MD5.Create().ComputeHash(data);
                    break;
                case HashMode.SHA1:
                    hash = SHA1.Create().ComputeHash(data);
                    break;
                case HashMode.SHA256:
                    hash = SHA256.Create().ComputeHash(data);
                    break;
                case HashMode.SHA384:
                    hash = SHA384.Create().ComputeHash(data);
                    break;
                case HashMode.SHA512:
                    hash = SHA512.Create().ComputeHash(data);
                    break;
                default:
                    break;
            }

            if (hash == null)
            {
                hash = MD5.Create().ComputeHash(data);
            }

            return hash;
        }

        public static string ConvertToHex(byte[] bArr)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < bArr.Length; i++)
            {
                byte b = bArr[i];
                int value = (b & 0xFF) + (b < 0 ? 128 : 0);
                builder.Append(value < 16 ? "0" : "");
                builder.Append(value.ToString("x"));
            }
            return builder.ToString();
        }

        public static string MD5HashHexStringFromUTF8String(string str)
        {
            var hash = ComputeHash(Encoding.UTF8.GetBytes(str));
            return ConvertToHex(hash);
        }

        public static string MD5HashHexStringFromBytes(byte[] bArr)
        {
            var hash = ComputeHash(bArr);
            return ConvertToHex(hash);
        }
    }
}
