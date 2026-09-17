using System;
using System.Diagnostics;
using System.IO;
using Microsoft.Win32;

namespace PrismStrap.Services
{
    public class RobloxLauncher
    {
        public static bool Launch(string executablePath, string[] args)
        {
            if (string.IsNullOrEmpty(executablePath) || !File.Exists(executablePath))
                return false;

            try
            {
                var cfg = PrismConfigManager.Load();
                var dir = Path.GetDirectoryName(executablePath);
                DeathSoundManager.ApplyDeathSound(dir, cfg.DeathSoundType, cfg.CustomDeathSoundPath, cfg.DeathSoundVolume, cfg.DeathSoundSpeed);
            }
            catch { }

            try
            {
                var psi = new ProcessStartInfo
                {
                    FileName = executablePath,
                    Arguments = string.Join(" ", args),
                    UseShellExecute = true,
                    WorkingDirectory = Path.GetDirectoryName(executablePath)
                };

                Process.Start(psi);
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Failed to launch Roblox: {ex.Message}");
                return false;
            }
        }

        public static bool IsProtocolRegistered()
        {
            try
            {
                using var key = Registry.CurrentUser.OpenSubKey(@"Software\Classes\roblox-player\shell\open\command");
                if (key != null)
                {
                    var val = key.GetValue("")?.ToString();
                    return val != null && val.Contains("PrismStrap", StringComparison.OrdinalIgnoreCase);
                }
            }
            catch { }
            return false;
        }

        public static bool RegisterProtocol(string? customExePath = null)
        {
            try
            {
                var exePath = customExePath ?? Process.GetCurrentProcess().MainModule?.FileName;
                if (string.IsNullOrEmpty(exePath)) return false;

                using var rootKey = Registry.CurrentUser.CreateSubKey(@"Software\Classes\roblox-player");
                rootKey.SetValue("", "URL:Roblox Protocol");
                rootKey.SetValue("URL Protocol", "");

                using var cmdKey = Registry.CurrentUser.CreateSubKey(@"Software\Classes\roblox-player\shell\open\command");
                cmdKey.SetValue("", $"\"{exePath}\" \"%1\"");

                // Register prismstrap:// protocol for Workshop One-Click Import
                using var pRootKey = Registry.CurrentUser.CreateSubKey(@"Software\Classes\prismstrap");
                pRootKey.SetValue("", "URL:PrismStrap Protocol");
                pRootKey.SetValue("URL Protocol", "");

                using var pCmdKey = Registry.CurrentUser.CreateSubKey(@"Software\Classes\prismstrap\shell\open\command");
                pCmdKey.SetValue("", $"\"{exePath}\" \"%1\"");

                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Failed to register protocol: {ex.Message}");
                return false;
            }
        }

        public static bool UnregisterProtocol()
        {
            try
            {
                Registry.CurrentUser.DeleteSubKeyTree(@"Software\Classes\roblox-player", throwOnMissingSubKey: false);
                Registry.CurrentUser.DeleteSubKeyTree(@"Software\Classes\prismstrap", throwOnMissingSubKey: false);
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Failed to unregister protocol: {ex.Message}");
                return false;
            }
        }
    }
}
