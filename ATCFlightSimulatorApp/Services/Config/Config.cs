namespace ATCFlightSimulatorApp.Services
{
    public class Config : IConfig
    {
        public Config()
        {
        }

        public string GetFileName()
        {
            return "AppConfig";
        }

    }
}
