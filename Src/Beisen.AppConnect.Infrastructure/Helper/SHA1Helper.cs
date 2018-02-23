using System;
using System.Security.Cryptography;
using System.Text;

namespace Beisen.AppConnect.Infrastructure.Helper
{
    public class SHA1Helper
    {
        public static string ConverToSHA1Str(string str)
        {
            var sha = new SHA1CryptoServiceProvider();
            var enc = new ASCIIEncoding();
            var dataToHash = enc.GetBytes(str);
            var dataHashed = sha.ComputeHash(dataToHash);
            var hash = BitConverter.ToString(dataHashed).Replace("-", "");

            return hash;
        }
    }
}
