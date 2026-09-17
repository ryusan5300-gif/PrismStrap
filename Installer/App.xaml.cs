using System;
using System.Windows;

namespace PrismStrap.Installer
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            AppDomain.CurrentDomain.UnhandledException += (s, ev) =>
            {
                var err = ev.ExceptionObject?.ToString() ?? "Unknown error";
                MessageBox.Show($"Setup Error:\n{err}", "PrismStrap Setup", MessageBoxButton.OK, MessageBoxImage.Error);
            };

            base.OnStartup(e);
        }
    }
}
