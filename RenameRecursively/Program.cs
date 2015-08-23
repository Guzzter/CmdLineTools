using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CommandLine;
using Shared;
using Shared.Options;

namespace RenameRecursively
{
    class Program : RootOfEvil
    {
        static void Main(string[] args)
        {
            var options = new RenameRecursivelyOptions();
            if (string.IsNullOrEmpty(options.Directory))
                options.Directory = Environment.CurrentDirectory;

#if DEBUG
            /*
            options.Directory = @"t:\Plex Media Files\Full\";
            options.SourceFile = "poster-0.jpg";
            options.TargetFile = "poster.jpg";
            options.LevelsDeep = 1;
            DoWork(options);*/
#endif
            if (Parser.Default.ParseArguments(args, options))
            {
                DoWork(options);
            }
        }

        private static void DoWork(RenameRecursivelyOptions options)
        {
            RenameFiles(options, 0);
        }

        private static void RenameFiles(RenameRecursivelyOptions options, int currentLevel)
        {
            if (Directory.Exists(options.Directory))
            {
                var files = Directory.GetFiles(options.Directory);
                var file = files.SingleOrDefault(x => x.EndsWith(options.SourceFile));
                if (file != null)
                {
                    //foreach (var file in, options.SourceFile))
                    //{
                    if (!options.PerformActions)
                    {
                        Console.WriteLine("Potential rename file: " + file);

                    }
                    else if (options.PerformActions)
                    {
                        var destination = file.Replace(options.SourceFile, options.TargetFile);
                        if (!File.Exists(destination))
                        {
                            Console.WriteLine("Renaming file: " + file);
                            File.Move(file, destination);
                        }
                        else
                        {
                            Console.WriteLine("Could not rename file, since it exists: " + destination);
                        }
                    }
                }

                if (currentLevel < options.LevelsDeep)
                {
                    foreach (var dirs in Directory.GetDirectories(options.Directory))
                    {
                        options.Directory = dirs;
                        RenameFiles(options, currentLevel++);
                    }

                }
            }
            else
            {
                Console.WriteLine("Sorry, directory doesn't exist: " + options.Directory);
            }
        }
    }
}
