using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace ATCSimulator.Models
{
    /// <summary>
    /// 飞行计划数据包
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ScenarioPacket
    {
        /// <summary>
        /// 飞行计划ID
        /// </summary>
        public int FlightPlanID;
        /// <summary>
        /// 飞行计划名称
        /// </summary>
        public string FPlanName;
        /// <summary>
        /// 二次码
        /// </summary>
        public string SSRCode;
        /// <summary>
        /// 所属管制扇区
        /// </summary>
        public int SectorID;
        /// <summary>
        /// 所属管制员席位
        /// </summary>
        public int ControllerID;
        /// <summary>
        /// 所属机长席位
        /// </summary>
        public int PilotID;
        /// <summary>
        /// 是否自动起飞
        /// </summary>
        public bool AutoTakeoff;
        /// <summary>
        /// 是否具备RVSM能力
        /// </summary>
        public bool IsRVSM;
        /// <summary>
        /// 飞行计划类型
        /// </summary>
        public int FlyPlanType;
        /// <summary>
        /// 航空公司ID
        /// </summary>
        public int CompanyID;
        /// <summary>
        /// 呼号(航空公司+数字编码)
        /// </summary>
        public string CallSign;
        /// <summary>
        /// 起飞机场
        /// </summary>
        public int DepDromeID;
        /// <summary>
        /// 降落机场
        /// </summary>
        public int ArrDromeID;
        /// <summary>
        /// 备降场
        /// </summary>
        public int AlternateDromeID;
        /// <summary>
        /// 燃油时间(单位：秒)
        /// </summary>
        public int FuelTime;
        /// <summary>
        /// 巡航高度(标准海压高度,单位:米)
        /// </summary>
        public int CruiseLevel;
        /// <summary>
        /// 巡航速度(表数或者马赫,表速单位:米/秒)
        /// </summary>
        public int CruiseSpeed;
        /// <summary>
        /// 降落跑道
        /// </summary>
        public int ArrRunwayID;
        /// <summary>
        /// 起飞跑道
        /// </summary>
        public int DepRunwayID;
        /// <summary>
        /// 载重
        /// </summary>
        public int Weight;
        /// <summary>
        /// 离场的停机位
        /// </summary>
        public int DepGateID;
        /// <summary>
        /// 进场的停机位
        /// </summary>
        public int ArrGateID;
        /// <summary>
        /// 脱离道ID
        /// </summary>
        public int VacateLineID;
        /// <summary>
        /// 进入道ID
        /// </summary>
        public int InRunwayLineID;
        /// <summary>
        /// 预计起飞时间(单位:秒)
        /// </summary>
        public int DepatureTime;
        /// <summary>
        /// 预计到达时间
        /// </summary>
        public int ArrivalTime;
        /// <summary>
        /// 备注信息
        /// </summary>
        public string Remark;
        /// <summary>
        /// 离场程序名称
        /// </summary>
        public string IdName;
        /// <summary>
        /// 航路点坐标
        /// </summary>
        public List<RoutePoint> RoutePoints;
    }
}
