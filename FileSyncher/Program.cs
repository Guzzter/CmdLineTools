using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared;

namespace FileSyncher
{
    class Program
    {
        static void Main(string[] args)
        {
            var conf = new SynchConfigReader("FileSyncherConfig.xml").Read();

            foreach (var grp in conf.groups)
            {
                
                var sourceDirInfo = new DirectoryInfo(Path.GetDirectoryName(grp.from));
                var targetDirInfo = new DirectoryInfo(Path.GetDirectoryName(grp.to));

                var direction = EnumHelper<SyncDirection>.Parse(grp.direction ?? conf.defaults.direction);
                var operation = EnumHelper<SyncOperation>.Parse(grp.operation ?? conf.defaults.operation);
                var resolutionmode = EnumHelper<SyncResolution>.Parse(grp.resolution ?? conf.defaults.resolution);

                if (sourceDirInfo.Exists)
                {
                    // Copy/Move from source to target
                    foreach(var f in sourceDirInfo.GetFiles(grp.filepattern))
                    {
                        if (!targetDirInfo.Exists)
                            targetDirInfo.Create();

                        PerformAction(operation, f, targetDirInfo.FullName, resolutionmode);

                    }

                    // Copy/Move from target to source (when required)
                    if (direction == SyncDirection.TwoWay)
                    {
                        foreach (var f in targetDirInfo.GetFiles(grp.filepattern))
                        {
                            if (!sourceDirInfo.Exists)
                                sourceDirInfo.Create();

                            PerformAction(operation, f, sourceDirInfo.FullName, resolutionmode);
                        }
                            
                    }


                }
            }
            
        }

        static void PerformAction(SyncOperation operation, FileInfo f, string targetDir, SyncResolution resolutionmode)
        {
            string newFile = Path.Combine(targetDir, f.Name);
            Console.WriteLine(operation + " " + f.FullName + Environment.NewLine + 
                                        " => " + newFile + Environment.NewLine);

            if (operation == SyncOperation.Copy)
            {
                if (!File.Exists(newFile) || resolutionmode == SyncResolution.overwrite)
                {
                    File.Copy(f.FullName, newFile, true);
                }
            }
            else if (operation == SyncOperation.Move)
            {
                if (!File.Exists(newFile) || resolutionmode == SyncResolution.overwrite)
                {
                    File.Delete(newFile);
                    File.Move(f.FullName, newFile);
                }
            }
        }

        
    }
}
