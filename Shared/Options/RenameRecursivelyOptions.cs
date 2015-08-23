using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommandLine;

namespace Shared.Options
{
    public class RenameRecursivelyOptions : BaseOptions
    {
        // http://commandline.codeplex.com/
        // d and p are reserved

        [Option('l', "levelsdeep", Required = false, HelpText = "How many levels deep needs the recursivy need to go?", DefaultValue = 1)]
        public int LevelsDeep { get; set; }

        [Option('s', "source", Required = true, HelpText = "Old file")]
        public string SourceFile { get; set; }

        [Option('t', "target", Required = true, HelpText = "New file")]
        public string TargetFile { get; set; }

    }
}
