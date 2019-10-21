
using System;

namespace THUFlightDataAdapterApp.Util
{
    public static class MathUtil
    {
        public static double Rad2Deg(double rad)
        {
            return rad / Math.PI * 180.0;
        }

        public static double Deg2Rad(double rad)
        {
            return rad / 180.0 * Math.PI ;
        }

    }

}
