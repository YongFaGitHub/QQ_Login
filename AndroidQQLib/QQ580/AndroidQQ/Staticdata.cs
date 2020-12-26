using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AndroidQQLib.QQ580.AndroidQQ
{
    public class Staticdata
    {
        // 常量：
        public int login_state_logining = 0; // 正在登陆
        public int login_state_veriy = 1; // 需要验证码
        public int login_state_success = 2;  // 验证成功
        public int TYPE_BYTE = 0;
        public int TYPE_DOUBLE = 5;
        public int TYPE_FLOAT = 4;
        public int TYPE_INT = 2;
        public int TYPE_JCE_MAX_STRING_LENGTH = 104857600;
        public int TYPE_LIST = 9;
        public int TYPE_LONG = 3;
        public int TYPE_MAP = 8;
        public int TYPE_SHORT = 1;
        public int TYPE_SIMPLE_LIST = 13;
        public int TYPE_STRING1 = 6;
        public int TYPE_STRING4 = 7;
        public int TYPE_STRUCT_BEGIN = 10;
        public int TYPE_STRUCT_END = 11;
        public int TYPE_ZERO_TAG = 12;
        public int Event_Get_Firends = 1; // 自己的好友
        public int Event_Get_Neighbor = 2; // 附近人
        public string kjdz = "68 74 74 70 3A 2F 2F 75 73 65 72 2E 71 7A 6F 6E 65 2E 71 71 2E 63 6F 6D 2F"; // 空间地址
        public string mood = "2F 6D 6F 6F 64 2F";
        public string gdz = "3C 41 01 37 58 00 08 00 30 16 01 30 00 04 16 00 00 05 16 41";// 固定值
        public string findpeople = "0A 0C 1C";
    }
}
