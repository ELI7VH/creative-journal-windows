using Microsoft.UI.Xaml;

namespace DaemonRecorder {
    public class SubWindow : Window {
        public SubWindow(string windowName) {
            this.AppWindow.Title = windowName;

            this.Closed += (object sender, WindowEventArgs e) => {
                e.Handled = true;
                AppLog.Write($"{windowName} Window Closed");
            };

            AppLog.Write($"{windowName} Window Opened");
        }
    }
}