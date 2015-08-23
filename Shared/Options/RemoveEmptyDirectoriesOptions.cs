using CommandLine;
using CommandLine.Text;

namespace Shared.Options
{
    public class RemoveEmptyDirectoriesOptions : BaseOptions
    {
        // http://commandline.codeplex.com/
        // d and p are reserved

        [Option('r', "recursive", Required = false, HelpText = "Also search in subdirectories?", DefaultValue = false)]
        public bool Recursive { get; set; }

    }
}
