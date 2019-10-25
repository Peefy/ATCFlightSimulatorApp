
using System;

using ATCSimulator.Models;

namespace THUFlightDataAdapterApp.Util
{
    public class WswHelper
    {
        static WswModelKind kind = WswModelKind.All;

        public static WswModelKind GetFlightKindFromIp(string ip)
        {
            if (kind != WswModelKind.All)
                return kind;
            var config = JsonFileConfig.Instance;
            if (ip.StartsWith(config.ComConfig.Ip720Platform))
            {
                kind = WswModelKind.F18;
                return kind;
            }
            else if (ip.StartsWith(config.ComConfig.Ip720Platform2))
            {
                kind = WswModelKind.CJ6;
                return kind;
            }
            else if (ip.StartsWith(config.ComConfig.IpWswUdpServer))
            {
                kind = WswModelKind.EH101;
                return kind;
            }
            return WswModelKind.All;
        }

        public const float TestDataX = -2185907.386139555368572f;
        public const float TestDataY = 4365170.829541899263859f;
        public const float TestDataZ = 4104669.64909399114549f;

        public const float TestFlightDataX = -2185240.567762811668217f;
        public const float TestFlightDataY = 4364298.105915346182883f;
        public const float TestFlightDataZ = 4105952.615580440033227f;

        public static float TestFlight2DataX => (TestDataX + TestFlightDataX) / 2.0f;
        public static float TestFlight2DataY => (TestDataY + TestFlightDataY) / 2.0f;
        public static float TestFlight2DataZ => (TestDataZ + TestFlightDataZ) / 2.0f;

        public const float Lon = 0.0f;
        public const float Lat = 0.0f;

    }

    public class PositionHelper
    {
        /// <summary>
        /// 地球坐标系坐标转经度坐标
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static double XYZToLon(double x, double y, double z)
        {
            // 1秒大概30m
            var lonoffset = -1.5 / 3600.0;
            var pi = Math.PI;
            var lon = Math.Atan2(y, x) * 180.0 / pi;
            if (lon < 0)
                lon = 180 + lon;
            return lon + lonoffset;
        }

        /// <summary>
        /// 地球坐标系坐标转纬度坐标
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public static double XYZToLat(double x, double y, double z)
        {
            var latoffset = 1.0 / 3600.0;
            var pi = Math.PI;
            var e2 = 0.00669437999013;
            var lat = Math.Atan2(z, Math.Sqrt(x * x + y * y) * (1 - e2 * e2)) * 180.0 / pi;
            return lat + latoffset;
        }

        /// <summary>
        /// 地球坐标系坐标转海拔高度
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public static double XYZToHeight(double x, double y, double z)
        {
            var heightOffset = 296.0;
            var earthRadius = 6378137.0;
            var e2 = 0.00669437999013;
            var height = Math.Sqrt((x * x + y * y + z * z) / ((1 - e2 * e2) * (1 - e2 * e2))) - earthRadius - heightOffset;
            return height;
        }
    }

}
