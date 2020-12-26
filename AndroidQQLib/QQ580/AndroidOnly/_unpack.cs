using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AndroidQQLib.QQ580.AndroidOnly
{
    public class _unpack
    {
        List<byte> m_bin;
        public _unpack()
        {
            m_bin = new List<byte>();
        }

        public byte[] GetAll()
        {
            return m_bin.ToArray();
        }
        public string GetAll_Hex()
        {
            _bin Xbin = new _bin();
            return Xbin.Bin2Hex(m_bin.ToArray());
        }

        public byte[] GetBin(int len)
        {
            byte[] t = m_bin.Take(len).ToArray(); 
            m_bin = m_bin.Skip(len).ToList(); 
            return t; 
        }
        public int GetByte()
        {
            byte[] t = m_bin.Take(1).ToArray();
            m_bin = m_bin.Skip(1).ToList();
            _bin Xbin = new _bin();
            return Xbin.Bin2Byte(t); 
        }
        public int GetInt()
        {
            byte[] t = m_bin.Take(4).ToArray();
            m_bin = m_bin.Skip(4).ToList();
            _bin Xbin = new _bin();
            return Xbin.Bin2Int(t);
        }
        public int GetLong()
        {
            byte[] t = m_bin.Take(8).ToArray();
            m_bin = m_bin.Skip(8).ToList();
            _bin Xbin = new _bin();
            return Xbin.Bin2Long(t);
        }
        public int GetShort()
        {
            byte[] t = m_bin.Take(2).ToArray();
            m_bin = m_bin.Skip(2).ToList();
            _bin Xbin = new _bin();
            return Xbin.Bin2Short(t);
        }

        public byte[] GetToken()
        {
            int le = GetShort();
            byte[] t = GetBin(le);
            return t;
        }
        public int Len()
        {
            return m_bin.Count;
        }

        public void SetData(byte[] t)
        {
            m_bin = t.ToList();
        }

        public void SetData_Hex(string t)
        {
            _bin Xbin = new _bin();
            m_bin = Xbin.Hex2Bin(t).ToList();
        }

    }
}
