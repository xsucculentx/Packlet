namespace Packlet.Core
{
    public class PackageInfo
    {
        public string PackageUrl { get; set; }
        public string PackageName { get; set; }
        public string PackageVersion { get; set; }
        public string PackageDescription { get; set; }
        
        public string PackageAuthor { get; set; }
        public string PackageMaintainer { get; set; }
        
        public string PackageMainExecutableLocation { get; set; }
        
        public bool CreateEnvVar { get; set; }
        public bool CreateStartShortcut { get; set; }
    }
}