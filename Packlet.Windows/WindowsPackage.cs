using System;
using System.Net;
using System.IO;
using System.IO.Compression;
using System.Threading;
using File = System.IO.File;

using Packlet.Core;

using Newtonsoft.Json;

namespace Packlet.Windows 
{
    class WindowsPackage
    {
        public static string ProgressBar;

        private static bool _downloadComplete = false;
        private static string _currentFileDownloading;

        // Package Info Location Data Ex: chrome;86;https://example.com/chromedownload.json
        public static void InstallPackage(string query)
        {
            WebClient client = new WebClient();
            
            Screen.PrintLn(":: Updating Repositories");
            MirrorManager.DownloadMirrors();
            
            string packageInstallJson = "";
            
            foreach (string mirror in Directory.GetFiles(Config.MirrorFolderLocation))
            {
                string[] fileContent = File.ReadAllLines(mirror);

                foreach (string line in fileContent)
                {
                    string[] lineSplit = line.Split(';');
                    
                    if (line.StartsWith(query))
                    {
                        Screen.Print($":: Package {lineSplit[0]} version {lineSplit[1]} (y/n)? ");
                        ConsoleKeyInfo keypress = Console.ReadKey();
                        
                        if (keypress.KeyChar is 'y' or 'Y')
                        {
                            packageInstallJson = client.DownloadString(lineSplit[2]);
                        }
                        else
                        {
                            client.Dispose();
                            Screen.PrintLn("Ok cancelled.");
                            
                            Environment.Exit(0);
                        }
                    }
                }
            }

            PackageInfo pkgInfo = JsonConvert.DeserializeObject<PackageInfo>(packageInstallJson);

            string zipLocation = $@"{Config.CacheFolderLocation}\{pkgInfo.PackageName}.zip";
            string installLocation = $@"{Config.PackagesLocation}\{pkgInfo.PackageName}";
            string mainExe = pkgInfo.PackageMainExecutableLocation.Replace("this/", $@"{installLocation}\");
            string shortcutLocation = $@"C:\Users\succulent\AppData\Roaming\Microsoft\Windows\Start Menu\Programs\";

            _currentFileDownloading = pkgInfo.PackageName;
            
            client.DownloadProgressChanged += _progressBar;
            client.DownloadFileAsync(new Uri(pkgInfo.PackageUrl), zipLocation);
            
            while (!_downloadComplete)
            {
                if (_downloadComplete)
                {
                    break;
                }
            }
            
            Console.Write("\r     (Download complete)                                  \n");
            Thread.Sleep(500);
            
            Directory.CreateDirectory(installLocation);
            ZipFile.ExtractToDirectory(zipLocation, installLocation);
            
            if (pkgInfo.CreateStartShortcut)
            {
                Screen.PrintLn(":: Creating Startmenu Shortcut");
                Utilities.CreateShortcut(pkgInfo.PackageName, $@"{shortcutLocation}", mainExe, pkgInfo.PackageDescription);
            }

            if (pkgInfo.CreateEnvVar)
            {
                Screen.PrintLn(":: Creating Environment Variable");
                string currentValue = Environment.GetEnvironmentVariable("Path", EnvironmentVariableTarget.User);
                string newValue = currentValue + $@";{installLocation}";
                Environment.SetEnvironmentVariable("Path", newValue, EnvironmentVariableTarget.User);
            }
            
            Screen.PrintLn(":: Creating Package Lock File");
            File.WriteAllText($@"{installLocation}\pkg.lock", packageInstallJson);
        }

        public static void RemovePackage(string query)
        {
            string[] installedPackages = Directory.GetDirectories(Config.PackagesLocation);

            foreach (string package in installedPackages)
            {
                if ($@"{Config.PackagesLocation}\{query}" == package)
                {
                    Screen.Print(":: Package Found, Uninstall? (y/n) ");

                    ConsoleKeyInfo keypress = Console.ReadKey(true);
                    
                    Screen.Print("\n");
                    
                    if (keypress.KeyChar is 'y' or 'Y')
                    {
                        PackageInfo pkgInfo = JsonConvert.DeserializeObject<PackageInfo>(File.ReadAllText($@"{package}\pkg.lock"));

                        Directory.Delete($@"{package}", true);
                        
                        if (pkgInfo.CreateStartShortcut)
                        {
                            Screen.PrintLn(":: Removing Shortcut");
                            
                            string shortcutLocation = $@"C:\Users\succulent\AppData\Roaming\Microsoft\Windows\Start Menu\Programs";
                            File.Delete($@"{shortcutLocation}\{pkgInfo.PackageName}.lnk");
                        }

                        if (pkgInfo.CreateEnvVar)
                        {
                            Screen.PrintLn(":: Removing Enviroment Variables");
                            
                            string currentValue = Environment.GetEnvironmentVariable("Path", EnvironmentVariableTarget.User);
                            string newValue = currentValue.Replace($@";{Config.PackagesLocation}\{query}", string.Empty);
                            Environment.SetEnvironmentVariable("Path", newValue, EnvironmentVariableTarget.User);
                        }
                    }

                    else
                    {
                        Screen.PrintLn("Ok cancelled.");
                        
                        Environment.Exit(0);
                    }
                }
            }
        }

        public static void UpdatePackage(string query)
        {
            string[] installedPackages = Directory.GetDirectories(Config.PackagesLocation);

            foreach (string package in installedPackages)
            {
                if ($@"{Config.PackagesLocation}\{query}" == package)
                {
                    Screen.Print(":: Package Found, Update? (y/n) ");
                    
                    ConsoleKeyInfo keypress = Console.ReadKey();
                        
                    Screen.Print("\n");
                    
                    if (keypress.KeyChar is 'y' or 'Y')
                    {
                        PackageInfo pkgInfo = JsonConvert.DeserializeObject<PackageInfo>(File.ReadAllText($@"{package}\pkg.lock"));

                        foreach (string mirror in Directory.GetFiles(Config.MirrorFolderLocation))
                        {
                            string[] fileContent = File.ReadAllLines(mirror);

                            foreach (string line in fileContent)
                            {
                                string[] lineSplit = line.Split(';');

                                if (int.Parse(pkgInfo.PackageVersion.Replace(".", string.Empty)) < int.Parse(lineSplit[1].Replace(".", string.Empty)))
                                {
                                    InstallPackage(pkgInfo.PackageName);
                                }
                                else
                                {
                                    Screen.PrintLn("\n:: No Update Needed. Quitting");

                                    Environment.Exit(0);
                                }
                            }
                        }
                    }

                    else
                    {
                        Screen.PrintLn("Ok cancelled.");
                        
                        Environment.Exit(0);
                    }
                }
            }
        }

        public static void GetVersion(string query)
        {
            string[] installedPackages = Directory.GetDirectories(Config.PackagesLocation);

            foreach (string package in installedPackages)
            {
                if ($@"{Config.PackagesLocation}\{query}" == package)
                {
                    Screen.PrintLn(":: Package Found, Getting Version.");
                    PackageInfo pkgInfo = JsonConvert.DeserializeObject<PackageInfo>(File.ReadAllText($@"{package}\pkg.lock"));

                    foreach (string mirror in Directory.GetFiles(Config.MirrorFolderLocation))
                    {
                        string[] fileContent = File.ReadAllLines(mirror);

                        foreach (string line in fileContent)
                        {
                            string[] lineSplit = line.Split(';');
                            
                            Screen.PrintLn($@":: {query}'s Version Is {lineSplit[1]}");
                        }
                    }
                }

                else
                {
                    Screen.PrintLn("Ok cancelled.");
                    
                    Environment.Exit(0);
                }
            }
        }
        
        
        private static void _progressBar(object sender, DownloadProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage != 100)
            {
                if (e.ProgressPercentage == 5) { ProgressBar = " [" + Config.downloadTail + "-------------------] "; }
                if (e.ProgressPercentage == 10) { ProgressBar = " [#" + Config.downloadTail + "------------------] "; }
                if (e.ProgressPercentage == 15) { ProgressBar = " [##" + Config.downloadTail + "-----------------] "; }
                if (e.ProgressPercentage == 20) { ProgressBar = " [###" + Config.downloadTail + "----------------] "; }
                if (e.ProgressPercentage == 25) { ProgressBar = " [####" + Config.downloadTail + "---------------] "; }
                if (e.ProgressPercentage == 30) { ProgressBar = " [#####" + Config.downloadTail + "--------------] "; }
                if (e.ProgressPercentage == 35) { ProgressBar = " [######" + Config.downloadTail + "-------------] "; }
                if (e.ProgressPercentage == 40) { ProgressBar = " [#######" + Config.downloadTail + "------------] "; }
                if (e.ProgressPercentage == 45) { ProgressBar = " [########" + Config.downloadTail + "-----------] "; }
                if (e.ProgressPercentage == 50) { ProgressBar = " [#########" + Config.downloadTail + "----------] "; }
                if (e.ProgressPercentage == 55) { ProgressBar = " [##########" + Config.downloadTail + "---------] "; }
                if (e.ProgressPercentage == 60) { ProgressBar = " [###########" + Config.downloadTail + "--------] "; }
                if (e.ProgressPercentage == 65) { ProgressBar = " [############" + Config.downloadTail + "-------] "; }
                if (e.ProgressPercentage == 70) { ProgressBar = " [#############" + Config.downloadTail + "------] "; }
                if (e.ProgressPercentage == 75) { ProgressBar = " [##############" + Config.downloadTail + "-----] "; }
                if (e.ProgressPercentage == 80) { ProgressBar = " [###############" + Config.downloadTail + "----] "; }
                if (e.ProgressPercentage == 85) { ProgressBar = " [################" + Config.downloadTail + "---] "; }
                if (e.ProgressPercentage == 90) { ProgressBar = " [#################" + Config.downloadTail + "--] "; }
                if (e.ProgressPercentage == 95) { ProgressBar = " [###################-] "; }
                string fullProgressBar = "\r    " + ProgressBar + e.ProgressPercentage + "% " + _currentFileDownloading;
                Console.Write("\r" + fullProgressBar);
            }
            else
            {
                if (e.ProgressPercentage == 100)
                {
                    ProgressBar = " [####################] ";
                    _downloadComplete = true;
                    string fullProgressBar = "\r    " + ProgressBar + e.ProgressPercentage + "% " + _currentFileDownloading + "\n";
                }
            }
        }
    }
}