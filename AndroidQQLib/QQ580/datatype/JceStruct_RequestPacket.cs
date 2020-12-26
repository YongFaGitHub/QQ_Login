using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AndroidQQLib.QQ580.datatype
{
    public class JceStruct_RequestPacket
    {
        public int iversion = 0;
        public int cPacketType = 0;
        public int iMessageType = 0;
        public int iRequestId = 0;
        public string sServantName = "";
        public string sFuncName = "";
        public byte[] sBuffer = new byte[] { };
        public int iTimeout = 0;
        public List<JceMap> context = new List<JceMap>(); //重定义下
        public List<JceMap> status = new List<JceMap>();  //重定义下
    }
}
