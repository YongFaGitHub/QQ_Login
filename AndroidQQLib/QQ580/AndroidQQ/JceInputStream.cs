using AndroidQQLib.QQ580.AndroidOnly;
using AndroidQQLib.QQ580.datatype;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AndroidQQLib.QQ580.AndroidQQ
{
    public class JceInputStream: Staticdata
    {
        _unpack unpackReq = new _unpack();
        public JceInputStream()
        {
            unpackReq = new _unpack();
        }

        public void wrap(byte[] b)
        {
            unpackReq.SetData(b);
        }

        public int SkipToTag(int p_tag) //跳到指定tag处
        {
            HeadData localHeadData = new HeadData();
            int step = 0;
            while(true)
            {
                step = this.peakHead(localHeadData);
                if(localHeadData.typ == TYPE_STRUCT_END)
                {
                    break;
                }
                if(localHeadData.tag <= p_tag)
                {
                    if(localHeadData.tag != p_tag)
                    {
                        break;
                    }
                    return 1;
                }
                else
                {
                    return 0;
                }
                this.skip(step);
                skipField(localHeadData.typ);
            }
            return 0;
        }
        public void skip(int step)
        {
            unpackReq.GetBin(step);
        }
        public void skipField(int p_type)
        {
            int t_len = 0;
            HeadData localHeadData = new HeadData();
            int t_count = 0;  int i = 0;

            if (p_type == this.TYPE_BYTE)
            {
                this.skip(1);
            }
            else if(p_type == this.TYPE_SHORT)
            {
                this.skip(2);
            }
            else if(p_type == this.TYPE_INT)
            {
                this.skip(4);
            }
            else if(p_type == this.TYPE_LONG)
            {
                this.skip(8);
            }

            else if(p_type == this.TYPE_SIMPLE_LIST)
            {
                this.unpackReq.GetByte();
                localHeadData = ReadHead(localHeadData);
                if (localHeadData.typ == TYPE_ZERO_TAG)
                {

                }
                else
                {
                    if (localHeadData.typ == TYPE_SHORT)
                    {
                        t_len = unpackReq.GetShort();
                    }
                    else
                    {
                        t_len = unpackReq.GetByte();
                    }
                    skip(t_len);
                }
            }
            else if(p_type == TYPE_MAP)
            {
                localHeadData = ReadHead(localHeadData);
                if (localHeadData.typ == TYPE_ZERO_TAG)
                {

                }
                else
                {
                    localHeadData = ReadHead(localHeadData);
                    t_count = localHeadData.typ;

                    for(int j=0;j<t_count;j++)
                    {
                        localHeadData = ReadHead(localHeadData);
                        skipField(localHeadData.typ);
                        localHeadData = ReadHead(localHeadData);
                        skipField(localHeadData.typ);
                    } 
                } 
            }
            else if(p_type == TYPE_ZERO_TAG)
            {

            }
            else if(p_type == TYPE_STRING1)
            {
                t_len = unpackReq.GetByte();
                skip(t_len);
            }
            else if(p_type == TYPE_LIST)
            {
                localHeadData = ReadHead(localHeadData);
                if (localHeadData.typ == TYPE_ZERO_TAG)
                {

                }
                else
                {
                    if(localHeadData.typ == TYPE_SHORT)
                    {
                        t_count = unpackReq.GetShort();
                    }
                    else
                    {
                        t_count = unpackReq.GetByte();
                    }

                    for(int j=0;j<t_count;j++)
                    {
                        localHeadData = ReadHead(localHeadData);
                        skipField(localHeadData.typ);
                    }
                }
            }
            else if(p_type == TYPE_STRING4)
            {
                t_len = unpackReq.GetInt();
                skip(t_len);
            }
            else if(p_type == TYPE_STRUCT_BEGIN)
            {
                localHeadData =  ReadHead(localHeadData);
                while (localHeadData.typ !=  TYPE_STRUCT_END)
                {
                    skipField(localHeadData.typ);
                    localHeadData = ReadHead(localHeadData);
                }
            }
            else
            {
                Console.WriteLine("error skipField  typ = " + p_type);
            } 
        }

        public int peakHead(HeadData localHeadData)
        {
            return ReadHead2(localHeadData, unpackReq.GetAll()[0]);
        }
        public HeadData ReadHead(HeadData paramHead,byte buffer = new byte())
        {
            int i = 0;
            _unpack unptmp = new _unpack();
            _pack ptmp = new _pack();
            if (buffer == new byte())
            {
                i = unpackReq.GetByte();
                paramHead.typ = i & 15;
                paramHead.tag = (i & 240) >> 4;
                if (paramHead.tag == 15)
                {
                    paramHead.tag = unpackReq.GetByte();
                    return paramHead;
                }
                return paramHead;
            }
            else
            {
                ptmp.Empty();
                ptmp.SetBin(buffer);
                unptmp.SetData(ptmp.GetAll());
                i = unptmp.GetByte();
                paramHead.typ = i & 15;
                paramHead.tag = (i & 240) >> 4;
                if (paramHead.tag == 15)
                {
                    paramHead.tag = unptmp.GetByte();
                    return paramHead;
                }
                return paramHead;
            } 
        }
        public int ReadHead2(HeadData paramHead, byte buffer = new byte())
        {
            int i = 0;
            _unpack unptmp = new _unpack();
            _pack ptmp = new _pack();
            if (buffer == new byte())
            {
                i = unpackReq.GetByte();
                paramHead.typ = i & 15;
                paramHead.tag = (i & 240) >> 4;
                if (paramHead.tag == 15)
                {
                    paramHead.tag = unpackReq.GetByte();
                    return 2;
                }
                return 1;
            }
            else
            {
                ptmp.Empty();
                ptmp.SetBin(buffer);
                unptmp.SetData(ptmp.GetAll());
                i = unptmp.GetByte();
                paramHead.typ = i & 15;
                paramHead.tag = (i & 240) >> 4;
                if (paramHead.tag == 15)
                {
                    paramHead.tag = unptmp.GetByte();
                    return 2;
                }
                return 1;
            }
        }

        public string ReadObj(int typ)
        {
            string t_val = "";
            int t_len = 0;
            HeadData localHeadData = new HeadData();
            int t_count = 0;
            int i = 0;

            if (typ == TYPE_BYTE)
            {
                t_val = unpackReq.GetByte().ToString();
            }
            else if (typ == TYPE_SHORT)
            {
                t_val = unpackReq.GetShort().ToString();
            }
            else if (typ == TYPE_INT)
            {
                t_val = unpackReq.GetInt().ToString();
            }
            else if (typ == TYPE_LONG)
            {
                t_val = unpackReq.GetLong().ToString();
            }
            else if (typ == TYPE_SIMPLE_LIST)
            {
                unpackReq.GetByte();

                if (unpackReq.GetByte() == TYPE_SHORT)
                {
                    t_len = unpackReq.GetShort();
                }
                else
                {
                    t_len = unpackReq.GetByte();
                }
                t_val = unpackReq.GetBin(t_len).ToString();
            }
            else if (typ == TYPE_MAP)
            {
                localHeadData = ReadHead(localHeadData);
                t_val = "{";

                if (typ == TYPE_ZERO_TAG)
                {

                }
                else
                {
                    localHeadData = ReadHead(localHeadData);
                    t_count = typ;
                    for (int j = 0; j < t_count; j++)
                    {
                        localHeadData = ReadHead(localHeadData);
                        t_val = t_val + "k = " + ReadObj(localHeadData.typ);
                        localHeadData = ReadHead(localHeadData);
                        t_val = t_val + ",  v = " + ReadObj(localHeadData.typ) + "  ";
                    }

                }
                t_val = t_val + "}";
            }
            else if (typ == TYPE_ZERO_TAG)
            {
                t_val = "0";
            }
            else if (typ == TYPE_STRING1)
            {
                t_len = unpackReq.GetByte();
                t_val = AndroidOnly.AndroidOnly.list2str(unpackReq.GetBin(t_len));
            }
            else if (typ == TYPE_LIST)
            {
                localHeadData = ReadHead(localHeadData);
                t_val = "[";
                if (typ == TYPE_ZERO_TAG)
                {

                }
                else
                {
                    if (typ == TYPE_SHORT)
                    {
                        t_count = unpackReq.GetShort();
                    }
                    else
                    {
                        t_count = unpackReq.GetByte();
                    }
                    for (int j = 0; j < t_count; j++)
                    {
                        localHeadData = ReadHead(localHeadData);
                        t_val = t_val + "" + ReadObj(localHeadData.typ);
                        if (i != t_count)
                        {
                            t_val = t_val + ",";
                        }

                    }
                }
                t_val = t_val + "]";
            }
            else if (typ == TYPE_STRING4)
            {
                t_len = unpackReq.GetInt();
                t_val = AndroidOnly.AndroidOnly.list2str(unpackReq.GetBin(t_len));
            }
            else if (typ == TYPE_STRUCT_BEGIN)
            {
                localHeadData = ReadHead(localHeadData);
                if (localHeadData.typ != TYPE_STRUCT_END)
                {
                    t_val = t_val + ReadObj(localHeadData.typ);
                    localHeadData = ReadHead(localHeadData);
                }
            }
            else
            {
                Console.WriteLine("error ReadValNum  typ = " + typ);
            }
            return t_val;
        }
        public int ReadByte(int p_tag)
        {
            HeadData localHeadData = new HeadData();
            int paramByte = 0;

            if ( SkipToTag(p_tag) == 0)
            {
                return paramByte;
            }
            localHeadData = ReadHead(localHeadData);

            if (localHeadData.typ == TYPE_ZERO_TAG)
            {
                paramByte = 0;
            }
            else if(localHeadData.typ == TYPE_BYTE)
            {
                paramByte = unpackReq.GetByte();
            }
            else if(localHeadData.typ == TYPE_SHORT)
            {
                paramByte = unpackReq.GetShort();
            }
            else
            {
                Console.WriteLine("read Byte :error typ mismatch");
            }
            return paramByte;
        }

        public int ReadShort(int p_tag)
        {
            HeadData localHeadData = new HeadData();
            int paramByte = 0;

            if (SkipToTag(p_tag) == 0)
            {
                return paramByte;
            }
            localHeadData = ReadHead(localHeadData);

            if (localHeadData.typ == TYPE_ZERO_TAG)
            {
                paramByte = 0;
            }
            else if (localHeadData.typ == TYPE_BYTE)
            {
                paramByte = unpackReq.GetByte();
            }
            else if (localHeadData.typ == TYPE_SHORT)
            {
                paramByte = unpackReq.GetShort();
            }
            else
            {
                Console.WriteLine("read short :error typ mismatch");
            }
            return paramByte;
        }
        public int ReadInt(int p_tag)
        {
            HeadData localHeadData = new HeadData();
            int paramByte = 0;

            if (SkipToTag(p_tag) == 0)
            {
                return paramByte;
            }
            localHeadData = ReadHead(localHeadData);

            if (localHeadData.typ == TYPE_ZERO_TAG)
            {
                paramByte = 0;
            }
            else if (localHeadData.typ == TYPE_BYTE)
            {
                paramByte = unpackReq.GetByte();
            }
            else if (localHeadData.typ == TYPE_SHORT)
            {
                paramByte = unpackReq.GetShort();
            }
            else if (localHeadData.typ == TYPE_INT)
            {
                paramByte = unpackReq.GetInt();
            }
            else
            {
                Console.WriteLine("read int :error typ mismatch");
            }
            return paramByte;
        }

        public int ReadLong(int p_tag)
        {
            HeadData localHeadData = new HeadData();
            int paramByte = 0;

            if (SkipToTag(p_tag) == 0)
            {
                return paramByte;
            }
            localHeadData = ReadHead(localHeadData);

            if (localHeadData.typ == TYPE_ZERO_TAG)
            {
                paramByte = 0;
            }
            else if (localHeadData.typ == TYPE_BYTE)
            {
                paramByte = unpackReq.GetByte();
            }
            else if (localHeadData.typ == TYPE_SHORT)
            {
                paramByte = unpackReq.GetShort();
            }
            else if (localHeadData.typ == TYPE_INT)
            {
                paramByte = unpackReq.GetInt();
            }
            else if (localHeadData.typ == TYPE_LONG)
            {
                paramByte = unpackReq.GetLong();
            }
            else
            {
                Console.WriteLine("read long :error typ mismatch");
            }
            return paramByte;
        }

        public string ReadString(int p_tag)
        {
            HeadData localHeadData = new HeadData();
            int t_len = 0;
            string t_val = "";

            if (SkipToTag(p_tag) == 0)
            {
                return t_val;
            }
            localHeadData = ReadHead(localHeadData);

            if (localHeadData.typ == TYPE_ZERO_TAG)
            {

            }
            else if (localHeadData.typ == TYPE_STRING1)
            {
                t_len = unpackReq.GetByte();
                t_val = AndroidOnly.AndroidOnly.list2str(unpackReq.GetBin(t_len));
            }
            else if (localHeadData.typ == TYPE_STRING4)
            {
                t_len = unpackReq.GetInt();
                t_val = AndroidOnly.AndroidOnly.list2str(unpackReq.GetBin(t_len));
            }  

            return t_val;
        }

        public byte[] ReadSimpleList(int p_tag)
        {
            HeadData localHeadData = new HeadData();
            byte[] t_val = new byte[] { };
            int t_l = 0;
            int t_type = 0;

            if (SkipToTag(p_tag) == 0)
            {
                return t_val;
            }
            localHeadData = ReadHead(localHeadData);

            if (localHeadData.typ == TYPE_SIMPLE_LIST)
            {
                unpackReq.GetByte();
                t_type = unpackReq.GetByte();
                if (t_type == TYPE_ZERO_TAG)
                {
                    return t_val;
                }
                else
                {
                    if (t_type == TYPE_SHORT)
                    {
                        t_l = unpackReq.GetShort();
                    }
                    else
                    {
                        t_l = unpackReq.GetByte();
                    }
                }
                t_val = unpackReq.GetBin(t_l);
            }
            return t_val;
        }

        public string ReadList(int p_tag,List<byte> ret_arr = null)
        {
            HeadData localHeadData = new HeadData();
            string t_val = "";
            int t_count = 0;
            int i = 0;

            if (SkipToTag(p_tag) == 0)
            {
                return t_val;
            }
            localHeadData = ReadHead(localHeadData);
            if (localHeadData.typ == TYPE_ZERO_TAG)
            {

            }
            else if(localHeadData.typ == TYPE_LIST)
            {
                t_count = ReadShort(0);
                for(int j=0;j<t_count;j++)
                {
                    localHeadData = ReadHead(localHeadData);
                    t_val = ReadObj(localHeadData.typ);
                    ret_arr.Add(byte.Parse(t_val));
                }
            }
            return t_val;
        }

        public int ReadType()
        {
            HeadData localHeadData = new HeadData();
            localHeadData = ReadHead(localHeadData);
            return localHeadData.typ;
        }

        public int ReadToTag(int p_tag) //返回type -1没找到
        {
            HeadData localHeadData = new HeadData();
            if(SkipToTag(p_tag) == 0)
            {
                return -1;
            }
            localHeadData = ReadHead(localHeadData);
            return localHeadData.typ;
        }
        public void skipToEnd()
        {
            string te = "";
            HeadData localHeadData = new HeadData();
            int step = 0;
            for (int i = 0; i < 100; i++)
            {
                te = AndroidOnly.AndroidOnly.tohex(unpackReq.GetAll());
                localHeadData = ReadHead(localHeadData);
                te = AndroidOnly.AndroidOnly.tohex(unpackReq.GetAll());
                if (localHeadData.typ == TYPE_ZERO_TAG)
                {
                    continue;
                }
                if (localHeadData.typ == TYPE_STRUCT_END)
                {
                    break;
                }
                skipField(localHeadData.typ);
            } 
        }

        public void ReadMap(int p_tag, List<JceMap> p_val)
        {
            HeadData localHeadData = new HeadData();
            string t_val = "";
            int t_count = 0;
            int i = 0;

            JceMap t_map = new JceMap();

            if (SkipToTag(p_tag) == 0)
            {
                return;
            }
            localHeadData = ReadHead(localHeadData);
            if (localHeadData.typ != TYPE_MAP)
            {
                return;
            }
            localHeadData = ReadHead(localHeadData);
            if (localHeadData.typ == TYPE_ZERO_TAG)
            {
                return;
            }
            else
            {
                if(localHeadData.typ ==  TYPE_SHORT)
                {
                    t_count = unpackReq.GetShort();
                }
                else
                {
                    t_count = unpackReq.GetByte();
                }

                for(int j = 0; j < t_count; j++)
                {
                    localHeadData = ReadHead(localHeadData);
                    t_val = ReadObj(localHeadData.typ);
                    t_map.key_type = localHeadData.typ;
                    t_map.key = AndroidOnly.AndroidOnly.tozjj(t_val, "str", 0).ToList();
                    localHeadData = ReadHead(localHeadData);
                    t_val = ReadObj(localHeadData.typ);
                    t_map.val_type = localHeadData.typ;
                    t_map.val = AndroidOnly.AndroidOnly.tozjj(t_val, "str", 0).ToList();
                    p_val.Add(t_map); 
                }
            }
        }

        public byte[] getAll()
        {
            return unpackReq.GetAll();
        } 

    }
}
