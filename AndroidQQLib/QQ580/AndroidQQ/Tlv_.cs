using AndroidQQLib.QQ580.AndroidOnly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AndroidQQLib.QQ580.AndroidQQ
{
    class Tlv_
    {
        public byte[] tlv_pk(string cmd, byte[] b)
        {
            _pack pk = new _pack();
            pk.Empty();
            pk.SetHex(cmd);
            pk.SetShort(b.Length.ToString());
            pk.SetBin(b);
            return pk.GetAll();
        }

        public byte[] tlv18(byte[] user)
        {
            _pack pk = new _pack();
            pk.Empty();
            pk.SetHex("00 01");
            pk.SetHex("00 00 06 00");
            pk.SetHex("00 00 00 10");
            pk.SetHex("00 00 00 00");
            pk.SetBin(user);
            pk.SetHex("00 00");
            pk.SetHex("00 00");
            byte[] retByte= this.tlv_pk("00 18", pk.GetAll());
            Console.WriteLine("tlv18:" + Environment.NewLine + BitConverter.ToString(retByte).Replace("-", " "));
            return retByte;
        }

        public byte[] tlv1(byte[] user, byte[] tim)
        {
            _pack pk = new _pack();
            pk.Empty();
            pk.SetHex("00 01");
            _bin Xbin = new _bin();
            pk.SetBin(Xbin.GetRandomBin(4));
            // pk.SetBin(new byte[]{0,0,0,0}) //写死数据，方便测试;
            pk.SetBin(user);
            pk.SetBin(tim);
            pk.SetHex("00 00 00 00");
            pk.SetHex("00 00");
            byte[] retByte = tlv_pk("00 01", pk.GetAll());
            Console.WriteLine("tlv1:" + Environment.NewLine + BitConverter.ToString(retByte).Replace("-", " "));
            return retByte;
        }
        public byte[] tlv2(string code, byte[] VieryToken1)
        {
            _pack pk = new _pack();
            pk.Empty();
            pk.SetInt(code.Length.ToString());
            pk.SetBin(AndroidOnly.AndroidOnly.tozjj(code, "str"));
            pk.SetShort(VieryToken1.Length.ToString());
            pk.SetBin(VieryToken1);
            byte[] retByte =this.tlv_pk("00 02", pk.GetAll());
            Console.WriteLine("tlv2:" + Environment.NewLine + BitConverter.ToString(retByte).Replace("-", " "));
            return retByte;
        }
        public byte[] tlv106(byte[] user, byte[] md5pass, byte[] md52pass, byte[] _TGTKey, byte[] imei_, byte[] tim, string appId)
        {
            _pack pk = new _pack();
            pk.SetHex("00 03");
            _bin Xbin = new _bin();
            pk.SetBin(Xbin.GetRandomBin(4));
            // pk.SetBin([0,0,0,0])  //写死数据，方便测试;
            pk.SetHex("00 00 00 05");
            pk.SetHex("00 00 00 10");
            pk.SetHex("00 00 00 00");
            pk.SetHex("00 00 00 00");
            pk.SetBin(user);
            pk.SetBin(tim);
            pk.SetHex("00 00 00 00 01");
            pk.SetBin(md5pass);
            pk.SetBin(_TGTKey);
            pk.SetHex("00 00 00 00");
            pk.SetHex("01");
            pk.SetBin(imei_);
            pk.SetInt(appId);
            pk.SetHex("00 00 00 01");
            pk.SetHex("00 00");
            _hash Hash = new _hash();
            byte[] retByte =this.tlv_pk("01 06", Hash.QQTEA(pk.GetAll(), md52pass));
            Console.WriteLine("tlv106:" + Environment.NewLine + BitConverter.ToString(retByte).Replace("-", " "));
            return retByte;
        }
        public byte[] tlv116()
        {
            _pack pk = new _pack();
            pk.Empty();
            pk.SetHex("00");
            pk.SetHex("00 00 7F 7C");
            pk.SetHex("00 01 04 00");
            pk.SetHex("00");
            byte[] retByte =this.tlv_pk("01 16", pk.GetAll());
            Console.WriteLine("tlv116:" + Environment.NewLine + BitConverter.ToString(retByte).Replace("-", " "));
            return retByte;
        }
        public byte[] tlv100(string appId)
        {
            _pack pk = new _pack();
            pk.Empty();
            pk.SetHex("00 01");
            pk.SetHex("00 00 00 05");
            pk.SetHex("00 00 00 10");
            pk.SetInt(appId);
            pk.SetHex("00 00 00 00");
            pk.SetHex("00 0E 10 E0");
            byte[] retByte =this.tlv_pk("01 00", pk.GetAll());
            Console.WriteLine("tlv100:" + Environment.NewLine + BitConverter.ToString(retByte).Replace("-", " "));
            return retByte;
        }
        public byte[] tlv104(byte[] VieryToken2)
        {
            _pack pk = new _pack();
            pk.Empty();
            pk.SetBin(VieryToken2);
            byte[] retByte =this.tlv_pk("01 04", pk.GetAll());
            Console.WriteLine("tlv104:" + Environment.NewLine + BitConverter.ToString(retByte).Replace("-", " "));
            return retByte;
        }
        public byte[] tlv107()
        {
            _pack pk = new _pack();
            pk.Empty();
            pk.SetHex("00 00");
            pk.SetHex("00");
            pk.SetHex("00 00");
            pk.SetHex("01");
            byte[] retByte =this.tlv_pk("01 07", pk.GetAll());
            Console.WriteLine("tlv107:" + Environment.NewLine + BitConverter.ToString(retByte).Replace("-", " "));
            return retByte;
        }
        public byte[] tlv108(byte[] _ksid)
        {
            _ksid = new byte[] { };
            byte[] retByte =this.tlv_pk("01 08", _ksid);
            Console.WriteLine("tlv108:" + Environment.NewLine + BitConverter.ToString(retByte).Replace("-", " "));
            return retByte;
        }
        public byte[] tlv144(byte[] TGTKey, byte[] tlv109, byte[] tlv124, byte[] tlv128, byte[] tlv16e)
        {
            _pack pk = new _pack();
            pk.Empty();
            pk.SetShort(4.ToString());
            pk.SetBin(tlv109);
            pk.SetBin(tlv124);
            pk.SetBin(tlv128);
            pk.SetBin(tlv16e);
            _hash Hash = new _hash();
            byte[] retByte =this.tlv_pk("01 44", Hash.QQTEA(pk.GetAll(), TGTKey));
            Console.WriteLine("tlv114:" + Environment.NewLine + BitConverter.ToString(retByte).Replace("-", " "));
            return retByte;
        }
        public byte[] tlv109(byte[] imei_)
        {
            _pack pk = new _pack();
            byte[] retByte =this.tlv_pk("01 09", imei_);
            Console.WriteLine("tlv109:" + Environment.NewLine + BitConverter.ToString(retByte).Replace("-", " "));
            return retByte;
        }
        public byte[] tlv124(string os_type, string os_version, string _network_type, string _apn)
        {
            _pack pk = new _pack();
            pk.SetShort(os_type.Length.ToString());
            pk.SetStr(os_type);
            pk.SetShort(os_version.Length.ToString());
            pk.SetStr(os_version);
            pk.SetShort(_network_type);
            pk.SetHex("00 00");
            pk.SetHex("00 00");
            pk.SetShort(_apn.Length.ToString());
            pk.SetStr(_apn);
            byte[] retByte =this.tlv_pk("01 24", pk.GetAll());
            Console.WriteLine("tlv124:" + Environment.NewLine + BitConverter.ToString(retByte).Replace("-", " "));
            return retByte;
        }
        public byte[] tlv128(string _device, byte[] imei_)
        {
            _pack pk = new _pack();
            pk.Empty();
            pk.SetHex("00 00");
            pk.SetHex("00");
            pk.SetHex("01");
            pk.SetHex("01");
            pk.SetHex("01 00 02 00");
            pk.SetShort(_device.Length.ToString());
            pk.SetStr(_device);
            pk.SetShort(imei_.Length.ToString());
            pk.SetBin(imei_);
            pk.SetHex("00 00");
            byte[] retByte =this.tlv_pk("01 28", pk.GetAll());
            Console.WriteLine("tlv128:" + Environment.NewLine + BitConverter.ToString(retByte).Replace("-", " "));
            return retByte;
        }
        public byte[] tlv16e(string _device)
        {
            _pack pk = new _pack();
            pk.Empty();
            pk.SetBin(AndroidOnly.AndroidOnly.tozjj(_device, "str", 0));
            byte[] retByte =this.tlv_pk("01 6E", pk.GetAll());
            Console.WriteLine("tlv16e:" + Environment.NewLine + BitConverter.ToString(retByte).Replace("-", " "));
            return retByte;
        }
        public byte[] tlv142(string _apk_id)
        {
            _pack pk = new _pack();
            pk.Empty();
            byte[] t = AndroidOnly.AndroidOnly.tozjj(_apk_id, "str");
            pk.SetInt(t.Length.ToString());
            pk.SetBin(t);
            byte[] retByte =this.tlv_pk("01 42", pk.GetAll());
            Console.WriteLine("tlv142:" + Environment.NewLine + BitConverter.ToString(retByte).Replace("-", " "));
            return retByte;
        }
        public byte[] tlv154(string _sso_seq)
        {
            _pack pk = new _pack();
            pk.Empty();
            pk.SetInt(_sso_seq);
            byte[] retByte =this.tlv_pk("01 54", pk.GetAll());
            Console.WriteLine("tlv154:" + Environment.NewLine + BitConverter.ToString(retByte).Replace("-", " "));
            return retByte;
        }
        public byte[] tlv145(byte[] imei_)
        {
            _pack pk = new _pack();
            pk.Empty();
            pk.SetBin(imei_);
            byte[] retByte =this.tlv_pk("01 45", pk.GetAll());
            Console.WriteLine("tlv145:" + Environment.NewLine + BitConverter.ToString(retByte).Replace("-", " "));
            return retByte;
        }
        public byte[] tlv141(string _network_type, string _apn)
        {
            _pack pk = new _pack();
            pk.Empty();
            pk.SetHex("00 01");
            pk.SetHex("00 00");
            pk.SetShort(_network_type);
            pk.SetShort(_apn.Length.ToString());
            pk.SetStr(_apn);
            byte[] retByte =this.tlv_pk("01 41", pk.GetAll());
            Console.WriteLine("tlv141:" + Environment.NewLine + BitConverter.ToString(retByte).Replace("-", " "));
            return retByte;
        }
        public byte[] tlv8()
        {
            _pack pk = new _pack();
            pk.SetHex("00 00 ");
            pk.SetHex("00 00 08 04");
            pk.SetHex("00 00");
            byte[] retByte =this.tlv_pk("00 08", pk.GetAll());
            Console.WriteLine("tlv8:" + Environment.NewLine + BitConverter.ToString(retByte).Replace("-", " "));
            return retByte;
        }
        public byte[] tlv16b()
        {
            _pack pk = new _pack();
            pk.Empty();
            pk.SetHex("00 01");
            pk.SetHex(" 00 0B");
            pk.SetHex("67 61 6D 65 2E 71 71 2E 63 6F 6D");
            byte[] retByte =this.tlv_pk("01 6B", pk.GetAll());
            Console.WriteLine("tlv16b:" + Environment.NewLine + BitConverter.ToString(retByte).Replace("-", " "));
            return retByte;
        }
        public byte[] tlv147(string _apk_v, byte[] _apk_sig)
        {
            _pack pk = new _pack();
            pk.Empty();
            pk.SetHex("00 00 00 10");
            pk.SetShort(_apk_v.Length.ToString());
            pk.SetStr(_apk_v);
            pk.SetShort(_apk_sig.Length.ToString());
            pk.SetBin(_apk_sig);
            byte[] retByte =this.tlv_pk("01 47", pk.GetAll());
            Console.WriteLine("tlv147:" + Environment.NewLine + BitConverter.ToString(retByte).Replace("-", " "));
            return retByte;
        }
        public byte[] tlv177()
        {
            _pack pk = new _pack();
            pk.Empty();
            pk.SetHex("01");
            pk.SetHex("53 FB 17 9B");
            pk.SetHex("00 07");
            pk.SetHex("35 2E 32 2E 33 2E 30");
            byte[] retByte =this.tlv_pk("01 77", pk.GetAll());
            Console.WriteLine("tlv177:" + Environment.NewLine + BitConverter.ToString(retByte).Replace("-", " "));
            return retByte;
        }
        public byte[] tlv114_get0058(byte[] b)
        {
            _unpack unPack = new _unpack();
            unPack.SetData(b);
            unPack.GetBin(6);
            int l = unPack.GetShort();
            if (l != 88)
            {
                Console.WriteLine("error tlv114_get0058 l != 0058");
            }
            return (unPack.GetBin(l));
        }
        public byte[] tlv187()
        {
            _pack pk = new _pack();
            pk.Empty();
            pk.SetHex("F8 FF 12 23 6E 0D AF 24 97 CE 7E D6 A0 7B DD 68");
            byte[] retByte =this.tlv_pk("01 87", pk.GetAll());
            Console.WriteLine("tlv187:" + Environment.NewLine + BitConverter.ToString(retByte).Replace("-", " "));
            return retByte;
        }
        public byte[] tlv188()
        {
            _pack pk = new _pack();
            pk.Empty();
            pk.SetHex("4D BF 65 33 D9 08 C2 73 63 6D E5 CD AE 83 C0 43");
            byte[] retByte =this.tlv_pk("01 88", pk.GetAll());
            Console.WriteLine("tlv188:" + Environment.NewLine + BitConverter.ToString(retByte).Replace("-", " "));
            return retByte;
        }
        public byte[] tlv191()
        {
            _pack pk = new _pack();
            pk.Empty();
            pk.SetHex("00");
            byte[] retByte =this.tlv_pk("01 91", pk.GetAll());
            Console.WriteLine("tlv191:" + Environment.NewLine + BitConverter.ToString(retByte).Replace("-", " "));
            return retByte;
        }
    }
}
