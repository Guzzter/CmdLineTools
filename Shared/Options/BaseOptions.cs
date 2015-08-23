using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommandLine;
using CommandLine.Text;

namespace Shared.Options
{
    public abstract class BaseOptions
    {
        // http://commandline.codeplex.com/

        [Option('d', "directory", Required = false, HelpText = "The directory that needs to be processed.")]
        public string Directory { get; set; }

        [Option('p', "performactions", Required = false, HelpText = "Do really perform actions, use only when you're 100% sure!", DefaultValue = false)]
        public bool PerformActions { get; set; }

        [ParserState]
        public IParserState LastParserState { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this,
              (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
        }
    }
}
