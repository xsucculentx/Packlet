using System;
using System.Runtime.InteropServices;

using Packlet.Core;
using Packlet.Windows;
using Packlet.MacOS;
using Packlet.Linux;

namespace Packlet
{
    class Program
    {
        public static double Version = 0.1;
        public static string FullVersion = "v0.1-alpha";

        private static void Main(string[] args)
        {
            try
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    new WindowsArgManager().Parse(args);
                }

                if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    new MacOSArgManager().Parse(args);
                }

                if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    new LinuxArgManager().Parse(args);
                }
            }
            catch (Exception ex)
            {
                Screen.PrintLn("Oops! something went wrong.\nPlease report this error message to Packlet's github page.\n\n" + ex);
            }
        }
    }
}
