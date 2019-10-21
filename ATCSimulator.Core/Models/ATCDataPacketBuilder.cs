using System;
using System.Collections.Generic;

using ATCSimulator.Util;

namespace ATCSimulator.Models
{
    public class ATCDataPacketBuilder
    {
        const ushort HeaderConst = 0xFDFD;
        const ushort PackageLengthConst = 225;
        const byte FrameTypeConst = 0x03;
        const ushort FlightSimulatorIDConst = 0x0001;
        const int ReservedByteCount = 100;

        ATCDataPacket _packet;

        public ATCDataPacketBuilder()
        {
            _packet = new ATCDataPacket
            {
                Header = HeaderConst,
                FrameType = FrameTypeConst,
                FlightSimulatorID = FlightSimulatorIDConst,
                PacketLength = PackageLengthConst
            };
        }

        public ATCDataPacket Build()
        {
            return _packet;
        }

        private uint ConvertDateTimeInt(DateTime time)
        {
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            double intResult = (time - startTime).TotalSeconds;
            return (uint)intResult;

        }

        /// <summary>
        /// 设置帧序号和UTC时间
        /// </summary>
        /// <param name="count"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public ATCDataPacketBuilder SetCountAndTime(uint count, DateTime time)
        {
            _packet.FrameCount = count;
            _packet.UtcTime = ConvertDateTimeInt(time);
            return this;
        }

        /// <summary>
        /// 设置状态
        /// </summary>
        /// <param name="engineStatus">发动机状态</param>
        /// <param name="lightStatus">灯光状态</param>
        /// <param name="landingGear">起落架状态</param>
        /// <param name="fanDoor">节风门数值</param>
        /// <returns></returns>
        public ATCDataPacketBuilder SetStatus(bool engineStatus, bool lightStatus, bool landingGear, int fanDoor)
        {
            _packet.EngineStatus = engineStatus == true ? (byte)0x0F : (byte)0x00;
            _packet.LightStatus = lightStatus == false ? (byte)0x01 : (byte)0x1E;
            _packet.LandingGear = landingGear == true ? (byte)0x01 : (byte)0x00;
            _packet.FanDoor1 = (byte)fanDoor;
            _packet.FanDoor2 = (byte)fanDoor;
            _packet.FanDoor3 = (byte)fanDoor;
            _packet.FanDoor4 = (byte)fanDoor;
            return this;
        }

        /// <summary>
        /// 设置发送角度
        /// </summary>
        /// <param name="roll">横滚角</param>
        /// <param name="pitch">俯仰角</param>
        /// <param name="yaw">偏航角</param>
        /// <returns></returns>
        public ATCDataPacketBuilder SetAngles(float roll, float pitch, float yaw)
        {
            _packet.Roll = roll;
            _packet.Pitch = pitch;
            _packet.Yaw = yaw;
            return this;
        }

        /// <summary>
        /// 设置发送角度
        /// </summary>
        /// <param name="roll">横滚角</param>
        /// <param name="pitch">俯仰角</param>
        /// <param name="yaw">偏航角</param>
        /// <returns></returns>
        public ATCDataPacketBuilder SetAngles(double roll, double pitch, double yaw)
        {
            _packet.Roll = Convert.ToSingle(roll);
            _packet.Pitch = Convert.ToSingle(pitch);
            _packet.Yaw = Convert.ToSingle(yaw);
            return this;
        }

        /// <summary>
        /// 设置发送位置
        /// </summary>
        /// <param name="lon">经度</param>
        /// <param name="lat">纬度</param>
        /// <param name="height">海拔高度</param>
        /// <returns></returns>
        public ATCDataPacketBuilder SetPositions(float lon, float lat, float height)
        {
            _packet.Longitude = lon;
            _packet.Latitude = lat;
            _packet.Altitude = height;
            return this;
        }

        /// <summary>
        /// 设置发送位置
        /// </summary>
        /// <param name="lon">经度</param>
        /// <param name="lat">纬度</param>
        /// <param name="height">海拔高度</param>
        /// <returns></returns>
        public ATCDataPacketBuilder SetPositions(double lon, double lat, double height)
        {
            _packet.Longitude = Convert.ToSingle(lon);
            _packet.Latitude = Convert.ToSingle(lat);
            _packet.Altitude = Convert.ToSingle(height);
            return this;
        }

        public ATCDataPacketBuilder SetFlightSimulatorKind(WswModelKind kind)
        {
            byte[] kindBytes = new byte[4];
            byte[] numBytes = new byte[6];
            switch (kind)
            {
                case WswModelKind.EH101:
                    kindBytes = System.Text.Encoding.ASCII.GetBytes("EH10");
                    numBytes = System.Text.Encoding.ASCII.GetBytes("EH1001");
                    break;
                case WswModelKind.CJ6:
                    kindBytes = System.Text.Encoding.ASCII.GetBytes("CJ6 ");
                    numBytes = System.Text.Encoding.ASCII.GetBytes("CJ6-01");
                    break;
                case WswModelKind.F18:
                    kindBytes = System.Text.Encoding.ASCII.GetBytes("F18H");
                    numBytes = System.Text.Encoding.ASCII.GetBytes("F18H01");
                    break;
                default: break;
            }
            _packet.FlightKind = Convert.ToUInt32((kindBytes[0] << 24) + (kindBytes[1] << 16) + (kindBytes[2] << 8) + kindBytes[3]);
            _packet.FlightRegisterNumber1 = Convert.ToUInt16((numBytes[0] << 8) + numBytes[1]);
            _packet.FlightRegisterNumber2 = Convert.ToUInt16((numBytes[2] << 8) + numBytes[3]);
            _packet.FlightRegisterNumber3 = Convert.ToUInt16((numBytes[4] << 8) + numBytes[5]);
            return this;
        }

        /// <summary>
        /// 构造通信结构体的字节(不包含保留字节和校验字节)
        /// </summary>
        /// <returns></returns>
        byte[] BuildCommandBytes() => StructHelper.StructToBytes(_packet);

        /// <summary>
        /// 构造通信结构体的全部字节
        /// </summary>
        /// <returns></returns>
        public byte[] BuildCommandTotalBytes()
        {
            var list = new List<byte>();
            var bytes = BuildCommandBytes();
            var checkushort = CalCheckByte(bytes);
            list.AddRange(bytes);
            list.AddRange(new byte[ReservedByteCount]);
            list.AddRange(BitConverter.GetBytes(checkushort));
            return list.ToArray();
        }

        private ushort CalCheckByte(byte[] bytes, int startIndex = 2)
        {
            ushort check = 0x0000;
            var n = bytes.Length;
            if (startIndex >= n)
                return check;
            for (int i = startIndex; i < n ;++i)
            {
                check += bytes[i];
            }
            return check;
        }
    }

}
