using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Linq;

namespace Shared
{
    public abstract class ConfigRepository
    {
        private string _configFile;

        public ConfigRepository(string configFile)
        {
            if (File.Exists(configFile))
            {
                _configFile = configFile;
            }
            else
            {
                string localFile = DetectLocalConfigFile(configFile);
                if (localFile == null)
                {
                    throw new ArgumentException("Could not find config file " + configFile);
                }else
                {
                    _configFile = localFile;
                }
            }
        }

        private string DetectLocalConfigFile(string configFilename)
        {
            String filePath = Assembly.GetExecutingAssembly().CodeBase;
            filePath = filePath.Replace("file:///", String.Empty);
            filePath = filePath.Replace("/", "\\");

            string localFile = string.Format("{0}\\{1}", Path.GetDirectoryName(filePath), Path.GetFileName(configFilename));
            if (File.Exists(localFile))
            {
                return localFile;
            }
            return null;
        }

        protected string GetConfigFileName()
        {
            return Path.Combine("", _configFile);
        }
    }
}
