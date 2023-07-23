using Microsoft.UI.Xaml;
using NAudio.Wave;
using System.Collections.Generic;
using System.Threading;

namespace DaemonRecorder {
    public sealed partial class SongPlayer : Window {
        public List<SongRecord> songs;
        public WasapiOut wo;
        // TODO: Streaming MP3 for local playback:
        // https://markheath.net/post/how-to-play-back-streaming-mp3-using

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

        public void Song_Play(string id) {
            var url = songs.Find((song) => song.id == id).link;

            if (url == null) return;

            wo?.Dispose();

            using var mf = new MediaFoundationReader(url);
            using (wo = new WasapiOut()) {
                wo.Init(mf);
                wo.Play();

                while (wo.PlaybackState == PlaybackState.Playing) {
                    Thread.Sleep(1000);
                }

                wo.Dispose();
                wo = null;
            }
        }


        public void RefreshSongList(List<SongRecord> songs) {
            SongList.IsReadOnly = false;
            var rows = SongRecordList.ToRows(songs);
            SongList.Document.SetText(Microsoft.UI.Text.TextSetOptions.None, string.Join("\n", rows));
            SongList.IsReadOnly = true;
        }
    }
}
