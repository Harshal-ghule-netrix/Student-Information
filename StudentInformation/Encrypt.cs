using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace StudentInformation
{
    public class Encrypt
    {
         
        public static string EncryptText(string text, string salt)
        {
            byte[] data = Encoding.UTF8.GetBytes(string.Concat(text, salt));
            SHA256 shaM = new SHA256Managed();
            byte[] hashedBytes = shaM.ComputeHash(data);

            return Convert.ToBase64String(hashedBytes);
        }
    }
}