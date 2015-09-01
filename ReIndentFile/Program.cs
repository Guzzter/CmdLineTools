using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace ReIndentFile
{
    /// <summary>
    /// Utility to convert a raw unformatted XML into a nice indented formatted XML file that humans can read
    /// Also works for .json files.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                checkArgs(args);

                if (Path.GetExtension(args[0]).ToLowerInvariant() == ".xml")
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(args[0]);
                    BeautifyAndSafeToFile(doc, args[1]);

                    Console.WriteLine("File saved: {0}", args[1]);
                }

                if (Path.GetExtension(args[0]).ToLowerInvariant() == ".json")
                {
                    string rawJson = File.ReadAllText(args[0]);
                    BeautifyAndSafeToFile(rawJson, args[1]);
                    Console.WriteLine("File saved: {0}", args[1]);
                }

            }
            catch (ArgumentException ae)
            {
                Console.WriteLine(ae.Message);
            }

        }

        static void checkArgs(string[] args)
        {
            if (args.Length == 2)
            {
                string source = args[0];
                string dest = args[1];
                if (!File.Exists(source))
                {
                    throw new ArgumentException("Error: Source file does not exist");
                }
                if (File.Exists(dest) && source != dest)
                {
                    throw new ArgumentException("Error: Destination file does already exist");
                }

                if (Path.GetExtension(source).ToLowerInvariant() != ".xml" && Path.GetExtension(source).ToLowerInvariant() != ".json")
                {
                    throw new ArgumentException("Error: Source file is no xml or json");
                }
                if (Path.GetExtension(dest).ToLowerInvariant() != ".xml" && Path.GetExtension(dest).ToLowerInvariant() != ".json")
                {
                    throw new ArgumentException("Error: Destination file is no xml or json");
                }
            }
            else
            {
                throw new ArgumentException("Usage: ReindentFile <sourcefile.[xml|json]> <destinationfile.[xml|json]>");
            }

        }

        private static void BeautifyAndSafeToFile(XmlDocument doc, string path)
        {
            XmlWriterSettings settings = new XmlWriterSettings
            {
                Indent = true,
                IndentChars = "  ",
                NewLineChars = "\r\n",
                NewLineHandling = NewLineHandling.Replace,
                Encoding = Encoding.UTF8
            };

            using (XmlWriter writer = XmlWriter.Create(path, settings))
            {
                doc.Save(writer);
            }
        }

        private static void BeautifyAndSafeToFile(string json, string path)
        {
            dynamic parsedJson = JsonConvert.DeserializeObject(json);
            string goodJson = JsonConvert.SerializeObject(parsedJson, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(path, goodJson);
        }
    }
}
