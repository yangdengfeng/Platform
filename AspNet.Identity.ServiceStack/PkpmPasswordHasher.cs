using Microsoft.AspNet.Identity;
using Pkpm.Framework.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AspNet.Identity.ServiceStack
{
   public class PkpmPasswordHasher: PasswordHasher
    {
        public override string HashPassword(string password)
        {
            return base.HashPassword(password);
        }

        public override PasswordVerificationResult VerifyHashedPassword(string hashedPassword, string providedPassword)
        {
            string[] passwordProperties = hashedPassword.Split('|');
            if (passwordProperties.Length != 3)
            {
                return base.VerifyHashedPassword(hashedPassword, providedPassword);
            }
            else
            {
                string passwordHash = passwordProperties[0];
               // var hashData = HashUtility.MD5HashHexStringFromUTF8String(providedPassword);
                string oldClipText = HashUtility.MD5HashHexStringFromUTF8String(providedPassword);
                if (oldClipText == passwordHash)
                {
                    return PasswordVerificationResult.SuccessRehashNeeded;
                }
                else
                {
                    return PasswordVerificationResult.Failed;
                }
            }
        } 
    }
}
