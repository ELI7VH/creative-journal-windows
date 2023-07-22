using System;
using System.IO;

namespace DaemonRecorder {
    // Singleton Class that writes to a log file
    internal class AppLog {
        readonly static string logFile = "log.txt";


        public static string Write(string message) {
            if (!File.Exists(logFile)) {
                // do nothing, just create the file
                using StreamWriter sw = File.CreateText(logFile);
            }

            // write line to the end of the log file on a new line
            using (StreamWriter sw = File.AppendText(logFile)) {
                sw.WriteLine($"{DateTime.Now}: {message}");
            }

            using StreamReader sr = File.OpenText(logFile);
            return sr.ReadToEnd();
        }

        public static string Read() {
            if (!File.Exists(logFile)) {
                // do nothing, just create the file
                using StreamWriter sw = File.CreateText(logFile);
            }

            // write line to the end of the log file on a new line
            using (StreamReader sr = File.OpenText(logFile)) {
                return sr.ReadToEnd();
            }
        }
    }
}
