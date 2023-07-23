using System;
using System.Diagnostics;
using System.IO;

namespace DaemonRecorder {
    // Singleton Class that writes to a log file
    internal class AppLog {
        readonly public static string filename = App.Settings.General.LogFile;
        readonly public static string folder = App.Settings.General.DataFolder;
        readonly public static string path = Path.Combine(folder, filename);

        public static string Write(string message) {
            if (!Directory.Exists(folder)) {
                Directory.CreateDirectory(folder);
            }

            if (!File.Exists(path)) {
                using StreamWriter sw = File.CreateText(path);
            }

            var line = $"{DateTime.Now}: {message}";

            Debug.WriteLine(line);

            // write line to the end of the log file on a new line
            using (StreamWriter sw = File.AppendText(path)) {
                sw.WriteLine(line);
            }

            using StreamReader sr = File.OpenText(path);
            return sr.ReadToEnd();
        }

        public static FileSystemWatcher Watch() {
            // watch file for changes
            Write($"Watching {path} for changes");

            var watcher = new FileSystemWatcher {
                Path = folder,
                NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.LastAccess,
                Filter = filename,

            };

            watcher.Disposed += (object sender, EventArgs e) =>
                Write("Watcher Disposed");

            watcher.EnableRaisingEvents = true;

            return watcher;
        }

        public static string Read() {
            if (!File.Exists(path)) {
                // do nothing, just create the file
                using StreamWriter sw = File.CreateText(path);
            }

            // write line to the end of the log file on a new line
            using (StreamReader sr = File.OpenText(path)) {
                return sr.ReadToEnd();
            }
        }
    }
}
