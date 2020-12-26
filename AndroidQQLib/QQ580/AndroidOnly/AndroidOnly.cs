using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AndroidQQLib.QQ580.AndroidOnly
{
    public class AndroidOnly
    {
        public static byte[] tozjj(string data, string method = "str2",int thkg= 1) //到字节集
        {
            if (method == "str2")
            {
                if (thkg == 1)
                {
                    data = data.Replace(" ", "");
                }
                List<byte> t = new List<byte>();

                for(int i=0;i<data.Length;i+=2)
                {
                    string tt = data.Substring(i,2);
                    t.Add((byte)Common.Hex2Ten(tt));
                }
                return t.ToArray();
            }
            else if (method == "str")
            {
                if (thkg == 1)
                {
                    data = data.Replace(" ", "");
                }
                data = Common.ToHex(data);
                List<byte> t = new List<byte>();

                for (int i = 0; i < data.Length; i += 2)
                {
                    string tt = data.Substring(i, 2);
                    t.Add((byte)Common.Hex2Ten(tt));
                }
                return t.ToArray();
            }
            else if (method == "byte")
            {
                byte[] t = new byte[] { (byte)(int.Parse(data) % 256) };
                return t;
            }
            else if (method == "int")
            {
                List<byte> t = new List<byte>();
                int n = int.Parse(data);

                if (n == 0)
                {
                    t.Add(0);
                }
                int m = 0;
                while(n>0)
                {
                    m = n % 256;
                    if (m == 0)
                    {
                        m = 256;
                    }
                    t.Add((byte)m);
                    n = (n - m) / 256;
                }
                if(t.Count%4!=0)
                {
                    int l = (t.Count % 4);
                    for (int i = 0; i < 4 - l; i++) 
                    {
                        t.Add(0);
                    }
                }
                return t.ToArray();
            }
            else if (method == "long")
            {
                List<byte> t = new List<byte>();
                long n = long.Parse(data);

                if (n == 0)
                {
                   t.Add(0);
                }
                long m = 0;
                while (n > 0)
                {
                    m = n % 256;
                    if (m == 0)
                    {
                        m = 256;
                    }
                    t.Add((byte)m);
                    n = (n - m) / 256;
                }
                if (t.Count % 8 != 0)
                {
                    int l = (t.Count % 8);
                    for (int i = 0; i < 8 - l; i++)
                    {
                        t.Add(0);
                    }
                }
                return t.ToArray();
            }
            else if (method == "short")
            {
                int n = int.Parse(data);
                List<byte> t = new List<byte>();
                if(n == 0)
                {
                    t.Add(0);
                }
                int m;
                while(n>0)
                {
                    m = n % 256;
                    if (m == 0)
                    {
                        m = 256;
                    }
                    t.Add((byte)(m));
                    n = (n - m) / 256;
                }
                if(t.Count %2 !=0)
                {
                    t.Add(0);
                }
                var tt = new byte[] { t[0], t[1] };
                return tt;
            }
            else
            {

            }
            return new byte[8]; 
        }

        public static string tohex(byte[] t)
        {
            string all = "";
            for (int i = 0; i < t.Length; i++)
            {
                int i3 = t[i];
                string t2 = Convert.ToString(i3, 16);
                if(t2.Length==1)
                {
                    t2 = "0" + t2;
                }
                all += t2;
            }
            return all.ToUpper();
        }

        public static string list2str(byte[] data)
        {
            string t = "";
            for (int i = 0; i < data.Length; i++) 
            {
                byte ii = data[i];

                string t2 = String.Format("{0:X}", ii);
                if (t2.Length == 1)
                {
                    t2 = "0" + t2;
                }
                t += t2;
            }
            
            string r = Common.UnHex(t);
            return r;
        }

        public static int findlist(byte[] rq,byte[] tt)
        {
            int pos1 = -1;
            int pos_tmp = 0;
            for(int i = 0; i < rq.Length; i++)
            {
                int flag = 0;
                pos_tmp += 1;

                for(int j = 0; j < tt.Length; j++)
                {
                    if(rq[i]==tt[j])
                    {
                        i += 1;
                        flag += 1;
                    }
                    else
                    {
                        break;
                    }
                }

                if (flag == tt.Length)
                {
                    pos1 = pos_tmp;
                    break;
                } 
            }
            return pos1; 
        }
    }
}
