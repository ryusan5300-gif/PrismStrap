using System;
using System.IO;
using System.Text.Json;

namespace PrismStrap.Services
{
    public class PrismAppConfig
    {
        public string Language { get; set; } = "ja";
        public string DeathSoundType { get; set; } = "Default"; // "Default", "ClassicOof", "VineBoom", "Custom"
        public string CustomDeathSoundPath { get; set; } = string.Empty;
        public int DeathSoundVolume { get; set; } = 100; // 0 - 300%
        public double DeathSoundSpeed { get; set; } = 1.0; // 0.5 - 2.0x

        // Appearance Mods (Font, Cursor, Roblox Icon)
        public string CustomFontType { get; set; } = "Default"; // "Default", "ComicSans", "SegoeUI", "Arial", "Custom"
        public string CustomFontPath { get; set; } = string.Empty;
        public string CursorType { get; set; } = "Default"; // "Default", "Classic2013", "Classic2006", "CleanModern", "Custom"
        public string CustomCursorPath { get; set; } = string.Empty;
        public string AppIconType { get; set; } = "Default"; // "Default", "Classic2008", "RedSquare2015", "Tilted2017", "Modern2022", "PrismNeon"
    }

    public static class PrismConfigManager
    {
        private static readonly string ConfigDir = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "PrismStrap");

        private static readonly string ConfigPath = Path.Combine(ConfigDir, "prism_config.json");

        public static PrismAppConfig Load()
        {
            try
            {
                if (File.Exists(ConfigPath))
                {
                    var json = File.ReadAllText(ConfigPath);
                    var cfg = JsonSerializer.Deserialize<PrismAppConfig>(json);
                    if (cfg != null) return cfg;
                }
            }
            catch { }

            return new PrismAppConfig();
        }

        public static void Save(PrismAppConfig config)
        {
            try
            {
                if (!Directory.Exists(ConfigDir))
                {
                    Directory.CreateDirectory(ConfigDir);
                }

                var options = new JsonSerializerOptions { WriteIndented = true };
                var json = JsonSerializer.Serialize(config, options);
                File.WriteAllText(ConfigPath, json);
            }
            catch { }
        }
    }
}
