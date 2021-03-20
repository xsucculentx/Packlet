using IWshRuntimeLibrary;

namespace Packlet.Windows
{
    public class Utilities
    {
        public static void CreateShortcut(string shortcutName, string shortcutPath, string targetFileLocation, string Desc)
        {
            string shortcutLocation = System.IO.Path.Combine(shortcutPath, shortcutName + ".lnk");
            WshShell shell = new WshShell();
            IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutLocation);

            shortcut.Description = Desc;
            shortcut.TargetPath = targetFileLocation;
            shortcut.Save();
        }
    }
}