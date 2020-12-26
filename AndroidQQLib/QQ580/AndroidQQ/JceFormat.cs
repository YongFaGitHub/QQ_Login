using AndroidQQLib.QQ580.AndroidOnly;
using AndroidQQLib.QQ580.datatype;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AndroidQQLib.QQ580.AndroidQQ
{
    class JceFormat: Staticdata
    {
        public int typ = 0;
        public int tag = 0;
        public _unpack unpackReq = new _unpack();
        public JceFormat()
        {
            int typ = 0;
            int tag = 0;
            _unpack unpackReq = new _unpack();
        }
        public void wrap(byte[] b)
        {
            this.unpackReq.SetData(b);
        }
         public string ReadValNum(int space)
        {
            string t_val = "";
            int t_len = 0;
            HeadData localHeadData = new HeadData();
            int t_count = 0;
            int t_l = 0;

            if (this.typ == this.TYPE_BYTE)
            {
                t_val = unpackReq.GetByte().ToString();
            }
            else if(this.typ == this.TYPE_SHORT)
            {
                t_val = this.unpackReq.GetShort().ToString();
            }
            else if(this.typ == this.TYPE_INT)
            {
                t_val = this.unpackReq.GetInt().ToString();
            }
            else if(this.typ == this.TYPE_LONG)
            {
                t_val = this.unpackReq.GetLong().ToString();
            }
            else if (this.typ == this.TYPE_SIMPLE_LIST)
            {
               
                this.unpackReq.GetByte();
                if (this.unpackReq.GetByte() == this.TYPE_SHORT)
                {
                    t_l = this.unpackReq.GetShort();
                }
                else
                {
                    t_l = this.unpackReq.GetByte();
                }
                t_val = Encoding.Default.GetString(this.unpackReq.GetBin(t_l));
            }
            else if (this.typ == this.TYPE_MAP)
            {
                localHeadData = this.ReadHead(localHeadData);
                t_val = "{";
                if (this.typ == this.TYPE_ZERO_TAG)
                {

                }
                else
                {
                    localHeadData = this.ReadHead(localHeadData);
                    t_count = this.typ;
                    t_val = t_val + "\n";
                    for(int i=0;i<t_count;i++)
                    {
                        localHeadData = this.ReadHead(localHeadData);
                        t_val = t_val + "k = " + this.ReadValNum(space);
                        localHeadData = this.ReadHead(localHeadData);
                        t_val = t_val + ",  v = " + this.ReadValNum(space) + "  ";
                        if (i != t_count)
                        {
                            t_val = t_val + "\n";
                        }
                      
                    }
                    t_val = t_val + "}";
                }
            } 
            else if(this.typ == this.TYPE_ZERO_TAG)
            {
                t_val = "0";      
            }
            else if(this.typ == this.TYPE_STRING1)
            {
                t_l = this.unpackReq.GetByte();
                t_val = AndroidOnly.AndroidOnly.list2str(this.unpackReq.GetBin(t_l));
            }
            else if(this.typ == this.TYPE_LIST)
            {
                localHeadData = this.ReadHead(localHeadData);
                t_val = "[";
                if (this.typ == this.TYPE_ZERO_TAG)
                {

                }
                else
                {
                    t_val = t_val + "\n";
                    for (int i = 0; i < t_count; i++)
                    {
                        while (i > 0)
                        {
                            localHeadData = this.ReadHead(localHeadData);
                            if (localHeadData.typ == this.TYPE_STRUCT_END)
                            {
                                break;
                            } 
                            if (localHeadData.typ == this.TYPE_STRUCT_BEGIN)
                            {
                                continue;
                            }
                            t_val = t_val + " " + "[" + localHeadData.tag + " " + localHeadData.typ + " " + "]" + this.ReadValNum(space) + "\n";
                        }
                        if (i != t_count)
                        {
                            t_val = t_val + "\n";
                        } 
                    }
                    if (t_count == 0)
                    {
                        t_val = t_val + "\n";
                    } 
                }
                t_val = t_val + "]";
            }
            else if(this.typ == this.TYPE_STRING4)
            {
                t_l = this.unpackReq.GetInt();
                t_val = AndroidOnly.AndroidOnly.list2str(this.unpackReq.GetBin(t_l));
            }
            else
            {
                Console.WriteLine("error ReadValNum  typ = " + this.typ);
            }
            return t_val;
        }
        
        public HeadData ReadHead(HeadData paramHead,byte buffer = new byte())
        {
            int i = 0;
            _unpack unptmp = new _unpack();
            _pack ptmp = new _pack();
            if (buffer == new byte()) 
            {
                i = this.unpackReq.GetByte();
            }
            else
            {
                ptmp.Empty();
                ptmp.SetBin(buffer);
                unptmp.SetData(ptmp.GetAll());
                i = unptmp.GetByte();
            }
            typ = i & 15;
            tag = (i & 240) >> 4;

            paramHead.typ = this.typ;
            paramHead.tag = this.tag;
            if (paramHead.tag == 15)
            {
                paramHead.tag = this.unpackReq.GetByte();
                return paramHead;
            }

            return paramHead;
        }
        public string getName(string[] nameArr,int p_tag)
        {
            int t_len = 0;
            t_len = nameArr.Length;

            if (t_len > 0 && p_tag <= t_len)
            {
                return nameArr[p_tag + 1].ToString();
            }
            return "";
        }

        public string format_requestPack()
        {
            HeadData localHeadData = new HeadData();
            string t_text = "";
            string t_name = "";
            string[] t_name_arr = new string[]
            {
                "", "iversion", "cPacketType", "iMessageType", "iRequestId", "sServantName", "sFuncName", "sBuffer", "iTimeout", "context", "status"
            };
            while(true)
            {
                localHeadData = this.ReadHead(localHeadData);
                t_name = getName(t_name_arr, this.tag);
                t_text = t_text + "[" + localHeadData.tag + " " + localHeadData.typ + " " + t_name + "]" + ReadValNum(0) + "\n";
                if ((unpackReq.GetAll().Length) != 0)
                {
                    break;
                }
            }
            return t_text;
        }

        public string format_JceStruct()
        {
            HeadData localHeadData = new HeadData();
            string t_text = "";
            while(true)
            {
                localHeadData = ReadHead(localHeadData);
                if (typ != TYPE_STRUCT_BEGIN && typ != TYPE_STRUCT_END)
                {
                    t_text = t_text + "[" + localHeadData.tag + " " + localHeadData.typ + " " + "]" + ReadValNum(0) + "\n";
                }
                if ((unpackReq.GetAll().Length) != 0)
                {
                    break;
                } 
            }
            return t_text; 
        }

        public string format_pack_top()
        {
            int ssoSeq_ = 0;
            int appId = 0;
            string serviceCmd = ""; 
            string t_text = "";
            int t_l = 0;
            int t_len = 0;
            ssoSeq_ = this.unpackReq.GetInt();
            t_text = t_text + "ssoSeq_ = " + (ssoSeq_) + "\n";
            appId = this.unpackReq.GetInt();
            t_text = t_text + "appId = " + (appId) + "\n";
            appId = this.unpackReq.GetInt();
            t_text = t_text + "appId = " + (appId) + "\n";
            t_text = t_text + "00 = " + ((this.unpackReq.GetBin(12))) + "\n";
            t_l = this.unpackReq.GetInt() - 4;
            t_text = t_text + "extBin = " + ((this.unpackReq.GetBin(t_l))) + "\n";
            t_l = this.unpackReq.GetInt() - 4;
            serviceCmd = (this.unpackReq.GetBin(t_l).ToString());
            t_text = t_text + "serviceCmd = " + serviceCmd + "\n";
            t_l = this.unpackReq.GetInt() - 4;
            t_text = t_text + "msgCookies = " + ((this.unpackReq.GetBin(t_l))) + "\n";
            t_l = this.unpackReq.GetInt() - 4;
            t_text = t_text + "imei = " + ((this.unpackReq.GetBin(t_l))) + "\n";
            t_l = this.unpackReq.GetInt() - 4;
            t_text = t_text + "ksid = " + ((this.unpackReq.GetBin(t_l))) + "\n";
            t_l = this.unpackReq.GetShort() - 2;
            t_text = t_text + "ver = " + ((this.unpackReq.GetBin(t_l))) + "\n";
            return t_text;
        }
    }
}
