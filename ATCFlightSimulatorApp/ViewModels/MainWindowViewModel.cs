using Prism.Mvvm;

using ATCFlightSimulatorApp.Services;

namespace ATCFlightSimulatorApp.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private string _title = "空管-飞行一体化";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        private IConfig _config;
        private IATCFlightClient _client;

        public MainWindowViewModel(IConfig config, IATCFlightClient client)
        {
            _config = config;
            _client = client;
            Title = _config.GetFileName();
        }
    }
}
