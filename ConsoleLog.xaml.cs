using Microsoft.UI.Xaml;
using System;
using System.Diagnostics;
using System.IO;

namespace DaemonRecorder {
    public sealed partial class ConsoleLog : Window {
        private FileSystemWatcher watcher;

        public ConsoleLog() {
            this.InitializeComponent();
            this.AppWindow.Resize(new(800, 500));
            this.Title = "Console Log";

            watcher = AppLog.Watch();
            watcher.Changed += (object sender, FileSystemEventArgs e) => {
                DispatcherQueue.TryEnqueue(() => UpdateLog());

                Debug.WriteLine($"{e.Name} {e.ChangeType}");
            };

            this.Closed += (object sender, WindowEventArgs e) => {
                AppLog.Write("Console Log Window Closed");
                watcher.Dispose();
            };

            UpdateLog();

            AppLog.Write("Console Log Window Opened");
        }

        public void UpdateLog() {
            Log.IsReadOnly = false;
            string content = AppLog.Read();
            // get last 1000 characters
            if (content.Length > 1000) {
                content = content.Substring(content.Length - 1000);
            }

            Log.Document.SetText(Microsoft.UI.Text.TextSetOptions.None, content);
            Log.IsReadOnly = true;

            Log.Document.Selection.StartPosition = content.Length;
        }

        public void LogMessage(string message) {
            AppLog.Write(message);
        }

        async public void OpenFolder() {
            var uri = new Uri(AppLog.folder);
            AppLog.Write($"Opening Folder {uri}...");
            await Windows.System.Launcher.LaunchUriAsync(uri);

        }
    }
}
