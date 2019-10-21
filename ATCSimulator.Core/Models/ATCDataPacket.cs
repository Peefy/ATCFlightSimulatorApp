using System.Runtime.InteropServices;

namespace ATCSimulator.Models
{
    /// <summary>
    /// 空管模拟器数据包
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ATCDataPacket
    {
        /// <summary>
        /// A 帧头 1
        /// </summary>
        public ushort Header;
        /// <summary> 
        /// B 帧长度 2
        /// </summary>
        public ushort PacketLength;
        /// <summary>
        /// C 帧类型 3
        /// </summary>
        public byte FrameType;
        /// <summary>
        /// D 帧序号 4
        /// </summary>
        public uint FrameCount;
        /// <summary>
        /// E 飞行模拟器ID 5
        /// </summary>
        public ushort FlightSimulatorID;
        /// <summary>
        /// F 航空器型号 6
        /// </summary>
        public uint FlightKind;
        /// <summary>
        /// G1 航空器注册号第一部分 7
        /// 总计6个字节
        /// </summary>
        public ushort FlightRegisterNumber1;
        /// <summary>
        /// G2 航空器注册号第二部分 7
        /// 总计6个字节
        /// </summary>
        public ushort FlightRegisterNumber2;
        /// <summary>
        /// G3 航空器注册号第三部分 7
        /// 总计6个字节
        /// </summary>
        public ushort FlightRegisterNumber3;
        /// <summary>
        /// 本帧发送时刻，地面站时间。从当天0时开始的毫秒数(UTC时间） 8
        /// </summary>
        public uint UtcTime;
        /// <summary>
        /// 俯仰角 单位：度抬头为正 9
        /// </summary>
        public float Pitch;
        /// <summary>
        /// 俯仰角速度 单位：度/s，抬头为正 10
        /// </summary>
        public float PitchSpeed;
        /// <summary>
        /// 滚转角 右滚为正 11
        /// </summary>
        public float Roll;
        /// <summary>
        /// 滚转角速度 单位：度/s，右滚为正 12
        /// </summary>
        public float RollSpeed;
        /// <summary>
        /// 航向角 单位：度，北向为0度，东向90度 13
        /// </summary>
        public float Yaw;
        /// <summary>
        /// 航向角速度 单位：度/s 14
        /// </summary>
        public float YawSpeed;
        /// <summary>
        /// 攻角 单位：度 15
        /// </summary>
        public float AttackAngle;
        /// <summary>
        /// 侧滑角 单位：度 16
        /// </summary>
        public float SideSlipAngle;
        /// <summary>
        /// 表速 单位：km/h 17
        /// </summary>
        public float TableSpeed;
        /// <summary>
        /// 地速 单位：km/h 18
        /// </summary>
        public float GroundSpeed;
        /// <summary>
        /// 航迹角 单位：度 19
        /// </summary>
        public float TrackAngle;
        /// <summary>
        /// 天向速度 单位：m/s，向上为正 20
        /// </summary>
        public float SkySpeed;
        /// <summary>
        /// 真空速 单位：km/h 21
        /// </summary>
        public float VacuumSpeed;
        /// <summary>
        /// 经度 单位：度，北纬为正 22
        /// </summary>
        public float Longitude;
        /// <summary>
        /// 纬度 单位：度，东经为正 23
        /// </summary>
        public float Latitude;
        /// <summary>
        /// 海拔 单位：m，平均海平面以下为负 24
        /// </summary>
        public float Altitude;
        /// <summary>
        /// 气压 单位：m。在过渡高度下，使用修正海压高度（QNH），在过渡高度之上，使用标准气压高度（QNE） 25
        /// </summary>
        public float AirPressure;
        /// <summary>
        /// 无线电高度 单位：m，数值小于0无效 26
        /// </summary>
        public float RadioHeight;
        /// <summary>
        /// 飞行阶段 Uint8 使用编号说明航空器状态 27
        /// </summary>
        public byte FlightStatus;
        /// <summary>
        /// 襟翼位置 单位度 28
        /// </summary>
        public float FlapsAngle;
        /// <summary>
        /// 副翼位置 单位：度 29
        /// </summary>
        public float AileronAngle;
        /// <summary>
        /// 升降舵位置 单位：度 30
        /// </summary>
        public float ElevatorAngle;
        /// <summary>
        /// 方向舵位置 单位：度 31
        /// </summary>
        public float RudderAngle;
        /// <summary>
        /// 起落架位置和故障 32
        /// </summary>
        public byte LandingGear;
        /// <summary>
        /// 1号发动机节风门 0~200 0关闭，100全开，101-200超功率运行或加力运行，200加力全开  33
        /// </summary>
        public byte FanDoor1;
        /// <summary>
        /// 2号发动机节风门0~200 0关闭，100全开，101-200超功率运行或加力运行，200加力全开  34
        /// </summary>
        public byte FanDoor2;
        /// <summary>
        /// 3号发动机节风门0~200 0关闭，100全开，101-200超功率运行或加力运行，200加力全开  35
        /// </summary>
        public byte FanDoor3;
        /// <summary>
        /// 4号发动机节风门0~200 0关闭，100全开，101-200超功率运行或加力运行，200加力全开  36
        /// </summary>
        public byte FanDoor4;
        /// <summary>
        /// 发动机运行状态和故障 37
        /// </summary>
        public byte EngineStatus;
        /// <summary>
        /// 其他故障1  38
        /// </summary>
        public byte OtherStatus1;
        /// <summary>
        /// 其他故障2  39
        /// </summary>
        public byte OtherStatus2;
        /// <summary>
        /// 灯光状态   40
        /// </summary>
        public byte LightStatus;
    }

}
