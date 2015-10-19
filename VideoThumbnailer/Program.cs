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


        static void Main(string[] args)
        {
            VideoThumbnailerConfig conf = new VideoThumbnailerConfigReader("VideoThumbnailerConfig.xml").Read();

            VidThumbnailer vt = new VidThumbnailer(conf);
            vt.DoWork();

            Console.WriteLine("Done..");
        }


    }

}
