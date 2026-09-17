using System;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace PrismStrap.Services
{
    public class VersionInfo
    {
        public string Version { get; set; } = string.Empty;
        public string ClientVersionUpload { get; set; } = string.Empty;
        public string BootstrapperVersion { get; set; } = string.Empty;
    }

    public class RobloxInstaller
    {
        private static readonly HttpClient _httpClient = new HttpClient { Timeout = TimeSpan.FromSeconds(30) };

        public static async Task<VersionInfo?> FetchLatestVersionAsync()
        {
            try
            {
                var url = "https://clientsettings.roblox.com/v2/client-version/WindowsPlayer";
                var response = await _httpClient.GetStringAsync(url);
                using var doc = JsonDocument.Parse(response);
                var root = doc.RootElement;

                return new VersionInfo
                {
                    Version = root.GetProperty("version").GetString() ?? "",
                    ClientVersionUpload = root.GetProperty("clientVersionUpload").GetString() ?? "",
                    BootstrapperVersion = root.GetProperty("bootstrapperVersion").GetString() ?? ""
                };
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to fetch latest version: {ex.Message}");
                return null;
            }
        }

        public static bool IsInstalled(string robloxDir, string versionHash)
        {
            if (string.IsNullOrEmpty(robloxDir) || string.IsNullOrEmpty(versionHash))
                return false;

            var targetExe = Path.Combine(robloxDir, "Versions", versionHash, "RobloxPlayerBeta.exe");
            return File.Exists(targetExe);
        }

        public static async Task<bool> InstallVersionAsync(string robloxDir, string versionHash, IProgress<string>? progress = null)
        {
            try
            {
                var targetDir = Path.Combine(robloxDir, "Versions", versionHash);
                Directory.CreateDirectory(targetDir);

                progress?.Report($"マニフェスト取得中: {versionHash}");
                var manifestUrl = $"https://setup.rbxcdn.com/{versionHash}-rbxPkgManifest.txt";
                var manifestText = await _httpClient.GetStringAsync(manifestUrl);

                var lines = manifestText.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var line in lines)
                {
                    var pkgName = line.Trim();
                    if (string.IsNullOrEmpty(pkgName) || pkgName == "v0") continue;

                    progress?.Report($"ダウンロード中: {pkgName}");
                    var pkgUrl = $"https://setup.rbxcdn.com/{versionHash}-{pkgName}";

                    var bytes = await _httpClient.GetByteArrayAsync(pkgUrl);

                    if (pkgName.EndsWith(".zip", StringComparison.OrdinalIgnoreCase))
                    {
                        using var ms = new MemoryStream(bytes);
                        using var archive = new ZipArchive(ms, ZipArchiveMode.Read);
                        archive.ExtractToDirectory(targetDir, overwriteFiles: true);
                    }
                    else
                    {
                        var destFile = Path.Combine(targetDir, pkgName);
                        await File.WriteAllBytesAsync(destFile, bytes);
                    }
                }

                progress?.Report("インストール完了！");
                return true;
            }
            catch (Exception ex)
            {
                progress?.Report($"エラー: {ex.Message}");
                return false;
            }
        }
    }
}
