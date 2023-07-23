using H.NotifyIcon;
using Microsoft.UI.Xaml;
using System;

namespace DaemonRecorder {
    public partial class MainWindow : Window {
        public int counter = 0;
        public MainWindow _instance;
        public string logFile = "";

        public VoiceNote voiceNoteWindow;
        public AudioSettings audioSettingsWindow;
        public SongPlayer songPlayerWindow;
        public ConsoleLog consoleLogWindow;

        public MainWindow() {
            this.InitializeComponent();
            _instance = this;

            this.AppWindow.Resize(new Windows.Graphics.SizeInt32(800, 400));
            this.Title = "Creative Journal";

            /*this.Closed += (object sender, WindowEventArgs e) => {
                voiceNoteWindow?.Close();
                audioSettingsWindow?.Close();
                songPlayerWindow?.Close();
                consoleLogWindow?.Close();

                App.Current.Exit();
                AppLog.Write("Main Window Closed");
            };*/

            AppLog.Write("Main Window Opened");
        }

        private void Exit_Click(object sender, RoutedEventArgs e) {
            AppLog.Write("Exit Clicked");
            App.Current.Exit();
        }

        private void VoiceNoteButton_Click(object sender, RoutedEventArgs e) {
            if (voiceNoteWindow == null) {
                voiceNoteWindow = new VoiceNote { Title = "Voice Note" };
                voiceNoteWindow.Closed += (object sender, WindowEventArgs e) => {
                    e.Handled = true;
                    AppLog.Write("Voice Note Window Closed");
                    voiceNoteWindow.Hide();
                };
            }
            voiceNoteWindow.Activate();
            AppLog.Write("Voice Note Window Opened");
        }

        private void ConsoleLogButton_Click(object sender, RoutedEventArgs e) {
            if (consoleLogWindow == null) {
                consoleLogWindow = new ConsoleLog { Title = "console.log" };
                consoleLogWindow.Closed += (object sender, WindowEventArgs e) => {
                    e.Handled = true;
                    AppLog.Write("console.log Window Closed");
                    consoleLogWindow.Hide();
                };
            }
            consoleLogWindow.Activate();
            AppLog.Write("console.log Window Opened");
        }

        private void SongPlayerButton_Click(object sender, RoutedEventArgs e) {
            if (songPlayerWindow == null) {
                songPlayerWindow = new SongPlayer { Title = "Song Player" };
                songPlayerWindow.Closed += (object sender, WindowEventArgs e) => {
                    e.Handled = true;
                    AppLog.Write("Song Player Window Closed");
                    songPlayerWindow.Hide();
                };
            }
            songPlayerWindow.Activate();
            AppLog.Write("Song Player Window Opened");
        }

        private void AudioSettingsButton_Click(object sender, RoutedEventArgs e) {
            if (audioSettingsWindow == null) {
                audioSettingsWindow = new AudioSettings { Title = "Audio Settings" };
                audioSettingsWindow.Closed += (object sender, WindowEventArgs e) => {
                    e.Handled = true;
                    AppLog.Write("Audio Settings Window Closed");
                    audioSettingsWindow.Hide();
                };
            }
            audioSettingsWindow.Activate();
            AppLog.Write("Audio Settings Window Opened");
        }

        private void LaunchUI_Click(object sender, RoutedEventArgs e) {
            var uri = new Uri($"{App.Settings.Api.BaseUrl}/experiments");
            var success = Windows.System.Launcher.LaunchUriAsync(uri);

            AppLog.Write($"Launched {uri} with success: {success}");
        }

        public void UpdateStatus(string message) {
            status.Text = message;
        }

        private void TaskbarIcon_DoubleTapped(object sender, Microsoft.UI.Xaml.Input.DoubleTappedRoutedEventArgs e) {
            AppLog.Write("TaskbarIcon DoubleTapped");
            this.Activate();
        }
    }
}

