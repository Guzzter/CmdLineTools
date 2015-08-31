using Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSyncher
{
    class SynchConfigReader : ConfigRepository
    {
        public SynchConfigReader(string filename) : base(filename)
        {

        }

        public SyncConfig Read()
        {
            // Nice trick: Paste XML special
            // http://stackoverflow.com/questions/3187444/convert-xml-string-to-object
            string filePath = GetConfigFileName();
            string xml = File.ReadAllText(filePath);
            var catalog1 = xml.ParseXML<SyncConfig>();
            return catalog1;
        }
    }
    
}
