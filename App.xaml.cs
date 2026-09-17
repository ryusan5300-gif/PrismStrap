using System;
using System.IO;
using System.Linq;
using System.Windows;
using PrismStrap.Services;

namespace PrismStrap
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            AppDomain.CurrentDomain.UnhandledException += (s, ev) =>
            {
                var err = ev.ExceptionObject?.ToString() ?? "Unknown error";
                MessageBox.Show($"致命的なエラーが発生しました:\n{err}", "PrismStrap", MessageBoxButton.OK, MessageBoxImage.Error);
            };
            DispatcherUnhandledException += (s, ev) =>
            {
                try
                {
                    var logDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "PrismStrap");
                    Directory.CreateDirectory(logDir);
                    File.AppendAllText(Path.Combine(logDir, "crash.log"), $"[{DateTime.Now}] {ev.Exception}\n\n");
                }
                catch { }

                MessageBox.Show($"アプリケーションエラーが発生しました:\n{ev.Exception.Message}\n\n{ev.Exception.StackTrace}", "PrismStrap", MessageBoxButton.OK, MessageBoxImage.Error);
                ev.Handled = true;
            };

            base.OnStartup(e);

            // Load saved language configuration
            try
            {
                var cfg = PrismConfigManager.Load();
                if (!string.IsNullOrEmpty(cfg.Language))
                {
                    LocalizationService.SetLanguage(cfg.Language);
                }
            }
            catch { }

            // Ensure protocols (prismstrap:// and roblox-player://) are registered in Windows
            try
            {
                RobloxLauncher.RegisterProtocol();
                AppearanceModManager.EnsureAssetsExtracted();
            }
            catch { }

            // Check command line arguments
            if (e.Args.Length > 0)
            {
                var arg0 = e.Args[0];

                if (arg0.Equals("--apply-icon", StringComparison.OrdinalIgnoreCase) && e.Args.Length >= 2)
                {
                    var iconType = e.Args[1];
                    AppearanceModManager.ApplyAppIcon(iconType);
                    Environment.Exit(0);
                    return;
                }

                if (arg0.Equals("--uninstall", StringComparison.OrdinalIgnoreCase))
                {
                    RobloxLauncher.UnregisterProtocol();
                    // Delete shortcuts
                    var desktopLnk = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), "PrismStrap.lnk");
                    var startLnk = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"Microsoft\Windows\Start Menu\Programs\PrismStrap.lnk");
                    if (File.Exists(desktopLnk)) File.Delete(desktopLnk);
                    if (File.Exists(startLnk)) File.Delete(startLnk);
                    Environment.Exit(0);
                    return;
                }

                // If launched via roblox-player:// or roblox:// URI
                bool isRobloxLaunch = e.Args.Any(a =>
                    a.StartsWith("roblox-player:", StringComparison.OrdinalIgnoreCase) ||
                    a.StartsWith("roblox:", StringComparison.OrdinalIgnoreCase));

                if (isRobloxLaunch)
                {
                    new LoadingWindow(e.Args).Show();
                    return;
                }

                // If launched via prismstrap:// (Workshop One-Click Import)
                var importArg = e.Args.FirstOrDefault(a => a.StartsWith("prismstrap:", StringComparison.OrdinalIgnoreCase));
                if (!string.IsNullOrEmpty(importArg))
                {
                    try
                    {
                        var uri = new Uri(importArg);
                        var query = System.Web.HttpUtility.ParseQueryString(uri.Query);
                        var flagsJson = query["flags"];
                        if (!string.IsNullOrEmpty(flagsJson))
                        {
                            var paths = new RobloxPaths();
                            var settingsPath = paths.GetAppSettingsPath();
                            if (!string.IsNullOrEmpty(settingsPath))
                            {
                                var ffManager = new FastFlagManager(settingsPath);
                                var flags = ffManager.LoadRawFlags();
                                var node = System.Text.Json.Nodes.JsonNode.Parse(flagsJson);
                                if (node is System.Text.Json.Nodes.JsonObject importedObj)
                                {
                                    foreach (var prop in importedObj)
                                    {
                                        flags[prop.Key] = prop.Value?.DeepClone();
                                    }
                                    ffManager.SaveRawFlags(flags);
                                    MessageBox.Show("ワークショップからFastFlagsを正常にインポートしました！", "PrismStrap ワークショップ", MessageBoxButton.OK, MessageBoxImage.Information);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"FastFlags のインポート中にエラーが発生しました:\n{ex.Message}", "PrismStrap", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                    Environment.Exit(0);
                    return;
                }
                // If --settings argument passed, open settings directly
                if (e.Args.Any(a => a.Equals("--settings", StringComparison.OrdinalIgnoreCase) || a.Equals("-settings", StringComparison.OrdinalIgnoreCase)))
                {
                    new MainWindow().Show();
                    return;
                }
            }

            // Normal launch -> Show prompt selection window (Play Roblox or Open Settings)
            new ModeSelectWindow().Show();
        }
    }
}
