using AndroidQQLib.QQ580.datatype;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AndroidQQLib.QQ580.AndroidQQ
{
    public class JceStruct_Factory: Staticdata
    {
        public void Read_RequestPacket(JceInputStream in_, JceStruct_RequestPacket struct_)
        {
            struct_.iversion = in_.ReadShort(1);
            struct_.cPacketType = in_.ReadShort(2);
            struct_.iMessageType = in_.ReadShort(3);
            struct_.iRequestId = in_.ReadInt(4);
            struct_.sServantName = in_.ReadString(5);
            struct_.sFuncName = in_.ReadString(6);
            struct_.sBuffer = in_.ReadSimpleList(7);
            struct_.iTimeout = in_.ReadInt(8);
            struct_.context = new List<JceMap>();
            in_.ReadMap(9, struct_.context);
            struct_.status = new List<JceMap>();
            in_.ReadMap(10, struct_.status);
        }

        public void Write_RequestPacket(JceOutputStream out_, JceStruct_RequestPacket struct_)
        {
            out_.WriteShort(struct_.iversion, 1);
            out_.WriteShort(struct_.cPacketType, 2);
            out_.WriteShort(struct_.iMessageType, 3);
            out_.WriteInt(struct_.iRequestId, 4);
            out_.WriteStringByte(struct_.sServantName, 5);
            out_.WriteStringByte(struct_.sFuncName, 6);
            out_.WriteSimpleList(struct_.sBuffer, 7);
            out_.WriteInt(struct_.iTimeout, 8);
            out_.WriteMap(struct_.context, 9);
            out_.WriteMap(struct_.status, 10);
        }

        public void Write_SvcReqRegister(JceOutputStream out_, JceStruct_SvcReqRegister struct_)
        {
            out_.WriteLong((int)struct_.lUin, 0);
            out_.WriteLong(struct_.lBid, 1);
            out_.WriteByte(struct_.cConnType, 2);
            out_.WriteStringByte(struct_.sOther, 3);
            out_.WriteInt(struct_.iStatus, 4);
            out_.WriteByte(struct_.bOnlinePush, 5);
            out_.WriteByte(struct_.bIsOnline, 6);
            out_.WriteByte(struct_.bIsShowOnline, 7);
            out_.WriteByte(struct_.bKikPC, 8);
            out_.WriteByte(struct_.bKikWeak, 9);
            out_.WriteLong(struct_.timeStamp, 10);
            ;
            out_.WriteByte(struct_._11, 11);
            out_.WriteByte(struct_._12, 12);
            out_.WriteStringByte(struct_._13, 13);
            out_.WriteByte(struct_._14, 14);
            out_.WriteSimpleList(struct_._imei_, 16);
            out_.WriteShort(struct_._17, 17);
            out_.WriteByte(struct_._18, 18);
            out_.WriteStringByte(struct_._19_device, 19);
            out_.WriteStringByte(struct_._20_device, 20);
            out_.WriteStringByte(struct_._21_sys_ver, 21);
        }

        
    }
}
