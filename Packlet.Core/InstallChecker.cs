using System;
using System.IO;

namespace Packlet.Core
{
    public class InstallChecker
    {
        public void Check()
        {
            if (!Directory.Exists(Config.InstallFolder))
            {
                Directory.CreateDirectory(Config.InstallFolder);
            }

            if (!Directory.Exists(Config.CacheFolderLocation))
            {
                Directory.CreateDirectory(Config.CacheFolderLocation);
            }

            if (!Directory.Exists(Config.MirrorFolderLocation))
            {
                Directory.CreateDirectory(Config.MirrorFolderLocation);
            }

            if (!Directory.Exists(Config.PluginFolderLocation))
            {
                Directory.CreateDirectory(Config.PluginFolderLocation);
            }

            if (!File.Exists(Config.MirrorsFileLocation))
            {
                File.WriteAllText(Config.MirrorsFileLocation, "main;https://packlet.xsucculentx.repl.co/mirrors/main.mirror");
            }
        }
    }
}