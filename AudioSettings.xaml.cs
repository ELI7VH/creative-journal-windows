using Microsoft.UI.Xaml;

namespace DaemonRecorder {
    public sealed partial class AudioSettings : Window {
        public AudioSettings() {
            this.InitializeComponent();
            this.AppWindow.Resize(new(400, 400));
            this.AppWindow.Title = "Audio Settings";

            var devices = AudioRecorder.GetDevices();
            foreach (var device in devices) {
                DeviceList.Items.Add(new Microsoft.UI.Xaml.Controls.ComboBoxItem() {
                    Content = device.ProductName,
                    Tag = device
                });
            }

            this.Closed += (object sender, WindowEventArgs e) =>
                AppLog.Write("Audio Settings Window Closed");

            AppLog.Write("Audio Settings Window Opened");
        }

        private void DeviceList_SelectionChanged(object sender, Microsoft.UI.Xaml.Controls.SelectionChangedEventArgs e) {
            var item = (Microsoft.UI.Xaml.Controls.ComboBoxItem)DeviceList.SelectedItem;
            var device = (NAudio.Wave.WaveInCapabilities)item.Tag;

            App.CurrentApp.recorder.SetDevice(device);

            AppLog.Write($"Selected Device: {device.ProductName}");
        }

        private void Close_Click(object sender, RoutedEventArgs e) {
            this.Close();
        }

    }
}
