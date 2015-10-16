using Shared;
using System.IO;

namespace VideoThumbnailer
{
    internal class VideoThumbnailerConfigReader : ConfigRepository
    {
        public VideoThumbnailerConfigReader(string filename) : base(filename)
        {

        }

        public VideoThumbnailerConfig Read()
        {
            // Nice trick: Paste XML special
            // http://stackoverflow.com/questions/3187444/convert-xml-string-to-object
            string filePath = GetConfigFileName();
            string xml = File.ReadAllText(filePath);
            var catalog1 = xml.ParseXML<VideoThumbnailerConfig>();
            return catalog1;
        }
    }
}