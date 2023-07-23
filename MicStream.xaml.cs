using Microsoft.UI.Xaml;
using NAudio.Wave;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace DaemonRecorder {
    public sealed partial class MicStream : Window {
        public AsioOut asioOut;
        public WaveFileWriter writer;
        public WaveFormat format;
        public string Folder => Path.Combine(App.Settings.AudioFolder, "mic-stream");
        public float amplitude = 0;
        public int channelCount = 1;
        public int selectedChannel;
        public string selectedDevice;

        public MicStream() {
            // TODO: Timer
            // TODO: Visualizations

            this.InitializeComponent();
            this.AppWindow.Resize(new(500, 400));

            var inputs = AsioOut.GetDriverNames();

            foreach (var input in inputs) {
                DeviceList.Items.Add(new Microsoft.UI.Xaml.Controls.ComboBoxItem() {
                    Content = input,
                    Tag = input
                });
                Debug.WriteLine(input);
            }

            this.Closed += (sender, e) => {
                asioOut?.Stop();
                asioOut?.Dispose();
                writer?.Dispose();
            };

            StartButton.IsEnabled = false;
            StopButton.IsEnabled = false;
        }

        private void SetDriver(string deviceName) {
            selectedDevice = deviceName;
            asioOut = new AsioOut(selectedDevice);

            InputSelect.Items.Clear();

            for (int i = 0; i < asioOut.DriverInputChannelCount; i++) {
                InputSelect.Items.Add(new Microsoft.UI.Xaml.Controls.ComboBoxItem() {
                    Content = asioOut.AsioInputChannelName(i),
                    Tag = i
                });
            };
        }

        private void SetChannel(int channel) {
            selectedChannel = channel;
            StartButton.IsEnabled = true;
        }

        private void InputSelect_SelectionChanged(object sender, Microsoft.UI.Xaml.Controls.SelectionChangedEventArgs e) {
            var item = (Microsoft.UI.Xaml.Controls.ComboBoxItem)InputSelect.SelectedItem;
            SetChannel((int)item.Tag);
        }

        private void DeviceList_SelectionChanged(object sender, Microsoft.UI.Xaml.Controls.SelectionChangedEventArgs e) {
            var item = (Microsoft.UI.Xaml.Controls.ComboBoxItem)DeviceList.SelectedItem;
            SetDriver((string)item.Tag);
        }

        private void Start_Click(object sender, RoutedEventArgs e) {
            asioOut?.Dispose();

            asioOut = new AsioOut(selectedDevice);
            asioOut.AsioInputChannelName(selectedChannel);
            asioOut.InitRecordAndPlayback(null, channelCount, 44100);
            asioOut.AudioAvailable += asioOut_DataAvailable;

            var path = Path.Combine(Folder, "test.wav");
            Directory.CreateDirectory(Folder);

            var format = new WaveFormat(44100, channelCount);
            writer = new WaveFileWriter(path, format);

            asioOut.Play();
            StartButton.IsEnabled = false;
            StopButton.IsEnabled = true;

            statusMessage.Text = "Recording";
        }

        private void Stop_Click(object sender, RoutedEventArgs e) {
            asioOut.Stop();
            writer.Dispose();
            StartButton.IsEnabled = true;
            StopButton.IsEnabled = false;
        }

        private void asioOut_DataAvailable(object sender, AsioAudioAvailableEventArgs e) {
            Debug.WriteLine($"Writing {e.SamplesPerBuffer} samples");

            var samples = e.GetAsInterleavedSamples();

            var max = samples.Max();
            var min = Math.Abs(samples.Min());

            amplitude = max > min ? max : min;
            Debug.WriteLine($"Amplitude: {Math.Round(amplitude, 5)}");

            writer.WriteSamples(samples, 0, samples.Length);
        }
    }
}
