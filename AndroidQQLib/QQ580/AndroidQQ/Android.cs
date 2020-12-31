using AndroidQQLib.QQ580.AndroidOnly;
using AndroidQQLib.QQ580.datatype;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace AndroidQQLib.QQ580.AndroidQQ
{
    public class Android: JceStruct_Factory
    {
        private Socket sock;
        private qq_info qq;
        private qq_global gb;
        private Tlv_ tlv;
        private int RequestId;
        private int pc_sub_cmd;
        private string last_error;
        private List<JceStruct_FriendInfo> m_friends; 
        private List<RespEncounterInfo> m_Neighbor;
        private List<byte> m_bin;

        public Android()
        { 
            sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream,ProtocolType.Tcp);

            qq = new qq_info();
            gb = new qq_global();
            tlv = new Tlv_();
            RequestId = 10000;
            pc_sub_cmd = 0;
            last_error = "";
            m_friends = new List<JceStruct_FriendInfo>();
            m_Neighbor = new List<RespEncounterInfo>();
            m_bin = new List<byte>(); 
        }

        public void init(string qquser,string qqpass)
        {
            this.qq.Account = qquser;
            long luin = long.Parse(qquser);
            if(luin > 2147483647)
            {
                _bin Xbin = new _bin();
                qq.user = Xbin.flip(AndroidOnly.AndroidOnly.tozjj(qquser, "int")).Take(4).ToArray();
            }
            else
            {
                long iuin = luin;
                _bin Xbin = new _bin();
                qq.user = Xbin.flip(AndroidOnly.AndroidOnly.tozjj(qquser, "int")); 
            }
            qq.QQ = long.Parse(qquser);
            qq.caption = AndroidOnly.AndroidOnly.tozjj(qquser, "str");
            qq.pas = qqpass;
            qq.md51 = _hash.md5_bin(qqpass);
            string tt = AndroidOnly.AndroidOnly.list2str(qq.md51) + AndroidOnly.AndroidOnly.list2str(new byte[] { 0, 0, 0, 0 }) + AndroidOnly.AndroidOnly.list2str(qq.user);
            List<byte> bb = new List<byte>();
            bb.AddRange(qq.md51);
            bb.AddRange(new byte[] { 0, 0, 0, 0 });
            bb.AddRange(qq.user);

            qq.md52 = _hash.md5_bin(bb.ToArray());

            qq._ksid = AndroidOnly.AndroidOnly.tozjj("93 AC 68 93 96 D5 7E 5F 94 96 B8 15 36 AA FE 91");

            gb.imei = "866819027236650";

            gb.ver = AndroidOnly.AndroidOnly.tozjj("5.8.0.2505", "str");

            gb.appId = 537042771;
            gb.pc_ver = "1F41";
            gb.os_type = "android";
            gb.os_version = "4.4.4";
            gb._network_type = 2;
            gb._apn = "wifi";
            gb.device = "Huawei";
            gb._apk_id = "com.tencent.mobileqq";
            gb._apk_v = "5.8.0.2505";
            gb._apk_sig = AndroidOnly.AndroidOnly.tozjj("A6 B7 45 BF 24 A2 C2 77 52 77 16 F6 F3 6E B6 8D");
            gb.imei_ = AndroidOnly.AndroidOnly.tozjj("38 36 36 38 31 39 30 32 37 32 33 36 36 35 30");
            RequestId = 10000;
            qq.Token002C = new byte[] { };
            qq.Token004C = new byte[] { };
            qq.key = new byte[16];
        }

        public byte[] getViery()
        {
            return qq.Viery;
        }

        public string Read_people(string b) //解开关键字找到的人
        {
            return b;
        }

        public byte[] tcp(string method = "", byte[] data =null, int tim = 0) 
        {
            sock.ReceiveTimeout = tim;
            sock.SendTimeout = tim;
            if (method == "connect")
            { 
                int port = 8080; 
                string host = "msfwifi.3g.qq.com";
                IPHostEntry hostInfo = Dns.GetHostEntry(host);
                IPAddress ip = hostInfo.AddressList[0];
                IPEndPoint ipe = new IPEndPoint(ip, port); 
                sock.Connect(ipe);
            }
            else if(method == "disconnect")
            {
                sock.Close(); 
                sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            }
            else if (method == "send")
            { 
                string tt = AndroidOnly.AndroidOnly.list2str(data);
                byte[] ttt = Encoding.UTF8.GetBytes(tt);
                sock.Send(data);
            }
            else if (method == "rec")
            {
                byte[] dat = new byte[20480];
                int t = sock.Receive(dat);
                byte[] tt = dat.Take(t).ToArray();
                return tt;
            }
            else
            {

            }
            return new byte[] { };
        }

        public string getLastError()
        {
            return this.last_error;
        }

        public int getSubCmd()
        {
            if (pc_sub_cmd > 2147483647)
            {
                pc_sub_cmd = 0;
            }
            pc_sub_cmd = pc_sub_cmd + 1;
            return pc_sub_cmd;
        }

        public byte[] Make_login_sendSsoMsg(string servicecmd,byte[] wupbuffer,byte[] ext_bin ,string imei,byte[] ksid,byte[] ver,int islogin)   //登陆使用
        {
            _pack pk = new _pack();
            byte[] msgcookies = AndroidOnly.AndroidOnly.tozjj("B6CC78FC");
            byte[] tmp = new byte[] { };

            pk.Empty();
            pk.SetInt(RequestId.ToString());
            pk.SetInt(gb.appId.ToString());
            pk.SetInt(gb.appId.ToString());
            pk.SetHex("01 00 00 00 00 00 00 00 00 00 00 00");

            pk.SetInt((ext_bin.Length + 4).ToString());
            pk.SetBin(ext_bin);

            pk.SetInt((servicecmd.Length + 4).ToString());
            pk.SetBin(AndroidOnly.AndroidOnly.tozjj(servicecmd, "str"));

            pk.SetInt((msgcookies.Length + 4).ToString());
            pk.SetBin(msgcookies);

            pk.SetInt((imei.Length + 4).ToString());
            pk.SetBin(AndroidOnly.AndroidOnly.tozjj(imei, "str"));
            pk.SetInt((ksid.Length + 4).ToString());
            pk.SetBin(ksid);

            pk.SetShort((ver.Length + 2).ToString());
            pk.SetBin(ver);

            tmp = pk.GetAll();
            pk.Empty();
            pk.SetInt((tmp.Length + 4).ToString());
            pk.SetBin(tmp);
            tmp = pk.GetAll();

            pk.Empty();
            pk.SetBin(tmp);
            pk.SetInt((wupbuffer.Length + 4).ToString());
            pk.SetBin(wupbuffer);
            if (islogin == 1)
            {
                islogin = 0;
            }
            else
            {
                islogin = 1;
            }
            _hash Hash = new _hash();
            byte[] t = Hash.QQTEA(pk.GetAll(), qq.key);
            t =  Pack((t), islogin);
            return t;
        }

        public byte[] Make_sendSsoMsg(string serviceCmd,byte[] wupBuffer)
        {
            _pack pk = new _pack();
            byte[] msgCookies = AndroidOnly.AndroidOnly.tozjj("B6CC78FC");
            byte[] tmp = new byte[] { };
            pk.Empty();
            pk.SetInt((serviceCmd.Length + 4).ToString());
            pk.SetBin(AndroidOnly.AndroidOnly.tozjj(serviceCmd, "str", 0));
            pk.SetInt((msgCookies.Length + 4).ToString());
            pk.SetBin(msgCookies);
            tmp = pk.GetAll();
            pk.Empty();
            pk.SetInt((tmp.Length + 4).ToString());
            pk.SetBin(tmp);
            pk.SetBin(wupBuffer);
            _hash Hash = new _hash();
            byte[] t = Hash.QQTEA(pk.GetAll(),qq.key);
            t = Pack(t, 2);
            return t;
        }
        public byte[]  Make_sendSsoMsg_simple(string  serviceCmd, int iversion, int iRequestId, string sServantName, string sFuncName, string mapKey, byte[] wupBuffer)      // 一个key
        {
            return Make_sendSsoMsg(serviceCmd, Pack_sendSsoMsg_simple(iversion, iRequestId, sServantName, sFuncName, mapKey, wupBuffer));
        }

        public byte[] Pack(byte[] b,int typ)
        {
            _pack pk =new _pack();
            pk.Empty();

            if(typ==0)
            {
                pk.SetHex("00 00 00 08 02 00 00 00 04");
            }
            else if(typ==1)
            {
                pk.SetHex("00 00 00 08 01 00 00");
                pk.SetShort((qq.Token002C.Length + 4).ToString());
                pk.SetBin(qq.Token002C);
            }
            else
            {
                pk.SetHex("00 00 00 09 01");
                pk.SetInt(RequestId.ToString());
            }

            pk.SetHex("00 00 00");
            pk.SetShort((qq.caption.Length + 4).ToString());
            pk.SetBin(qq.caption);
            pk.SetBin(b);
            b = pk.GetAll();
            pk.Empty();
            pk.SetInt((b.Length + 4).ToString());
            pk.SetBin(b);
            b = pk.GetAll();
            return b; 
        }
        public byte[] Pack_sendSsoMsg_simple(int iversion, int iRequestId, string sServantName, string sFuncName,string mapKey,byte[] wupBuffer)
        {
            JceStruct_RequestPacket req = new JceStruct_RequestPacket();
            JceOutputStream out_ = new JceOutputStream();
            List<JceMap> m = new List<JceMap>();
            m.Add(new JceMap());
            byte[] b = new byte[] { };
            _pack pk = new _pack();

            out_.clear();
            out_.WriteJceStruct(wupBuffer, 0);
            b = out_.toByteArray();
            out_.clear();
            m[0].key_type = TYPE_STRING1;
            m[0].val_type = TYPE_SIMPLE_LIST;
            m[0].key = AndroidOnly.AndroidOnly.tozjj(mapKey, "str", 0).ToList();
            m[0].val = b.ToList();
            out_.WriteMap(m, 0);
            b = out_.toByteArray();
            out_.clear();
            req.iversion = iversion;
            req.iRequestId = iRequestId;
            req.sServantName = sServantName;
            req.sFuncName = sFuncName;
            req.sBuffer = b;
            req.context = new List<JceMap>();
            req.status = new List<JceMap>();
            Write_RequestPacket(out_, req);
            b = out_.toByteArray();
            pk.Empty();
            pk.SetInt((b.Length + 4).ToString());
            pk.SetBin(b);
            return pk.GetAll();
        }
        public byte[] Pack_Login()
        {
            string tt = "";
            _pack pk = new _pack();
            byte[] b = new byte[] { };
            byte[] tmp = new byte[] { };
            byte[] wupbuffer = new byte[] { };
            byte[] tlv109 = new byte[] { };
            byte[] tlv124 = new byte[] { };
            byte[] tlv128 = new byte[] { };
            byte[] tlv16e = new byte[] { };

            qq.shareKey = AndroidOnly.AndroidOnly.tozjj("957C3AAFBF6FAF1D2C2F19A5EA04E51C");
            qq.pub_key = AndroidOnly.AndroidOnly.tozjj("02244B79F2239755E73C73FF583D4EC5625C19BF8095446DE1");
            _bin Xbin = new _bin();

            //qq.TGTKey = Xbin.GetRandomBin(16);
            qq.TGTKey = new byte[16] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            //self.qq.TGTKey = [0 for i in range(16)] //写死TGTKey，方便测试
            //qq.tim = Xbin.flip(AndroidOnly.AndroidOnly.tozjj(Common.GetTimeStamp().ToString(), "int"));
            qq.tim = new byte[] { 86, 247, 21, 166 };
            //self.qq.tim = [86, 247, 21, 166]  //写死time，方便测试
            //qq.randKey = Xbin.GetRandomBin(16);

            //tlv.tlv114_get0058(new byte[] { 11, 22, 33,44,55,66,77,88,99 });
            qq.randKey = new byte[16] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            //self.qq.randKey = [0 for i in range(16)] //写死randKey，方便测试

            pk.Empty();
            pk.SetHex("00 09");
            pk.SetShort(19.ToString());       // 00 13 //下面tlv个数

            pk.SetBin(this.tlv.tlv18(this.qq.user));
            pk.SetBin(this.tlv.tlv1(this.qq.user, this.qq.tim));
            pk.SetBin(this.tlv.tlv106(this.qq.user, this.qq.md51, this.qq.md52, this.qq.TGTKey, this.gb.imei_, this.qq.tim, this.gb.appId.ToString()));
            pk.SetBin(this.tlv.tlv116());
            pk.SetBin(this.tlv.tlv100(this.gb.appId.ToString()));
            pk.SetBin(this.tlv.tlv107());
            pk.SetBin(this.tlv.tlv108(this.qq._ksid));
            this.qq._ksid = new byte[] { };


            tlv109 = this.tlv.tlv109(this.gb.imei_);
            tlv124 = this.tlv.tlv124(this.gb.os_type, this.gb.os_version, this.gb._network_type.ToString(), this.gb._apn);
            tlv128 = this.tlv.tlv128(this.gb.device, this.gb.imei_);
            tlv16e = this.tlv.tlv16e(this.gb.device);
            pk.SetBin(this.tlv.tlv144(this.qq.TGTKey, tlv109, tlv124, tlv128, tlv16e));
            pk.SetBin(this.tlv.tlv142(this.gb._apk_id));
            pk.SetBin(this.tlv.tlv145(this.gb.imei_));
            pk.SetBin(this.tlv.tlv154(this.RequestId.ToString()));
            pk.SetBin(this.tlv.tlv141(this.gb._network_type.ToString(), this.gb._apn));
            pk.SetBin(this.tlv.tlv8());
            pk.SetBin(this.tlv.tlv16b());
            pk.SetBin(this.tlv.tlv147(this.gb._apk_v, this.gb._apk_sig));
            pk.SetBin(this.tlv.tlv177());
            pk.SetBin(this.tlv.tlv187());
            pk.SetBin(this.tlv.tlv188());
            pk.SetBin(this.tlv.tlv191());
            wupbuffer = pk.GetAll();
            

            _hash Hash = new _hash();
              
            byte[] tb = Hash.QQTEA(wupbuffer, this.qq.shareKey);
             
            wupbuffer = this.Pack_Pc("0810", tb, this.qq.randKey, this.qq.pub_key);

           
            byte[] t = this.Make_login_sendSsoMsg("wtlogin.login", wupbuffer,new byte[] { }, this.gb.imei, this.qq._ksid, this.gb.ver, 1);

            tt = "";
            for (int j=0;j<t.Length;j++)
            {
               tt+=(t[j] + ",");
            }
            return t;

        }

        public byte[] Pack_OidbSvc_0x7a2_0()
        {
            byte[] b = AndroidOnly.AndroidOnly.tozjj("08 A2 0F 10 00 18 00 22 02 08 00"); 
            return (this.Make_login_sendSsoMsg("OidbSvc.0x7a2_0", b, this.qq.Token004C, this.gb.imei, this.qq._ksid, this.gb.ver, 0)); 
        }

        public byte[] Pack_Pc(string cmd,byte[] b,byte[] ext_key,byte[] ext_bin)
        {
            _pack pk = new _pack();
            int ext_bin_null = 0;
            byte[] tmp = new byte[] { };
            pk.Empty();
            pk.SetHex(this.gb.pc_ver);
            pk.SetHex(cmd);
            pk.SetShort(this.getSubCmd().ToString());
            pk.SetBin(this.qq.user);
            pk.SetHex("03 07 00 00 00 00 02 00 00 00 00 00 00 00 00");

            if (ext_bin != new byte[] { } && ext_bin.Length > 0)
            {
                ext_bin_null = 0;
                pk.SetHex("01 01");
            }
            else
            {
                ext_bin_null = 1;
                pk.SetHex("01 02");
            }
            pk.SetBin(ext_key);
            pk.SetHex("01 02");
            pk.SetShort(ext_bin.Length.ToString());

            if (ext_bin_null == 1)
            {
                pk.SetHex("00 00");
            }
            else
            {
                pk.SetBin(ext_bin);
            }

            pk.SetBin(b);
            pk.SetHex("03");
            tmp = pk.GetAll();
            pk.Empty();
            pk.SetHex("02");
            pk.SetShort((tmp.Length + 3).ToString());
            pk.SetBin(tmp);
            tmp = pk.GetAll();
            return tmp; 
        }

        public byte[] Pack_StatSvc_Register_online() //上线
        {
            return this.Pack_StatSvc_Register(7, 11, 0);
        }

        public byte[] Pack_StatSvc_Register(int lBid,int iStatus,int timeStamp) //StatSvc_Register
        {
            // lbid    7上线 0下线
            // istatus    11上线 21下线
            // timestamp     0 上 5下
            JceStruct_RequestPacket req = new JceStruct_RequestPacket();
            JceOutputStream out_ = new JceOutputStream();
            List<JceMap> m = new List<JceMap>();
            m.Add(new JceMap());
            byte[] b;
            JceStruct_SvcReqRegister struct_ =new JceStruct_SvcReqRegister();
            struct_.lUin = this.qq.QQ;
            struct_.lBid = lBid;
            struct_.iStatus = iStatus;
            struct_.timeStamp = timeStamp;
            struct_._11 = 15;
            struct_._12 = 1;
            struct_._imei_ = this.gb.imei_;
            struct_._17 = 2052;
            struct_._19_device = this.gb.device;
            struct_._20_device = this.gb.device;
            struct_._21_sys_ver = this.gb.os_version;
            this.Write_SvcReqRegister(out_, struct_);

            b = out_.toByteArray();
            out_.clear();
            out_.WriteJceStruct(b, 0);
            b = out_.toByteArray();
            out_.clear();
            m[0].key_type = this.TYPE_STRING1;
            m[0].val_type = this.TYPE_SIMPLE_LIST;
            m[0].key =AndroidOnly.AndroidOnly. tozjj("SvcReqRegister", "str").ToList();
            m[0].val = b.ToList();
            out_.WriteMap(m, 0);
            b = out_.toByteArray();
            out_.clear();
            req.iversion = 3;
            req.sServantName = "PushService";
            req.sFuncName = "SvcReqRegister";
            req.sBuffer = b;
            req.context =new List<JceMap>();
            req.status = new List<JceMap>();
            this.Write_RequestPacket(out_, req);
            b = out_.toByteArray();
            byte[] t = this.Make_login_sendSsoMsg("StatSvc.register", b, this.qq.Token004C, this.gb.imei, this.qq._ksid, this.gb.ver, 0);
            return t;
        }

        public string QQ_online() //正式上线
        {
            byte[] b;
            b = this.Fun_send(this.Pack_OidbSvc_0x7a2_0(), 3000);
            this.Fun_recv(b);
            b = this.Fun_send(this.Pack_StatSvc_Register_online(), 3000);
            this.Fun_recv(b);
            if (b.Length <= 0)
            {
                Console.WriteLine("二次登录数据失效或QQ号冻结，请检查或重新取二次登录数据");
            } 
            else {
                Console.WriteLine("登陆完成");
                //启动线程(&循环处理消息, , )
            }
            return System.Text.Encoding.UTF8.GetString(this.Pack_StatSvc_Register_online());
        }

        public void increase_ssoSeq()
        {
            if (RequestId > 2147483647)
            {
                RequestId = 10000;
            }
            else
            {
                RequestId += 1;
            } 
        }
        public byte[] Fun_send(byte[] b, int wait = 0)
        {
            int suc = 0;
            increase_ssoSeq();
            tcp("send", b, wait);
            b = tcp("rec",null, wait);
            return b; 
        }

        public byte[] Un_pack(byte[] b,int index = 0)
        {
            int pos1 = 0;
            pos1 = AndroidOnly.AndroidOnly.findlist(b, qq.caption);
            b = b.Skip((b.Length - (b.Length - pos1 - qq.caption.Length + 1))).ToArray();
            if (index == 1)
            {
                pos1 = AndroidOnly.AndroidOnly.findlist(b, qq.caption);
                b = b.Skip((b.Length - (b.Length - pos1 - qq.caption.Length + 1))).ToArray();
            }
            return b;
        }

        public void Fun_recv(byte[] data)
        {
            byte[] b = new byte[] { };
            int bl = 0;
            byte[] test = new byte[] { };
            _unpack unPack = new _unpack();
            int sso_seq = 0;
            int l = 0;
            string serviceCmd = "";
            int head_len = 0;
            byte[] body_bin = new byte[] { };

            if(data.Length == 0)
            {
                return;
            }
            //一次性把包收完
            b = Un_pack(data);
            //大于一定长度之和会分包发送，要自己处理下哦
            _hash Hash = new _hash();
            b = Hash.UNQQTEA(b,qq.key);
            l = b.Length;
            unPack.SetData(b);
            Console.WriteLine(b.Length);
            //上面包内容
            head_len = unPack.GetInt();
            b = unPack.GetBin(head_len - 4);
            body_bin = unPack.GetAll();
            Console.WriteLine(b.Length);

            unPack.SetData(b);
            sso_seq = unPack.GetInt();

            if (unPack.GetBin(4) == new byte[] { 0, 0, 0, 0 })
            {
                unPack.GetBin(4);
            }
            else
            {
                unPack.GetBin(unPack.GetInt() - 4);
            }
            serviceCmd = AndroidOnly.AndroidOnly.list2str(unPack.GetBin(unPack.GetInt() - 4));

            if (serviceCmd == "wtlogin.login")
            {
                Un_Pack_Login(body_bin);
            }
            else
            {
                Fun_msg_Handle(sso_seq, serviceCmd, body_bin);
            }
        }

        public void Fun_msg_Handle(int sso_seq, string serviceCmd ,byte[] b)
        {

        }

        public void Un_Tlv(byte[] b)
        {
            _unpack unPack =new _unpack();
            int tlv_count = 0;
            byte[] tlv_cmd =new byte[] { };
            int tlv_len = 0;
            unPack.SetData(b);
            tlv_count = unPack.GetShort();
            
            for(int i=0;i<tlv_count;i++)
            {
                tlv_cmd = unPack.GetBin(2);
                tlv_len = unPack.GetShort();
                _bin Xbin = new _bin();
                tlv_get((Xbin.Bin2Hex(tlv_cmd)), unPack.GetBin(tlv_len));
            } 
        }

        public void tlv_get(string tlv_cmd, byte[] b)
        {
            _unpack unPack = new _unpack();

            int l = 0;
            int face = 0;
            int age = 0;
            int gander = 0;
            int i = 0;
            int flag = 0;
            int tim = 0;
            string ip = "";
            unPack.SetData(b);
            if (tlv_cmd == "")
            {
                return;
            }
            tlv_cmd = tlv_cmd.Replace(" ", "");
            if (tlv_cmd == "010A")
            {
                qq.Token004C = b;
            }
            else if (tlv_cmd == "0114")
            {
                qq.Token0058 = tlv.tlv114_get0058(b);
            }
            else if (tlv_cmd == "010E")
            {
                qq.mST1Key = b;
            }
            else if (tlv_cmd == "0103")
            {
                qq.stweb = AndroidOnly.AndroidOnly.tohex(b);
            }
            else if (tlv_cmd == "0138")
            {
                l = unPack.GetInt();
                for (int j = 0; j < l; j++)
                {
                    flag = unPack.GetShort();
                    tim = unPack.GetInt();
                    unPack.GetInt();
                }
            }
            else if (tlv_cmd == "011A")
            {
                l = unPack.GetInt();
                for (int j = 0; j < l; j++)
                {
                    face = unPack.GetShort();
                    age = unPack.GetByte();
                    gander = unPack.GetByte();
                    l = unPack.GetByte();
                    qq.nick = AndroidOnly.AndroidOnly.list2str((unPack.GetBin(l)));
                    Console.WriteLine("昵称", qq.nick, "face：", face, "age:", age, "gander：", gander);
                }
            }
            else if (tlv_cmd == "0120")
            {
                qq.skey = b;
            }
            else if (tlv_cmd == "0136")
            {
                qq.vkey = b;
            }
            else if (tlv_cmd == "0305")
            {
                qq.sessionKey = b;
            }
            else if (tlv_cmd == "0143")
            {
                qq.Token002C = b;
            }
            else if (tlv_cmd == "0164")
            {
                qq.sid = b;
            }
            else if (tlv_cmd == "0130")
            {
                unPack.GetShort();
                tim = unPack.GetInt();
                byte[] ip2 = unPack.GetBin(4);
                ip = (ip2[0]) + "." + (ip2[1]) + "." + (ip2[2]) + "." + (ip2[3]);
                Console.WriteLine("time:", tim, "ip:", ip);
            }
            else if (tlv_cmd == "0105")
            {
                l = unPack.GetShort();
                qq.VieryToken1 = unPack.GetBin(l);
                l = unPack.GetShort();
                qq.Viery = unPack.GetBin(l);
                Console.WriteLine(qq.Viery.Length.ToString(), qq.Viery);
            }
            else if (tlv_cmd == "0104")
            {
                qq.VieryToken2 = b;
            }
            else if (tlv_cmd == "016C")
            {
                qq.pskey = b;
            }
            else
            {
                Console.WriteLine("Unknown tlv_cmd" + tlv_cmd);
            }
        }


        public void Un_Pack_VieryImage(byte[] b)
        {
            _unpack Unpack =new _unpack();
            int i = 0;
            Unpack.SetData(b);
            Unpack.GetBin(3);
            Un_Tlv(Unpack.GetAll());
        }

        public byte[] Un_Pack_Login_Pc(byte[] b)
        {
            _unpack unPack =new _unpack();
            int l = 0;
            int result = 0;
            unPack.SetData(b);
            l = unPack.GetInt();
            b = unPack.GetAll();

            unPack.SetData(b);
            unPack.GetByte();
            l = unPack.GetShort();
            unPack.GetBin(10); //1F 41 08 10 00 01 18 B4 A1 BC
            unPack.GetBin(2);
            result = unPack.GetByte();

            b = unPack.GetBin(l - 16 - 1);
            _hash Hash = new _hash();
            b = Hash.UNQQTEA(b, qq.shareKey);
            if(result != 0)
            {
                if (result == 2)
                {
                    this.Un_Pack_VieryImage(b);
                    this.last_error = "需要输入验证码";
                    this.qq.loginState = this.login_state_veriy; 
                    return new byte[] { };
                }
                Un_Pack_ErrMsg(b);
                b = new byte[] { };
                qq.loginState = login_state_logining;
            }
            return b;
        }
        public void Un_Pack_ErrMsg(byte[] b)
        {
            string title = "";
            string message = "";
            int typ = 0;
            _unpack unPack = new _unpack();
            unPack.SetData(b);
            unPack.GetShort();
            unPack.GetByte();
            unPack.GetInt();
            unPack.GetShort();
            typ = unPack.GetInt();
            title = Encoding.UTF8.GetString(unPack.GetBin(unPack.GetShort())); 
            message = Encoding.UTF8.GetString(unPack.GetBin(unPack.GetShort()));
            last_error = title + ":" + message;
        }

        public int Un_Pack_Login(byte[] b)
        {
            string tt = "";
            int l = 0;

            for(int i=0;i<b.Length;i++)
            {
                tt += b[i] + ",";
            }

            _unpack unPack =new _unpack();
            byte[] _0030 = new byte[] { };
            b = this.Un_Pack_Login_Pc(b);


            if (b.Length == 0)
            {
                return 0;
            }
            unPack.SetData(b);
            unPack.GetShort();
            unPack.GetByte();
            unPack.GetInt();
            l = unPack.GetShort();
            b = unPack.GetBin(l);

            _hash Hash = new _hash();
            b = Hash.UNQQTEA(b, qq.TGTKey);

            Un_Tlv(b);

            qq.key =  qq.sessionKey;

            qq.loginState = login_state_success;
            return 1;

        }

        public byte[] Pack_VieryImage(string code)
        {
            _pack pk = new _pack();
            byte[] wupbuffer;
            pk.Empty();
            pk.SetHex("00 02 00 ");
            pk.SetByte(4.ToString());
            pk.SetBin(tlv.tlv2(code, qq.VieryToken1));
            pk.SetBin(tlv.tlv8());
            pk.SetBin(tlv.tlv104(qq.VieryToken2));
            pk.SetBin(tlv.tlv116());
            wupbuffer = pk.GetAll();
            _hash Hash = new _hash();
            wupbuffer = Pack_Pc("0810", Hash.QQTEA(wupbuffer,qq.shareKey),qq.randKey,qq.pub_key);
            byte[] t = Make_login_sendSsoMsg("wtlogin.login", wupbuffer,new byte[] { }, gb.imei,qq._ksid, gb.ver, 1);
            return t;
        }

        public int Fun_SendCode(string code)
        {
            byte[] b;
            b = Fun_send(Pack_VieryImage(code), 3000);
            Fun_recv(b);
            return qq.loginState;
        }

        public int Fun_Login()
        {
            byte[] b;
            tcp("disconnect");
            qq.loginState = login_state_logining;
            Console.WriteLine("开始登录");
            tcp("connect");
            b = Fun_send(Pack_Login(), 3000);
            if(b.Length == 0)
            {
                last_error = "登陆返回包为空";
                return qq.loginState;
            }
            Fun_recv(b);
            return qq.loginState;
        }

    }
}
