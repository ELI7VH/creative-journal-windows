using Microsoft.UI.Xaml;

namespace DaemonRecorder {
    public sealed partial class ConsoleLog : Window {
        public ConsoleLog() {
            this.InitializeComponent();

            Log.IsReadOnly = false;
            string content = AppLog.Read();
            Log.Document.SetText(Microsoft.UI.Text.TextSetOptions.None, content);
            Log.IsReadOnly = true;

            // scroll to end
            Log.Document.Selection.StartPosition = content.Length;
        }

        public void LogMessage(string message) {
            AppLog.Write(message);
        }
    }
}
