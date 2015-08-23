using CommandLine;
using CommandLine.Text;

namespace Shared.Options
{
    public class GroupSimilarFilesOptions : BaseOptions
    {
        // http://commandline.codeplex.com/

        [Option('i', "min", Required = false, HelpText = "Minimum filename length condition.", DefaultValue = 10)]
        public int StartCharsMin { get; set; }

        [Option('x', "max", Required = false, HelpText = "Maximum filename length condition.", DefaultValue = 15)]
        public int StartCharsMax { get; set; }

        [Option('a', "minamountoffiles", Required = false, HelpText = "Mimimum amount of files needed for grouping in directory.", DefaultValue = 10)]
        public int MinAmountOfFilesNeededForGroupDir { get; set; }

    }
}
