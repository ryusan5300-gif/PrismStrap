using System;
using System.Diagnostics;
using System.IO;
using Microsoft.Win32;

namespace PrismStrap.Services
{
    public class AppSetup
    {
        public static string GetInstallDir()
        {
            var localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            return Path.Combine(localAppData, "PrismStrap");
        }

        public static string GetInstalledExe()
        {
            return Path.Combine(GetInstallDir(), "PrismStrap.exe");
        }

        public static bool IsRunningFromInstallDir()
        {
            var currentExe = Environment.ProcessPath;
            var installedExe = GetInstalledExe();
            if (string.IsNullOrEmpty(currentExe) || string.IsNullOrEmpty(installedExe))
                return false;

            return string.Equals(Path.GetFullPath(currentExe), Path.GetFullPath(installedExe), StringComparison.OrdinalIgnoreCase);
        }

        public static bool Install()
        {
            try
            {
                var installDir = GetInstallDir();
                var targetExe = GetInstalledExe();
                var currentExe = Environment.ProcessPath;

                Directory.CreateDirectory(installDir);

                if (!string.IsNullOrEmpty(currentExe) && !string.Equals(Path.GetFullPath(currentExe), Path.GetFullPath(targetExe), StringComparison.OrdinalIgnoreCase))
                {
                    File.Copy(currentExe, targetExe, overwrite: true);
                }

                // Desktop shortcut
                var desktopDir = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
                CreateShortcut(targetExe, Path.Combine(desktopDir, "PrismStrap.lnk"), "PrismStrap - Next-Gen Roblox Bootstrapper");

                // Start menu shortcut
                var startMenuDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"Microsoft\Windows\Start Menu\Programs");
                CreateShortcut(targetExe, Path.Combine(startMenuDir, "PrismStrap.lnk"), "PrismStrap - Next-Gen Roblox Bootstrapper");

                // Windows Uninstall registration
                RegisterUninstall(targetExe, installDir);

                // Protocol registration (roblox-player & prismstrap)
                RobloxLauncher.RegisterProtocol(targetExe);

                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Installation failed: {ex.Message}");
                return false;
            }
        }

        private static void RegisterUninstall(string exePath, string installDir)
        {
            try
            {
                using var key = Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Uninstall\PrismStrap");
                key.SetValue("DisplayName", "PrismStrap");
                key.SetValue("DisplayVersion", "0.1.0");
                key.SetValue("Publisher", "PrismStrap Team");
                key.SetValue("DisplayIcon", exePath);
                key.SetValue("InstallLocation", installDir);
                key.SetValue("UninstallString", $"\"{exePath}\" --uninstall");
            }
            catch { }
        }

        public static void CreateShortcut(string targetPath, string shortcutPath, string description)
        {
            try
            {
                // Use WScript.Shell through COM or PowerShell script
                var workDir = Path.GetDirectoryName(targetPath);
                var script = $"$ws = New-Object -ComObject WScript.Shell; $s = $ws.CreateShortcut('{shortcutPath}'); $s.TargetPath = '{targetPath}'; $s.WorkingDirectory = '{workDir}'; $s.Description = '{description}'; $s.Save()";

                var psi = new ProcessStartInfo
                {
                    FileName = "powershell.exe",
                    Arguments = $"-NoProfile -Command \"{script}\"",
                    CreateNoWindow = true,
                    UseShellExecute = false
                };
                Process.Start(psi)?.WaitForExit();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Failed to create shortcut: {ex.Message}");
            }
        }
    }
}
