using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Shared
{
    public class RootOfEvil : LoggingHelpers
    {
        public const bool _logWorkOnly = false;
        public const string _downloadDir = @"e:\grabit\download\";
        public const string _extractDir = @"e:\grabit\extracted\";
        public const string _cleanedDir = @"e:\grabit\cleaned\";
        public const string _mp3Dir = @"e:\_nieuwe_muziekjes\";
        public static string[] _imageExtension = new[] {"jpg", "gif", "jpeg", "png", "bmp"};
        public static string[] _musicExtension = new[] { "mp3", "wma", "flac", "wav" };
        public static string[] _videoExtension = new[] {"wmv", "mp4", "avi", "mov", "mpg", "mpeg", "flv"};
        public static int _removedDirs = 0;
        public static int _movedFiles = 0;
        public static int _deletedFiles = 0;

        protected static void RenameImageAndVideoDuo(string path, string destinationPath)
        {
            if (!Directory.Exists(path))
                return;

            string[] files = Directory.GetFiles(path);
            var images = new List<string>();
            var movies = new List<string>();
            string imageFilePath = "";
            string movieFilePath = "";
            foreach (string file in files)
            {
                if (
                    _imageExtension.Contains(
                        file.Substring(file.LastIndexOf(".", StringComparison.InvariantCulture) + 1).ToLowerInvariant()))
                {
                    images.Add(file);
                    imageFilePath = file;
                }
                if (
                    _videoExtension.Contains(
                        file.Substring(file.LastIndexOf(".", StringComparison.InvariantCulture) + 1).ToLowerInvariant()))
                {
                    movies.Add(file);
                    movieFilePath = file;
                }
            }

            if (images.Count == 1 && movies.Count == 1)
            {
                string pathName = path.Substring(0, path.LastIndexOf(@"\"));
                string dirName = path.Substring(path.LastIndexOf(@"\") + 1);

                string imageExt = imageFilePath.Substring(imageFilePath.LastIndexOf("."));
                string videoExt = movieFilePath.Substring(movieFilePath.LastIndexOf("."));

                string proposedVidFile = Path.Combine(destinationPath, dirName + @"\", dirName + videoExt);
                string proposedImgFile = Path.Combine(destinationPath, dirName + @"\", dirName + imageExt);
                Console.WriteLine("Video = " + proposedVidFile);
                Console.WriteLine("Image = " + proposedImgFile);


                if (!File.Exists(proposedVidFile))
                {
                    if (!_logWorkOnly) MoveSafely(movieFilePath, proposedVidFile, true);
                }

                if (!File.Exists(proposedImgFile))
                {
                    if (!_logWorkOnly) MoveSafely(imageFilePath, proposedImgFile, true);
                }
            }
        }

        protected static void ScanForThumbNailSheetsWithinDownloaded(string path)
        {
            if (!Directory.Exists(path))
                return;
            string matchingDownloadDir = "";
            if (path.Contains(_extractDir))
            {
                matchingDownloadDir = path.Replace(_extractDir, _downloadDir);
                if (!Directory.Exists(matchingDownloadDir))
                    return;
            }
            else
            {
                return;
            }

            foreach (string file in Directory.GetFiles(matchingDownloadDir))
            {
                if (!string.IsNullOrEmpty(Path.GetExtension(file)) && 
                    _imageExtension.Contains(Path.GetExtension(file).Replace(".", "").ToLowerInvariant()))
                {
                    LogAction("Found image in download matching extracted: " + Path.GetFileName(file));
                    MoveSafely(file, Path.Combine(path, Path.GetFileName(file)), false);
                }
            }

        }

        protected static void ScanForThumbNailSheets(string path)
        {
            if (!Directory.Exists(path))
                return;

            string[] files = Directory.GetFiles(path);
            var images = new List<string>();
            var movies = new List<string>();
            string imageFilePath = "";
            string movieFilePath = "";
            foreach (string file in files)
            {
                if (
                    _imageExtension.Contains(
                        file.Substring(file.LastIndexOf(".", StringComparison.InvariantCulture) + 1).ToLowerInvariant()))
                {
                    images.Add(file);
                    imageFilePath = file;
                    //LogAction("Found image: " + file.Substring(file.LastIndexOf(@"\", StringComparison.InvariantCulture)+1));
                }
                if (
                    _videoExtension.Contains(
                        file.Substring(file.LastIndexOf(".", StringComparison.InvariantCulture) + 1).ToLowerInvariant()))
                {
                    movies.Add(file);
                    movieFilePath = file;
                    //LogAction("Found video: " + file.Substring(file.LastIndexOf(@"\", StringComparison.InvariantCulture)+1));
                }
            }

            if (images.Count == 1 && movies.Count == 1)
            {
                string imageExt = imageFilePath.Substring(imageFilePath.LastIndexOf("."));
                string videoExt = movieFilePath.Substring(movieFilePath.LastIndexOf("."));
                string proposedFile = movieFilePath.Replace(videoExt, imageExt);
                if (!File.Exists(proposedFile))
                {
                    File.Move(imageFilePath, proposedFile);
                }
            }
        }

        protected static void MoveMp3Dirs(string path, string destPath, int minAmountOfMp3s)
        {
            if (!Directory.Exists(path))
                return;

            var destDirOneLevelHigher = destPath.Substring(0, destPath.LastIndexOf(@"\", StringComparison.InvariantCulture));
            if (!Directory.Exists(destDirOneLevelHigher))
            {
                Directory.CreateDirectory(destDirOneLevelHigher);
            }

            string[] files = Directory.GetFiles(path);
            var mp3files = new List<string>();
            foreach (string file in files)
            {
                if (_musicExtension.Contains(
                        file.Substring(file.LastIndexOf(".", StringComparison.InvariantCulture) + 1).ToLowerInvariant()))
                {
                    mp3files.Add(file);
                    //LogAction("Found image: " + file.Substring(file.LastIndexOf(@"\", StringComparison.InvariantCulture)+1));
                }
            }

            //contains enough mp3 files?
            if (mp3files.Count() >= minAmountOfMp3s)
            {
                string dir = path.Substring(path.LastIndexOf(@"\", StringComparison.InvariantCulture) + 1);
                string destinationDir = destPath + dir + @"\";

                if (!Directory.Exists(destPath) && !_logWorkOnly)
                {
                    Directory.CreateDirectory(destPath);
                }

                if (!Directory.Exists(destinationDir))
                {
                    //Directory
                    LogAction("Move dir " + path + " to " + destinationDir);
                    if (!_logWorkOnly) Directory.Move(path, destinationDir);
                }
            }
        }

        protected static void MoveImageOnlyDirs(string path, string destPath, int minAmountOfImages)
        {
            if (!Directory.Exists(path))
                return;

            var destDirOneLevelHigher = destPath.Substring(0, destPath.LastIndexOf(@"\", StringComparison.InvariantCulture));
            if (!Directory.Exists(destDirOneLevelHigher))
            {
                Directory.CreateDirectory(destDirOneLevelHigher);
            }

            string[] files = Directory.GetFiles(path);
            var images = new List<string>();
            foreach (string file in files)
            {
                if (
                    _imageExtension.Contains(
                        file.Substring(file.LastIndexOf(".", StringComparison.InvariantCulture) + 1).ToLowerInvariant()))
                {
                    images.Add(file);
                    //LogAction("Found image: " + file.Substring(file.LastIndexOf(@"\", StringComparison.InvariantCulture)+1));
                }
            }

            //Only images?
            if (files.Count() == images.Count() && images.Count() >= minAmountOfImages)
            {
                string dir = path.Substring(path.LastIndexOf(@"\", StringComparison.InvariantCulture) + 1);
                string destinationDir = destPath + dir + @"\";

                if (!Directory.Exists(destPath) && !_logWorkOnly)
                {
                    Directory.CreateDirectory(destPath);
                }

                if (!Directory.Exists(destinationDir))
                {
                    //Directory
                    LogAction("Move dir " + path + " to " + destinationDir);
                    if (!_logWorkOnly) Directory.Move(path, destinationDir);
                }
            }
        }

        protected static int MoveFromSubDirToMainDir(string rootPath, string path, bool doRecursive, bool logWorkOnly,
                                                     int maximumAmountOfFiles = 5)
        {
            int movedFiles = 0;
            if (!Directory.Exists(rootPath) || !Directory.Exists(path))
                return movedFiles;

            string[] dirs = Directory.GetDirectories(path);
            if (dirs.Count() > 0)
            {
                foreach (string dir in dirs)
                {
                    movedFiles += MoveFromSubDirToMainDir(rootPath, dir, doRecursive, logWorkOnly, maximumAmountOfFiles);
                }
            }

            if (rootPath != path)
            {
                if (Directory.GetFiles(path).Count() <= maximumAmountOfFiles)
                {
                    foreach (string file in Directory.GetFiles(path))
                    {
                        movedFiles++;
                        int pos2 = file.LastIndexOf(@"\", StringComparison.InvariantCulture) + 1;
                        string targetFile = Path.Combine(rootPath, file.Substring(pos2));

                        if (logWorkOnly)
                        {
                            LogAction("want to move file " + file.Substring(pos2) + " to " + targetFile);
                        }
                        else
                        {
                            MoveSafely(file, targetFile, false);
                            LogAction("move file " + file.Substring(pos2) + " to " + targetFile);
                        }
                    }
                }
            }
            return movedFiles;
        }

        protected static void MoveSingleFile(string path, string destination, string[] extensions, bool recursive)
        {
            if (!Directory.Exists(path))
                return;

            string[] dirs = Directory.GetDirectories(path);
            if (dirs.Count() > 0 && recursive)
            {
                foreach (string dir in dirs)
                {
                    MoveSingleFile(dir, destination, extensions, true);
                }
            }

            string[] files = Directory.GetFiles(path);
            if (files.Count() == 1)
            {
                foreach (string file in files)
                {
                    foreach (string ext in extensions)
                    {
                        if (file.EndsWith(ext, StringComparison.InvariantCultureIgnoreCase))
                        {
                            _movedFiles++;
                            int pos2 = file.LastIndexOf(@"\") + 1;
                            string targetFile = Path.Combine(destination, "_" + ext + @"\", file.Substring(pos2));

                            MoveSafely(file, targetFile, true);
                        }
                    }
                }
            }
        }

        protected static void MoveSafely(string sourceFilePath, string destinationFilePath,
                                         bool createDestinationDirectory)
        {
            if (!File.Exists(sourceFilePath))
            {
                LogAction("could not find source file to move: " + sourceFilePath);
                return;
            }

            int pos = destinationFilePath.LastIndexOf(@"\") + 1;
            string destinationDir = destinationFilePath.Substring(0, pos);
            if (!Directory.Exists(destinationDir))
            {
                if (createDestinationDirectory)
                {
                    LogAction("unexisting directory and created: " + destinationDir);
                    if (!_logWorkOnly) Directory.CreateDirectory(destinationDir);
                }
                else
                {
                    LogAction("could not find destination directory: " + destinationDir);
                    return;
                }
            }

            if (File.Exists(destinationFilePath))
            {
                for (int i = 0; i < 1000; i++)
                {
                    string proposedNewFileName =
                        destinationFilePath.Insert(destinationFilePath.IndexOf(".", StringComparison.InvariantCulture),
                                                   "-" + i);
                    if (!File.Exists(proposedNewFileName))
                    {
                        LogAction("existing destination file, so new name: " + proposedNewFileName);
                        if (!_logWorkOnly) File.Move(sourceFilePath, proposedNewFileName);
                        break;
                    }
                }
            }
            else
            {
                // file doesn't exist, so just move
                if (!_logWorkOnly) File.Move(sourceFilePath, destinationFilePath);
            }
        }

        protected static void MoveFiles(string path, string destination)
        {
            if (!Directory.Exists(path))
                return;

            int pos = path.LastIndexOf(@"\") + 1;
            string directoryName = path.Substring(pos);
            string targetDir = Path.Combine(destination, directoryName);
            if (Directory.Exists(targetDir))
            {
                foreach (string file in Directory.GetFiles(path))
                {
                    if (file.EndsWith("jpg", StringComparison.InvariantCultureIgnoreCase) ||
                        file.EndsWith("jpeg", StringComparison.InvariantCultureIgnoreCase) ||
                        file.EndsWith("gif", StringComparison.InvariantCultureIgnoreCase))
                    {
                        _movedFiles++;
                        int pos2 = file.LastIndexOf(@"\") + 1;
                        string targetFile = Path.Combine(targetDir, file.Substring(pos2));

                        if (!_logWorkOnly)
                        {
                            MoveSafely(file, Path.Combine(targetDir, targetFile), false);
                        }
                        LogAction("move file " + file.Substring(pos2) + " to " + targetFile);
                    }
                }
            }
        }

        protected static int RemoveUnwantedFiles(string path, bool recursive, IList<string> blackListedFiles,
                                                 bool logWorkOnly)
        {
            if (!Directory.Exists(path))
                return 0;

            string[] files = Directory.GetFiles(path);
            string[] dirs = Directory.GetDirectories(path);
            int deletedFiles = 0;

            if (dirs.Any() && recursive)
            {
                foreach (string dir in dirs)
                {
                    deletedFiles += RemoveUnwantedFiles(dir, true, blackListedFiles, logWorkOnly);
                }
            }

            List<string> prefixPattern = blackListedFiles.Where(f => f.EndsWith("*")).ToList();
            List<string> postfixPattern = blackListedFiles.Where(f => f.StartsWith("*")).ToList();
            foreach (string file in files)
            {
                string foundFile = Path.GetFileName(file).ToLowerInvariant();

                if (blackListedFiles.Contains(foundFile))
                {
                    if (logWorkOnly)
                    {
                        deletedFiles++;
                        LogAction("blacklist del " + file);
                    }
                    else
                    {
                        if (File.Exists(file))
                        {
                            deletedFiles++;
                            File.Delete(file);
                            LogAction("delete file " + file);
                        }
                    }
                    continue;
                }

                if (prefixPattern.Count() > 0)
                {
                    foreach (string prefix in prefixPattern)
                    {
                        if (foundFile.StartsWith(prefix.Replace("*", ""), StringComparison.InvariantCulture))
                        {
                            if (logWorkOnly)
                            {
                                deletedFiles++;
                                LogAction("prefix del " + file);
                            }
                            else
                            {
                                if (File.Exists(file))
                                {
                                    deletedFiles++;
                                    File.Delete(file);
                                    LogAction("delete file " + file);
                                }
                            }
                        }
                    }
                }

                if (postfixPattern.Count() > 0)
                {
                    foreach (string postfix in postfixPattern)
                    {
                        if (foundFile.EndsWith(postfix.Replace("*", ""), StringComparison.InvariantCulture))
                        {
                            if (logWorkOnly)
                            {
                                deletedFiles++;
                                LogAction("postfix del " + file);
                            }
                            else
                            {
                                if (File.Exists(file))
                                {
                                    deletedFiles++;
                                    File.Delete(file);
                                    LogAction("delete file " + file);
                                }
                            }
                        }
                    }
                }
            }

            if (deletedFiles > 0)
                Console.WriteLine("Total deleted " + deletedFiles + " from directory " + path);

            return deletedFiles;
        }

        protected static void RemoveUnneededFiles(string path)
        {
            if (!Directory.Exists(path))
                return;

            string[] files = Directory.GetFiles(path);
            var unneededFiles = new List<string>();
            foreach (string file in files)
            {
                if (file.EndsWith(".par2", StringComparison.InvariantCultureIgnoreCase) ||
                    file.EndsWith(".sfv", StringComparison.InvariantCultureIgnoreCase) ||
                    file.EndsWith(".nfo", StringComparison.InvariantCultureIgnoreCase) ||
                    file.EndsWith(".txt", StringComparison.InvariantCultureIgnoreCase) ||
                    file.EndsWith(".nzb", StringComparison.InvariantCultureIgnoreCase) ||
                    file.EndsWith(".url", StringComparison.InvariantCultureIgnoreCase) ||
                    file.EndsWith(".db", StringComparison.InvariantCultureIgnoreCase) ||
                    file.EndsWith("sample.wmv", StringComparison.InvariantCultureIgnoreCase) ||
                    file.EndsWith("sample.mp4", StringComparison.InvariantCultureIgnoreCase) ||
                    file.EndsWith("sample.mov", StringComparison.InvariantCultureIgnoreCase) ||
                    file.EndsWith(".par", StringComparison.InvariantCultureIgnoreCase))
                {
                    unneededFiles.Add(file);
                }
            }

            //Does the directory only contain unwanted files?
            if (files.Count() == unneededFiles.Count())
            {
                foreach (string file in files)
                {
                    _deletedFiles++;
                    if (!_logWorkOnly)
                    {
                        File.Delete(file);
                    }
                    LogAction("delete file " + file);
                }
            }
            else if (_logWorkOnly)
            {
                LogAction("directory is still important " + path);
            }
        }


        protected static int CleanEmptyFolders(string path, bool doRecursive, bool logWorkOnly, bool levelOne = true)
        {
            int removedDirectories = 0;
            if (!Directory.Exists(path))
                return removedDirectories;

            string[] dirs = Directory.GetDirectories(path);
            if (dirs.Any() && !doRecursive/* && levelOne*/)
            {
                removedDirectories += Directory.GetDirectories(path).Sum(dir => CleanEmptyFolders(dir, doRecursive, logWorkOnly, false));
                //recalculate since subdir could be empty and removed
                dirs = Directory.GetDirectories(path);
            }

            string[] files = Directory.GetFiles(path);
            if (!files.Any())
            {
                if (!dirs.Any())
                {
                    removedDirectories++;

                    if (logWorkOnly)
                    {
                        LogAction("found empty dir: " + path);
                    }
                    else
                    {
                        LogAction("removed empty dir: " + path);
                        Directory.Delete(path);
                    }
                }
            }

            return removedDirectories;
        }

        protected static int FormatFolderNames(string path, bool logWorkOnly)
        {
            int formattedFolders = 0;
            if (!Directory.Exists(path))
                return formattedFolders;

            //Note key values are also search in UPPER and lower case
            var renameDirPatterns = new Dictionary<string, string>()
                {
                    {".mp4", string.Empty},
                    {"(SD mp4)", string.Empty},
                    {"-sample.wmv", string.Empty},
                    {"720p", string.Empty},
                    {".par2", string.Empty},
                    {" & ", " and "},
                    {"&", " and "},
                    {" ", " "},
                    {"(MP3)", string.Empty},
                    {"MP3", string.Empty},
                    {"MP4", string.Empty}
                };

            string[] dirs = Directory.GetDirectories(path);
            foreach (var dir in dirs)
            {
                string newDir = dir;
                foreach (var renameDirPattern in renameDirPatterns)
                {
                    newDir = newDir.Replace(renameDirPattern.Key, renameDirPattern.Value);
                    newDir = newDir.Replace(renameDirPattern.Key.ToLowerInvariant(), renameDirPattern.Value);
                    newDir = newDir.Replace(renameDirPattern.Key.ToUpperInvariant(), renameDirPattern.Value);
                }

                //Remove trailing spaces
                newDir = newDir.Trim();
                
                //Check if a newDir is proposed
                if (!newDir.Equals(dir, StringComparison.Ordinal))
                {
                    Console.WriteLine("Old dir name = " + dir + @"\");
                    Console.WriteLine("New dir name = " + newDir + @"\");
                    
                    if (Directory.Exists(newDir))
                    {
                        Console.WriteLine("Error new directory already exist (so skipped move): " + newDir);
                    }
                    else
                    {
                        formattedFolders++;
                        if (!logWorkOnly)
                        {
                            Directory.Move(dir, newDir);
                        }
                    }
                }
            }
            
            return formattedFolders;
        }

        protected static void RemoveReadOnlyFlag(string path)
        {
            if (!Directory.Exists(path))
                return;

            string[] dirs = Directory.GetDirectories(path);
            if (dirs.Count() > 0)
            {
                foreach (string dir in Directory.GetDirectories(path))
                {
                    RemoveReadOnlyFlag(dir);
                }
            }

            string[] files = Directory.GetFiles(path);
            foreach (string file in files)
            {
                File.SetAttributes(file, File.GetAttributes(file) & ~FileAttributes.ReadOnly);
            }
        }

        protected static string GetDirPath(string filePath)
        {
            return filePath.Substring(0, filePath.LastIndexOf(@"\", StringComparison.InvariantCulture));
        }
    }
}