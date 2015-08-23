using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncDirNames
{
    /// <summary>
    /// Search for directories in [sourcedir] and creates them in [targetdir] when not existing.
    /// Does a testrun first and then asks for permission
    /// 
    /// Syntax: SyncDirNames [sourcedir] [targetdir]
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            string[] test = new[] { @"c:\temp\sourcedir", @"t:\temp\targetdir" };
            test = args;
            if (test.Length != 2)
            {
                Console.WriteLine("ERR: please add SourceDir and TargetDir as cmdline params");
            }
            else
            {
                int totalDirs = SyncDirs(test[0], test[1]);
                if (totalDirs > 0)
                {
                    Console.Write("Create dirs? [y/N] ");
                    if (Console.ReadKey().KeyChar == 'y' || Console.ReadKey().KeyChar == 'Y')
                    {
                        totalDirs = SyncDirs(test[0], test[1], false);
                    }
                    Console.Write("Done. {0} dirs created.", totalDirs);
                }
            }


        }


        static int SyncDirs(string sourceDir, string destDir, bool onlyLog = true)
        {
            int totalDirs = 0;
            if (!Directory.Exists(sourceDir))
            {
                Console.WriteLine("Dir not exist: " + sourceDir);
                return totalDirs;
            }
            if (!Directory.Exists(destDir))
            {
                Console.WriteLine("Dir not exist: " + destDir);
                return totalDirs;
            }

            var destDirs = new DirectoryInfo(destDir).GetDirectories().Select(d => d.Name).ToList();
            foreach(var sourceDirName in (new DirectoryInfo(sourceDir).GetDirectories()))
            {
                if (!destDirs.Contains(sourceDirName.Name))
                {
                    totalDirs++;
                    string newDir = Path.Combine(destDir, sourceDirName.Name);
                    if (onlyLog)
                    {
                        Console.WriteLine("Create dir {0}?", newDir);
                    }else
                    {
                        Console.WriteLine("Create dir {0}", newDir);
                        Directory.CreateDirectory(newDir);
                    }
                }
            }
            return totalDirs;
        }
    }
}
