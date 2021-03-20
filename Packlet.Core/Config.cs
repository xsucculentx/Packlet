using System;

namespace Packlet.Core
{
    public class Config
    {
        /*
         * Packlet Install Folder Structure:
         *
         * \--\cache *for holding temp data
         *    \plugins *for holding plugins
         *    \packages *for storing installed programs
         *    \mirrors *for storing mirrors
         *    \settings.conf *for configuration of packlet
         *    \mirrors.conf *for storing a list of mirrors
         */
        
        public static string InstallFolder = @"C:\Users\" + Environment.UserName + @"\AppData\Roaming\Packlet\";
        public static string CacheFolderLocation = InstallFolder + @"cache";
        public static string PluginFolderLocation = InstallFolder + @"plugins";
        public static string PackagesLocation = InstallFolder + @"packages";
        public static string MirrorFolderLocation = InstallFolder + @"mirrors";
        public static string SettingsFileLocation = InstallFolder + @"settings.conf";
        public static string MirrorsFileLocation = InstallFolder + @"mirrors.conf";
        public static string downloadTail = ">";
    }
}