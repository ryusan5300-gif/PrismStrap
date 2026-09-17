using System;
using System.IO;
using System.Linq;

namespace PrismStrap.Services
{
    public class RobloxPaths
    {
        public string LocalAppData { get; }
        public string RobloxDir { get; }
        public string VersionsDir { get; }
        public string? CurrentVersionDir { get; }
        public string? PlayerExecutable { get; }
        public string? VersionHash { get; }

        public RobloxPaths()
        {
            LocalAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            RobloxDir = Path.Combine(LocalAppData, "Roblox");
            VersionsDir = Path.Combine(RobloxDir, "Versions");

            if (Directory.Exists(VersionsDir))
            {
                var directories = Directory.GetDirectories(VersionsDir);
                // Search for the latest directory containing RobloxPlayerBeta.exe
                var matching = directories
                    .Select(d => new DirectoryInfo(d))
                    .Where(d => File.Exists(Path.Combine(d.FullName, "RobloxPlayerBeta.exe")))
                    .OrderByDescending(d => d.LastWriteTimeUtc)
                    .FirstOrDefault();

                if (matching != null)
                {
                    CurrentVersionDir = matching.FullName;
                    PlayerExecutable = Path.Combine(matching.FullName, "RobloxPlayerBeta.exe");
                    VersionHash = matching.Name;
                }
            }
        }

        public string GetAppSettingsPath()
        {
            if (CurrentVersionDir != null)
            {
                return Path.Combine(CurrentVersionDir, "ClientSettings", "ClientAppSettings.json");
            }
            return string.Empty;
        }
    }
}
