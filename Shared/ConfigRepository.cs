using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Shared
{
    public class ConfigRepository
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
                throw new ArgumentException("Could not find config file " + configFile);
            }
        }

        public IList<string> GetBlackListedFiles()
        {
            string file = GetConfigFileName();
            XDocument xdoc = XDocument.Load(file);
            var files = xdoc.Descendants("blacklist").Descendants("filename").Select(p => p.Value).ToList();

            return files;
        }

        /*
        public void Save(string recipe)
        {
            string fileName = GetRecipeFileName();
            File.WriteAllText(fileName, recipe);
        }
        */

        private string GetConfigFileName()
        {
            return Path.Combine("", _configFile);
        }
    }
}
