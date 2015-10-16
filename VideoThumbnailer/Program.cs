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
                ProcessDirectory(path.Value, path.resursiveSpecified, path.resursive, conf);
            }

            Console.WriteLine("Done..");
        }

        private static void ProcessDirectory(string dirpath, bool resursiveSpecified, bool resursive, VideoThumbnailerConfig _conf)
        {
            if (Directory.Exists(dirpath))
            {
                Console.WriteLine("Processing Directory: " + dirpath);

                var files = new DirectoryInfo(dirpath).GetFiles()
                           .Where(
                               f =>
                               f.Extension.ToLowerInvariant().In(wantedFileExts)).ToList();

                foreach (var videoFile in files)
                {
                    GenerateForVid(videoFile.FullName,
                        _conf.ffmpeg,
                        _conf.settings.thumbcount,
                        _conf.settings.thumbquality,
                        _conf.settings.info_json_generation);
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
                        ProcessDirectory(dir.FullName, resursiveSpecified, resursive, _conf);
                    }
                }
            }
        }

        private static void GenerateForVid(string videoFile, VideoThumbnailerConfigFfmpeg ffmpeg, byte thumbcount, byte thumbquality, bool info_json_generation)
        {
            FFmpegMediaInfo info = new FFmpegMediaInfo(videoFile, ffmpeg.ffprobe_path);

            if (info_json_generation)
                WriteVideoInfoToFile(info, videoFile);

            //Take 10 snapshots
            double length = info.Duration.TotalSeconds;
            double step = length / thumbcount;
            double pos = 1;
            Dictionary<TimeSpan, Bitmap> snapshots = new Dictionary<TimeSpan, Bitmap>();
            while (pos < length)
            {
                TimeSpan position = TimeSpan.FromSeconds(pos);
                try
                {
                    Bitmap bmp = info.GetSnapshot(position, ffmpeg.ffmpeg_path);
                    snapshots[position] = bmp;
                }
                catch (Exception ex)
                {
                    string msg = ex.Message;
                }

                pos += step;
            }

            //Write snapshots to file
            WriteSnapshotsToFile(snapshots, videoFile, thumbquality);
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

        private static void WriteSnapshotsToFile(Dictionary<TimeSpan, Bitmap> snapshots, string orignalVideoFileName, int quality)
        {


            int snapNr = 0;
            System.Drawing.Imaging.EncoderParameters parameters = new System.Drawing.Imaging.EncoderParameters(1);
            parameters.Param[0] = new System.Drawing.Imaging.EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
            ImageCodecInfo codec = GetCodecInfo("image/jpeg");

            foreach (var snap in snapshots)
            {
                if (snap.Value != null)
                {
                    var key = snap.Key.Hours + "-" + snap.Key.Minutes + "-" + snap.Key.Seconds;
                    var bmp = snap.Value;
                    var snapFile = ExtractFileNameForSave(orignalVideoFileName, ".jpg", "_" + key);
                    bmp.Save(snapFile, codec, parameters);
                    Console.WriteLine("Saved " + Path.GetFileName(snapFile));
                }
            }
            
        }

    }

}
