using ATCFlightSimulatorApp.Utils.JsonModels;

namespace ATCFlightSimulatorApp.Services
{
    public class Config : IConfig
    {
        public Config()
        {
        }

        public string GetAppName()
        {
            return "空管-飞行一体化测试显示软件";
        }

        public string GetFileName()
        {
            return "AppConfig";
        }
    }
}
