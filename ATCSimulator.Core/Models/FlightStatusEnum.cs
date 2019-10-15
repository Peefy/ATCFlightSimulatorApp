namespace ATCSimulator.Models
{
    /// <summary>
    /// 编号说明航空器状态。
    /// 十位上数字表示航空器状态。有的状态可能需要细分为子状态，则使用个位数字区分。
    /// </summary>
    public enum FlightStatusEnum
    {
        /// <summary>
        /// 无法确定
        /// </summary>
        Unconfirmed = 0,
        /// <summary>
        /// 发动机开车
        /// </summary>
        EngineDriving = 10,
        /// <summary>
        /// 在停机位停靠
        /// </summary>
        StandStop = 20,
        /// <summary>
        /// 停机位滑出
        /// </summary>
        Standout = 30,
        /// <summary>
        /// 停机位推出
        /// </summary>
        StandRelease = 40,
        /// <summary>
        /// 在滑行道（包括跑道）上滑行
        /// </summary>
        Taxiway = 50,
        /// <summary>
        /// 沿跑道滑行（不是起飞加速滑行，也不是落地后刹车滑行，正常滑行）
        /// </summary>
        Runway = 51,
        /// <summary>
        /// 穿越跑道
        /// </summary>
        CrossRunway = 52,
        /// <summary>
        /// 快速脱离道脱离滑行
        /// </summary>
        OffSliding = 53,
        /// <summary>
        /// 在滑行道上停止
        /// </summary>
        TaxiwayStop = 60,
        /// <summary>
        /// 进跑道前的停止
        /// </summary>
        BeforeRunwayStop = 61,
        /// <summary>
        /// 在穿越跑道时停止
        /// </summary>
        CrossRunwayStop = 62,
        /// <summary>
        /// 进入跑道，对准跑道
        /// </summary>
        AlignRunway = 70,
        /// <summary>
        /// 对准跑道完毕，准备起飞
        /// </summary>
        AlignRunwayFinish = 80,
        /// <summary>
        /// 起飞指令正在执行，准备起飞
        /// </summary>
        ReadyTakeOff = 81,
        /// <summary>
        /// 在跑道上起飞滑跑
        /// </summary>
        TakeOffRun = 100,
        /// <summary>
        /// 着陆接地后加速起飞
        /// </summary>
        LandingAndGo = 101,
        /// <summary>
        /// 中断起飞滑跑
        /// </summary>
        TakeOffRunStop = 110,
        /// <summary>
        /// 中断起飞滑跑后刹车滑行
        /// </summary>
        TakeOffRunBrake = 120,
        /// <summary>
        /// 离地爬升
        /// </summary>
        ClimbOff = 130,
        /// <summary>
        /// 标准离场程序
        /// </summary>
        DepartureProcedure = 140,
        /// <summary>
        /// 航线飞行，巡航
        /// </summary>
        Cruise = 150,
        /// <summary>
        /// 巡航中的爬升
        /// </summary>
        CruiseUp = 151,
        /// <summary>
        /// 巡航中的下降
        /// </summary>
        CruiseDown = 152,
        /// <summary>
        /// 标准进场程序
        /// </summary>
        EntryProcedure = 160,
        /// <summary>
        /// 最后进近阶段
        /// </summary>
        Approach = 170,
        /// <summary>
        /// 复飞
        /// </summary>
        GoAround = 180,
        /// <summary>
        /// 决断高度前的复飞
        /// </summary>
        GoAroundHeight = 181,
        /// <summary>
        /// 触地后的复飞
        /// </summary>
        GoAroundTouch = 182,
        /// <summary>
        /// 着陆
        /// </summary>
        Landing = 190,
        /// <summary>
        /// 着陆接地后刹车滑行
        /// </summary>
        LandingBrake = 200,
        /// <summary>
        /// 着陆接地后加速起飞
        /// </summary>
        LandingAcc = 210,
        /// <summary>
        /// 滑入停机位
        /// </summary>
        Standin = 220,
        /// <summary>
        /// 拖车拖行
        /// </summary>
        TrailerRun = 230,
        /// <summary>
        /// 发动机关车
        /// </summary>
        EngineOffDriving = 10,
    }
}
