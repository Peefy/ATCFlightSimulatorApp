
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

    }

    public class PositionHelper
    {
        /// <summary>
        /// 地球坐标系坐标转经纬度坐标
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static double XYZToLon(double x, double y, double z)
        {
            var pi = Math.PI;
            var lon = Math.Atan2(y, x) * 180.0 / pi;
            if (lon < 0)
                lon = 180 + lon;
            return lon;
        }

        /// <summary>
        /// 地球坐标系坐标转经纬度坐标
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public static double XYZToLat(double x, double y, double z)
        {
            var pi = Math.PI;
            var e2 = 0.00669437999013;
            var lat = Math.Atan2(z, Math.Sqrt(x * x + y * y) * (1 - e2 * e2)) * 180.0 / pi;
            return lat;
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
            var earthRadius = 6378137.0;
            var e2 = 0.00669437999013;
            var height = Math.Sqrt((x * x + y * y + z * z) / ((1 - e2 * e2) * (1 - e2 * e2))) - earthRadius;
            return height;
        }
    }

}
