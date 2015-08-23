using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommandLine;
using CommandLine.Text;

namespace Shared.Options
{
    public class RemoveBlackListedFilesOptions : BaseOptions
    {
        [Option('c', "config", Required = false, HelpText = "Configuration file with blacklist patterns", DefaultValue = "BlackList.xml")]
        public string ConfigFile { get; set; }

        [Option('r', "recursive", Required = false, HelpText = "Also search in subdirectories?", DefaultValue = false)]
        public bool Recursive { get; set; }
    }
}
