using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightSimulator.Models
{
    /// <summary>
    /// 目标信息
    /// </summary>
    public struct SimPlanePacket
    {
        /// <summary>
        /// 目标ID
        /// </summary>
        public int ID;
        /// <summary>
        /// 飞行计划ID
        /// </summary>
        public int PlanID;
        /// <summary>
        /// 航空器ICAO地址
        /// </summary>
        public int ICAOAddress;
        /// <summary>
        /// 航空器监视设备
        /// </summary>
        public int SurvelEquip;
        /// <summary>
        /// 航空器注册号
        /// </summary>
        public string RegisterCode;
        /// <summary>
        /// 航空器呼号
        /// </summary>
        public string CallSign;
        /// <summary>
        /// 二次雷达码
        /// </summary>
        public string SSRCode;
        /// <summary>
        /// 航空器经度
        /// </summary>
        public double Longtitude;
        /// <summary>
        /// 航空器纬度
        /// </summary>
        public double Latitude;
        /// <summary>
        /// 航空器高度(单位:米)
        /// </summary>
        public double Altitude;
        /// <summary>
        /// 航空器高度类型
        /// </summary>
        public int AltitudeType;
        /// <summary>
        /// 航空器IAS km/h
        /// </summary>
        public double IAS;
        /// <summary>
        /// 航空器TAS km/h
        /// </summary>
        public double TAS;
        /// <summary>
        /// 航空器GAS km/h
        /// </summary>
        public double GAS;
        /// <summary>
        /// 航空器机头朝向
        /// </summary>
        public double Heading;
        /// <summary>
        /// 航空器航迹朝向
        /// </summary>
        public double TrackAngle;
        /// <summary>
        /// 航空器垂直状态:上升,平飞,下降.
        /// </summary>
        public int VerticalState;
        /// <summary>
        /// 上升下降率,为正值
        /// </summary>
        public double VerticalRate;
        /// <summary>
        /// 航空器横滚角
        /// </summary>
        public double RollAngle;
        /// <summary>
        /// 航空器目的高度
        /// </summary>
        public int DesAltitude;
        /// <summary>
        /// 航空器目的高度类型
        /// </summary>
        public int DesAltitudeType;
        /// <summary>
        /// 设置航空器目的朝向
        /// </summary>
        public double DesHeading;
        /// <summary>
        /// 设置航空器SPI标志
        /// </summary>
        public bool SPIFlag;
        /// <summary>
        /// 设置航空器紧急情况类型
        /// </summary>
        public int EmergencyType;
        /// <summary>
        /// 当前飞机是否具备RVSM能力
        /// </summary>
        public bool IsRVSM;
        /// <summary>
        /// 飞机状态:坠毁,阻塞,冷等待,热等待,滑行,起飞,在空中,降落.
        /// </summary>
        public int StateType;
        /// <summary>
        /// 当前QEN高度,避免将QNH/QFE高度转回QNE高度
        /// </summary>
        public double QNEAlt;
        /// <summary>
        /// 
        /// </summary>
        public string LandingRunway;
        /// <summary>
        /// 轮胎
        /// </summary>
        public byte WheelFire;
        /// <summary>
        /// 起落架状态字
        /// </summary>
        public byte GearStatus;
        /// <summary>
        /// EngineSmoke状态字
        /// </summary>
        public byte EngineSmoke;
        /// <summary>
        /// EngineFire状态字
        /// </summary>
        public byte EngineFire;
        /// <summary>
        /// 信号灯状态字
        /// </summary>
        public byte SignalStatus;
        /// <summary>
        /// 目标状态
        /// </summary>
        public int Status;
    }

}
