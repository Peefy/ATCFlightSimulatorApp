namespace ATCSimulator.Models
{
    /// <summary>
    /// Wsw模型类型
    /// </summary>
    public enum WswModelKind
    {
        /// <summary>
        /// 直升机
        /// </summary>
        EH101 = 0,
        /// <summary>
        /// 战斗机
        /// </summary>
        CJ6 = 1,
        /// <summary>
        /// 导弹
        /// </summary>
        Missile = 2,
        /// <summary>
        /// 第2个战斗机
        /// </summary>
        F18 = 3,
        /// <summary>
        /// 所有模型
        /// </summary>
        All = EH101 + CJ6 + F18 + Missile
    }

}
