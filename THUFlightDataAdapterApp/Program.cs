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

                            var earthRadius = 6378137.0;
                            var pi = Math.PI;
                            var e2 = 0.00669437999013;
                            var lon = Math.Atan2(angleWithLocation.Y, angleWithLocation.X) * 180.0 / pi;
                            if (lon < 0)
                                lon = 180 + lon;
                            var lat = Math.Atan2(angleWithLocation.Z, Math.Sqrt(angleWithLocation.X * angleWithLocation.X + angleWithLocation.Y * angleWithLocation.Y) * (1 - e2 * e2)) * 180.0 / pi;
                            var height = Math.Sqrt((angleWithLocation.X * angleWithLocation.X + angleWithLocation.Y * angleWithLocation.Y + angleWithLocation.Z * angleWithLocation.Z) / ((1 - e2 * e2) * (1 - e2 * e2))) - earthRadius;

                            packetBuilder.SetAngles(Rad2Deg(angleWithLocation.Roll), Rad2Deg(angleWithLocation.Pitch), Rad2Deg(angleWithLocation.Yaw));
                            packetBuilder.SetPositions(lon, lat, height);
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




