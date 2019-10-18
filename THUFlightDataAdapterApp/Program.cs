using System;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

using ATCSimulator.Models;

using THUFlightDataAdapterApp.Model;
using THUFlightDataAdapterApp.Util;
using THUFlightDataAdapterApp.Util.JsonModels;
using System.Runtime.InteropServices;

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
        const int sendInterval = 30;
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
                tcpClient.Connect("183.173.81.17", comConfig.ATCSimulatorPort);
            }
        
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
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

        static double Rad2Deg(double rad)
        {
            return rad / Math.PI * 180.0;
        }

        static void UdpTask()
        {
            Task.Run(() =>
            {
                try
                {
                    while (true)
                    {
                        //从WswTHUSim接收飞行模拟器姿态和经纬度坐标
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
                    while (true)
                    {
                        lock (lockobj)
                        {
                            if (isTest == true)
                            {
                                tcpClient.Send(new ATCDataPacketBuilder()
                                    .SetAngles(10, 20, 30)
                                    .SetFlightSimulatorKind(WswModelKind.CJ6)
                                    .SetPositions(PositionHelper.XYZToLon(WswHelper.TestDataX, WswHelper.TestDataY, WswHelper.TestDataZ),
                                        PositionHelper.XYZToLat(WswHelper.TestDataX, WswHelper.TestDataY, WswHelper.TestDataZ),
                                        PositionHelper.XYZToHeight(WswHelper.TestDataX, WswHelper.TestDataY, WswHelper.TestDataZ))
                                    .BuildCommandTotalBytes());
                            }
                            else
                            {
                                tcpClient.Send(datas);
                            }
                        }
                        Thread.Sleep(sendInterval);
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
        private static ControlCtrlDelegate cancelHandler = new ControlCtrlDelegate(HandlerRoutine);

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
            Console.WriteLine("ATC Data Adapter");
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




