using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AndroidQQLib.QQ580.AndroidOnly
{
    public class _hash
    {
        public byte[] QQTEA(byte[] data ,byte[] key)
        { 
            QTea qTea = new QTea();
            return qTea.Hash(data, key); 
        }
        public byte[] UNQQTEA(byte[] data, byte[] key)
        {
            QTea qTea = new QTea();
            return qTea.UnHash(data, key);
        }

        public static byte[] md5_bin(string data)
        {
            string t = MD5Encrypt(data);

            return AndroidOnly.tozjj(t); 
        }

        public static byte[] md5_bin(byte[] data)
        {
            string t = MD5Encrypt(data);

            return AndroidOnly.tozjj(t);
        }

        /// <summary>
        /// 用MD5加密字符串 
        /// </summary>
        /// <param name="password">待加密的字符串</param> 
        /// <returns>返回的加密后的字符串</returns>
        public static string MD5Encrypt(string password)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] t = Encoding.UTF8.GetBytes(password);
            string a = BitConverter.ToString(md5.ComputeHash(t, 0, t.Length));
            a = a.Replace("-", "");
            return a.ToUpper();
        }

        /// <summary>
        /// 用MD5加密字符串 
        /// </summary>
        /// <param name="password">待加密的字符串</param> 
        /// <returns>返回的加密后的字符串</returns>
        public static string MD5Encrypt(byte[] b)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider(); 
            string a = BitConverter.ToString(md5.ComputeHash(b, 0, b.Length));
            a = a.Replace("-", "");
            return a.ToUpper();
        }
    }
}
