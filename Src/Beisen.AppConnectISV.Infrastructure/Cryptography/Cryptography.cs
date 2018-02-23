using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Beisen.AppConnectISV.Infrastructure
{
    public class Cryptography
    {
        private static string Key = @"C(h0[rE],6}-zh%na+{~GBaby9>Q'y6M";
        private static string IV = @"X-*~9Z.IP(8$)=yh";


        static Cryptography()
        {
        }

        public static string AESEncrypt(string letter)
        {
            MemoryStream msEncrypt = null;
            CryptoStream csEncrypt = null;
            StreamWriter swEncrypt = null;
            Aes aesAlg = null;
            try
            {
                aesAlg = Aes.Create();
                aesAlg.Key = Encoding.UTF8.GetBytes(Key);
                aesAlg.IV = Encoding.UTF8.GetBytes(IV);
                var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                msEncrypt = new MemoryStream();
                csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);
                swEncrypt = new StreamWriter(csEncrypt);
                swEncrypt.Write(letter);

            }
            catch (Exception ex)
            {
                LogHelper.Instance.Error("AES加密失败：letter=" + letter, ex);
                return null;
            }
            finally
            {
                if (swEncrypt != null)
                {
                    swEncrypt.Close();
                }
                if (csEncrypt != null)
                {
                    csEncrypt.Close();
                }
                if (msEncrypt != null)
                {
                    msEncrypt.Close();
                }
                if (aesAlg != null)
                {
                    aesAlg.Clear();
                }
            }
            return ToBase64(msEncrypt.ToArray());
        }

        public static string AESDecrypt(string cipher)
        {
            MemoryStream msDecrypt = null;
            CryptoStream csDecrypt = null;
            StreamReader srDecrypt = null;
            Aes aesAlg = null;
            string letter = null;
            try
            {
                var decryptedBuffer = Base64ToBytes(cipher);
                aesAlg = Aes.Create();
                aesAlg.Key = Encoding.UTF8.GetBytes(Key);
                aesAlg.IV = Encoding.UTF8.GetBytes(IV);
                var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                msDecrypt = new MemoryStream(decryptedBuffer);
                csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
                srDecrypt = new StreamReader(csDecrypt);
                letter = srDecrypt.ReadToEnd();
            }
            catch (Exception ex)
            {
                LogHelper.Instance.Error("AES解密失败：cipher=" + cipher, ex);
                return null;
            }
            finally
            {
                if (srDecrypt != null)
                {
                    srDecrypt.Close();
                }
                if (csDecrypt != null)
                {
                    csDecrypt.Close();
                }
                if (msDecrypt != null)
                {
                    msDecrypt.Close();
                }
                if (aesAlg != null)
                {
                    aesAlg.Clear();
                }
            }
            return letter;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="binBuffer">Byte[]</param>
        /// <returns></returns>
        public static string ToBase64(byte[] binBuffer)
        {
            var base64ArraySize = (int)Math.Ceiling(binBuffer.Length / 3d) * 4;
            var charBuffer = new char[base64ArraySize];
            Convert.ToBase64CharArray(binBuffer, 0, binBuffer.Length, charBuffer, 0);
            var s = new string(charBuffer);
            return s;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="base64">Base64编À¨¤码?文?本À?</param>
        /// <returns></returns>
        public static byte[] Base64ToBytes(string base64)
        {
            //解决url将+号变为空格
            base64 = base64.Replace(' ', '+');
            var charBuffer = base64.ToCharArray();
            var bytes = Convert.FromBase64CharArray(charBuffer, 0, charBuffer.Length);
            return bytes;
        }
    }
}
