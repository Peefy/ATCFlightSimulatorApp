using System;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

using ATCSimulator.Models;

using THUFlightDataAdapterApp.Model;
using THUFlightDataAdapterApp.Util;
using THUFlightDataAdapterApp.Util.JsonModels;

using static THUFlightDataAdapterApp.Util.MathUtil;

namespace THUFlightDataAdapterApp
{
    class Program
    {
        static object lockobj;
        static UdpClient udpClient;
        static Socket tcpClient;
        static ComConfig comConfig;
        static ATCDataPacketBuilder packetBuilder;
        static byte[] datas = new byte[1];
        const int sendInterval = 27;
        static bool isTest = true;

        static void BuildTcpUdpNet()
        {
            lockobj = new object();
            packetBuilder = new ATCDataPacketBuilder();
            comConfig = JsonFileConfig.Instance.ComConfig;
            udpClient = new UdpClient(comConfig.SelfPort);
            tcpClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                tcpClient.Connect(comConfig.ATCSimulatorIp, comConfig.ATCSimulatorPort);
                Console.WriteLine("Connect server success!");
            }
        
            catch (Exception ex)
            {
                Console.WriteLine("Connect server failure! The reason as follows");
                Console.WriteLine(ex.Message.ToString());
            }
        }

        private static void CloseTcpUdpNet()
        {
            if (udpClient != null)
            {
                udpClient.Close();
            }
            if (tcpClient != null)
            {
                tcpClient.Disconnect(false);
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
                        var ip = ipEndPoint.ToString();
                        var length = recieveBytes.Length;
                        if (length == StructHelper.GetStructSize<AngleWithLocation>() && ip == comConfig.SelfIp)
                        {
                            // 地球坐标系坐标 x y z roll pitch yaw
                            var angleWithLocation = StructHelper.BytesToStruct<AngleWithLocation>(recieveBytes);

                            packetBuilder.SetAngles(Rad2Deg(angleWithLocation.Roll), Rad2Deg(angleWithLocation.Pitch), Rad2Deg(angleWithLocation.Yaw))
                                .SetPositions(PositionHelper.XYZToLon(angleWithLocation.X, angleWithLocation.Y, angleWithLocation.Z),
                                        PositionHelper.XYZToLat(angleWithLocation.X, angleWithLocation.Y, angleWithLocation.Z),
                                        PositionHelper.XYZToHeight(angleWithLocation.X, angleWithLocation.Y, angleWithLocation.Z))
                                .SetFlightSimulatorKind(WswHelper.GetFlightKindFromIp(ip));
                            
                            lock (lockobj)
                            {
                                datas = packetBuilder.BuildCommandTotalBytes();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
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
                                    tcpClient.Send(new ATCDataPacketBuilder()
                                        .SetCountAndTime(count, DateTime.UtcNow)
                                        .SetStatus(false, false, false, 0)
                                        .SetAngles(0, 0, stand806.InitialHeading)
                                        .SetFlightSimulatorKind(WswModelKind.CJ6)
                                        .SetPositions(stand806.Lontitude, stand806.Latitude, stand806.InitialAltitude)
                                        .BuildCommandTotalBytes());
                                    Thread.Sleep(sendInterval / 3);

                                    tcpClient.Send(new ATCDataPacketBuilder()
                                        .SetCountAndTime(count, DateTime.UtcNow)
                                        .SetStatus(false, false, false, 0)
                                        .SetAngles(0, 0, stand808.InitialHeading)
                                        .SetFlightSimulatorKind(WswModelKind.F18)
                                        .SetPositions(stand808.Lontitude, stand808.Latitude, stand808.InitialAltitude)                                    
                                        .BuildCommandTotalBytes());
                                    Thread.Sleep(sendInterval / 3);

                                    tcpClient.Send(new ATCDataPacketBuilder()
                                        .SetCountAndTime(count, DateTime.UtcNow)
                                        .SetStatus(false, false, false, 0)
                                        .SetAngles(0, 0, stand810.InitialHeading)
                                        .SetFlightSimulatorKind(WswModelKind.EH101)
                                        .SetPositions(stand810.Lontitude, stand810.Latitude, stand810.InitialAltitude)
                                        .BuildCommandTotalBytes());
                                    Thread.Sleep(sendInterval / 3);

                                }
                                else if ((count / 667) % 2 == 1)
                                {
                                    tcpClient.Send(new ATCDataPacketBuilder()
                                        .SetCountAndTime(count, DateTime.UtcNow)
                                        .SetStatus(true, true, true, 50)
                                        .SetAngles(-10, 20, stand806.InitialHeading)
                                        .SetFlightSimulatorKind(WswModelKind.CJ6)
                                        .SetPositions(stand806.Lontitude, stand806.Latitude, stand806.InitialAltitude)
                                        .BuildCommandTotalBytes());
                                    Thread.Sleep(sendInterval / 3);

                                    tcpClient.Send(new ATCDataPacketBuilder()
                                        .SetCountAndTime(count, DateTime.UtcNow)
                                        .SetStatus(true, true, true, 75)
                                        .SetAngles(30, -10, stand808.InitialHeading)
                                        .SetFlightSimulatorKind(WswModelKind.F18)
                                        .SetPositions(stand808.Lontitude, stand808.Latitude, stand808.InitialAltitude)
                                        .BuildCommandTotalBytes());
                                    Thread.Sleep(sendInterval / 3);

                                    tcpClient.Send(new ATCDataPacketBuilder()
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
                                tcpClient.Send(datas);
                            }
                        }               
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            });
        }

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

        static void Main(string[] args)
        {
            Console.WriteLine("THU ATC Data Adapter");
            SetConsoleCtrlHandler(cancelHandler, true);
            BuildTcpUdpNet();
            UdpTask();
            TcpTask();
            Console.WriteLine("Press to exit");
            Console.ReadLine();
            CloseTcpUdpNet();
        }

    }
}




