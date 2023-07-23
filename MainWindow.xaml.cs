using CommunityToolkit.Mvvm.Input;
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

            this.Closed += (object sender, WindowEventArgs e) => {
                e.Handled = true;
                this.Hide();
                /*voiceNoteWindow?.Close();
                audioSettingsWindow?.Close();
                songPlayerWindow?.Close();
                consoleLogWindow?.Close();

                App.Current.Exit();
                */
                AppLog.Write("Main Window Closed");
            };

            AppLog.Write("Main Window Opened");
        }

        public void Exit_Click(object sender, RoutedEventArgs e) {
            AppLog.Write("Exit Clicked");
            App.Current.Exit();
        }

        [RelayCommand]
        public void ShowHideWindow() {
            if (this.AppWindow == null) {
                AppLog.Write("AppWindow is null");
                this.Show();
                return;
            }

            if (this.AppWindow.IsVisible) {
                this.Hide();
                AppLog.Write("Main Window Hidden");
            } else {
                this.Show();
                AppLog.Write("Main Window Shown");
            }
        }

        [RelayCommand]
        public void Open_VoiceNote() {
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

        [RelayCommand]
        public void Open_ConsoleLog() {
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

        [RelayCommand]
        public void Open_SongPlayer() {
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

        [RelayCommand]
        public void Open_AudioSettings() {
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

        [RelayCommand]
        public void Open_MicStream() {
            var micStreamWindow = new MicStream();
            micStreamWindow.Activate();

        }

        [RelayCommand]
        public void Open_UI() {
            var uri = new Uri($"{App.Settings.Api.BaseUrl}/experiments");
            var success = Windows.System.Launcher.LaunchUriAsync(uri);

            AppLog.Write($"Launched {uri} with success: {success}");
        }

        [RelayCommand]
        public void TaskbarIcon_DoubleTapped() {
            AppLog.Write("TaskbarIcon DoubleTapped");
            this.Activate();
        }

        // TODO: Meme Machine
        // https://markheath.net/post/fire-and-forget-audio-playback-with

        // TODO: Song Encoder
        // https://github.com/naudio/NAudio/blob/master/Docs/MediaFoundationEncoder.md

        // TODO: Song Browser and Organizer
        // Tagged song management
        // Automatic re-distribution of files
        // Cloud Sync
        // MP3 Tag Management
        // Song Metadata Management
        // File Renaming, etc

        // TODO: Wav Editor
        // https://markheath.net/post/trimming-wav-file-using-naudio
        // Quck and dirty start / end trimmer
        // https://github.com/naudio/NAudio/blob/master/Docs/FadeInOutSampleProvider.md

        // TOOD: Chainsong Concatenator
        // https://github.com/naudio/NAudio/blob/master/Docs/ConcatenatingAudio.md

        // TODO: Spatial Audio Manager
        // https://markheath.net/post/handling-multi-channel-audio-in-naudio

        // TODO: The Drone
        // https://github.com/naudio/NAudio/blob/master/Docs/PlaySineWave.md
    }
}

