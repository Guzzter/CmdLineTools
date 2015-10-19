using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared;

namespace VideoThumbnailer
{
    class Program
    {
        private static string[] wantedFileExts = new[] { ".wmv", ".avi", ".mp4", ".mov", ".flv", ".mkv", ".m4v" };

        static void Main(string[] args)
        {
            VideoThumbnailerConfig conf = new VideoThumbnailerConfigReader("VideoThumbnailerConfig.xml").Read();

            foreach (var path in conf.paths)
            {
                ProcessDirectory(path.Value, path.resursiveSpecified, path.resursive, path.remove_old_firstSpecified, path.remove_old_first, conf);
            }

            Console.WriteLine("Done..");
        }

        private static void ProcessDirectory(string dirpath, bool resursiveSpecified, bool resursive, bool removeSpecified, bool remove, VideoThumbnailerConfig _conf)
        {
            if (Directory.Exists(dirpath))
            {
                Console.WriteLine("Processing Directory: " + dirpath);

                // default remove setting
                bool doRemoveFirst = _conf.settings.remove_old_first;
                // override for specific directory?
                if (removeSpecified)
                {
                    doRemoveFirst = remove;
                }

                var files = new DirectoryInfo(dirpath).GetFiles()
                           .Where(
                               f =>
                               f.Extension.ToLowerInvariant().In(wantedFileExts)).ToList();

                foreach (var videoFile in files)
                {
                    GenerateForVid(videoFile.FullName, _conf, doRemoveFirst);
                    //Debug.WriteLine(fileName);
                }

                // default: recurse setting
                bool doRecursive = _conf.settings.resursive;

                // override for specific directory?
                if (resursiveSpecified)
                {
                    doRecursive = resursive;
                }


                // So, do we need to process sub directories?
                if (doRecursive)
                {
                    var dirs = new DirectoryInfo(dirpath).GetDirectories();
                    foreach(var dir in dirs)
                    {
                        ProcessDirectory(dir.FullName, resursiveSpecified, resursive, removeSpecified, remove, _conf);
                    }
                }
            }
        }

        private static void GenerateForVid(string videoFile, VideoThumbnailerConfig config, bool doRemoveFirst)
        {
            FFmpegMediaInfo info = new FFmpegMediaInfo(videoFile, config.ffmpeg.ffprobe_path);

            if (config.settings.info_json_generation)
                WriteVideoInfoToFile(info, videoFile);

            //Take 10 snapshots
            double length = info.Duration.TotalSeconds;
            double step = length / config.settings.thumbcount;
            double pos = config.settings.first_thumbnail_sec; // set first thumb
            Dictionary<TimeSpan, Bitmap> snapshots = new Dictionary<TimeSpan, Bitmap>();
            while (pos < length)
            {
                TimeSpan position = TimeSpan.FromSeconds(pos);
                try
                {
                    if (pos + step > length)
                    {
                        //it is the last
                        position = TimeSpan.FromSeconds(pos-config.settings.last_thumbnail_sec);
                    }

                    Bitmap bmp = info.GetSnapshot(position, config.ffmpeg.ffmpeg_path);
                    snapshots[position] = bmp;

                }
                catch (Exception ex)
                {
                    string msg = ex.Message;
                }

                pos += step;
            }

            //Write snapshots to file
            WriteSnapshotsToFile(snapshots, videoFile, config.settings.subdir, config.settings.thumbquality, doRemoveFirst);
        }


        private static ImageCodecInfo GetCodecInfo(string mimeType)
        {
            foreach (ImageCodecInfo encoder in ImageCodecInfo.GetImageEncoders())
                if (encoder.MimeType == mimeType)
                    return encoder;
            throw new ArgumentOutOfRangeException(
                string.Format("'{0}' not supported", mimeType));
        }

        private static void WriteVideoInfoToFile(FFmpegMediaInfo info, string orignalVideoFileName)
        {
            string json = JsonConvert.SerializeObject(info, Formatting.Indented);
            var jsonInfoFile = ExtractFileNameForSave(orignalVideoFileName, ".json");
            System.IO.File.WriteAllText(jsonInfoFile, json);
        }

        private static string ExtractFileNameForSave(string file, string newExtension, string suffix = "")
        {
            string newFile = file.Replace(Path.GetExtension(file), suffix + newExtension);
            if (File.Exists(newFile))
            {
                File.Delete(newFile);
            }
            return newFile;
        }

        public const string DEFAULT_THUMB_EXT = ".jpg";

        private static void WriteSnapshotsToFile(Dictionary<TimeSpan, Bitmap> snapshots, string orignalVideoFileName, string vidPrevFolder, int quality, bool cleanFirst)
        {

            string storeFolder = Path.GetDirectoryName(orignalVideoFileName);
            if (!string.IsNullOrEmpty(vidPrevFolder))
            {
                var vpFolder = Path.Combine(storeFolder, vidPrevFolder);
                if (!Directory.Exists(vpFolder))
                {
                    Directory.CreateDirectory(vpFolder);
                }
                storeFolder = vpFolder;
            }
            var filePattern = ExtractFileNameForSave(Path.GetFileName(orignalVideoFileName), "") + "*" + DEFAULT_THUMB_EXT;

            var fld = new DirectoryInfo(storeFolder);

            // Do we need to clean first? (a.k.a. regenerate always?)
            if (cleanFirst)
            {
                var oldThumbs = fld.GetFiles(filePattern);
                foreach (var oldThumb in oldThumbs)
                {
                    File.Delete(oldThumb.FullName);
                }
            }

            if (fld.GetFiles(filePattern).Length == 0)
            {
                int snapNr = 0;
                EncoderParameters parameters = new EncoderParameters(1);
                parameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
                ImageCodecInfo codec = GetCodecInfo("image/jpeg");
                foreach (var snap in snapshots)
                {
                    if (snap.Value != null)
                    {
                        var key = "";
                        if (snap.Key.Hours < 10)
                        {
                            key += "0";
                        }
                        key += snap.Key.Hours + "-";
                        if (snap.Key.Minutes < 10)
                        {
                            key += "0";
                        }
                        key += snap.Key.Minutes + "-";
                        if (snap.Key.Seconds < 10)
                        {
                            key += "0";
                        }
                        key += snap.Key.Seconds;

                        var bmp = snap.Value;
                        var snapFile =
                            ExtractFileNameForSave(storeFolder + "\\" + Path.GetFileName(orignalVideoFileName),
                                DEFAULT_THUMB_EXT, "_" + key);
                        bmp.Save(snapFile, codec, parameters);
                        Console.WriteLine("Saved " + Path.GetFileName(snapFile));
                    }
                }
            }

        }

    }

}
