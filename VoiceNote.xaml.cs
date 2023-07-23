using H.NotifyIcon;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using Windows.UI;


namespace DaemonRecorder {
    public sealed partial class VoiceNote : Window {
        public AudioRecorder recorder;

        public VoiceNote() {
            this.InitializeComponent();
            this.Title = "Voice Note";
            this.AppWindow.Resize(new Windows.Graphics.SizeInt32(600, 200));

            recorder = new AudioRecorder {
                outputFolder = "voice-notes",
                OnStop = () => {
                    RecordButton.IsEnabled = true;
                    StopButton.IsEnabled = false;
                },
                OnRecord = () => {
                    RecordButton.IsEnabled = false;
                    StopButton.IsEnabled = true;
                }
            };

            this.Closed += (sender, e) => {
                e.Equals(true);
                this.Hide();
                AppLog.Write("VoiceNote window closed");
            };

            AppLog.Write("VoiceNote window opened");
        }


        public void SetTransportPanelColor(Color color) {
            TransportPanel.Background = new SolidColorBrush(color);
        }

        public void Record_Click(object sender, RoutedEventArgs e) {
            outputFileName.Text = recorder.Record(recordingName.Text);
        }

        public void Play_Click(object sender, RoutedEventArgs e) {
            // TODO should be able to review last recordings
            // recorder.Play();
        }

        public void Stop_Click(object sender, RoutedEventArgs e) {
            recorder.Stop();
        }
    }
}
