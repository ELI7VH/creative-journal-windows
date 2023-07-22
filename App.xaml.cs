using Microsoft.UI.Xaml;
using SocketIOClient;
using System.Diagnostics;

// WinUI project structure: http://aka.ms/winui-project-info.

namespace DaemonRecorder {
    public partial class App : Application {
        public SocketIO client;
        public MainWindow window;
        public AudioRecorder recorder;

        public App() {
            this.InitializeComponent();
        }

        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args) {
            window = new MainWindow();
            window.Activate();

            window.Title = "Daemon Recorder 🍕";

            GetSongs();
            ConnectToWebsocket();
            InitializeAudio();
        }

        private void GetSongs() {
            // TODO: move to it's own window
            window.LogMessage("Getting Song List");

            var songs = Api.GetSongs();

            if (songs != null) {
                window.LogMessage($"Got {songs.Count} songs!");
                window.SetSongList(songs);

            } else {
                window.LogMessage("Error getting songs!");
            }
        }

        private void InitializeAudio() {
            // TODO: make its own window (select)
            window.LogMessage("Initializing Audio");
            recorder = new AudioRecorder();

            foreach (var device in recorder.devices) {
                window.LogMessage($"Audio Device Available: {device.ProductName}");
            }
        }

        async private void ConnectToWebsocket() {
            // TODO: create own socket log window I think
            // this is the annoying one
            // maybe pass the socket reference to a new window instance?
            var url = "https://elijahlucian.ca";
            client = new SocketIO(url);

            var path = client.Options.Path;

            LogSocket($"Connecting... {url}{path}");

            client.OnAny(
                (eventName, data) => {
                    LogSocket($"Any Event rcvd: {eventName} {data}");
                }
            );

            client.OnError += (sender, e) => {
                LogSocket($"Error: {e}");
            };

            client.OnReconnectFailed += (sender, e) => {
                LogSocket($"Reconnect Failed: {e}");
            };


            client.OnConnected += async (sender, e) => {
                await client.EmitAsync("join", "DaemonRecorder");
                LogSocket("Connected to socket!");
            };

            client.On("zen-text", (e) => {
                LogSocket(e.ToString());
            });

            client.On("ping", e => {
                LogSocket(e.ToString());
            });

            await client.ConnectAsync();
        }

        private void LogSocket(string message) {
            Debug.WriteLine(message);
        }
    }
}
