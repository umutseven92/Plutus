using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json.Linq;
using Plutus.Core.Interfaces;

namespace Plutus.Core.Helpers
{
    public class SettingsLoader : ISettingsLoader
    {
        public int BuyInterval { get; }

        public int SellInterval { get; }

        public bool Test { get; }

        public string RedisUrl { get; }

        public List<ExhangeConfig> ExhangeConfigurations { get; }

        public SettingsLoader(string path)
        {
            var json = File.ReadAllText(path);
            var obj = JObject.Parse(json);

            var exchanges = obj["Exchanges"];

            ExhangeConfigurations = new List<ExhangeConfig>();

            foreach (var exchange in exchanges)
            {
                var name = exchange["Name"].Value<string>();

#if DEBUG
                var apiKey = Environment.GetEnvironmentVariable($"{name}_ApiKey");
                var secretKey = Environment.GetEnvironmentVariable($"{name}_SecretKey");;
#else
                var apiKey = exchange["ApiKey"].Value<string>();
                var secretKey = exchange["SecretKey"].Value<string>();

#endif

                ExhangeConfigurations.Add(new ExhangeConfig()
                {
                    ExhangeName = name,
                    ApiKey = apiKey,
                    SecretKey = secretKey
                });
            }

            BuyInterval = GetAppConfig<int>(obj, "BuyInterval");

            SellInterval = GetAppConfig<int>(obj, "SellInterval");

            Test = GetAppConfig<bool>(obj, "Test");

            RedisUrl = GetAppConfig<string>(obj, "RedisUrl");
        }

        private static T GetAppConfig<T>(JObject config, string key)
        {
            return config["AppConfig"][key].Value<T>();
        }
    }
}