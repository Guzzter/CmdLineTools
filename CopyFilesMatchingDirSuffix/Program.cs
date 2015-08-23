using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace CopyFilesMatchingDirSuffix
{
    /// <summary>
    /// Search for files in [sourcedir] and tries to determine in which subdir of the [targetdir] it should be copied.
    /// Does a testrun first and then asks for permission
    /// 
    /// Example subdir in [targetdir]: 'e:\targetdir\series - South Park (sp.,sp-,south)'
    /// Extracted rules: Move every file that
    ///  - contains 'South Park' or 'SouthPark' or 'South-Park' or 'South_Park' or 'South.Park' 
    ///  - starts with 'sp.' or 'sp-' or 'south'
    /// Note that matches are case-insensitive and that 'series - ' is not mandatory and is not used.
    /// 
    /// Syntax: CopyFilesMatchingDirSuffix [sourcedir] [targetdir]
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
            }else
            {
                long totalMoveSize = 0;
                int movedFiles = MoveFiles(test[0], test[1], out totalMoveSize);

                if (movedFiles > 0)
                {
                    Console.WriteLine("Found {0} with total size of {1}", movedFiles, totalMoveSize.FormatBytes());
                    Console.Write("Proceed moving files? [y/N] ");
                    if (Console.ReadKey().KeyChar == 'y' || Console.ReadKey().KeyChar == 'Y')
                    {
                        Console.WriteLine("");
                        Console.WriteLine("Start moving files");
                        movedFiles = MoveFiles(test[0], test[1], out totalMoveSize, false);

                    }
                }
                Console.WriteLine("Moved {0} files with total size of {1}", movedFiles, totalMoveSize.FormatBytes());
            }

            Console.WriteLine(Environment.NewLine + "=========================");
            Console.WriteLine("Done");

        }

        static int MoveFiles(string sourceDir, string destDir, out long totalSize, bool onlyLog = true)
        {
            totalSize = 0;
            if (!Directory.Exists(sourceDir)) {
                Console.WriteLine("Dir not exist: " + sourceDir);
                return 0;
            }
            if (!Directory.Exists(destDir)) {
                Console.WriteLine("Dir not exist: " + destDir);
                return 0;
            }

            int amountOfFilesFound = 0;

            var mapping = GetDirNames(destDir);
            foreach (var file in new DirectoryInfo(sourceDir).GetFiles())
            {
                string foundKey = "";
                foreach (var mapKey in mapping.Keys)
                {
                    if (file.Name.ToLowerInvariant().Contains(mapKey.ToLowerInvariant())) {
                        foundKey = mapKey;
                        break;
                    }
                    else if(file.Name.ToLowerInvariant().StartsWith(mapKey.Replace("*", "").ToLowerInvariant()))
                    {
                        foundKey = mapKey;
                        break;
                    }
                }
               
                if (!string.IsNullOrWhiteSpace(foundKey) && mapping.Keys.Contains<string>(foundKey))
                {
                    amountOfFilesFound++;
                    string targetFile = Path.Combine(mapping[foundKey], file.Name);

                    if (onlyLog)
                    {
                        totalSize += file.Length;
                        Console.WriteLine("Match NR {0} based on term '{1}'. Copy/To:", amountOfFilesFound, foundKey);
                        Console.WriteLine(" " + file.Name);
                        Console.WriteLine(" " + targetFile);
                        Console.WriteLine();
                    }
                    else 
                    {
                        Console.WriteLine("Copy {0}: {1}", amountOfFilesFound, Path.GetFileName(targetFile));
                        if (File.Exists(targetFile))
                        {
                            long targetLength = (new FileInfo(targetFile).Length);
                            string sizeInfo = string.Format(" ({0} vs {1}).", targetLength.FormatBytes(), file.Length.FormatBytes());

                            if (targetLength > file.Length)
                            {
                                Console.WriteLine("Info: target file is bigger" + sizeInfo);
                                Console.Write("Overwrite target file? [y/N] ");
                                if (Console.ReadKey().KeyChar == 'y' || Console.ReadKey().KeyChar == 'Y')
                                {
                                    Console.WriteLine("Overwriting target file");
                                    File.Delete(targetFile);
                                    file.MoveTo(targetFile);
                                    totalSize += file.Length;
                                }
                            }
                            else
                            {
                                Console.WriteLine("Info: target file is smaller" + sizeInfo);

                                Console.Write("Remove source file? [y/N] ");
                                if (Console.ReadKey().KeyChar == 'y' || Console.ReadKey().KeyChar == 'Y')
                                {
                                    Console.WriteLine("Deleting source file");
                                    file.Delete();

                                }
                            }
                        }else
                        {
                            file.MoveTo(targetFile);
                            totalSize += file.Length;
                        }
                    }
                }
            }

            return amountOfFilesFound;
        }

        static Dictionary<string, string> GetDirNames(string destDir)
        {
            Dictionary<string, string> list = new Dictionary<string, string>();

            if (!Directory.Exists(destDir)) {
                Console.WriteLine("Dir not exist: " + destDir);
                return list;
            }

            foreach (var dirInfo in new DirectoryInfo(destDir).GetDirectories())
            {
                //Add full dirname
                string key = dirInfo.Name;
                if (!list.ContainsKey(key))
                {
                    list.Add(key, dirInfo.FullName);
                }

                //Add full suffix dirname
                if (key.Contains("-"))
                {
                    string subkey = key.Substring(key.IndexOf("-")+1).Trim();
                    string[] directoryTags = null;
                    if (subkey.Contains("(")) {
                        directoryTags = subkey.Substring(subkey.IndexOf("(") + 1).Trim(')').Split(',');
                        subkey = subkey.Substring(0, subkey.IndexOf("(")).Trim();
                    }
                    if (directoryTags != null)
                    {
                        foreach (var tag in directoryTags)
                        {
                            if (!list.ContainsKey(tag.Trim()+"*"))
                            {
                                list.Add(tag.Trim()+"*", dirInfo.FullName);
                            }
                        }
                    }

                    if (!list.ContainsKey(subkey))
                    {
                        list.Add(subkey, dirInfo.FullName);
                        if (subkey.Contains(' '))
                        {
                            //Alternative 1: with underline
                            if (!list.ContainsKey(subkey.Replace(" ", "_")))
                                list.Add(subkey.Replace(" ", "_"), dirInfo.FullName);
                            //Alternative 2: with dash
                            if (!list.ContainsKey(subkey.Replace(" ", "-")))
                                list.Add(subkey.Replace(" ", "-"), dirInfo.FullName);
                            //Alternative 3: without spaces
                            if (!list.ContainsKey(subkey.Replace(" ", "")))
                                list.Add(subkey.Replace(" ", ""), dirInfo.FullName);
                            //Alternative 4: with point
                            if (!list.ContainsKey(subkey.Replace(" ", ".")))
                                list.Add(subkey.Replace(" ", "."), dirInfo.FullName);


                        }
                    }


                }

                //Search for synonims:
                var synoList = getMetaSynonyms(dirInfo.FullName);
                foreach (var syno in synoList)
                {
                    if (!string.IsNullOrWhiteSpace(syno) && !list.ContainsKey(syno))
                    {
                        list.Add(syno, dirInfo.FullName);
                    }
                }
            }

            return list;
        }

        //todo read JSON synonyms list
        static List<string> getMetaSynonyms(string dirname)
        {
            var otherKeys = new List<string>();
            string metafile = Path.Combine(dirname, "meta.json");
            if (File.Exists(metafile))
            {
                string content = File.ReadAllText(metafile);
                try {
                    var obj = JsonConvert.DeserializeObject<MetaJson>(content);
                    return obj.synonyms;
                }catch(Exception ex)
                {
                    throw new Exception(string.Format("Could not parse {0}", metafile), ex);
                    
                }

            }
            return otherKeys;
        }
    }

    public class MetaJson
    {
        public string site { get; set; }
        public List<string> synonyms { get; set; }
    }

    public static class ByteExtensions
    {
        public static string FormatBytes(this long bytes)
        {
            string[] Suffix = { "B", "KB", "MB", "GB", "TB" };
            int i;
            double dblSByte = bytes;
            for (i = 0; i < Suffix.Length && bytes >= 1024; i++, bytes /= 1024)
            {
                dblSByte = bytes / 1024.0;
            }

            return string.Format("{0:0.##} {1}", dblSByte, Suffix[i]);
        }
    }
}
