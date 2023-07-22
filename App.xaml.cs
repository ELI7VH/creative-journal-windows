using Microsoft.UI.Xaml;
using SocketIOClient;

// WinUI project structure: http://aka.ms/winui-project-info.

namespace DaemonRecorder {
    public partial class App : Application {
        public SocketIO client;
        public MainWindow window;
        public AudioRecorder recorder;
        public static readonly string appFolder = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

        public static App CurrentApp => (App)Current;

        public App() {
            AppLog.Write("DaemonRecorder starting...");

            this.InitializeComponent();
        }

        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args) {
            window = new MainWindow();
            window.Activate();

            window.Title = "Daemon Recorder 🍕";
            AppLog.Write("DaemonRecorder started!");

            ConnectToWebsocket();
            InitializeAudio();
        }

        private void InitializeAudio() {
            // TODO: make its own window (select)
            AppLog.Write("Initializing Audio");
            recorder = new AudioRecorder();

            foreach (var device in recorder.devices) {
                AppLog.Write($"Audio Device Available: {device.ProductName}");
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
            AppLog.Write(message);
        }
    }
}
