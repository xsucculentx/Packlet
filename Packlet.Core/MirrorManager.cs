using System;
using System.IO;
using System.Collections.Generic;
using System.Net;

using Newtonsoft.Json;

namespace Packlet.Core
{
    public class MirrorManager
    {
        public static void DownloadMirrors()
        {
            foreach (string mirrorLine in File.ReadAllLines(Config.MirrorsFileLocation))
            {
                string[] mirrorSplit = mirrorLine.Split(';');

                using (WebClient client = new WebClient())
                {
                    client.DownloadFile(mirrorSplit[1], $@"{Config.MirrorFolderLocation}\{mirrorSplit[0]}.mirror");
                }
            }
        }
    }
}