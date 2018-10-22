
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using Prism.Mvvm;

using XPlane10DataAdapter;

using ATCFlightSimulatorApp.Models;
using ATCFlightSimulatorApp.Services;

namespace ATCFlightSimulatorApp.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {

        private IConfig _config;
        private IATCFlightClient _client;
        IXPlaneConnect _xplane;

        private string _title = "空管-飞行一体化";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public MainWindowViewModel(IConfig config, IATCFlightClient client, IXPlaneConnect xplane)
        {
            _config = config;
            _client = client;
            _xplane = xplane;
            Title = _config.GetAppName();
        }
    }
}
