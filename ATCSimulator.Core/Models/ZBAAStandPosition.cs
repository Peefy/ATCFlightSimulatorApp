using System;

namespace ATCSimulator.Models
{
    public class ZBAAStandPosition
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 纬度
        /// </summary>
        public float Latitude { get; set; }

        /// <summary>
        /// 经度
        /// </summary>
        public float Lontitude { get; set; }

        /// <summary>
        /// 初始朝向
        /// </summary>
        public float InitialHeading { get; set; }

        /// <summary>
        /// 初始高度
        /// </summary>
        public float InitialAltitude { get; set; }


        public static ZBAAStandPosition New(string name, float lat, float lon, float heading, float height)
        {
            return new ZBAAStandPosition()
            {
                Name = name,
                Latitude = lat,
                Lontitude = lon,
                InitialHeading = heading,
                InitialAltitude = height,
            };
        }

    }
        
}
