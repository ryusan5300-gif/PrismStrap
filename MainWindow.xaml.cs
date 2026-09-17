using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using PrismStrap.Services;

namespace PrismStrap
{
    public class FlagItem
    {
        public string Key { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
    }

    public partial class MainWindow : Window
    {
        private readonly RobloxPaths _paths;
        private readonly FastFlagManager _ffManager;
        private PrismAppConfig _prismConfig;
        private DispatcherTimer? _toastTimer;
        private bool _isUpdatingUi = false;

        public MainWindow()
        {
            _isUpdatingUi = true;
            _paths = new RobloxPaths();
            _ffManager = new FastFlagManager(_paths.GetAppSettingsPath());
            _prismConfig = PrismConfigManager.Load();

            LocalizationService.SetLanguage(_prismConfig.Language);
            LocalizationService.LanguageChanged += OnLanguageChanged;

            InitializeComponent();
            _isUpdatingUi = false;

            Loaded += MainWindow_Loaded;
            Closing += (s, e) => DeathSoundManager.StopPreview();
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                DeathSoundManager.EnsureSoundFilesExtracted();

                _isUpdatingUi = true;
                SelectComboLanguage(ComboLanguage, _prismConfig.Language);
                SelectComboLanguage(ComboSysLanguage, _prismConfig.Language);
                _isUpdatingUi = false;

                RefreshAll();
                UpdateLocalizedStrings();
                InitAppearanceModsUI();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Initialization error:\n{ex.Message}", "PrismStrap", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
            }
        }

        private void OnLanguageChanged()
        {
            UpdateLocalizedStrings();
        }

        private void SelectComboLanguage(ComboBox combo, string langCode)
        {
            if (combo == null) return;
            foreach (ComboBoxItem item in combo.Items)
            {
                if (item.Tag is string tag && tag.Equals(langCode, StringComparison.OrdinalIgnoreCase))
                {
                    combo.SelectedItem = item;
                    break;
                }
            }
        }

        private void UpdateLocalizedStrings()
        {
            // Navigation
            NavHome.Content = "🏠  " + LocalizationService.Get("NavHome");
            NavPerf.Content = "⚡  " + LocalizationService.Get("NavPerf");
            NavGfx.Content = "🎨  " + LocalizationService.Get("NavGfx");
            NavSounds.Content = "🔊  " + LocalizationService.Get("NavSounds");
            NavFlags.Content = "🛠️  " + LocalizationService.Get("NavFlags");
            NavMods.Content = "🎭  " + LocalizationService.Get("NavMods");
            NavSys.Content = "⚙️  " + LocalizationService.Get("NavSys");

            // Header & App Info
            TxtAppSubtitle.Text = LocalizationService.Get("HomeSubtitle");
            BtnTopLaunch.Content = LocalizationService.Get("BtnLaunchRoblox");
            BtnCheckUpdates.Content = LocalizationService.Get("BtnCheckUpdates");

            // Update Current Page Header Title
            UpdateHeaderTitle();

            // Home Page
            TxtBadgeReady.Text = LocalizationService.Get("StatusLatest");
            TxtHeroTitle.Text = LocalizationService.Get("HomeTitle");
            TxtHeroDesc.Text = LocalizationService.Get("HomeSubtitle");
            BtnHeroLaunch.Content = "▶  " + LocalizationService.Get("BtnLaunchRoblox");

            LblStatFps.Text = LocalizationService.Get("StatFps");
            LblStatGfx.Text = LocalizationService.Get("StatGfx");
            LblStatSound.Text = LocalizationService.Get("StatSound");
            LblStatProto.Text = LocalizationService.Get("StatProtocol");

            LblSysInfo.Text = LocalizationService.Get("SysInfoTitle");
            LblRobloxDir.Text = LocalizationService.Get("RobloxDir");
            LblPlayerExe.Text = LocalizationService.Get("PlayerExe");

            // Performance Page
            LblFpsCardTitle.Text = LocalizationService.Get("FpsCardTitle");
            LblFpsCardDesc.Text = LocalizationService.Get("FpsCardDesc");
            LblTargetFps.Text = LocalizationService.Get("TargetFpsLabel");
            LblPresets.Text = LocalizationService.Get("PresetsLabel") + ":";
            BtnPresetUnlimited.Content = LocalizationService.Get("Unlimited") + " (0)";
            BtnSavePerf.Content = LocalizationService.Get("SaveSettings");

            // Graphics Page
            LblApiCardTitle.Text = LocalizationService.Get("ApiCardTitle");
            LblApiCardDesc.Text = LocalizationService.Get("ApiCardDesc");
            RadioApiDefault.Content = LocalizationService.Get("ApiAuto");
            RadioApiVulkan.Content = LocalizationService.Get("ApiVulkan");
            RadioApiD3d11.Content = LocalizationService.Get("ApiD3d11");

            LblMaxTexture.Text = LocalizationService.Get("MaxTexture");
            LblMaxTextureDesc.Text = LocalizationService.Get("MaxTextureDesc");
            LblDisablePostFx.Text = LocalizationService.Get("DisablePostFx");
            LblDisablePostFxDesc.Text = LocalizationService.Get("DisablePostFxDesc");
            LblClassicMenu.Text = LocalizationService.Get("ClassicMenu");
            LblClassicMenuDesc.Text = LocalizationService.Get("ClassicMenuDesc");
            BtnSaveGfx.Content = LocalizationService.Get("SaveSettings");

            // Sounds Page
            LblDeathSoundTitle.Text = LocalizationService.Get("DeathSoundCardTitle");
            LblDeathSoundDesc.Text = LocalizationService.Get("DeathSoundCardDesc");
            RadioSoundClassicOof.Content = LocalizationService.Get("SoundClassicOof");
            RadioSoundVineBoom.Content = LocalizationService.Get("SoundVineBoom");
            RadioSoundCustom.Content = LocalizationService.Get("SoundCustom");
            RadioSoundDefault.Content = LocalizationService.Get("SoundDefault");
            LblCustomSoundInstruction.Text = LocalizationService.Get("DeathSoundCardDesc");
            BtnBrowseCustomSound.Content = LocalizationService.Get("BrowseFile");
            BtnPreviewSound.Content = LocalizationService.Get("BtnPreview");
            BtnApplySound.Content = LocalizationService.Get("BtnApplySound");
            LblAudioEffectsTitle.Text = LocalizationService.Get("LblAudioEffectsTitle");
            LblAudioEffectsDesc.Text = LocalizationService.Get("LblAudioEffectsDesc");
            LblVolume.Text = LocalizationService.Get("LblVolume");
            LblSpeed.Text = LocalizationService.Get("LblSpeed");
            BtnResetAudio.Content = LocalizationService.Get("BtnResetAudio");

            // FastFlags Page
            LblAddFlagTitle.Text = LocalizationService.Get("AddFlagTitle");
            BtnAddFlag.Content = LocalizationService.Get("BtnAddFlag");
            LblRegisteredFlags.Text = LocalizationService.Get("FlagsTitle");

            // System Page
            LblSysLangTitle.Text = LocalizationService.Get("LangCardTitle");
            LblSysLangDesc.Text = LocalizationService.Get("LangCardDesc");
            LblProtocolTitle.Text = LocalizationService.Get("ProtocolCardTitle");
            LblProtocolDesc.Text = LocalizationService.Get("ProtocolCardDesc");
            LblShortcutTitle.Text = LocalizationService.Get("ShortcutCardTitle");
            LblShortcutDesc.Text = LocalizationService.Get("ShortcutCardDesc");
            BtnRecreateShortcuts.Content = LocalizationService.Get("BtnRecreateShortcuts");

            // Protocol Toggle Button
            bool isProto = RobloxLauncher.IsProtocolRegistered();
            TxtStatProtocol.Text = isProto ? "Active" : "Disabled";
            BtnProtocolToggle.Content = isProto ? LocalizationService.Get("BtnProtocolUnregister") : LocalizationService.Get("BtnProtocolRegister");
        }

        private void UpdateHeaderTitle()
        {
            if (PageHome == null || PagePerf == null || PageGfx == null || PageSounds == null || PageFlags == null || PageMods == null || PageSys == null)
                return;

            if (PageHome.Visibility == Visibility.Visible)
            {
                TxtHeaderTitle.Text = LocalizationService.Get("NavHome");
                TxtHeaderSubtitle.Text = LocalizationService.Get("HomeSubtitle");
            }
            else if (PagePerf.Visibility == Visibility.Visible)
            {
                TxtHeaderTitle.Text = LocalizationService.Get("NavPerf");
                TxtHeaderSubtitle.Text = LocalizationService.Get("PerfDesc");
            }
            else if (PageGfx.Visibility == Visibility.Visible)
            {
                TxtHeaderTitle.Text = LocalizationService.Get("NavGfx");
                TxtHeaderSubtitle.Text = LocalizationService.Get("GfxDesc");
            }
            else if (PageSounds.Visibility == Visibility.Visible)
            {
                TxtHeaderTitle.Text = LocalizationService.Get("NavSounds");
                TxtHeaderSubtitle.Text = LocalizationService.Get("SoundsDesc");
            }
            else if (PageFlags.Visibility == Visibility.Visible)
            {
                TxtHeaderTitle.Text = LocalizationService.Get("NavFlags");
                TxtHeaderSubtitle.Text = LocalizationService.Get("FlagsDesc");
            }
            else if (PageMods.Visibility == Visibility.Visible)
            {
                TxtHeaderTitle.Text = LocalizationService.Get("NavMods");
                TxtHeaderSubtitle.Text = LocalizationService.Get("ModsDesc");
            }
            else if (PageSys.Visibility == Visibility.Visible)
            {
                TxtHeaderTitle.Text = LocalizationService.Get("NavSys");
                TxtHeaderSubtitle.Text = LocalizationService.Get("SysDesc");
            }
        }

        private void RefreshAll()
        {
            // System info
            TxtVersionHash.Text = _paths.VersionHash ?? "Not detected";
            TxtRobloxDir.Text = _paths.RobloxDir;
            TxtPlayerExe.Text = _paths.PlayerExecutable ?? "Not detected";

            // Load FastFlag config
            var config = _ffManager.LoadConfig();

            // FPS
            int fps = config.TargetFps ?? 0;
            TxtTargetFps.Text = fps.ToString();
            TxtStatFps.Text = fps == 0 ? "UNLIMITED" : $"{fps} FPS";

            // Graphics API
            string api = config.GraphicsApi.ToLower();
            if (api == "vulkan")
            {
                RadioApiVulkan.IsChecked = true;
                TxtStatApi.Text = "Vulkan";
            }
            else if (api == "d3d11")
            {
                RadioApiD3d11.IsChecked = true;
                TxtStatApi.Text = "Direct3D 11";
            }
            else
            {
                RadioApiDefault.IsChecked = true;
                TxtStatApi.Text = "Auto";
            }

            // Graphics options
            SwitchTexture.IsChecked = config.MaxTextureQuality;
            SwitchPostFx.IsChecked = config.DisablePostFx;
            SwitchClassicMenu.IsChecked = config.ClassicEscMenu;

            // Protocol
            bool isProto = RobloxLauncher.IsProtocolRegistered();
            TxtStatProtocol.Text = isProto ? "Active" : "Disabled";
            BtnProtocolToggle.Content = isProto ? LocalizationService.Get("BtnProtocolUnregister") : LocalizationService.Get("BtnProtocolRegister");

            // Sound Config
            if (_prismConfig.DeathSoundType == "ClassicOof")
            {
                RadioSoundClassicOof.IsChecked = true;
                TxtStatSound.Text = "Classic OOF";
            }
            else if (_prismConfig.DeathSoundType == "VineBoom")
            {
                RadioSoundVineBoom.IsChecked = true;
                TxtStatSound.Text = "Vine Boom";
            }
            else if (_prismConfig.DeathSoundType == "Custom")
            {
                RadioSoundCustom.IsChecked = true;
                TxtStatSound.Text = "Custom MP3";
            }
            else
            {
                RadioSoundDefault.IsChecked = true;
                TxtStatSound.Text = "Default";
            }

            if (!string.IsNullOrEmpty(_prismConfig.CustomDeathSoundPath))
            {
                TxtCustomSoundPath.Text = _prismConfig.CustomDeathSoundPath;
            }
            else
            {
                TxtCustomSoundPath.Text = LocalizationService.Get("NoFileSelected");
            }

            // Audio sliders
            SliderVolume.Value = _prismConfig.DeathSoundVolume;
            SliderSpeed.Value = _prismConfig.DeathSoundSpeed;
            TxtVolumeVal.Text = $"{_prismConfig.DeathSoundVolume}%";
            TxtSpeedVal.Text = $"{_prismConfig.DeathSoundSpeed:0.00}x";

            // Flags list
            RefreshFlagsList();
        }

        private void RefreshFlagsList()
        {
            var rawFlags = _ffManager.LoadRawFlags();
            var list = new List<FlagItem>();
            foreach (var kv in rawFlags)
            {
                list.Add(new FlagItem
                {
                    Key = kv.Key,
                    Value = kv.Value?.ToString() ?? "null"
                });
            }

            ListFlags.ItemsSource = list;
            TxtFlagCount.Text = $"{list.Count} flags";
        }

        private void Nav_Checked(object sender, RoutedEventArgs e)
        {
            if (PageHome == null || PagePerf == null || PageGfx == null || PageSounds == null || PageFlags == null || PageMods == null || PageSys == null)
                return;

            PageHome.Visibility = Visibility.Collapsed;
            PagePerf.Visibility = Visibility.Collapsed;
            PageGfx.Visibility = Visibility.Collapsed;
            PageSounds.Visibility = Visibility.Collapsed;
            PageFlags.Visibility = Visibility.Collapsed;
            PageMods.Visibility = Visibility.Collapsed;
            PageSys.Visibility = Visibility.Collapsed;

            if (sender == NavHome) PageHome.Visibility = Visibility.Visible;
            else if (sender == NavPerf) PagePerf.Visibility = Visibility.Visible;
            else if (sender == NavGfx) PageGfx.Visibility = Visibility.Visible;
            else if (sender == NavSounds) PageSounds.Visibility = Visibility.Visible;
            else if (sender == NavFlags) PageFlags.Visibility = Visibility.Visible;
            else if (sender == NavMods) PageMods.Visibility = Visibility.Visible;
            else if (sender == NavSys) PageSys.Visibility = Visibility.Visible;

            UpdateHeaderTitle();
        }

        private void ComboLanguage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_isUpdatingUi) return;

            if (ComboLanguage.SelectedItem is ComboBoxItem item && item.Tag is string langCode)
            {
                _isUpdatingUi = true;
                _prismConfig.Language = langCode;
                PrismConfigManager.Save(_prismConfig);
                LocalizationService.SetLanguage(langCode);
                SelectComboLanguage(ComboSysLanguage, langCode);
                _isUpdatingUi = false;
                ShowToast(LocalizationService.Get("ToastSettingsSaved"));
            }
        }

        private void ComboSysLanguage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_isUpdatingUi) return;

            if (ComboSysLanguage.SelectedItem is ComboBoxItem item && item.Tag is string langCode)
            {
                _isUpdatingUi = true;
                _prismConfig.Language = langCode;
                PrismConfigManager.Save(_prismConfig);
                LocalizationService.SetLanguage(langCode);
                SelectComboLanguage(ComboLanguage, langCode);
                _isUpdatingUi = false;
                ShowToast(LocalizationService.Get("ToastSettingsSaved"));
            }
        }

        private void SoundRadio_Checked(object sender, RoutedEventArgs e)
        {
            if (PanelCustomSound == null) return;
            PanelCustomSound.Visibility = RadioSoundCustom.IsChecked == true ? Visibility.Visible : Visibility.Collapsed;
        }

        private void BtnBrowseCustomSound_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Filter = "MP3 Audio (*.mp3)|*.mp3|All Files (*.*)|*.*",
                Title = LocalizationService.Get("BrowseFile")
            };

            if (dialog.ShowDialog() == true)
            {
                TxtCustomSoundPath.Text = dialog.FileName;
                _prismConfig.CustomDeathSoundPath = dialog.FileName;
                PrismConfigManager.Save(_prismConfig);
            }
        }

        private void SliderAudio_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (TxtVolumeVal == null || TxtSpeedVal == null || SliderVolume == null || SliderSpeed == null) return;
            int vol = (int)Math.Round(SliderVolume.Value);
            double spd = Math.Round(SliderSpeed.Value * 20.0) / 20.0;
            TxtVolumeVal.Text = $"{vol}%";
            TxtSpeedVal.Text = $"{spd:0.00}x";
        }

        private void BtnResetAudio_Click(object sender, RoutedEventArgs e)
        {
            if (SliderVolume == null || SliderSpeed == null) return;
            SliderVolume.Value = 100;
            SliderSpeed.Value = 1.0;
        }

        private void BtnPreviewSound_Click(object sender, RoutedEventArgs e)
        {
            string soundType = "ClassicOof";
            if (RadioSoundVineBoom.IsChecked == true) soundType = "VineBoom";
            else if (RadioSoundCustom.IsChecked == true) soundType = "Custom";
            else if (RadioSoundDefault.IsChecked == true) soundType = "Default";

            string customPath = TxtCustomSoundPath.Text;
            if (customPath == LocalizationService.Get("NoFileSelected")) customPath = string.Empty;

            int vol = (int)Math.Round(SliderVolume.Value);
            double spd = Math.Round(SliderSpeed.Value * 20.0) / 20.0;

            DeathSoundManager.PlayPreview(soundType, customPath, vol, spd);
            ShowToast(LocalizationService.Get("SoundPlaying"));
        }

        private void BtnApplySound_Click(object sender, RoutedEventArgs e)
        {
            string soundType = "ClassicOof";
            if (RadioSoundVineBoom.IsChecked == true) soundType = "VineBoom";
            else if (RadioSoundCustom.IsChecked == true) soundType = "Custom";
            else if (RadioSoundDefault.IsChecked == true) soundType = "Default";

            string customPath = TxtCustomSoundPath.Text;
            if (customPath == LocalizationService.Get("NoFileSelected")) customPath = string.Empty;

            if (soundType == "Custom" && (string.IsNullOrEmpty(customPath) || !File.Exists(customPath)))
            {
                System.Windows.MessageBox.Show("有効なMP3ファイルを選択してください。", "PrismStrap", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            int vol = (int)Math.Round(SliderVolume.Value);
            double spd = Math.Round(SliderSpeed.Value * 20.0) / 20.0;

            _prismConfig.DeathSoundType = soundType;
            _prismConfig.CustomDeathSoundPath = customPath;
            _prismConfig.DeathSoundVolume = vol;
            _prismConfig.DeathSoundSpeed = spd;
            PrismConfigManager.Save(_prismConfig);

            bool ok = DeathSoundManager.ApplyDeathSound(_paths.CurrentVersionDir, soundType, customPath, vol, spd);
            if (ok)
            {
                RefreshAll();
                ShowToast(LocalizationService.Get("ToastSoundApplied"));
            }
            else
            {
                System.Windows.MessageBox.Show("サウンドの適用に失敗しました。Robloxが起動中の場合は終了してから再試行してください。", "PrismStrap", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnPresetFps_Click(object sender, RoutedEventArgs e)
        {
            if (sender is System.Windows.Controls.Button btn && btn.Tag != null)
            {
                TxtTargetFps.Text = btn.Tag.ToString();
            }
        }

        private void BtnSaveSettings_Click(object sender, RoutedEventArgs e)
        {
            int fps = 0;
            int.TryParse(TxtTargetFps.Text, out fps);

            string api = "default";
            if (RadioApiVulkan.IsChecked == true) api = "vulkan";
            else if (RadioApiD3d11.IsChecked == true) api = "d3d11";

            var config = new FastFlagConfig
            {
                TargetFps = fps,
                GraphicsApi = api,
                MaxTextureQuality = SwitchTexture.IsChecked == true,
                DisablePostFx = SwitchPostFx.IsChecked == true,
                ClassicEscMenu = SwitchClassicMenu.IsChecked == true
            };

            _ffManager.SaveConfig(config);
            RefreshAll();
            ShowToast(LocalizationService.Get("ToastSettingsSaved"));
        }

        private void BtnAddFlag_Click(object sender, RoutedEventArgs e)
        {
            var key = TxtNewFlagKey.Text.Trim();
            var val = TxtNewFlagVal.Text.Trim();
            if (string.IsNullOrEmpty(key)) return;

            _ffManager.SetFlag(key, val);
            TxtNewFlagKey.Text = string.Empty;
            TxtNewFlagVal.Text = string.Empty;

            RefreshFlagsList();
            ShowToast(LocalizationService.Get("ToastFlagAdded") + $": '{key}'");
        }

        private void BtnDeleteFlag_Click(object sender, RoutedEventArgs e)
        {
            if (sender is System.Windows.Controls.Button btn && btn.Tag is string key)
            {
                _ffManager.RemoveFlag(key);
                RefreshFlagsList();
                ShowToast(LocalizationService.Get("ToastFlagRemoved") + $": '{key}'");
            }
        }

        private void BtnLaunchRoblox_Click(object sender, RoutedEventArgs e)
        {
            // Auto-save settings first
            BtnSaveSettings_Click(sender, e);

            // Open launch loading window
            var loadingWin = new LoadingWindow(Array.Empty<string>());
            loadingWin.Owner = this;
            loadingWin.Show();
        }

        private async void BtnCheckUpdates_Click(object sender, RoutedEventArgs e)
        {
            ShowToast(LocalizationService.Get("ToastUpdateChecking"));
            TxtStatusBadge.Text = LocalizationService.Get("StatusChecking");

            var latest = await RobloxInstaller.FetchLatestVersionAsync();
            if (latest == null)
            {
                TxtStatusBadge.Text = LocalizationService.Get("StatusError");
                ShowToast(LocalizationService.Get("ToastUpdateFailed"));
                return;
            }

            bool isInstalled = RobloxInstaller.IsInstalled(_paths.RobloxDir, latest.ClientVersionUpload);
            if (!isInstalled)
            {
                var res = System.Windows.MessageBox.Show(
                    LocalizationService.Get("DialogNewVersionFound") + $"\n({latest.ClientVersionUpload})",
                    LocalizationService.Get("DialogNewVersionTitle"),
                    System.Windows.MessageBoxButton.YesNo,
                    MessageBoxImage.Information);

                if (res == System.Windows.MessageBoxResult.Yes)
                {
                    TxtStatusBadge.Text = LocalizationService.Get("StatusDownloading");
                    var progress = new Progress<string>(msg =>
                    {
                        TxtStatusBadge.Text = msg;
                    });

                    bool ok = await RobloxInstaller.InstallVersionAsync(_paths.RobloxDir, latest.ClientVersionUpload, progress);
                    if (ok)
                    {
                        // Ensure death sound applied to new version
                        var newPaths = new RobloxPaths();
                        DeathSoundManager.ApplyDeathSound(newPaths.CurrentVersionDir, _prismConfig.DeathSoundType, _prismConfig.CustomDeathSoundPath, _prismConfig.DeathSoundVolume, _prismConfig.DeathSoundSpeed);

                        ShowToast(LocalizationService.Get("ToastUpdateSuccess"));
                        TxtStatusBadge.Text = LocalizationService.Get("StatusLatest");
                        RefreshAll();
                    }
                    else
                    {
                        ShowToast(LocalizationService.Get("ToastUpdateFailed"));
                        TxtStatusBadge.Text = LocalizationService.Get("StatusError");
                    }
                }
            }
            else
            {
                ShowToast(LocalizationService.Get("ToastUpdateLatest"));
                TxtStatusBadge.Text = LocalizationService.Get("StatusLatest");
            }
        }

        private void BtnProtocolToggle_Click(object sender, RoutedEventArgs e)
        {
            bool isProto = RobloxLauncher.IsProtocolRegistered();
            if (isProto)
            {
                RobloxLauncher.UnregisterProtocol();
                ShowToast(LocalizationService.Get("ToastProtocolUnregistered"));
            }
            else
            {
                RobloxLauncher.RegisterProtocol();
                ShowToast(LocalizationService.Get("ToastProtocolRegistered"));
            }
            RefreshAll();
        }

        private void BtnRecreateShortcuts_Click(object sender, RoutedEventArgs e)
        {
            AppSetup.Install();
            ShowToast(LocalizationService.Get("ToastShortcutsRecreated"));
        }

        private void ShowToast(string message)
        {
            TxtToast.Text = message;
            ToastNotification.Visibility = Visibility.Visible;

            _toastTimer?.Stop();
            _toastTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(2.5) };
            _toastTimer.Tick += (s, ev) =>
            {
                ToastNotification.Visibility = Visibility.Collapsed;
                _toastTimer.Stop();
            };
            _toastTimer.Start();
        }

        // ====================================================================
        // APPEARANCE & MODS LOGIC
        // ====================================================================
        private void InitAppearanceModsUI()
        {
            try
            {
                AppearanceModManager.EnsureAssetsExtracted();

                // Set image sources for icons
                SetImageSource(ImgIcon2022, AppearanceModManager.GetIconPreviewPath("Modern2022"));
                SetImageSource(ImgIcon2017, AppearanceModManager.GetIconPreviewPath("Tilted2017"));
                SetImageSource(ImgIcon2015, AppearanceModManager.GetIconPreviewPath("RedSquare2015"));
                SetImageSource(ImgIcon2008, AppearanceModManager.GetIconPreviewPath("Classic2008"));
                SetImageSource(ImgIconPrism, AppearanceModManager.GetIconPreviewPath("PrismNeon"));

                // Select Font
                SelectComboByTag(ComboCustomFont, _prismConfig.CustomFontType);
                if (BtnBrowseFont != null)
                {
                    BtnBrowseFont.Visibility = (_prismConfig.CustomFontType == "Custom") ? Visibility.Visible : Visibility.Collapsed;
                }
                UpdateFontPreview();

                // Select Cursor
                SelectComboByTag(ComboCursor, _prismConfig.CursorType);
                if (BtnBrowseCursor != null)
                {
                    BtnBrowseCursor.Visibility = (_prismConfig.CursorType == "Custom") ? Visibility.Visible : Visibility.Collapsed;
                }
                UpdateCursorPreview();

                // Select Icon Radio
                if (RadioIcon2017 != null && RadioIcon2015 != null && RadioIcon2008 != null && RadioIconPrism != null && RadioIcon2022 != null)
                {
                    switch (_prismConfig.AppIconType)
                    {
                        case "Tilted2017": RadioIcon2017.IsChecked = true; break;
                        case "RedSquare2015": RadioIcon2015.IsChecked = true; break;
                        case "Classic2008": RadioIcon2008.IsChecked = true; break;
                        case "PrismNeon": RadioIconPrism.IsChecked = true; break;
                        default: RadioIcon2022.IsChecked = true; break;
                    }
                }
            }
            catch { }
        }

        private static void SetImageSource(Image img, string path)
        {
            if (img == null || !File.Exists(path)) return;
            try
            {
                var bmp = new BitmapImage();
                bmp.BeginInit();
                bmp.CacheOption = BitmapCacheOption.OnLoad;
                bmp.UriSource = new Uri(path, UriKind.Absolute);
                bmp.EndInit();
                img.Source = bmp;
            }
            catch { }
        }

        private static void SelectComboByTag(ComboBox combo, string tagValue)
        {
            if (combo == null) return;
            foreach (ComboBoxItem item in combo.Items)
            {
                if (item.Tag is string tag && tag.Equals(tagValue, StringComparison.OrdinalIgnoreCase))
                {
                    combo.SelectedItem = item;
                    return;
                }
            }
            if (combo.Items.Count > 0) combo.SelectedIndex = 0;
        }

        private void ComboCustomFont_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_isUpdatingUi) return;
            if (BtnBrowseFont != null && ComboCustomFont?.SelectedItem is ComboBoxItem item && item.Tag is string tag)
            {
                BtnBrowseFont.Visibility = (tag == "Custom") ? Visibility.Visible : Visibility.Collapsed;
                UpdateFontPreview();
            }
        }

        private void BtnBrowseFont_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new OpenFileDialog
            {
                Title = "カスタムフォントを選択",
                Filter = "フォントファイル (*.ttf;*.otf)|*.ttf;*.otf|すべてのファイル (*.*)|*.*"
            };

            if (dlg.ShowDialog() == true)
            {
                if (_prismConfig != null) _prismConfig.CustomFontPath = dlg.FileName;
                UpdateFontPreview();
                ShowToast($"フォントを選択: {Path.GetFileName(dlg.FileName)}");
            }
        }

        private void UpdateFontPreview()
        {
            if (TxtFontPreview == null || _prismConfig == null || ComboCustomFont?.SelectedItem is not ComboBoxItem item) return;

            var tag = item.Tag as string ?? "Default";
            try
            {
                if (tag == "ComicSans")
                {
                    TxtFontPreview.FontFamily = new FontFamily("Comic Sans MS");
                }
                else if (tag == "SegoeUI")
                {
                    TxtFontPreview.FontFamily = new FontFamily("Segoe UI Variable Text, Segoe UI");
                }
                else if (tag == "Arial")
                {
                    TxtFontPreview.FontFamily = new FontFamily("Arial");
                }
                else if (tag == "Custom" && !string.IsNullOrEmpty(_prismConfig.CustomFontPath) && File.Exists(_prismConfig.CustomFontPath))
                {
                    var dir = Path.GetDirectoryName(_prismConfig.CustomFontPath);
                    var file = Path.GetFileName(_prismConfig.CustomFontPath);
                    TxtFontPreview.FontFamily = new FontFamily(new Uri($"file:///{dir}/"), $"./#{file}");
                }
                else
                {
                    TxtFontPreview.FontFamily = new FontFamily("Segoe UI");
                }
            }
            catch { }
        }

        private void ComboCursor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_isUpdatingUi) return;
            if (BtnBrowseCursor != null && ComboCursor?.SelectedItem is ComboBoxItem item && item.Tag is string tag)
            {
                BtnBrowseCursor.Visibility = (tag == "Custom") ? Visibility.Visible : Visibility.Collapsed;
                UpdateCursorPreview();
            }
        }

        private void BtnBrowseCursor_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new OpenFileDialog
            {
                Title = "カスタムカーソル画像を選択",
                Filter = "PNG画像 (*.png)|*.png|すべてのファイル (*.*)|*.*"
            };

            if (dlg.ShowDialog() == true)
            {
                if (_prismConfig != null) _prismConfig.CustomCursorPath = dlg.FileName;
                UpdateCursorPreview();
                ShowToast($"カーソル画像を選択: {Path.GetFileName(dlg.FileName)}");
            }
        }

        private void UpdateCursorPreview()
        {
            if (ImgCursorPreview == null || _prismConfig == null || ComboCursor?.SelectedItem is not ComboBoxItem item) return;

            var tag = item.Tag as string ?? "Default";
            if (TxtCursorPreviewName != null)
            {
                TxtCursorPreviewName.Text = item.Content?.ToString() ?? tag;
            }

            try
            {
                if (tag == "Custom" && !string.IsNullOrEmpty(_prismConfig.CustomCursorPath) && File.Exists(_prismConfig.CustomCursorPath))
                {
                    SetImageSource(ImgCursorPreview, _prismConfig.CustomCursorPath);
                }
                else
                {
                    var previewPath = AppearanceModManager.GetCursorPreviewPath(tag);
                    SetImageSource(ImgCursorPreview, previewPath);
                }
            }
            catch { }
        }

        private void RadioIcon_Checked(object sender, RoutedEventArgs e)
        {
            if (_isUpdatingUi || _prismConfig == null) return;
            if (sender is RadioButton rb && rb.Tag is string tag)
            {
                _prismConfig.AppIconType = tag;
            }
        }

        private void BtnApplyIconNow_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                AppearanceModManager.ApplyAppIcon(_prismConfig.AppIconType);
                PrismConfigManager.Save(_prismConfig);
                ShowToast(LocalizationService.Get("IconAppliedSuccess"));
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"アイコン適用エラー:\n{ex.Message}", "PrismStrap", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void BtnSaveMods_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ComboCustomFont?.SelectedItem is ComboBoxItem fontItem && fontItem.Tag is string fontTag)
                {
                    _prismConfig.CustomFontType = fontTag;
                }

                if (ComboCursor?.SelectedItem is ComboBoxItem cursorItem && cursorItem.Tag is string cursorTag)
                {
                    _prismConfig.CursorType = cursorTag;
                }

                PrismConfigManager.Save(_prismConfig);

                // If Roblox is installed, apply directly
                if (!string.IsNullOrEmpty(_paths.CurrentVersionDir))
                {
                    AppearanceModManager.ApplyAllMods(_paths.CurrentVersionDir, _prismConfig);
                }

                ShowToast(LocalizationService.Get("ToastSettingsSaved"));
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"設定保存エラー:\n{ex.Message}", "PrismStrap", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}