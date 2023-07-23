using Microsoft.UI.Xaml;
using System.Collections.Generic;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace DaemonRecorder {
    public sealed partial class SongPlayer : Window {
        public List<SongRecord> songs;

        public SongPlayer() {
            this.InitializeComponent();
            GetSongs();
            this.AppWindow.Resize(new(600, 400));
        }

        private void GetSongs() {
            AppLog.Write("Getting Song List");
            var songs = Api.GetSongs();

            if (songs != null) {
                AppLog.Write($"Got {songs.Count} songs!");
                SetSongList(songs);

            } else {
                AppLog.Write("Error getting songs!");
            }
        }

        public void SetSongList(List<SongRecord> _songs) {
            songs = _songs;

            RefreshSongList(songs);
        }

        public void Search_TextChanged(object sender, RoutedEventArgs e) {
            var text = Search.Text;

            if (text.Length == 0) {
                RefreshSongList(songs);
                return;
            }

            var filtered = songs.FindAll((song) => song.name.ToLower().Contains(text.ToLower()));

            RefreshSongList(filtered);
        }


        public void RefreshSongList(List<SongRecord> songs) {
            SongList.IsReadOnly = false;
            var rows = SongRecordList.ToRows(songs);
            SongList.Document.SetText(Microsoft.UI.Text.TextSetOptions.None, string.Join("\n", rows));
            SongList.IsReadOnly = true;
        }
    }
}
