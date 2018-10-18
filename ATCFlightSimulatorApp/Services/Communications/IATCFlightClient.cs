using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATCFlightSimulatorApp.Services
{
    public interface IATCFlightClient
    {
        void SendTo(byte[] bytes);
        byte[] Recieve();
        void SendData();
        void RecieveData();
        bool Connect();
        bool DisConnect();
    }

}
