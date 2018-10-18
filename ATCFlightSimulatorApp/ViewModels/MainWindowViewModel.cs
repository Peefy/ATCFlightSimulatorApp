using Prism.Mvvm;

using ATCFlightSimulatorApp.Services;

namespace ATCFlightSimulatorApp.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {

        private IConfig _config;
        private IATCFlightClient _client;

        private string _title = "空管-飞行一体化";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public MainWindowViewModel(IConfig config, IATCFlightClient client)
        {
            _config = config;
            _client = client;
            Title = _config.GetAppName();
        }
    }
}
