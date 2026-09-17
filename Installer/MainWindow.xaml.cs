using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PrismStrap.Installer
{
    public partial class MainWindow : Window
    {
        private bool _isUpdatingUi = false;

        public MainWindow()
        {
            InitializeComponent();
            TxtInstallDir.Text = InstallerEngine.GetDefaultInstallDir();

            PopulateLanguageCombo();
            UpdateLocalization();
        }

        private void PopulateLanguageCombo()
        {
            _isUpdatingUi = true;
            ComboLang.Items.Clear();
            foreach (var (code, label) in InstallerLocalization.SupportedLanguages)
            {
                var item = new ComboBoxItem
                {
                    Content = label,
                    Tag = code,
                    Style = (Style)FindResource("DarkComboBoxItemStyle")
                };
                ComboLang.Items.Add(item);
                if (code == InstallerLocalization.CurrentLanguage)
                {
                    ComboLang.SelectedItem = item;
                }
            }
            _isUpdatingUi = false;
        }

        private void UpdateLocalization()
        {
            TxtTitle.Text = InstallerLocalization.Get("SetupTitle");
            TxtWelcomeHeader.Text = InstallerLocalization.Get("WelcomeHeader");
            TxtWelcomeDesc.Text = InstallerLocalization.Get("WelcomeDesc");
            TxtInstallDirLabel.Text = InstallerLocalization.Get("InstallDirLabel");
            BtnBrowse.Content = InstallerLocalization.Get("Browse");
            ChkDesktop.Content = InstallerLocalization.Get("OptShortcuts");
            ChkStartMenu.Content = InstallerLocalization.Get("OptStartMenu");
            ChkProtocol.Content = InstallerLocalization.Get("OptProtocol");
            BtnCancel.Content = InstallerLocalization.Get("BtnCancel");
            BtnInstall.Content = InstallerLocalization.Get("BtnInstall");

            TxtProgressStatus.Text = InstallerLocalization.Get("StatusExtracting");
            TxtFinishHeader.Text = InstallerLocalization.Get("FinishTitle");
            TxtFinishDesc.Text = InstallerLocalization.Get("FinishDesc");
            ChkLaunchNow.Content = InstallerLocalization.Get("LaunchNow");
            BtnFinish.Content = InstallerLocalization.Get("BtnFinish");
        }

        private void ComboLang_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_isUpdatingUi) return;
            if (ComboLang.SelectedItem is ComboBoxItem item && item.Tag is string langCode)
            {
                InstallerLocalization.SetLanguage(langCode);
                UpdateLocalization();
            }
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

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void BtnBrowse_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new Microsoft.Win32.OpenFolderDialog
            {
                Title = InstallerLocalization.Get("InstallDirLabel")
            };

            if (dlg.ShowDialog() == true)
            {
                TxtInstallDir.Text = Path.Combine(dlg.FolderName, "PrismStrap");
            }
        }

        private async void BtnInstall_Click(object sender, RoutedEventArgs e)
        {
            PanelConfig.Visibility = Visibility.Collapsed;
            PanelProgress.Visibility = Visibility.Visible;
            ComboLang.IsEnabled = false;
            BtnClose.IsEnabled = false;

            var progress = new Progress<string>(msg =>
            {
                Dispatcher.Invoke(() => TxtProgressStatus.Text = msg);
            });

            string installDir = TxtInstallDir.Text.Trim();
            bool createDesktop = ChkDesktop.IsChecked == true;
            bool createStartMenu = ChkStartMenu.IsChecked == true;
            bool registerProtocol = ChkProtocol.IsChecked == true;

            bool success = await InstallerEngine.InstallAsync(installDir, createDesktop, createStartMenu, registerProtocol, progress);

            PanelProgress.Visibility = Visibility.Collapsed;
            PanelFinish.Visibility = Visibility.Visible;
            BtnClose.IsEnabled = true;

            if (!success)
            {
                TxtFinishHeader.Text = "エラーが発生しました";
                TxtFinishDesc.Text = "インストールの実行中に問題が発生しました。権限や空き容量をご確認ください。";
            }
        }

        private void BtnFinish_Click(object sender, RoutedEventArgs e)
        {
            if (ChkLaunchNow.IsChecked == true)
            {
                var targetExe = Path.Combine(TxtInstallDir.Text.Trim(), "PrismStrap.exe");
                if (File.Exists(targetExe))
                {
                    try
                    {
                        Process.Start(new ProcessStartInfo
                        {
                            FileName = targetExe,
                            UseShellExecute = true
                        });
                    }
                    catch { }
                }
            }

            Close();
        }
    }
}
