using System;
using System.Collections.Generic;

using ATCSimulator.Util;

namespace ATCSimulator.Models
{
    public class ATCDataPacketBuilder
    {
        const ushort HeaderConst = 0xFDFD;
        const ushort PackageLengthConst = 221;
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
                    kindBytes = System.Text.Encoding.ASCII.GetBytes("CJ60");
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
