using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace DaemonRecorder {

    public partial class MainWindow : Window {
        public int counter = 0;
        public MainWindow _instance;
        public List<SongRecord> songs;
        public string logFile = "";

        public MainWindow() {
            this.InitializeComponent();
            _instance = this;

        }

        private void myButton_Click(object sender, RoutedEventArgs e) {
            myButton.Content = "Clicked";

            string message = $"Clicked {counter} times";

            counter++;

            LogMessage(message);

            // App.client.EmitAsync("message", message);
        }

        public void SetSongList(List<SongRecord> _songs) {

            songs = _songs;

            RefreshSongList(songs);
        }

        public void RefreshSongList(List<SongRecord> songs) {
            songList.IsReadOnly = false;
            var rows = SongRecordList.ToRows(songs);
            songList.Document.SetText(Microsoft.UI.Text.TextSetOptions.None, string.Join("\n", rows));
            songList.IsReadOnly = true;
        }

        public void search_TextChanged(object sender, RoutedEventArgs e) {
            var text = search.Text;

            if (text.Length == 0) {
                RefreshSongList(songs);
                return;
            }

            var filtered = songs.FindAll((song) => song.name.ToLower().Contains(text.ToLower()));

            RefreshSongList(filtered);
        }

        private void launch_Click(object sender, RoutedEventArgs e) {
            var uri = new Uri("https://elijahlucian.ca/experiments");
            var success = Windows.System.Launcher.LaunchUriAsync(uri);

            LogMessage($"Launched {uri} with success: {success}");

        }

        public void UpdateSocketStatus(string status) {
            data.Text = status;
        }

        public void LogMessage(string message) {
            // consoleLog.IsReadOnly = false;
            string content = $"{message}\n{logFile}";
            consoleLog.Document.SetText(Microsoft.UI.Text.TextSetOptions.None, content);
            logFile = content;
            // consoleLog.IsReadOnly = true;
        }
    }
}
