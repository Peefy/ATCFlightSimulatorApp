using System;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

using ATCSimulator.Models;

using THUFlightDataAdapterApp.Model;
using THUFlightDataAdapterApp.Util;
using THUFlightDataAdapterApp.Util.JsonModels;

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

        static void BuildTcpUdpNet()
        {
            lockobj = new object();
            packetBuilder = new ATCDataPacketBuilder();
            comConfig = JsonFileConfig.Instance.ComConfig;
            udpClient = new UdpClient(comConfig.SelfPort);
            tcpClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //tcpClient.Connect(comConfig.ATCSimulatorIp, comConfig.ATCSimulatorPort);
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
                            // 在此处编写处理数据的程序
                            packetBuilder.SetAngles(angleWithLocation.Roll, angleWithLocation.Pitch, angleWithLocation.Yaw);
                            packetBuilder.SetPositions(0, 0, 0);
                            packetBuilder.SetFlightSimulatorKind(WswHelper.GetFlightKindFromIp(ip));
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
                            tcpClient.Send(datas);
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

        static void Main(string[] args)
        {
            Console.WriteLine("ATC Data Adapter");      
            BuildTcpUdpNet();
            UdpTask();
            //TcpTask();
            Console.WriteLine("Press three to exit");
            Console.ReadLine();
            Console.ReadLine();
            Console.ReadLine();
        }
    }
}
