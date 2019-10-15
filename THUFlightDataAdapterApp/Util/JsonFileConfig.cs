using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Newtonsoft.Json;

using THUFlightDataAdapterApp.Util.JsonModels;

namespace THUFlightDataAdapterApp.Util
{
    public class JsonFileConfig
    {
        [JsonIgnore]
        protected static Lazy<JsonFileConfig> _lazyInstance;

        [JsonIgnore]
        protected static Lazy<JsonFileConfig> LazyInstance =>
            _lazyInstance ?? (_lazyInstance = new Lazy<JsonFileConfig>(() => ReadFromFile(),
                LazyThreadSafetyMode.PublicationOnly));

        /// <summary>
        /// 配置文件静态实例
        /// </summary>
        [JsonIgnore]
        public static JsonFileConfig Instance => LazyInstance.Value;

        /// <summary>
        /// 配置文件路径和文件名称
        /// </summary>
        [JsonIgnore]
        public static string PathAndFileName { get; set; } =
            Path.Combine(Environment.CurrentDirectory, "config.json");

       
        /// <summary>
        /// 通信 配置
        /// </summary>
        [JsonProperty("comConfig")]
        public ComConfig ComConfig { get; set; }

        /// <summary>
        /// 配置写入文件
        /// </summary>
        public void WriteToFile()
        {
            try
            {
                var str = JsonConvert.SerializeObject(this, Formatting.Indented);
                File.WriteAllText(PathAndFileName, str);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        /// <summary>
        /// 从文件读取配置
        /// </summary>
        /// <returns></returns>
        public static JsonFileConfig ReadFromFile()
        {
            try
            {
                var str = File.ReadAllText(PathAndFileName);
                var config = JsonConvert.DeserializeObject<JsonFileConfig>(str);
                return config;
            }
            catch (Exception)
            {
                var config = new JsonFileConfig();
                config.WriteToFile();
                return new JsonFileConfig();
            }
        }

        /// <summary>
        /// 无参构造函数
        /// </summary>
        public JsonFileConfig()
        {
            this.ComConfig = new ComConfig();
        
        }

        /// <summary>
        /// 返回配置文件的json字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            try
            {
                return JsonConvert.SerializeObject(this, Formatting.Indented);
            }
            catch (Exception ex)
            {
                return "toString() call error:" + ex.Message;
            }

        }

    }
}
