using Microsoft.UI.Xaml;
using System;

namespace DaemonRecorder {
    public partial class MainWindow : Window {
        public int counter = 0;
        public MainWindow _instance;
        public string logFile = "";

        public MainWindow() {
            this.InitializeComponent();
            _instance = this;

            this.AppWindow.Resize(new Windows.Graphics.SizeInt32(800, 700));
        }

        private void DebugButton_Click(object sender, RoutedEventArgs e) {
            string message = $"Clicked {counter} times";

            counter++;

            UpdateStatus(message);
            LogMessage(message);
        }

        private void AudioSettings_Click(object sender, RoutedEventArgs e) {
            var audioSettings = new AudioSettings();
            audioSettings.Activate();
        }

        private void LaunchUI_Click(object sender, RoutedEventArgs e) {
            var uri = new Uri("https://elijahlucian.ca/experiments");
            var success = Windows.System.Launcher.LaunchUriAsync(uri);

            LogMessage($"Launched {uri} with success: {success}");
        }

        public void UpdateStatus(string message) {
            status.Text = message;
        }

        public void LogMessage(string message) {
            AppLog.Write(message);
            consoleLog.IsReadOnly = false;
            string content = $"{message}\n{logFile}";
            consoleLog.Document.SetText(Microsoft.UI.Text.TextSetOptions.None, content);
            logFile = content;
            consoleLog.IsReadOnly = true;
        }
    }
}
