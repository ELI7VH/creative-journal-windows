using Microsoft.UI.Xaml;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace DaemonRecorder {
    public sealed partial class AudioSettings : Window {
        public AudioSettings() {
            this.InitializeComponent();
            this.AppWindow.Title = "Audio Settings";
            this.AppWindow.Resize(new(400, 400));
        }
    }
}
