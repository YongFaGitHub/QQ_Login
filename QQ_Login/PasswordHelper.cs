using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace QQ_Login
{
    public class PasswordHelper
    {
        /// <summary>
        /// 根据QQ号码和验证码加密密码
        /// </summary>
        /// <param name="qqNum">QQ号码</param>
        /// <param name="password">QQ密码</param>
        /// <param name="verifycode">验证码</param>
        /// <returns>密码密文</returns>
        public static string GetPassword(string qqNum, string password, string verifycode)
        {
            //uin为QQ号码转换为16位的16进制
            int qq;
            int.TryParse(qqNum, out qq);

            qqNum = qq.ToString("x");
            qqNum = qqNum.PadLeft(16, '0');

            String P = hexchar2bin(md5(password));
            String U = md5(P + hexchar2bin(qqNum)).ToUpper();
            String V = md5(U + verifycode.ToUpper()).ToUpper();
            return V;
        }

        public static string md5(string input)
        {
            byte[] buffer = MD5.Create().ComputeHash(Encoding.GetEncoding("ISO-8859-1").GetBytes(input));
            return binl2hex(buffer);
        }

        public static string binl2hex(byte[] buffer)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < buffer.Length; i++)
            {
                builder.Append(buffer[i].ToString("x2"));
            }
            return builder.ToString();
        }

        public static string hexchar2bin(string passWord)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < passWord.Length; i = i + 2)
            {
                builder.Append(Convert.ToChar(Convert.ToInt32(passWord.Substring(i, 2), 16)));
            }
            return builder.ToString();
        }
    }
}
