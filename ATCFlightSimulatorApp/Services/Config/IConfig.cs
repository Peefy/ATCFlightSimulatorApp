using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATCFlightSimulatorApp.Services
{
    public interface IConfig
    {
        string GetFileName();
        string GetAppName();
    }
}
