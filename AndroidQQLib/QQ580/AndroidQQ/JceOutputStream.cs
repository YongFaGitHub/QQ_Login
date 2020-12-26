using AndroidQQLib.QQ580.AndroidOnly;
using AndroidQQLib.QQ580.datatype;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AndroidQQLib.QQ580.AndroidQQ
{
    public class JceOutputStream: Staticdata
    {
        _pack pk = new _pack();
        public JceOutputStream()
        {
            pk = new _pack();
        }
        public void clear()
        {
            pk.Empty();
        }
        public byte[] toByteArray()
        {
            return pk.GetAll();
        }

        public void wrap(byte[] b)
        {
            pk.SetData(b);
        }

        public void WriteHead(int p_val,int p_tag)
        {
            int t_val = 0;

            if (p_tag >= 15)
            {
                t_val = p_val | 240;
                pk.SetByte(t_val.ToString());
                pk.SetByte(p_tag.ToString());
            }
            else
            {
                t_val = p_val | (p_tag << 4);
                pk.SetByte(t_val.ToString());
            }
        }
        public void WriteObj(int p_type,int p_val, int p_tag)
        {
            if (p_type == this.TYPE_BYTE)
                this.WriteByte((p_val), p_tag);
            else if (p_type == this.TYPE_SHORT)
                this.WriteShort((p_val), p_tag);
            else if (p_type == this.TYPE_INT)
                this.WriteInt((p_val), p_tag);
            else if (p_type == this.TYPE_LONG)
                this.WriteLong((p_val), p_tag); 
            else if (p_type == this.TYPE_MAP)
                Console.WriteLine("error can# t write map "); 
            else
                Console.WriteLine("error this.WriteObj  typ = " + p_type);
        }

        public void WriteObj(int p_type, byte[] p_val, int p_tag)
        { 
            if (p_type == this.TYPE_SIMPLE_LIST)
                this.WriteSimpleList(p_val, p_tag);
            else if (p_type == this.TYPE_MAP)
                Console.WriteLine("error can# t write map ");
            else if (p_type == this.TYPE_STRING1)
                this.WriteStringByte(AndroidOnly.AndroidOnly.list2str(p_val), p_tag);
            else if (p_type == this.TYPE_LIST)
                this.WriteList(p_val, p_tag);
            else if (p_type == this.TYPE_STRING4)
                this.WriteStringByte(AndroidOnly.AndroidOnly.list2str(p_val), p_tag);
            else
                Console.WriteLine("error this.WriteObj  typ = " + p_type);
        }

        public void WriteByte(int p_val,int p_tag)
        {
            if(p_val == 0)
            {
                WriteHead(TYPE_ZERO_TAG, p_tag);
            }
            else
            {
                WriteHead(TYPE_BYTE, p_tag);
                pk.SetByte(p_val.ToString());
            }
        }

        public void WriteShort(int p_val, int p_tag)
        {
            if (p_val >= -128 && p_val <= 127)
            {
                WriteByte(p_val, p_tag);
            }
            else
            {
                WriteHead(TYPE_SHORT, p_tag);
                pk.SetShort(p_val.ToString());
            }
        }

        public void WriteInt(int p_val, int p_tag)
        {
            if (p_val >= -32768 && p_val <= 32767)
            {
                WriteShort(p_val, p_tag);
            }
            else
            {
                WriteHead(TYPE_INT, p_tag);
                pk.SetInt(p_val.ToString());
            }
        }

        public void WriteLong(int p_val, int p_tag)
        {
            if (p_val >= -2147483648 && p_val <= 2147483647)
            {
                WriteInt(p_val, p_tag);
            }
            else
            {
                WriteHead(TYPE_LONG, p_tag);
                _bin Xbin = new _bin();
                pk.SetBin(Xbin.Long2Bin(p_val.ToString()));
            }
        }

        public void WriteByteString(string p_val, int p_tag)
        {
            byte[] t_val;
            t_val = AndroidOnly.AndroidOnly.tozjj(p_val, "str2");
            if(t_val.Length >255)
            {
                WriteHead(TYPE_STRING4, p_tag);
                pk.SetInt(t_val.Length.ToString());
                pk.SetBin(t_val);
            }
            else
            {
                WriteHead(TYPE_STRING1, p_tag);
                pk.SetByte(t_val.Length.ToString());
                pk.SetBin(t_val);
            }
        }
        public void WriteStringByte(string p_val, int p_tag)
        {
            byte[] t_val;
            t_val = AndroidOnly.AndroidOnly.tozjj(p_val, "str", 0);
            if (t_val.Length > 255)
            {
                WriteHead(TYPE_STRING4, p_tag);
                pk.SetInt(t_val.Length.ToString());
                pk.SetBin(t_val);
            }
            else
            {
                WriteHead(TYPE_STRING1, p_tag);
                pk.SetByte(t_val.Length.ToString());
                pk.SetBin(t_val);
            }
        }

        public void WriteJceStruct(byte[] p_val, int p_tag)
        {
            WriteHead(TYPE_STRUCT_BEGIN, p_tag);
            pk.SetBin(p_val);
            WriteHead(TYPE_STRUCT_END, 0);
        }

        public void WriteSimpleList(byte[] p_val, int p_tag)
        {
            WriteHead(TYPE_SIMPLE_LIST, p_tag);
            WriteHead(0, 0);
            WriteInt(p_val.Length, 0);
            pk.SetBin(p_val);
        }

        public void WriteList(byte[] p_val, int p_tag)
        {
            WriteHead(TYPE_LIST, p_tag);
            WriteHead(p_val.Length, 0);
            for (int i = 0; i < p_val.Length; i++)
            {
                WriteInt(p_val[i], 0);
            }
        }
        public void WriteMap(List<JceMap> p_key, int p_tag)
        {
            int i = 0;
            int l = 0;
            WriteHead(TYPE_MAP, p_tag);
            l = p_key.Count;
            WriteShort(l, 0);

            if (l == 0)
            {
                return;
            }

            for (int j = 0; j < l; j++)
            {
                WriteObj(p_key[i].key_type, p_key[i].key.ToArray(), 0);
                WriteObj(p_key[i].val_type, p_key[i].val.ToArray(), 1);
            }
        }

        public void putHex(string h)
        {
            pk.SetHex(h);
        }
    }
}
