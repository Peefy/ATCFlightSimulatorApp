using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace THUFlightDataAdapterApp.Model
{
    /// <summary>
    /// Wsw姿态数据通信Packet
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct AngleWithLocation
    {
        /// <summary>
        /// X坐标(地心坐标系)
        /// </summary>
        public double X;

        /// <summary>
        /// Y坐标(地心坐标系)
        /// </summary>
        public double Y;

        /// <summary>
        /// Z坐标高度
        /// </summary>
        public double Z;

        /// <summary>
        /// 偏航角
        /// </summary>
        public double Yaw;

        /// <summary>
        /// 俯仰角
        /// </summary>
        public double Pitch;

        /// <summary>
        /// 横滚角
        /// </summary>
        public double Roll;
    }

}
