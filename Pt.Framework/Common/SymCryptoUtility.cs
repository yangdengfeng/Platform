using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Pkpm.Framework.Common
{
   public static class SymCryptoUtility
    {
       private static byte[] key = { 145, 12, 32, 245, 98, 132, 98, 214, 6, 77, 131, 44, 221, 3, 9, 50 };
       private static byte[] iv = { 15, 122, 132, 5, 93, 198, 44, 31, 9, 39, 241, 49, 250, 188, 80, 7 };

        public static string Encrypt(string data)
        {
            return Encrypt(data, key, iv);
        }

        public static string Decrypt(string data)
        {
            return Decrypt(data, key, iv);
        }

        public static string Encrypt(string data, byte[] key, byte[] iv)
        {
            return Convert.ToBase64String(Encrypt(Encoding.UTF8.GetBytes(data), key, iv));
        }

        public static string Decrypt(string data, byte[] key, byte[] iv)
        {
            return Encoding.UTF8.GetString(Decrypt(Convert.FromBase64String(data), key, iv));
        }

        public static byte[] Encrypt(byte[] data, byte[] key, byte[] iv)
        {
            using (Aes algorithm = Aes.Create())
            using (ICryptoTransform encryptor = algorithm.CreateEncryptor(key, iv))
            {
                return Crypt(data, encryptor);
            }
        }

        public static byte[] Decrypt(byte[] data, byte[] key, byte[] iv)
        {
            using (Aes algorithm = Aes.Create())
            using (ICryptoTransform decryptor = algorithm.CreateDecryptor(key, iv))
            {
                return Crypt(data, decryptor);
            }
        }

       private  static byte[] Crypt(byte[] data, ICryptoTransform cryptor)
        {
            MemoryStream m = new MemoryStream();
            using (Stream c = new CryptoStream(m, cryptor, CryptoStreamMode.Write))
            {
                c.Write(data, 0, data.Length);
            }
            return m.ToArray();
        }
    }
}
