using CommandLine;
using CommandLine.Text;

namespace Shared.Options
{
    public class RenamePreFixOrPostFixOrStripOptions : BaseOptions
    {
        // http://commandline.codeplex.com/
        // d and p are reserved

        [Option('f', "prefix", Required = false, HelpText = "Prefix filenames with certain text.", DefaultValue = "")]
        public string PrefixString { get; set; }

        [Option('a', "append", Required = false, HelpText = "Postfix filenames with cerain text", DefaultValue = "")]
        public string AppendString { get; set; }

        [Option('s', "strip", Required = false, HelpText = "Do not append but strip from filename", DefaultValue = false)]
        public bool StripFromFilename { get; set; }
        

    }
}
