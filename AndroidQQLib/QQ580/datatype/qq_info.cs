using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AndroidQQLib.QQ580.datatype
{
    class qq_info
    {
        public string Account = "";    //  qq
        public long QQ = 0;  //  qq 10
        public byte[] user = new byte[] { };     //  qq_hex
        public byte[] caption = new byte[] { };     //  qq_utf-8
        public string pas = "";
        public byte[] md51 = new byte[] { };
        public byte[] md52 = new byte[] { };
        public byte[] tim = new byte[] { };
        public byte[] key = new byte[] { };
        public string nick = "";
        public byte[] Token002C = new byte[] { };
        public byte[] Token004C = new byte[] { };     //  A2
        public byte[] Token0058 = new byte[] { };
        public byte[] TGTKey = new byte[] { };
        public byte[] shareKey = new byte[] { };
        public byte[] pub_key = new byte[] { };
        public byte[] _ksid = new byte[] { };
        public byte[] randKey = new byte[] { };
        public byte[] mST1Key = new byte[] { };     //  transport秘药
        public string stweb = "";
        public byte[] skey = new byte[] { };
        public byte[] pskey = new byte[] { };     //  01 6C 暂时没返回
        public byte[] superkey = new byte[] { };     //  01 6D 暂时没返回
        public byte[] vkey = new byte[] { };
        public byte[] sid = new byte[] { };
        public byte[] sessionKey = new byte[] { };
        public int loginState = 0;  //  登陆是否验证成功
        public byte[] VieryToken1 = new byte[] { };    //  验证码token
        public byte[] VieryToken2 = new byte[] { };     //  验证码token
        public byte[] Viery = new byte[] { };     //  y验证码

    }
}
