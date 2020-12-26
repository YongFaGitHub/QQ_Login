using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AndroidQQLib.QQ580
{
    /**//// <summary>
        /// 加密解密QQ消息包的工具类.
        /// </summary>
    public static class QQCrypter
    {
        private static void code(byte[] In, int inOffset, int inPos, byte[] Out, int outOffset, int outPos, byte[] key)
        {
            if (outPos > 0)
            {
                for (int i = 0; i < 8; i++)
                {
                    In[outOffset + outPos + i] = (byte)(In[inOffset + inPos + i] ^ Out[outOffset + outPos + i - 8]);
                }
            }
            uint[] formattedKey = FormatKey(key);
            uint y = ConvertByteArrayToUInt(In, outOffset + outPos);
            uint z = ConvertByteArrayToUInt(In, outOffset + outPos + 4);
            uint sum = 0;
            uint delta = 0x9e3779b9;
            uint n = 16;
            while (n-- > 0)
            {
                sum += delta;
                y += ((z << 4) + formattedKey[0]) ^ (z + sum) ^ ((z >> 5) + formattedKey[1]);
                z += ((y << 4) + formattedKey[2]) ^ (y + sum) ^ ((y >> 5) + formattedKey[3]);
            }
            Array.Copy(ConvertUIntToByteArray(y), 0, Out, outOffset + outPos, 4);
            Array.Copy(ConvertUIntToByteArray(z), 0, Out, outOffset + outPos + 4, 4);
            if (inPos > 0)
            {
                for (int i = 0; i < 8; i++)
                {
                    Out[outOffset + outPos + i] = (byte)(Out[outOffset + outPos + i] ^ In[inOffset + inPos + i - 8]);
                }
            }
        }
        private static void decode(byte[] In, int inOffset, int inPos, byte[] Out, int outOffset, int outPos, byte[] key)
        {
            if (outPos > 0)
            {
                for (int i = 0; i < 8; i++)
                {
                    Out[outOffset + outPos + i] = (byte)(In[inOffset + inPos + i] ^ Out[outOffset + outPos + i - 8]);
                }
            }
            else
            {
                Array.Copy(In, inOffset, Out, outOffset, 8);
            }
            uint[] formattedKey = FormatKey(key);
            uint y = ConvertByteArrayToUInt(Out, outOffset + outPos);
            uint z = ConvertByteArrayToUInt(Out, outOffset + outPos + 4);
            uint sum = 0xE3779B90;
            uint delta = 0x9e3779b9;
            uint n = 16;
            while (n-- > 0)
            {
                z -= ((y << 4) + formattedKey[2]) ^ (y + sum) ^ ((y >> 5) + formattedKey[3]);
                y -= ((z << 4) + formattedKey[0]) ^ (z + sum) ^ ((z >> 5) + formattedKey[1]);
                sum -= delta;
            }
            Array.Copy(ConvertUIntToByteArray(y), 0, Out, outOffset + outPos, 4);
            Array.Copy(ConvertUIntToByteArray(z), 0, Out, outOffset + outPos + 4, 4);
        }
        /**//// <summary>
            /// 解密
            /// </summary>
            /// <param name="In">密文</param>
            /// <param name="offset">密文开始的位置</param>
            /// <param name="len">密文长度</param>
            /// <param name="key">密钥</param>
            /// <returns>返回明文</returns>
        public static byte[] Decrypt(byte[] In, int offset, int len, byte[] key)
        {
            // 因为QQ消息加密之后至少是16字节，并且肯定是8的倍数，这里检查这种情况
            if ((len % 8 != 0) || (len < 16))
            {
                return null;
            }
            byte[] Out = new byte[len];
            for (int i = 0; i < len; i += 8)
            {
                decode(In, offset, i, Out, 0, i, key);
            }
            for (int i = 8; i < len; i++)
            {
                Out[i] = (byte)(Out[i] ^ In[offset + i - 8]);
            }
            int pos = Out[0] & 0x07;
            len = len - pos - 10;
            byte[] res = new byte[len];
            Array.Copy(Out, pos + 3, res, 0, len);
            return res;
        }
        public static byte[] Encrypt(byte[] In, int offset, int len, byte[] key)
        {
            // 计算头部填充字节数
            Random Rnd = new Random();
            int pos = (len + 10) % 8;
            if (pos != 0)
            {
                pos = 8 - pos;
            }
            byte[] plain = new byte[len + pos + 10];
            plain[0] = (byte)((Rnd.Next() & 0xF8) | pos);
            for (int i = 1; i < pos + 3; i++)
            {
                plain[i] = (byte)(Rnd.Next() & 0xFF);
            }
            Array.Copy(In, 0, plain, pos + 3, len);
            for (int i = pos + 3 + len; i < plain.Length; i++)
            {
                plain[i] = 0x0;
            }
            // 定义输出流
            byte[] outer = new byte[len + pos + 10];
            for (int i = 0; i < outer.Length; i += 8)
            {
                code(plain, 0, i, outer, 0, i, key);
            }
            return outer;
        }
        private static uint[] FormatKey(byte[] key)
        {
            if (key.Length == 0)
            {
                throw new ArgumentException("Key must be between 1 and 16 characters in length");
            }
            byte[] refineKey = new byte[16];
            if (key.Length < 16)
            {
                Array.Copy(key, 0, refineKey, 0, key.Length);
                for (int k = key.Length; k < 16; k++)
                {
                    refineKey[k] = 0x20;
                }
            }
            else
            {
                Array.Copy(key, 0, refineKey, 0, 16);
            }
            uint[] formattedKey = new uint[4];
            int j = 0;
            for (int i = 0; i < refineKey.Length; i += 4)
            {
                formattedKey[j++] = ConvertByteArrayToUInt(refineKey, i);
            }
            return formattedKey;
        }
        private static byte[] ConvertUIntToByteArray(uint v)
        {
            byte[] result = new byte[4];
            result[0] = (byte)((v >> 24) & 0xFF);
            result[1] = (byte)((v >> 16) & 0xFF);
            result[2] = (byte)((v >> 8) & 0xFF);
            result[3] = (byte)((v >> 0) & 0xFF);
            return result;
        }
        private static uint ConvertByteArrayToUInt(byte[] v, int offset)
        {
            if (offset + 4 > v.Length)
            {
                return 0;
            }
            uint output;
            output = (uint)(v[offset] << 24);
            output |= (uint)(v[offset + 1] << 16);
            output |= (uint)(v[offset + 2] << 8);
            output |= (uint)(v[offset + 3] << 0);
            return output;
        }
    }
}
