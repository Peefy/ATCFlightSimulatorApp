using System;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

using ATCSimulator.Models;

using THUFlightDataAdapterApp.Model;
using THUFlightDataAdapterApp.Util;
using THUFlightDataAdapterApp.Util.JsonModels;


namespace THUFlightDataAdapterApp
{
    class Program
    {
        /// <summary>
        /// 线程锁
        /// </summary>
        static object lockobj;
        /// <summary>
        /// UDP客户端，用于接收飞行模拟器数据
        /// </summary>
        static UdpClient udpClient;
        /// <summary>
        /// TCP客户端，用于给空管模拟器发送数据
        /// </summary>
        static TcpClient tcpClient;
        /// <summary>
        /// 读配置文件
        /// </summary>
        static ComConfig comConfig;
        /// <summary>
        /// 构造发送数据
        /// </summary>
        static ATCDataPacketBuilder packetBuilder;
        /// <summary>
        /// 默认发送间隔
        /// </summary>
        static int sendInterval = 20;
        /// <summary>
        /// 是否是TCP测试软件
        /// </summary>
        static bool isTest = false;
        /// <summary>
        /// 是否使用TCP
        /// </summary>
        static bool isUseTCP = true;
        /// <summary>
        /// 是否接收到UDP数据包
        /// </summary>
        static bool isRevcUdp = false;
        /// <summary>
        /// 是否TCP异常
        /// </summary>
        static bool isTcpError = true;
        /// <summary>
        /// 是否UDP异常
        /// </summary>
        static bool isUdpError = false;
        /// <summary>
        /// UDP接收数据包个数
        /// </summary>
        static int udpCount = 0;

        /// <summary>
        /// TCP客户端连接服务端
        /// </summary>
        static void DoTcpConnect()
        {
        retry:
            try
            {

                if (isUseTCP == true && tcpClient.Connected == false)
                {
                    Console.WriteLine("Connect server ing...");
                    tcpClient.Connect(comConfig.ATCSimulatorIp, comConfig.ATCSimulatorPort);
                    Console.WriteLine("Connect server success!");
                    isTcpError = false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Connect server failure! The reason as follows");
                Console.WriteLine(ex.ToString());
                Thread.Sleep(100);
                goto retry;
            }
        }

        static void BuildTcpUdpNet()
        {
            lockobj = new object();
            packetBuilder = new ATCDataPacketBuilder();
            comConfig = JsonFileConfig.Instance.ComConfig;
            Console.WriteLine($"Me is {WswHelper.GetFlightKindFromIp(comConfig.SelfIp)}");
            udpClient = new UdpClient(comConfig.SelfPort);
            tcpClient = new TcpClient();
            sendInterval = comConfig.SendDataIntervalMs;
            isTest = comConfig.IsTCPTest;
        }

        private static void CloseTcpUdpNet()
        {
            if (udpClient != null)
            {
                udpClient.Close();
            }
            if (tcpClient != null && isUseTCP == true)
            {
                if (tcpClient.Connected == true)
                    tcpClient.Close();
            }
        }

        static void UdpTask()
        {
            Task.Run(() =>
            {
                try
                {
                    while (true)
                    {
                        // 从WswTHUSim接收飞行模拟器姿态和经纬度坐标
                        var ipEndPoint = new IPEndPoint(IPAddress.Any, 0);
                        var recieveBytes = udpClient.Receive(ref ipEndPoint);
                        var ip = ipEndPoint.Address.ToString();
                        var length = recieveBytes.Length;
                        if (length == StructHelper.GetStructSize<AngleWithLocation>() && 
                            (ip.StartsWith(comConfig.SelfIp) == true || ip.StartsWith("127.0.0.1")))
                        {
                            // 地球坐标系坐标 x y z roll pitch yaw                 
                            var angleWithLocation = StructHelper.BytesToStruct<AngleWithLocation>(recieveBytes);
                            var lon = PositionHelper.XYZToLon(angleWithLocation.X, angleWithLocation.Y, angleWithLocation.Z);
                            var lat = PositionHelper.XYZToLat(angleWithLocation.X, angleWithLocation.Y, angleWithLocation.Z);
                            var height = PositionHelper.XYZToHeight(angleWithLocation.X, angleWithLocation.Y, angleWithLocation.Z);
                            var rolldeg = (angleWithLocation.Roll);
                            var pitchdeg = (angleWithLocation.Pitch);
                            var yawdeg = (angleWithLocation.Yaw);
                            var kind = WswHelper.GetFlightKindFromIp(ip);
                            udpCount++;
                            isRevcUdp = true;
                            lock (lockobj)
                            {
                                packetBuilder.SetAngles(rolldeg, pitchdeg, yawdeg)
                                    .SetPositions(lon, lat, height)
                                    .SetStatus(true, false, true, 80)
                                    .SetFlightSimulatorKind(WswModelKind.EH101);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("udp error as follows:");
                    Console.WriteLine(ex.ToString());
                    isUdpError = true;
                }
            });
        }

        static void TcpTask()
        {
            Task.Run(() =>
            {
                try
                {
                    uint count = 0;
                    var stand806 = ZBAAStandPositionFactory.Get("806");
                    var stand808 = ZBAAStandPositionFactory.Get("808");
                    var stand810 = ZBAAStandPositionFactory.Get("810");
                    while (true)
                    {
                        lock (lockobj)
                        {
                            if (isTest == true)
                            {                     
                                if ((count / 667) % 2 == 0)
                                {
                                    tcpClient.Client.Send(new ATCDataPacketBuilder()
                                        .SetCountAndTime(count, DateTime.UtcNow)
                                        .SetStatus(true, true, true, 40 )
                                        .SetAngles(0, 0, stand806.InitialHeading)
                                        .SetFlightSimulatorKind(WswModelKind.CJ6)
                                        .SetPositions(stand806.Lontitude, stand806.Latitude, stand806.InitialAltitude)
                                        .BuildCommandTotalBytes());
                                    Thread.Sleep(sendInterval / 3);

                                    tcpClient.Client.Send(new ATCDataPacketBuilder()
                                        .SetCountAndTime(count, DateTime.UtcNow)
                                        .SetStatus(true, true, true, 40)
                                        .SetAngles(0, 0, stand808.InitialHeading)
                                        .SetFlightSimulatorKind(WswModelKind.F18)
                                        .SetPositions(stand808.Lontitude, stand808.Latitude, stand808.InitialAltitude)                                    
                                        .BuildCommandTotalBytes());
                                    Thread.Sleep(sendInterval / 3);

                                    tcpClient.Client.Send(new ATCDataPacketBuilder()
                                        .SetCountAndTime(count, DateTime.UtcNow)
                                        .SetStatus(true, true, true, 40)
                                        .SetAngles(0, 0, stand810.InitialHeading)
                                        .SetFlightSimulatorKind(WswModelKind.EH101)
                                        .SetPositions(stand810.Lontitude, stand810.Latitude, stand810.InitialAltitude)
                                        .BuildCommandTotalBytes());
                                    Thread.Sleep(sendInterval / 3);

                                }
                                else if ((count / 667) % 2 == 1)
                                {
                                    tcpClient.Client.Send(new ATCDataPacketBuilder()
                                        .SetCountAndTime(count, DateTime.UtcNow)
                                        .SetStatus(true, true, true, 50)
                                        .SetAngles(-10, 20, stand806.InitialHeading)
                                        .SetFlightSimulatorKind(WswModelKind.CJ6)
                                        .SetPositions(stand806.Lontitude, stand806.Latitude, stand806.InitialAltitude)
                                        .BuildCommandTotalBytes());
                                    Thread.Sleep(sendInterval / 3);

                                    tcpClient.Client.Send(new ATCDataPacketBuilder()
                                        .SetCountAndTime(count, DateTime.UtcNow)
                                        .SetStatus(true, true, true, 75)
                                        .SetAngles(30, -10, stand808.InitialHeading)
                                        .SetFlightSimulatorKind(WswModelKind.F18)
                                        .SetPositions(stand808.Lontitude, stand808.Latitude, stand808.InitialAltitude)
                                        .BuildCommandTotalBytes());
                                    Thread.Sleep(sendInterval / 3);

                                    tcpClient.Client.Send(new ATCDataPacketBuilder()
                                        .SetCountAndTime(count, DateTime.UtcNow)
                                        .SetStatus(true, true, true, 100)
                                        .SetAngles(30, 40, stand810.InitialHeading)
                                        .SetFlightSimulatorKind(WswModelKind.EH101)
                                        .SetPositions(stand810.Lontitude, stand810.Latitude, stand810.InitialAltitude)
                                        .BuildCommandTotalBytes());
                                    Thread.Sleep(sendInterval / 3);
                                }
                                count++;
                            }
                            else
                            {
                                tcpClient.Client.Send(packetBuilder
                                        .SetCountAndTime(count++, DateTime.UtcNow)
                                        .BuildCommandTotalBytes());  
                            }
                        }
                        Thread.Sleep(sendInterval);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("tcp error as follows:");
                    Console.WriteLine(ex.Message.ToString());
                    isTcpError = true;
                    if (tcpClient.Connected == false)
                    {
                        tcpClient.Close();
                        tcpClient = new TcpClient();
                        DoTcpConnect();
                    }
                }
            });
        }

        #region Console_Delegate
        public delegate bool ControlCtrlDelegate(int CtrlType);
        [DllImport("kernel32.dll")]
        private static extern bool SetConsoleCtrlHandler(ControlCtrlDelegate HandlerRoutine, bool Add);
        private static readonly ControlCtrlDelegate cancelHandler = new ControlCtrlDelegate(HandlerRoutine);

        public static bool HandlerRoutine(int CtrlType)
        {
            switch (CtrlType)
            {
                case 0:
                    CloseTcpUdpNet();
                    break;
                case 2:
                    CloseTcpUdpNet();
                    break;
            }
            return false;
        }
        #endregion

        static void ShowHeaderInfo()
        {
            Console.WriteLine("THU ATC Data Adapter");
        }

        static void Main(string[] args)
        {
            ShowHeaderInfo();
            SetConsoleCtrlHandler(cancelHandler, true);
            BuildTcpUdpNet();
            UdpTask();
            DoTcpConnect();
            TcpTask();
            Console.WriteLine("Press enter to exit");        
            while (true)
            {
                var line = Console.ReadLine();
                if (line == "exit")
                    break;
                if (line == "udp")
                {
                    Console.WriteLine($"udp state is {isRevcUdp}, count is {udpCount} and error is {isUdpError}");
                }
                if (line == "tcp")
                {
                    Console.WriteLine($"tcp state is {tcpClient.Connected} and error is {isTcpError}");
                }
                if (line == "me" || line == "self")
                {
                    Console.WriteLine($"Me is {WswHelper.GetFlightKindFromIp(comConfig.SelfIp)}");
                }
                if (line == "run udp" && isUdpError == true)
                {
                    UdpTask();
                    Console.WriteLine("udp run success");
                }
                if (line == "run tcp" && isUseTCP == true && isTcpError == true)
                {
                    DoTcpConnect();
                    TcpTask();
                    Console.WriteLine("tcp run success");
                }
                if (line == "config")
                {
                    Process.Start(JsonFileConfig.PathAndFileName);
                }
                if (line == "save config")
                {
                    JsonFileConfig.Instance.WriteToFile();
                    Console.WriteLine("save file success");
                }
            }
            CloseTcpUdpNet();
        }

    }
}




