using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows.Media;

namespace PrismStrap.Services
{
    public static class DeathSoundManager
    {
        private static readonly string AppDataDir = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "PrismStrap");

        private static readonly string CacheSoundsDir = Path.Combine(AppDataDir, "Sounds");

        private static MediaPlayer? _previewPlayer;

        static DeathSoundManager()
        {
            EnsureSoundFilesExtracted();
        }

        public static void EnsureSoundFilesExtracted()
        {
            try
            {
                if (!Directory.Exists(CacheSoundsDir))
                {
                    Directory.CreateDirectory(CacheSoundsDir);
                }

                ExtractResource("classic_oof.mp3");
                ExtractResource("classic_oof.ogg");
                ExtractResource("vine_boom.mp3");
                ExtractResource("vine_boom.ogg");
            }
            catch { }
        }

        private static void ExtractResource(string filename)
        {
            var targetPath = Path.Combine(CacheSoundsDir, filename);
            if (File.Exists(targetPath) && new FileInfo(targetPath).Length > 0)
                return;

            // Try assembly resource
            var asm = Assembly.GetExecutingAssembly();
            var resourceName = $"PrismStrap.Sounds.{filename}";

            using var stream = asm.GetManifestResourceStream(resourceName);
            if (stream != null)
            {
                using var fs = File.Create(targetPath);
                stream.CopyTo(fs);
                return;
            }

            // Fallback: check development directory
            var localDevPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Sounds", filename);
            if (File.Exists(localDevPath))
            {
                File.Copy(localDevPath, targetPath, true);
                return;
            }

            var scratchPath = Path.Combine(@"C:\Users\ryuku\.gemini\antigravity\scratch\PrismStrap\Sounds", filename);
            if (File.Exists(scratchPath))
            {
                File.Copy(scratchPath, targetPath, true);
            }
        }

        public static string GetBaseSoundPath(string soundType, string customPath, bool forRoblox)
        {
            EnsureSoundFilesExtracted();

            if (soundType == "ClassicOof")
            {
                return Path.Combine(CacheSoundsDir, forRoblox ? "classic_oof.ogg" : "classic_oof.mp3");
            }
            if (soundType == "VineBoom")
            {
                return Path.Combine(CacheSoundsDir, forRoblox ? "vine_boom.ogg" : "vine_boom.mp3");
            }
            if (soundType == "Custom")
            {
                return customPath;
            }
            if (soundType == "Default")
            {
                var paths = new RobloxPaths();
                if (!string.IsNullOrEmpty(paths.CurrentVersionDir))
                {
                    var backup = Path.Combine(paths.CurrentVersionDir, "content", "sounds", "ouch.ogg.bak");
                    var cur = Path.Combine(paths.CurrentVersionDir, "content", "sounds", "ouch.ogg");
                    if (File.Exists(backup)) return backup;
                    if (File.Exists(cur)) return cur;
                }
            }

            return string.Empty;
        }

        private static string ProcessSound(string inputPath, int volume, double speed, bool forRoblox)
        {
            if (string.IsNullOrEmpty(inputPath) || !File.Exists(inputPath))
                return string.Empty;

            bool isDefaultFilter = (volume == 100 && Math.Abs(speed - 1.0) < 0.01);

            // If no filter needed
            if (isDefaultFilter)
            {
                if (forRoblox)
                {
                    // If already genuine ogg, return directly
                    if (inputPath.EndsWith(".ogg", StringComparison.OrdinalIgnoreCase))
                    {
                        return inputPath;
                    }
                    // Otherwise convert to ogg
                    var directOgg = Path.Combine(CacheSoundsDir, "custom_direct.ogg");
                    RunFfmpeg($"-y -i \"{inputPath}\" -c:a libvorbis -q:a 5 \"{directOgg}\"");
                    if (File.Exists(directOgg)) return directOgg;
                }
                else
                {
                    // For preview, return directly
                    return inputPath;
                }
            }

            // Apply ffmpeg audio filter
            double volMult = Math.Clamp(volume, 0, 300) / 100.0;
            double spd = Math.Clamp(speed, 0.5, 2.0);

            string filter = $"asetrate=44100*{spd:F2},aresample=44100,volume={volMult:F2}";
            string outputPath = Path.Combine(CacheSoundsDir, forRoblox ? "processed_roblox.ogg" : "processed_preview.mp3");
            string codecArgs = forRoblox ? "-c:a libvorbis -q:a 5" : "-c:a libmp3lame -q:a 2";

            bool ok = RunFfmpeg($"-y -i \"{inputPath}\" -filter:a \"{filter}\" {codecArgs} \"{outputPath}\"");
            if (ok && File.Exists(outputPath) && new FileInfo(outputPath).Length > 0)
            {
                return outputPath;
            }

            return inputPath;
        }

        private static bool RunFfmpeg(string arguments)
        {
            try
            {
                var psi = new ProcessStartInfo
                {
                    FileName = "ffmpeg",
                    Arguments = arguments,
                    CreateNoWindow = true,
                    UseShellExecute = false
                };
                using var proc = Process.Start(psi);
                proc?.WaitForExit(3000);
                return proc?.ExitCode == 0;
            }
            catch
            {
                return false;
            }
        }

        public static bool ApplyDeathSound(string? versionDir, string soundType, string customPath, int volume = 100, double speed = 1.0)
        {
            if (string.IsNullOrEmpty(versionDir) || !Directory.Exists(versionDir))
                return false;

            var soundsDir = Path.Combine(versionDir, "content", "sounds");
            if (!Directory.Exists(soundsDir))
            {
                Directory.CreateDirectory(soundsDir);
            }

            var ouchPath = Path.Combine(soundsDir, "ouch.ogg");
            var ouchBakPath = Path.Combine(soundsDir, "ouch.ogg.bak");
            var oofPath = Path.Combine(soundsDir, "oof.ogg");
            var oofBakPath = Path.Combine(soundsDir, "oof.ogg.bak");

            try
            {
                // 1. Ensure backups exist for both ouch.ogg and oof.ogg
                if (!File.Exists(ouchBakPath) && File.Exists(ouchPath))
                {
                    File.Copy(ouchPath, ouchBakPath, true);
                }
                if (!File.Exists(oofBakPath) && File.Exists(oofPath))
                {
                    File.Copy(oofPath, oofBakPath, true);
                }

                // 2. If Default and no filters, restore originals
                if (soundType == "Default" && volume == 100 && Math.Abs(speed - 1.0) < 0.01)
                {
                    if (File.Exists(ouchBakPath))
                    {
                        File.Copy(ouchBakPath, ouchPath, true);
                    }
                    if (File.Exists(oofBakPath))
                    {
                        File.Copy(oofBakPath, oofPath, true);
                    }
                    return true;
                }

                // 3. Get base sound and process with volume and speed
                var baseSource = GetBaseSoundPath(soundType, customPath, forRoblox: true);
                if (string.IsNullOrEmpty(baseSource) || !File.Exists(baseSource))
                    return false;

                var finalOgg = ProcessSound(baseSource, volume, speed, forRoblox: true);
                if (string.IsNullOrEmpty(finalOgg) || !File.Exists(finalOgg))
                    return false;

                // 4. Replace BOTH ouch.ogg AND oof.ogg
                File.Copy(finalOgg, ouchPath, true);
                File.Copy(finalOgg, oofPath, true);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public static void PlayPreview(string soundType, string customPath, int volume = 100, double speed = 1.0)
        {
            try
            {
                StopPreview();

                var baseSound = GetBaseSoundPath(soundType, customPath, forRoblox: false);
                if (string.IsNullOrEmpty(baseSound) || !File.Exists(baseSound))
                    return;

                var previewFile = ProcessSound(baseSound, volume, speed, forRoblox: false);
                if (!string.IsNullOrEmpty(previewFile) && File.Exists(previewFile))
                {
                    _previewPlayer = new MediaPlayer();
                    _previewPlayer.Open(new Uri(previewFile, UriKind.Absolute));
                    _previewPlayer.Play();
                }
            }
            catch { }
        }

        public static void StopPreview()
        {
            try
            {
                if (_previewPlayer != null)
                {
                    _previewPlayer.Stop();
                    _previewPlayer.Close();
                    _previewPlayer = null;
                }
            }
            catch { }
        }
    }
}
