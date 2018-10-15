using Prism.Mvvm;

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

        public MainWindowViewModel()
        {

        }
    }
}
