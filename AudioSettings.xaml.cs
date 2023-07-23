using Microsoft.UI.Xaml;

namespace DaemonRecorder {
    public sealed partial class AudioSettings : Window {
        public AudioSettings() {
            this.InitializeComponent();
            this.AppWindow.Resize(new(400, 400));

            var devices = AudioRecorder.GetDevices();

            foreach (var device in devices) {
                DeviceList.Items.Add(new Microsoft.UI.Xaml.Controls.ComboBoxItem() {
                    Content = device.ProductName,
                    Tag = device
                });
            }

            // todo try and find a way to set the selected item by tag
            // if null then set to default
            DeviceList.SelectedIndex = App.Settings.Audio.DeviceIndex;
        }

        private void DeviceList_SelectionChanged(object sender, Microsoft.UI.Xaml.Controls.SelectionChangedEventArgs e) {
            var item = (Microsoft.UI.Xaml.Controls.ComboBoxItem)DeviceList.SelectedItem;
            // var device = (NAudio.Wave.WaveInCapabilities)item.Tag;

            App.Settings.Audio.DeviceIndex = DeviceList.SelectedIndex;
        }

        private void Close_Click(object sender, RoutedEventArgs e) {
            this.Close();
        }

    }
}
