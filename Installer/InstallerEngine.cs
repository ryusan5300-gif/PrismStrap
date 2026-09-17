using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace PrismStrap.Installer
{
    public class InstallerEngine
    {
        public static string GetDefaultInstallDir()
        {
            var localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            return Path.Combine(localAppData, "PrismStrap");
        }

        public static async Task<bool> InstallAsync(
            string installDir,
            bool createDesktop,
            bool createStartMenu,
            bool registerProtocol,
            IProgress<string>? progress = null)
        {
            try
            {
                // 1. Create Target Directory
                progress?.Report(InstallerLocalization.Get("StatusExtracting"));
                Directory.CreateDirectory(installDir);

                // Stop any running PrismStrap processes if updating
                try
                {
                    var procs = Process.GetProcessesByName("PrismStrap");
                    foreach (var p in procs)
                    {
                        try { p.Kill(); p.WaitForExit(1000); } catch { }
                    }
                }
                catch { }

                // 2. Extract Embedded PrismStrap.exe
                var targetExe = Path.Combine(installDir, "PrismStrap.exe");
                var asm = Assembly.GetExecutingAssembly();
                var resName = asm.GetManifestResourceNames()
                    .FirstOrDefault(n => n.EndsWith("PrismStrap.exe", StringComparison.OrdinalIgnoreCase));

                if (string.IsNullOrEmpty(resName))
                {
                    throw new FileNotFoundException("PrismStrap embedded executable not found in installer.");
                }

                using (var stream = asm.GetManifestResourceStream(resName))
                {
                    if (stream == null) throw new InvalidOperationException("Failed to read embedded executable stream.");
                    using (var fileStream = new FileStream(targetExe, FileMode.Create, FileAccess.Write, FileShare.None))
                    {
                        await stream.CopyToAsync(fileStream);
                    }
                }

                await Task.Delay(400); // Smooth visual feedback

                // 3. Create Shortcuts
                progress?.Report(InstallerLocalization.Get("StatusShortcuts"));
                if (createDesktop)
                {
                    var desktopDir = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
                    CreateShortcut(targetExe, Path.Combine(desktopDir, "PrismStrap.lnk"), "PrismStrap - Next-Gen Roblox Bootstrapper");
                }

                if (createStartMenu)
                {
                    var startMenuDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"Microsoft\Windows\Start Menu\Programs");
                    CreateShortcut(targetExe, Path.Combine(startMenuDir, "PrismStrap.lnk"), "PrismStrap - Next-Gen Roblox Bootstrapper");
                }

                await Task.Delay(300);

                // 4. Configure System & Registry
                progress?.Report(InstallerLocalization.Get("StatusRegistering"));

                // Register Uninstall
                RegisterUninstall(targetExe, installDir);

                // Register Protocol
                if (registerProtocol)
                {
                    RegisterProtocolKey(targetExe);
                }

                // Track genuine installation / download
                _ = Task.Run(async () =>
                {
                    try
                    {
                        using var client = new System.Net.Http.HttpClient { Timeout = TimeSpan.FromSeconds(5) };
                        await client.GetAsync("https://countapi.mileshilliard.com/api/v1/hit/prismstrap_v1_total_downloads");
                    }
                    catch { }
                });

                await Task.Delay(300);
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[InstallerEngine] Installation failed: {ex.Message}");
                return false;
            }
        }

        private static void RegisterUninstall(string exePath, string installDir)
        {
            try
            {
                using var key = Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Uninstall\PrismStrap");
                key.SetValue("DisplayName", "PrismStrap");
                key.SetValue("DisplayVersion", "0.3.0");
                key.SetValue("Publisher", "PrismStrap Team");
                key.SetValue("DisplayIcon", exePath);
                key.SetValue("InstallLocation", installDir);
                key.SetValue("UninstallString", $"\"{exePath}\" --uninstall");
            }
            catch { }
        }

        private static void RegisterProtocolKey(string exePath)
        {
            try
            {
                using var rootKey = Registry.CurrentUser.CreateSubKey(@"Software\Classes\roblox-player");
                rootKey.SetValue("", "URL:Roblox Protocol");
                rootKey.SetValue("URL Protocol", "");

                using var cmdKey = Registry.CurrentUser.CreateSubKey(@"Software\Classes\roblox-player\shell\open\command");
                cmdKey.SetValue("", $"\"{exePath}\" \"%1\"");

                using var pRootKey = Registry.CurrentUser.CreateSubKey(@"Software\Classes\prismstrap");
                pRootKey.SetValue("", "URL:PrismStrap Protocol");
                pRootKey.SetValue("URL Protocol", "");

                using var pCmdKey = Registry.CurrentUser.CreateSubKey(@"Software\Classes\prismstrap\shell\open\command");
                pCmdKey.SetValue("", $"\"{exePath}\" \"%1\"");
            }
            catch { }
        }

        private static void CreateShortcut(string targetPath, string shortcutPath, string description)
        {
            try
            {
                var workDir = Path.GetDirectoryName(targetPath);
                var script = $"$ws = New-Object -ComObject WScript.Shell; $s = $ws.CreateShortcut('{shortcutPath}'); $s.TargetPath = '{targetPath}'; $s.WorkingDirectory = '{workDir}'; $s.Description = '{description}'; $s.Save()";

                var psi = new ProcessStartInfo
                {
                    FileName = "powershell.exe",
                    Arguments = $"-NoProfile -Command \"{script}\"",
                    CreateNoWindow = true,
                    UseShellExecute = false
                };
                Process.Start(psi)?.WaitForExit(3000);
            }
            catch { }
        }
    }
}
