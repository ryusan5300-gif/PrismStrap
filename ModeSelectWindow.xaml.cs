using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using PrismStrap.Services;

namespace PrismStrap
{
    public partial class ModeSelectWindow : Window
    {
        private static readonly HttpClient _httpClient = new HttpClient();
        private System.Windows.Threading.DispatcherTimer? _statsTimer;
        private long _cachedDownloads = 0;

        public ModeSelectWindow()
        {
            InitializeComponent();
            Loaded += ModeSelectWindow_Loaded;
            Closed += ModeSelectWindow_Closed;
        }

        private void ModeSelectWindow_Loaded(object sender, RoutedEventArgs e)
        {
            TxtSubtitle.Text = LocalizationService.Get("PromptSubtitle");
            TxtPlayTitle.Text = LocalizationService.Get("PromptPlayRoblox");
            TxtPlayDesc.Text = LocalizationService.Get("PromptPlayRobloxDesc");
            TxtSettingsTitle.Text = LocalizationService.Get("PromptOpenSettings");
            TxtSettingsDesc.Text = LocalizationService.Get("PromptOpenSettingsDesc");

            // Initial check based on actual running process
            RefreshLocalAndRemoteStats();

            // Set up 2-second timer to track Roblox process state in real time
            _statsTimer = new System.Windows.Threading.DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(2)
            };
            _statsTimer.Tick += (s, ev) => RefreshLocalAndRemoteStats();
            _statsTimer.Start();
        }

        private void ModeSelectWindow_Closed(object? sender, EventArgs e)
        {
            _statsTimer?.Stop();
            _statsTimer = null;
        }

        private void RefreshLocalAndRemoteStats()
        {
            bool isRunning = System.Diagnostics.Process.GetProcessesByName("RobloxPlayerBeta").Length > 0;
            long onlineCount = isRunning ? 1 : 0;
            long offlineCount = Math.Max(0, _cachedDownloads - onlineCount);

            UpdateStatsUI(onlineCount, offlineCount, _cachedDownloads);

            // Synchronize with remote CountAPI in background
            _ = SyncCountApiAsync(isRunning);
        }

        private void UpdateStatsUI(long online, long offline, long downloads)
        {
            var playFmt = LocalizationService.Get("LivePlayingFormat");
            var offFmt = LocalizationService.Get("LiveOfflineFormat");
            var dlFmt = LocalizationService.Get("LiveDownloadsFormat");

            TxtLiveOnline.Text = string.Format(string.IsNullOrEmpty(playFmt) ? "プレイ中: {0} 人" : playFmt, online.ToString("N0"));
            TxtLiveOffline.Text = string.Format(string.IsNullOrEmpty(offFmt) ? "オフライン: {0} 人" : offFmt, offline.ToString("N0"));
            TxtLiveDownloads.Text = string.Format(string.IsNullOrEmpty(dlFmt) ? "🚀 {0} DL" : dlFmt, downloads.ToString("N0"));
        }

        private async Task SyncCountApiAsync(bool isRunning)
        {
            try
            {
                // Sync current state to server
                var targetState = isRunning ? 1 : 0;
                await _httpClient.GetAsync($"https://countapi.mileshilliard.com/api/v1/set/prismstrap_v1_active_players?value={targetState}");

                // Fetch latest downloads count
                var dlJson = await _httpClient.GetStringAsync("https://countapi.mileshilliard.com/api/v1/get/prismstrap_v1_total_downloads");
                using (var doc = JsonDocument.Parse(dlJson))
                {
                    if (doc.RootElement.TryGetProperty("value", out var v))
                    {
                        _cachedDownloads = v.GetInt64();
                        Dispatcher.Invoke(() =>
                        {
                            long online = isRunning ? 1 : 0;
                            long offline = Math.Max(0, _cachedDownloads - online);
                            UpdateStatsUI(online, offline, _cachedDownloads);
                        });
                    }
                }
            }
            catch { }
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                try { DragMove(); } catch { }
            }
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void BtnPlayRoblox_Click(object sender, RoutedEventArgs e)
        {
            var loadingWin = new LoadingWindow(Array.Empty<string>());
            loadingWin.Show();
            Close();
        }

        private void BtnOpenSettings_Click(object sender, RoutedEventArgs e)
        {
            var mainWin = new MainWindow();
            mainWin.Show();
            Close();
        }
    }
}
