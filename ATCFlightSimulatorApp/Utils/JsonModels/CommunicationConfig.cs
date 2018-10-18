using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATCFlightSimulatorApp.Utils.JsonModels
{
    public class CommunicationConfig
    {
        public string HostIp { get; set; } = "192.168.0.130";

        public int HostPort { get; set; } = 12000;

    }
}
