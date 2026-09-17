using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using PrismStrap.Services;

namespace PrismStrap
{
    public partial class LoadingWindow : Window
    {
        private readonly string[] _launchArgs;
        private readonly CancellationTokenSource _cts = new();
        private bool _isCompleted = false;

        public LoadingWindow(string[]? launchArgs = null)
        {
            InitializeComponent();
            _launchArgs = launchArgs ?? Array.Empty<string>();

            Loaded += LoadingWindow_Loaded;
            Closed += LoadingWindow_Closed;
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                try { DragMove(); } catch { }
            }
        }

        private async void LoadingWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Apply localized text
            TxtLoadingTitle.Text = LocalizationService.Get("LoadingStartingRoblox");
            TxtLoadingDesc.Text = LocalizationService.Get("LoadingApplyingMods");
            BtnCancel.Content = LocalizationService.Get("LoadingCancel");

            await StartLaunchSequenceAsync();
        }

        private async Task StartLaunchSequenceAsync()
        {
            try
            {
                var token = _cts.Token;

                await Task.Run(async () =>
                {
                    // 1. Locate Roblox
                    var paths = new RobloxPaths();
                    if (string.IsNullOrEmpty(paths.PlayerExecutable) || !File.Exists(paths.PlayerExecutable))
                    {
                        // Roblox not found or needs install
                        await Dispatcher.InvokeAsync(() =>
                        {
                            TxtLoadingDesc.Text = LocalizationService.Get("DialogExeNotFound");
                        });

                        var latest = await RobloxInstaller.FetchLatestVersionAsync();
                        if (latest != null && !string.IsNullOrEmpty(latest.ClientVersionUpload))
                        {
                            var progress = new Progress<string>(msg =>
                            {
                                Dispatcher.InvokeAsync(() => TxtLoadingDesc.Text = msg);
                            });
                            await RobloxInstaller.InstallVersionAsync(paths.RobloxDir, latest.ClientVersionUpload, progress);
                        }
                        else
                        {
                            await Task.Delay(1500, token);
                            return;
                        }

                        paths = new RobloxPaths();
                    }

                    if (token.IsCancellationRequested || string.IsNullOrEmpty(paths.PlayerExecutable))
                        return;

                    // 2. Apply Mods / Death Sounds & Appearance Mods
                    var cfg = PrismConfigManager.Load();
                    var dir = Path.GetDirectoryName(paths.PlayerExecutable);
                    if (!string.IsNullOrEmpty(dir))
                    {
                        DeathSoundManager.ApplyDeathSound(dir, cfg.DeathSoundType, cfg.CustomDeathSoundPath, cfg.DeathSoundVolume, cfg.DeathSoundSpeed);
                        AppearanceModManager.ApplyAllMods(dir, cfg);
                    }

                    if (token.IsCancellationRequested) return;

                    // 3. Update description before launch
                    await Dispatcher.InvokeAsync(() =>
                    {
                        TxtLoadingDesc.Text = LocalizationService.Get("LoadingWaitingForRoblox");
                    });

                    // 4. Launch Roblox
                    bool launched = RobloxLauncher.Launch(paths.PlayerExecutable, _launchArgs);
                    if (!launched)
                    {
                        await Task.Delay(1000, token);
                        return;
                    }

                    // 5. Monitor for RobloxPlayerBeta process start
                    var sw = Stopwatch.StartNew();
                    while (!token.IsCancellationRequested && sw.ElapsedMilliseconds < 25000)
                    {
                        var procs = Process.GetProcessesByName("RobloxPlayerBeta");
                        if (procs.Length > 0)
                        {
                            var targetProc = procs[0];

                            // Track genuine active session: 1 while playing, 0 when exited
                            _ = Task.Run(async () =>
                            {
                                try
                                {
                                    using var client = new System.Net.Http.HttpClient { Timeout = TimeSpan.FromSeconds(5) };
                                    // Set to 1 when Roblox starts
                                    await client.GetAsync("https://countapi.mileshilliard.com/api/v1/set/prismstrap_v1_active_players?value=1");

                                    // Wait until Roblox is closed
                                    targetProc.WaitForExit();

                                    // Reset to 0 when Roblox exits
                                    await client.GetAsync("https://countapi.mileshilliard.com/api/v1/set/prismstrap_v1_active_players?value=0");
                                }
                                catch { }
                            });

                            // Wait a short moment for the window to emerge smoothly
                            await Task.Delay(1000, token);
                            _isCompleted = true;
                            break;
                        }

                        await Task.Delay(300, token);
                    }
                }, token);
            }
            catch (OperationCanceledException)
            {
                // Launch cancelled by user
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[LoadingWindow] Error during launch sequence: {ex.Message}");
            }
            finally
            {
                // Close loading window
                try
                {
                    Close();
                }
                catch { }
            }
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            CancelAndClose();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            CancelAndClose();
        }

        private void CancelAndClose()
        {
            if (!_isCompleted)
            {
                try
                {
                    _cts.Cancel();
                }
                catch { }
            }
            Close();
        }

        private void LoadingWindow_Closed(object? sender, EventArgs e)
        {
            try
            {
                _cts.Cancel();
                _cts.Dispose();
            }
            catch { }

            // If no other windows are visible, exit application
            var hasOtherVisibleWindows = Application.Current.Windows
                .Cast<Window>()
                .Any(w => w != this && w.IsVisible);

            if (!hasOtherVisibleWindows)
            {
                Application.Current.Shutdown();
            }
        }
    }
}
