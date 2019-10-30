using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace THUFlightDataAdapterApp.Util.JsonModels
{
    /// <summary>
    /// 通信 配置
    /// </summary>
    public class ComConfig
    {
        /// <summary>
        /// 自己PC设备Udp的监听端口号
        /// </summary>
        [JsonProperty("selfPort")]
        public int SelfPort { get; set; } = 16000;

        /// <summary>
        /// ATC数据接收PC的IP地址
        /// </summary>
        [JsonProperty("aTCSimulatorIp")]
        public string ATCSimulatorIp { get; set; } = "192.168.0.100";

        /// <summary>
        /// ATC雷达模拟器PC的IP地址
        /// </summary>
        [JsonProperty("aTCSimulatorRadarIp")]
        public string ATCSimulatorRadarIp { get; set; } = "192.168.0.101";

        /// <summary>
        /// ATC视景模拟器PC1的IP地址
        /// </summary>
        [JsonProperty("aTCSimulatorSim1Ip")]
        public string ATCSimulatorSim1Ip { get; set; } = "192.168.0.201";

        /// <summary>
        /// ATC视景模拟器PC2的IP地址
        /// </summary>
        [JsonProperty("aTCSimulatorSim2Ip")]
        public string ATCSimulatorSim2Ip { get; set; } = "192.168.0.202";

        /// <summary>
        /// ATC视景模拟器PC3的IP地址
        /// </summary>
        [JsonProperty("aTCSimulatorSim3Ip")]
        public string ATCSimulatorSim3Ip { get; set; } = "192.168.0.203";

        /// <summary>
        /// 空管模拟器TCP服务器端口号
        /// </summary>
        [JsonProperty("aTCSimulatorPort")]
        public int ATCSimulatorPort { get; set; } = 7109;

        /// <summary>
        /// 给空管TCP服务器发送数据的间隔
        /// </summary>
        [JsonProperty("sendDataIntervalMs")]
        public int SendDataIntervalMs { get; set; } = 30;

        /// <summary>
        /// 720度平台PC的Ip地址
        /// </summary>
        [JsonProperty("ip720Platform")]
        public string Ip720Platform { get; set; } = "192.168.0.136";

        /// <summary>
        /// 第二个720度平台PC的Ip地址
        /// </summary>
        [JsonProperty("ip720Platform2")]
        public string Ip720Platform2 { get; set; } = "192.168.0.137";

        /// <summary>
        /// Wsw视景软件UdpServer(六自由度平台)PC的Ip地址
        /// </summary>
        [JsonProperty("ipWswUdpServer")]
        public string IpWswUdpServer { get; set; } = "192.168.0.131";

        /// <summary>
        /// 单兵平台PC的Ip地址
        /// </summary>
        [JsonProperty("ipGunBarrel")]
        public string IpGunBarrel { get; set; } = "192.168.0.133";

        /// <summary>
        /// 自身的ip，决定了发送哪种机型的数据
        /// </summary>
        [JsonProperty("selfIp ")]
        public string SelfIp { get; set; } = "192.168.0.131";

        /// <summary>
        /// 所有Wsw视景软件Udp的监听端口号
        /// </summary>
        [JsonProperty("wswUdpPort")]
        public int WswUdpPort { get; set; } = 14000;

        /// <summary>
        /// 是否是TCP测试软件
        /// </summary>
        [JsonProperty("isTCPTest")]
        public bool IsTCPTest { get; set; } = false;

    }
}
