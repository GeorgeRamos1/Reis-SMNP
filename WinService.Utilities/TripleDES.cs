using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace WinService.Utilities
{
    public class TripleDES
    {
        private static Byte[] bPassword;
        private static String sPassword;

        TripleDES(String password = "654321")
        {
            setPassword(password);
        }

        public static void setPassword(String value)
        {
            UTF8Encoding UTF8 = new UTF8Encoding();
            MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
            bPassword = hashmd5.ComputeHash(UTF8.GetBytes(value));
            sPassword = value;
            hashmd5.Clear();
        }

        #region Encrypt

        public static String Encrypt(String value)
        {
            Byte[] results;
            UTF8Encoding UTF8 = new UTF8Encoding();
           

            using (MD5CryptoServiceProvider hashProvider = new MD5CryptoServiceProvider())
            {
                Byte[] TDESKey = bPassword;
                using (TripleDESCryptoServiceProvider TDESAlgorithm = new TripleDESCryptoServiceProvider())
                {
                    TDESAlgorithm.Key = TDESKey;
                    TDESAlgorithm.Mode = CipherMode.ECB;
                    TDESAlgorithm.Padding = PaddingMode.PKCS7;
                    Byte[] dataToEncrypt = UTF8.GetBytes(value);
                    try
                    {
                        ICryptoTransform encryptor = TDESAlgorithm.CreateEncryptor();
                        results = encryptor.TransformFinalBlock(dataToEncrypt, 0, dataToEncrypt.Length);
                    }
                    finally
                    {
                        TDESAlgorithm.Clear();
                        hashProvider.Clear();
                    }
                }
            }

            return Convert.ToBase64String(results);
        }

        #endregion

        #region Decrypt

        public static String Decrypt(String value)
        {
            Byte[] results;
            UTF8Encoding UTF8 = new UTF8Encoding();

            using (MD5CryptoServiceProvider hashProvider = new MD5CryptoServiceProvider())
            {
                Byte[] TDESKey = bPassword;
                using (TripleDESCryptoServiceProvider TDESAlgorithm = new TripleDESCryptoServiceProvider())
                {
                    TDESAlgorithm.Key = TDESKey;
                    TDESAlgorithm.Mode = CipherMode.ECB;
                    TDESAlgorithm.Padding = PaddingMode.PKCS7;
                    Byte[] dataToDecrypt = Convert.FromBase64String(value);
                    try
                    {
                        ICryptoTransform encryptor = TDESAlgorithm.CreateDecryptor();
                        results = encryptor.TransformFinalBlock(dataToDecrypt, 0, dataToDecrypt.Length);
                    }
                    finally
                    {
                        TDESAlgorithm.Clear();
                        hashProvider.Clear();
                    }
                }
            }

            return UTF8.GetString(results);
        }

        #endregion

    }
}
