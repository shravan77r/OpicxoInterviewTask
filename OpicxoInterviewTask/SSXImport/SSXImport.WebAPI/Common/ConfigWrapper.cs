using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml;

namespace SSXImport.WebAPI.Helper
{
    /// <summary>
    /// Used to read config Data from config file
    /// </summary>
    public class ConfigWrapper
    {
        private static string configFileName = "/generated.config";
        private static string settingsPath = "appSettings/add";

        private static Configuration Configuration { get; set; }

        public static string GetAppSettings(string key)
        {
            return Configuration.AppSettings[key];
        }

        static ConfigWrapper()
        {
            XmlDocument doc = new XmlDocument();
            string path = new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath;
            string rootDir = Path.GetDirectoryName(path);
            doc.Load(rootDir + configFileName);
            var root = doc.DocumentElement;
            var list = root.SelectNodes(settingsPath);
            Configuration = new Configuration();
            foreach (var setting in list)
            {
                var element = (XmlElement)setting;
                var key = element.Attributes["key"].Value;
                var value = element.Attributes["value"].Value;
                Configuration.AppSettings.Add(key, value);
            }
        }
    }

    /// <summary>
    /// Configuration used as a Property helper for ConfigWrapper
    /// </summary>
    public class Configuration
    {
        public Dictionary<string, string> AppSettings { get; }
        public Configuration()
        {
            AppSettings = new Dictionary<string, string>();
        }
    }
}
