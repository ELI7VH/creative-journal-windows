using Microsoft.UI.Xaml;
using SocketIOClient;
using System.Diagnostics;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace DaemonRecorder {
    public partial class App : Application {
        public SocketIO client;
        public MainWindow window;
        public AudioRecorder recorder;

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App() {
            this.InitializeComponent();
            // AppWindow.Resize(500, 500);
            // icon click
        }

        /// <summary>
        /// Invoked when the application is launched.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args) {
            window = new MainWindow();
            window.Activate();

            window.Title = "Daemon Recorder 🍕";

            GetSongs();
            ConnectToWebsocket();
            InitializeAudio();
        }

        private void GetSongs() {
            // make it's own window
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
            // make its own window (select)
            window.LogMessage("Initializing Audio");
            recorder = new AudioRecorder();

            foreach (var device in recorder.devices) {
                window.LogMessage($"Audio Device Available: {device.ProductName}");
            }
        }

        async private void ConnectToWebsocket() {
            // this is the annoying one
            var url = "https://elijahlucian.ca";
            client = new SocketIO(url);

            var path = client.Options.Path;

            LogSocket($"Connecting... {url}{path}");

            client.OnAny(
                (eventName, data) => {
                    LogSocket($"Event: {eventName} {data}");
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
            // window.LogMessage(message);
        }
    }
}
