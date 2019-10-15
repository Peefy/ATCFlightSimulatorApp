
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
    }

}
