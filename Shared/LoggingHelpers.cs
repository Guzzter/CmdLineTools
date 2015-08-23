using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Shared.Options;

namespace Shared
{
    public class LoggingHelpers
    {

        protected static void LogAction(string msg, int count)
        {
            LogAction(msg, "" + count);
        }

        protected static void LogAction(string msg, string count = "")
        {
            if (!string.IsNullOrEmpty(count))
                count += " ";
            Console.WriteLine(" " + count + msg);
        }

        protected static string StartDefaultAction(BaseOptions opt)
        {
            Console.WriteLine(Environment.NewLine + "Starting..");
            if (!opt.PerformActions)
            {
                Console.WriteLine("No option -p found: Verbose test run only");
            }
            string dir = opt.Directory;
            Console.WriteLine("Directory specified: " + dir);
            if (!Directory.Exists(dir))
            {
                dir = Path.Combine(Environment.CurrentDirectory, dir);
                //Maybe a relative dir?
                if (!Directory.Exists(dir))
                {
                    throw new Exception("Directory invalid: " + opt.Directory);
                }
            }
            Console.WriteLine("");
            Console.WriteLine("Scanning " + dir);
            return dir;
        }

        protected static bool FinishDefaultAction(BaseOptions opt, int numberOfActionsPerformed = 0)
        {

            if (!opt.PerformActions && numberOfActionsPerformed > 0)
            {
                Console.WriteLine(Environment.NewLine + "This was verbose only, perform actions [y/N]?");
                var key = Console.ReadKey();
                if (key.KeyChar.ToString().ToLowerInvariant().Equals("y"))
                {
                    return true;
                }
            }
            else
            {
                Console.WriteLine("Done..");
            }
            return false;
        }

    }
}
