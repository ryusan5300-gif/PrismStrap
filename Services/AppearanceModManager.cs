using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace PrismStrap.Services
{
    public static class AppearanceModManager
    {
        private static readonly string AppDataDir = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "PrismStrap");

        private static readonly string CacheAppearanceDir = Path.Combine(AppDataDir, "Appearance");
        private static readonly string CacheCursorsDir = Path.Combine(CacheAppearanceDir, "Cursors");
        private static readonly string CacheIconsDir = Path.Combine(CacheAppearanceDir, "Icons");

        [DllImport("shell32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern void SHChangeNotify(int wEventId, uint uFlags, IntPtr dwItem1, IntPtr dwItem2);
        private const int SHCNE_ASSOCCHANGED = 0x08000000;
        private const uint SHCNF_IDLIST = 0x0000;

        static AppearanceModManager()
        {
            EnsureAssetsExtracted();
        }

        public static void EnsureAssetsExtracted()
        {
            try
            {
                Directory.CreateDirectory(CacheCursorsDir);
                Directory.CreateDirectory(CacheIconsDir);

                ExtractResource("Cursors/cursor_classic2013.png", Path.Combine(CacheCursorsDir, "cursor_classic2013.png"));
                ExtractResource("Cursors/cursor_classic2006.png", Path.Combine(CacheCursorsDir, "cursor_classic2006.png"));
                ExtractResource("Cursors/cursor_clean.png", Path.Combine(CacheCursorsDir, "cursor_clean.png"));

                ExtractResource("Icons/icon_2008.ico", Path.Combine(CacheIconsDir, "icon_2008.ico"));
                ExtractResource("Icons/icon_2015.ico", Path.Combine(CacheIconsDir, "icon_2015.ico"));
                ExtractResource("Icons/icon_2017.ico", Path.Combine(CacheIconsDir, "icon_2017.ico"));
                ExtractResource("Icons/icon_2022.ico", Path.Combine(CacheIconsDir, "icon_2022.ico"));
                ExtractResource("Icons/icon_prism.ico", Path.Combine(CacheIconsDir, "icon_prism.ico"));

                ExtractResource("Icons/icon_2008.png", Path.Combine(CacheIconsDir, "icon_2008.png"));
                ExtractResource("Icons/icon_2015.png", Path.Combine(CacheIconsDir, "icon_2015.png"));
                ExtractResource("Icons/icon_2017.png", Path.Combine(CacheIconsDir, "icon_2017.png"));
                ExtractResource("Icons/icon_2022.png", Path.Combine(CacheIconsDir, "icon_2022.png"));
                ExtractResource("Icons/icon_prism.png", Path.Combine(CacheIconsDir, "icon_prism.png"));
            }
            catch { }
        }

        private static void ExtractResource(string subPath, string targetPath)
        {
            try
            {
                var asm = Assembly.GetExecutingAssembly();
                var resName = $"PrismStrap.Assets.Appearance.{subPath.Replace('/', '.')}";

                using (var stream = asm.GetManifestResourceStream(resName))
                {
                    if (stream != null)
                    {
                        if (File.Exists(targetPath) && new FileInfo(targetPath).Length == stream.Length)
                            return;

                        using var fs = File.Create(targetPath);
                        stream.CopyTo(fs);
                        return;
                    }
                }

                // Fallback: check local dev directory
                var localDev = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "Appearance", subPath.Replace('/', Path.DirectorySeparatorChar));
                if (File.Exists(localDev))
                {
                    if (!File.Exists(targetPath) || new FileInfo(targetPath).Length != new FileInfo(localDev).Length)
                    {
                        File.Copy(localDev, targetPath, true);
                    }
                    return;
                }

                var scratchPath = Path.Combine(@"C:\Users\ryuku\.gemini\antigravity\scratch\PrismStrap\Assets\Appearance", subPath.Replace('/', Path.DirectorySeparatorChar));
                if (File.Exists(scratchPath))
                {
                    if (!File.Exists(targetPath) || new FileInfo(targetPath).Length != new FileInfo(scratchPath).Length)
                    {
                        File.Copy(scratchPath, targetPath, true);
                    }
                }
            }
            catch { }
        }

        public static string GetIconPreviewPath(string iconType)
        {
            EnsureAssetsExtracted();
            var filename = iconType switch
            {
                "Classic2008" => "icon_2008.png",
                "RedSquare2015" => "icon_2015.png",
                "Tilted2017" => "icon_2017.png",
                "Modern2022" => "icon_2022.png",
                "PrismNeon" => "icon_prism.png",
                _ => "icon_2022.png"
            };
            return Path.Combine(CacheIconsDir, filename);
        }

        public static string GetCursorPreviewPath(string cursorType)
        {
            EnsureAssetsExtracted();
            var filename = cursorType switch
            {
                "Classic2013" => "cursor_classic2013.png",
                "Classic2006" => "cursor_classic2006.png",
                "CleanModern" => "cursor_clean.png",
                _ => "cursor_clean.png"
            };
            return Path.Combine(CacheCursorsDir, filename);
        }

        public static string GetIconFilePath(string iconType)
        {
            EnsureAssetsExtracted();
            var filename = iconType switch
            {
                "Classic2008" => "icon_2008.ico",
                "RedSquare2015" => "icon_2015.ico",
                "Tilted2017" => "icon_2017.ico",
                "Modern2022" => "icon_2022.ico",
                "PrismNeon" => "icon_prism.ico",
                _ => "icon_2022.ico"
            };
            return Path.Combine(CacheIconsDir, filename);
        }

        // ====================================================================
        // FONT MOD APPLICATION
        // ====================================================================
        private static readonly string[] TargetFontNames = new[]
        {
            "BuilderSans-Regular.otf",
            "BuilderSans-Bold.otf",
            "BuilderSans-Medium.otf",
            "BuilderSans-ExtraBold.otf",
            "SourceSansPro-Regular.ttf",
            "SourceSansPro-Bold.ttf",
            "SourceSansPro-Semibold.ttf"
        };

        public static void ApplyFont(string robloxDir, string fontType, string customFontPath)
        {
            try
            {
                var fontsDir = Path.Combine(robloxDir, "content", "fonts");
                if (!Directory.Exists(fontsDir)) return;

                if (fontType == "Default" || string.IsNullOrEmpty(fontType))
                {
                    // Restore original fonts from backup if available
                    foreach (var fontName in TargetFontNames)
                    {
                        var bakPath = Path.Combine(fontsDir, fontName + ".bak");
                        var curPath = Path.Combine(fontsDir, fontName);
                        if (File.Exists(bakPath))
                        {
                            File.Copy(bakPath, curPath, true);
                        }
                    }
                    return;
                }

                // Determine replacement font source
                string sourceFont = string.Empty;
                if (fontType == "ComicSans")
                {
                    var winComic = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "comic.ttf");
                    if (File.Exists(winComic)) sourceFont = winComic;
                }
                else if (fontType == "SegoeUI")
                {
                    var winSegoe = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "segoeui.ttf");
                    if (File.Exists(winSegoe)) sourceFont = winSegoe;
                }
                else if (fontType == "Arial")
                {
                    var winArial = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "arial.ttf");
                    if (File.Exists(winArial)) sourceFont = winArial;
                }
                else if (fontType == "Custom" && !string.IsNullOrEmpty(customFontPath) && File.Exists(customFontPath))
                {
                    sourceFont = customFontPath;
                }

                if (string.IsNullOrEmpty(sourceFont) || !File.Exists(sourceFont))
                    return;

                // Backup originals and replace
                foreach (var fontName in TargetFontNames)
                {
                    var curPath = Path.Combine(fontsDir, fontName);
                    var bakPath = Path.Combine(fontsDir, fontName + ".bak");

                    if (File.Exists(curPath) && !File.Exists(bakPath))
                    {
                        try { File.Copy(curPath, bakPath, true); } catch { }
                    }

                    try
                    {
                        File.Copy(sourceFont, curPath, true);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"[AppearanceModManager] Font replace failed: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[AppearanceModManager] Error applying font: {ex.Message}");
            }
        }

        // ====================================================================
        // CURSOR MOD APPLICATION
        // ====================================================================
        private static readonly string[] TargetCursorPaths = new[]
        {
            Path.Combine("content", "textures", "Cursors", "KeyboardMouse", "ArrowCursor.png"),
            Path.Combine("content", "textures", "Cursors", "KeyboardMouse", "ArrowFarCursor.png"),
            Path.Combine("content", "textures", "ArrowCursor.png"),
            Path.Combine("content", "textures", "ArrowFarCursor.png")
        };

        public static void ApplyCursor(string robloxDir, string cursorType, string customCursorPath)
        {
            try
            {
                EnsureAssetsExtracted();

                if (cursorType == "Default" || string.IsNullOrEmpty(cursorType))
                {
                    // Restore from backup
                    foreach (var relPath in TargetCursorPaths)
                    {
                        var curPath = Path.Combine(robloxDir, relPath);
                        var bakPath = curPath + ".bak";
                        if (File.Exists(bakPath))
                        {
                            File.Copy(bakPath, curPath, true);
                        }
                    }
                    return;
                }

                string sourceCursor = string.Empty;
                if (cursorType == "Classic2013")
                {
                    sourceCursor = Path.Combine(CacheCursorsDir, "cursor_classic2013.png");
                }
                else if (cursorType == "Classic2006")
                {
                    sourceCursor = Path.Combine(CacheCursorsDir, "cursor_classic2006.png");
                }
                else if (cursorType == "CleanModern")
                {
                    sourceCursor = Path.Combine(CacheCursorsDir, "cursor_clean.png");
                }
                else if (cursorType == "Custom" && !string.IsNullOrEmpty(customCursorPath) && File.Exists(customCursorPath))
                {
                    sourceCursor = customCursorPath;
                }

                if (string.IsNullOrEmpty(sourceCursor) || !File.Exists(sourceCursor))
                    return;

                foreach (var relPath in TargetCursorPaths)
                {
                    var curPath = Path.Combine(robloxDir, relPath);
                    var dir = Path.GetDirectoryName(curPath);
                    if (!string.IsNullOrEmpty(dir) && Directory.Exists(dir))
                    {
                        var bakPath = curPath + ".bak";
                        if (File.Exists(curPath) && !File.Exists(bakPath))
                        {
                            try { File.Copy(curPath, bakPath, true); } catch { }
                        }

                        try
                        {
                            File.Copy(sourceCursor, curPath, true);
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine($"[AppearanceModManager] Cursor replace failed: {ex.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[AppearanceModManager] Error applying cursor: {ex.Message}");
            }
        }

        // ====================================================================
        // ROBLOX APP ICON APPLICATION (Only modifies Roblox, protects PrismStrap)
        // ====================================================================
        public static void ApplyAppIcon(string iconType)
        {
            try
            {
                EnsureAssetsExtracted();
                var icoPath = GetIconFilePath(iconType);
                if (!File.Exists(icoPath)) return;

                // 1. Ensure PrismStrap's own shortcut is restored to its proper icon
                RestorePrismStrapShortcutIcon();

                // 2. Update Windows Protocol Association DefaultIcon for Roblox
                try
                {
                    using var key = Registry.CurrentUser.CreateSubKey(@"Software\Classes\roblox-player\DefaultIcon");
                    if (key != null)
                    {
                        key.SetValue("", $"{icoPath},0");
                    }
                }
                catch { }

                try
                {
                    using var key2 = Registry.CurrentUser.CreateSubKey(@"Software\Classes\roblox\DefaultIcon");
                    if (key2 != null)
                    {
                        key2.SetValue("", $"{icoPath},0");
                    }
                }
                catch { }

                // Clean up any incorrect prismstrap protocol DefaultIcon override
                try
                {
                    Registry.CurrentUser.DeleteSubKeyTree(@"Software\Classes\prismstrap\DefaultIcon", false);
                }
                catch { }

                // 3. Find and update all Roblox shortcuts across Desktop and Start Menu
                var searchDirs = new List<string>
                {
                    Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory),
                    Environment.GetFolderPath(Environment.SpecialFolder.CommonDesktopDirectory),
                    Environment.GetFolderPath(Environment.SpecialFolder.Programs),
                    Environment.GetFolderPath(Environment.SpecialFolder.CommonPrograms)
                };

                var shellType = Type.GetTypeFromProgID("WScript.Shell");
                dynamic? shell = shellType != null ? Activator.CreateInstance(shellType) : null;

                bool updatedDesktopRoblox = false;

                foreach (var dir in searchDirs)
                {
                    if (string.IsNullOrEmpty(dir) || !Directory.Exists(dir)) continue;

                    try
                    {
                        var lnkFiles = Directory.GetFiles(dir, "*.lnk", SearchOption.AllDirectories);
                        foreach (var lnk in lnkFiles)
                        {
                            var name = Path.GetFileNameWithoutExtension(lnk).ToLowerInvariant();
                            // Match Roblox shortcuts, but do NOT match Studio or PrismStrap
                            if (name.Contains("roblox") && !name.Contains("studio") && !name.Contains("prism"))
                            {
                                UpdateShortcutIconWithShell(shell, lnk, icoPath);
                                if (dir.IndexOf("Desktop", StringComparison.OrdinalIgnoreCase) >= 0)
                                {
                                    updatedDesktopRoblox = true;
                                }
                            }
                        }
                    }
                    catch { }
                }

                // 4. If no Roblox shortcut was on the Desktop, create one so user immediately sees the custom icon
                if (!updatedDesktopRoblox)
                {
                    try
                    {
                        var paths = new RobloxPaths();
                        if (!string.IsNullOrEmpty(paths.PlayerExecutable) && File.Exists(paths.PlayerExecutable) && shell != null)
                        {
                            var userDesktop = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
                            var desktopRobloxLnk = Path.Combine(userDesktop, "Roblox Player.lnk");
                            dynamic? shortcut = shell.CreateShortcut(desktopRobloxLnk);
                            if (shortcut != null)
                            {
                                shortcut.TargetPath = paths.PlayerExecutable;
                                shortcut.WorkingDirectory = Path.GetDirectoryName(paths.PlayerExecutable);
                                shortcut.Description = "Roblox Player";
                                shortcut.IconLocation = $"{icoPath},0";
                                shortcut.Save();
                            }
                        }
                    }
                    catch { }
                }

                // 5. Notify Windows Shell to refresh icon cache immediately
                SHChangeNotify(SHCNE_ASSOCCHANGED, SHCNF_IDLIST, IntPtr.Zero, IntPtr.Zero);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[AppearanceModManager] Error applying app icon: {ex.Message}");
            }
        }

        public static void RestorePrismStrapShortcutIcon()
        {
            try
            {
                var desktopLnk = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), "PrismStrap.lnk");
                var startMenuLnk = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Programs), "PrismStrap.lnk");

                var shellType = Type.GetTypeFromProgID("WScript.Shell");
                if (shellType == null) return;
                dynamic shell = Activator.CreateInstance(shellType)!;

                foreach (var lnk in new[] { desktopLnk, startMenuLnk })
                {
                    if (!File.Exists(lnk)) continue;
                    dynamic sc = shell.CreateShortcut(lnk);
                    string iconLoc = sc.IconLocation ?? "";
                    // If the icon was mistakenly pointed to Appearance\Icons or icon_*
                    if (iconLoc.IndexOf("Appearance", StringComparison.OrdinalIgnoreCase) >= 0 ||
                        iconLoc.IndexOf("icon_20", StringComparison.OrdinalIgnoreCase) >= 0 ||
                        iconLoc.IndexOf("icon_prism", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        // Reset to target exe default icon
                        sc.IconLocation = $"{sc.TargetPath},0";
                        sc.Save();
                    }
                }
            }
            catch { }
        }

        private static void UpdateShortcutIconWithShell(dynamic? shell, string shortcutPath, string iconPath)
        {
            try
            {
                if (!File.Exists(shortcutPath) || shell == null) return;
                dynamic? shortcut = shell.CreateShortcut(shortcutPath);
                if (shortcut != null)
                {
                    shortcut.IconLocation = $"{iconPath},0";
                    shortcut.Save();
                }
            }
            catch { }
        }

        // ====================================================================
        // MASTER APPLY (Called during launch in LoadingWindow)
        // ====================================================================
        public static void ApplyAllMods(string robloxDir, PrismAppConfig config)
        {
            if (string.IsNullOrEmpty(robloxDir) || config == null) return;
            ApplyFont(robloxDir, config.CustomFontType ?? "Default", config.CustomFontPath ?? string.Empty);
            ApplyCursor(robloxDir, config.CursorType ?? "Default", config.CustomCursorPath ?? string.Empty);
            ApplyAppIcon(config.AppIconType ?? "Default");
        }
    }
}
