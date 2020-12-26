using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AndroidQQLib.QQ580.AndroidOnly
{
    public class _pack
    {
        List<byte> m_bin;
        public _pack()
        {
            m_bin = new List<byte>(); 
        }

        public void Empty()
        {
            m_bin = new List<byte>();
        }

        public byte[] GetAll()
        {
            return m_bin.ToArray();
        }

        public int Len()
        {
            return m_bin.Count;
        }
        
        public void SetBin(byte t)
        {
            m_bin.Add(t);
        }

        public void SetBin(byte[] t)
        {
            m_bin.AddRange(t);
        }

        public void SetByte(string t)
        {
            _bin Xbin = new _bin();
            m_bin.AddRange(Xbin.Byte2Bin(t));
        }
        public void SetData(byte[] t)
        {
            m_bin = t.ToList();
        }

        public void SetHex(string t)
        {
            t = t.Replace(" ","");
            _bin Xbin = new _bin();
            m_bin.AddRange(Xbin.Hex2Bin(t).ToList());
        }
        public void SetInt(string t)
        { 
            _bin Xbin = new _bin();
            m_bin.AddRange(Xbin.Int2Bin(t).ToList());
        }

        public void SetShort(string t)
        {
            _bin Xbin = new _bin();
            m_bin.AddRange(Xbin.Short2Bin(t).ToList());
        }

        public void SetLong(string t)
        {
            m_bin.AddRange(AndroidOnly.tozjj(t, "int").ToList());
        }

        public void SetUint(string t)
        {
            byte t2 = byte.Parse(t);
            SetBin(t2);
        }
        public void SetStr(string t)
        {
            m_bin.AddRange(AndroidOnly.tozjj(t, "str", 0).ToList());
        } 
        public void SetToken(string t)
        {
            SetShort(t.Length.ToString());
            byte t2 = byte.Parse(t);
            SetBin(t2);
        }
    }
}
