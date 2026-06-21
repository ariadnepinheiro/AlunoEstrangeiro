using System.Text;
using System;
using System.Security.Cryptography;

namespace SRV.Security
{

    public static class Crypt
    {

        public static string Encrypt(string text)
        {
            byte[] buffer =  new UTF8Encoding().GetBytes(text);
            SHA1CryptoServiceProvider cryptoTransformSHA1 = new SHA1CryptoServiceProvider();
            return BitConverter.ToString(cryptoTransformSHA1.ComputeHash(buffer)).Replace("-", "");
        }

    }
}

