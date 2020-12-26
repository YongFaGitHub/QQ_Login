using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AndroidQQLib.QQ580.AndroidOnly
{
    public class _bin
    {
        public byte[] flip(byte[] data) //反转字节集
        {
            int len = data.Length;
            byte[] t = new byte[len];
            for(int i=0;i<len;i++)
            {
                t[i] = data[len - i - 1];
            }
            return t; 
        }
        
        public byte[] Hex2Bin(string t)
        {
            return AndroidOnly.tozjj(t);
        }

        public byte[] Long2Bin(string t)
        {
            return flip(AndroidOnly.tozjj(t, "long"));
        }
        public byte[] Int2Bin(string t)
        {
            return flip(AndroidOnly.tozjj(t, "int"));
        }
        public byte[] Byte2Bin(string t)
        {
            return AndroidOnly.tozjj(t, "byte");
        }
        public byte[] Short2Bin(string t)
        {
            return flip(AndroidOnly.tozjj(t, "short"));
        }
        public int Bin2Byte(byte[] t)
        {
            if(t.Length == 0 )
            {
                return 0;
            }
            int a = t[0] % 256;
            return a;
        }
        public string Bin2Hex(byte[] t)
        {
            return AndroidOnly.tohex(t);
        }
        public int Bin2Int(byte[] t)
        {
            int n = 0;
            for (int i = 0; i < t.Length; i++)
            {
                if (t[i] == 0)
                {

                }
                else
                {
                    break;
                }
            }

            for (int i = 0; i < t.Length; i++)
            {
                t[i] = (byte)(t[i] % 256);
                n = n + (255 * n + t[i]);
            }
            return n;
        }
        public int Bin2Short(byte[] t)
        {
            t = t.Take(2).ToArray();
            int n = 0;
            for(int i=0; i<t.Length;i++)
            {
                t[i] = (byte)(t[i] % 256);
                n = n + (255 * n + t[i]);
            }
            return n;
        }
        public int Bin2Long(byte[] t)
        {
            return Bin2Int(t);
        }
        public byte[] GetRandomBin(int len)
        {
            List<byte> d = new List<byte>();
            for(int i=0;i<len;i++)
            {
                Random rd = new Random();
                //d.Add((byte)rd.Next(0, 255));
                d.Add(0);
            }
            return d.ToArray();
        }
    }
}
