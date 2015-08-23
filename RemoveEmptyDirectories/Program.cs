using System;
using CommandLine;
using Shared;
using Shared.Options;

namespace RemoveEmptyDirectories
{
    /// <summary>
    /// Utility to remove empty directories. When no directory specified the current directory is used.
    /// Run RemoveEmptyDirectories -? for help. 
    /// Options:
    ///  -r, --recursive         (Default: False) Also search in subdirectories?
    ///  -d, --directory The directory that needs to be processed.
    ///  -p, --performactions    (Default: False) Do really perform actions, use only when you're 100% sure!
    ///  --help Display this help screen.
    /// 
    /// Example syntax: RemoveEmptyDirectories -recursive
    /// When -p is not specified, a testrun is done
    /// </summary>
    class Program : RootOfEvil
    {
        
        static void Main(string[] args)
        {
            var options = new RemoveEmptyDirectoriesOptions();
            if (string.IsNullOrEmpty(options.Directory))
                options.Directory = Environment.CurrentDirectory;

            if (Parser.Default.ParseArguments(args, options))
            {
                DoWork(options);
            }
        }

        protected static void DoWork(RemoveEmptyDirectoriesOptions options)
        {
            // Start
            options.Directory = StartDefaultAction(options);

            // Do work
            int modifiedFiles = CleanEmptyFolders(options.Directory, options.Recursive, !options.PerformActions);
            
            // Finish
            var boolRerunWithPerformActions = FinishDefaultAction(options, modifiedFiles);
            if (boolRerunWithPerformActions) { options.PerformActions = true; DoWork(options); }
        }
    }
}
