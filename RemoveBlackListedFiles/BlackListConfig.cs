using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace RemoveBlackListedFiles
{
    public class BlackListConfig : ConfigRepository
    {
        public BlackListConfig(string file) : base(file) { }

        /// <summary>
        /// Reads config file
        /// </summary>
        /// <returns>a list of blacklisted patterns</returns>
        public IList<string> GetBlackListedFiles()
        {
            string file = GetConfigFileName();
            XDocument xdoc = XDocument.Load(file);
            var files = xdoc.Descendants("blacklist").Descendants("filename").Select(p => p.Value).ToList();

            return files;
        }

    }
}
