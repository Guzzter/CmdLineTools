using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CommandLine;
using Shared;
using Shared.Options;

namespace RemoveBlackListedFiles
{
    /// <summary>
    /// Removes all files matching a certain pattern. Patterns are managed in a XML file (see BlackList.xml) 
    /// Options:
    ///   -c, --config            (Default: RemoveBlackListedFiles.xml) Only show actions verbosely, do not perform actions.
    ///   -r, --recursive(Default: False) Also search in subdirectories?
    ///   -d, --directory The directory that needs to be processed.
    ///   -p, --performactions    (Default: False) Do really perform actions, use only when you're 100% sure!
    ///   --help Display this help screen.
    /// </summary>
    class Program : RootOfEvil
    {
        static void Main(string[] args)
        {
            var options = new RemoveBlackListedFilesOptions();
            if (string.IsNullOrEmpty(options.Directory))
                options.Directory = Environment.CurrentDirectory;

            if (Parser.Default.ParseArguments(args, options))
            {
                /*
                options.Directory = _downloadDir;
                options.ConfigFile = Path.Combine(_downloadDir, "RemoveBlackListedFiles.xml");
                options.Recursive = true;
                */

                DoWork(options);
            }
        }
        
        protected static void DoWork(RemoveBlackListedFilesOptions options)
        {
            //Start
            options.Directory = StartDefaultAction(options);

            //Do work
            int modifiedFiles = RemovedBlackListedFiles(options);

            //Finish
            var boolRerunWithPerformActions = FinishDefaultAction(options, modifiedFiles);
            if (boolRerunWithPerformActions) { options.PerformActions = true; DoWork(options); }
        }

        private static int RemovedBlackListedFiles(RemoveBlackListedFilesOptions options)
        {
            var blackListedFiles = new ConfigRepository(options.ConfigFile).GetBlackListedFiles();

            foreach (var file in blackListedFiles)
            {
                if (!options.PerformActions)
                {
                    Console.WriteLine("Blacklisted file in config: " + file);
                }
            }

            if (!options.PerformActions) Console.WriteLine("Search recursive = " + options.Recursive);
            return RemoveUnwantedFiles(options.Directory, options.Recursive, blackListedFiles, !options.PerformActions);
        }
    }
}
