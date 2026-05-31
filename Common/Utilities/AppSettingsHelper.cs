using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Common.Utilities
{
    public class AppSettingsHelper
    {
        public static string? GetSettingValue(string settingKey)
        {
            var config = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                                                   .AddJsonFile("appsettings.json", true)
                                                   .Build();

            return config[settingKey];
        }

        public static string? GetConnectionString(string connectionStringName)
        {
            string connectionStringKey = $"ConnectionStrings:{connectionStringName}";

            return GetSettingValue(connectionStringKey);
        }
    }
}
