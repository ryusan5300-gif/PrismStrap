using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace PrismStrap.Services
{
    public class FastFlagConfig
    {
        public int? TargetFps { get; set; } = 0; // 0 = unlimited
        public string GraphicsApi { get; set; } = "default"; // vulkan, d3d11, default
        public bool MaxTextureQuality { get; set; } = true;
        public bool DisablePostFx { get; set; } = false;
        public bool ClassicEscMenu { get; set; } = false;
    }

    public class FastFlagManager
    {
        private readonly string _settingsFilePath;

        public FastFlagManager(string settingsFilePath)
        {
            _settingsFilePath = settingsFilePath;
        }

        public JsonObject LoadRawFlags()
        {
            if (string.IsNullOrEmpty(_settingsFilePath) || !File.Exists(_settingsFilePath))
            {
                return new JsonObject();
            }

            try
            {
                var json = File.ReadAllText(_settingsFilePath);
                var node = JsonNode.Parse(json);
                return node as JsonObject ?? new JsonObject();
            }
            catch
            {
                return new JsonObject();
            }
        }

        public FastFlagConfig LoadConfig()
        {
            var flags = LoadRawFlags();
            var config = new FastFlagConfig();

            // FPS
            if (flags.TryGetPropertyValue("DFIntTaskSchedulerTargetFps", out var fpsNode) && fpsNode != null)
            {
                if (int.TryParse(fpsNode.ToString(), out int fps))
                {
                    config.TargetFps = fps;
                }
            }

            // Graphics API
            bool vulkan = flags.TryGetPropertyValue("FFlagDebugGraphicsPreferVulkan", out var v) && v?.ToString().ToLower() == "true";
            bool d3d11 = flags.TryGetPropertyValue("FFlagDebugGraphicsPreferD3D11", out var d) && d?.ToString().ToLower() == "true";
            if (vulkan)
                config.GraphicsApi = "vulkan";
            else if (d3d11)
                config.GraphicsApi = "d3d11";
            else
                config.GraphicsApi = "default";

            // Textures
            if (flags.TryGetPropertyValue("DFFlagTextureQualityOverrideEnabled", out var tq) && tq != null)
            {
                config.MaxTextureQuality = tq.ToString().ToLower() == "true";
            }

            // Post FX
            if (flags.TryGetPropertyValue("FFlagDisablePostFx", out var pfx) && pfx != null)
            {
                config.DisablePostFx = pfx.ToString().ToLower() == "true";
            }

            // Classic Esc Menu
            if (flags.TryGetPropertyValue("FFlagEnableInGameMenuControls", out var igm) && igm != null)
            {
                config.ClassicEscMenu = igm.ToString().ToLower() == "false";
            }

            return config;
        }

        public void SaveConfig(FastFlagConfig config)
        {
            if (string.IsNullOrEmpty(_settingsFilePath)) return;

            var flags = LoadRawFlags();

            // FPS: 0 means unlimited (set to high number or 0 for Roblox unlock)
            if (config.TargetFps.HasValue)
            {
                flags["DFIntTaskSchedulerTargetFps"] = config.TargetFps.Value;
            }
            else
            {
                flags.Remove("DFIntTaskSchedulerTargetFps");
            }

            // Graphics API
            flags.Remove("FFlagDebugGraphicsDisableVulkan");
            flags.Remove("FFlagDebugGraphicsPreferVulkan");
            flags.Remove("FFlagDebugGraphicsDisableDirect3D11");
            flags.Remove("FFlagDebugGraphicsPreferD3D11");

            switch (config.GraphicsApi.ToLower())
            {
                case "vulkan":
                    flags["FFlagDebugGraphicsDisableVulkan"] = false;
                    flags["FFlagDebugGraphicsPreferVulkan"] = true;
                    flags["FFlagDebugGraphicsDisableDirect3D11"] = true;
                    break;
                case "d3d11":
                    flags["FFlagDebugGraphicsDisableDirect3D11"] = false;
                    flags["FFlagDebugGraphicsPreferD3D11"] = true;
                    flags["FFlagDebugGraphicsDisableVulkan"] = true;
                    break;
                default:
                    // default / auto: flags removed
                    break;
            }

            // Texture quality
            if (config.MaxTextureQuality)
            {
                flags["DFFlagTextureQualityOverrideEnabled"] = true;
                flags["DFIntTextureQualityOverride"] = 3;
            }
            else
            {
                flags.Remove("DFFlagTextureQualityOverrideEnabled");
                flags.Remove("DFIntTextureQualityOverride");
            }

            // Post FX
            if (config.DisablePostFx)
            {
                flags["FFlagDisablePostFx"] = true;
            }
            else
            {
                flags.Remove("FFlagDisablePostFx");
            }

            // Classic Esc menu
            if (config.ClassicEscMenu)
            {
                flags["FFlagEnableInGameMenuControls"] = false;
                flags["FFlagDisableNewIGM"] = true;
            }
            else
            {
                flags.Remove("FFlagEnableInGameMenuControls");
                flags.Remove("FFlagDisableNewIGM");
            }

            SaveRawFlags(flags);
        }

        public void SetFlag(string key, string value)
        {
            var flags = LoadRawFlags();
            if (bool.TryParse(value, out bool bVal))
                flags[key] = bVal;
            else if (long.TryParse(value, out long lVal))
                flags[key] = lVal;
            else if (double.TryParse(value, out double dVal))
                flags[key] = dVal;
            else
                flags[key] = value;

            SaveRawFlags(flags);
        }

        public void RemoveFlag(string key)
        {
            var flags = LoadRawFlags();
            flags.Remove(key);
            SaveRawFlags(flags);
        }

        public void SaveRawFlags(JsonObject flags)
        {
            if (string.IsNullOrEmpty(_settingsFilePath)) return;

            var dir = Path.GetDirectoryName(_settingsFilePath);
            if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            var options = new JsonSerializerOptions { WriteIndented = true };
            File.WriteAllText(_settingsFilePath, flags.ToJsonString(options));
        }
    }
}
