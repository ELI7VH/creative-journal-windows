using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using System;
using Windows.UI;

namespace DaemonRecorder {
    public partial class MainWindow : Window {
        public int counter = 0;
        public MainWindow _instance;
        public string logFile = "";

        public MainWindow() {
            this.InitializeComponent();
            _instance = this;

            this.AppWindow.Resize(new Windows.Graphics.SizeInt32(800, 400));

            this.Title = "Daemon Recorder";
        }

        private void DebugButton_Click(object sender, RoutedEventArgs e) {
            string message = $"Clicked {counter} times";

            counter++;

            UpdateStatus(message);
            AppLog.Write(message);
        }

        private void AudioSettingsButton_Click(object sender, RoutedEventArgs e) {
            var audioSettings = new AudioSettings();
            audioSettings.Activate();
        }

        private void ConsoleLogButton_Click(object sender, RoutedEventArgs e) {
            var consoleLog = new ConsoleLog();
            consoleLog.Activate();
        }

        private void SongPlayerButton_Click(object sender, RoutedEventArgs e) {
            var songPlayer = new SongPlayer();
            songPlayer.Activate();
        }

        private void LaunchUI_Click(object sender, RoutedEventArgs e) {
            var uri = new Uri("https://elijahlucian.ca/experiments");
            var success = Windows.System.Launcher.LaunchUriAsync(uri);

            AppLog.Write($"Launched {uri} with success: {success}");
        }

        public void UpdateStatus(string message) {
            status.Text = message;
        }


        public void SetTransportPanelColor(Color color) {
            TransportPanel.Background = new SolidColorBrush(color);
        }

        public void Record_Click(object sender, RoutedEventArgs e) {
            App.CurrentApp.recorder.Record();
        }

        public void Play_Click(object sender, RoutedEventArgs e) {
            App.CurrentApp.recorder.Play();
        }

        public void Stop_Click(object sender, RoutedEventArgs e) {
            App.CurrentApp.recorder.Stop();

        }
    }
}
