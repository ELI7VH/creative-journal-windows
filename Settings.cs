using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text.Json;

namespace DaemonRecorder {
    public class Settings : INotifyPropertyChanged {
        public static string appDataPath = "F:\\Desktop\\CreativeJournal";
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            Debug.WriteLine($"Settings changed: {propertyName}");
            Save();
        }

        public IAudio Audio { get; set; }
        public IApi Api { get; set; }
        public IGeneral General { get; set; }

        public Settings() {
            General = new IGeneral() {
                DataFolder = appDataPath,
                LogFile = "log.txt",
                ContextMenuAction = ContextMenuActions.MinMax
            };

            Audio = new IAudio() {
                DeviceIndex = 0,
                Channels = 2,
                SampleRate = 44100,
                Folder = "audio"
            };

            Api = new IApi() {
                BaseUrl = "https://elijahlucian.ca"
            };
        }

        public static Settings Load() {
            Directory.CreateDirectory(appDataPath);

            var path = Path.Combine(appDataPath, "settings.json");

            if (!File.Exists(path)) {
                var settings = new Settings();
                var _f = JsonSerializer.Serialize(settings);
                File.WriteAllText(path, _f);
            }

            var f = File.ReadAllText(path);
            return JsonSerializer.Deserialize<Settings>(f);
        }


        public void Save() {
            var path = Path.Combine(appDataPath, "settings.json");

            var f = JsonSerializer.Serialize(this);
            File.WriteAllText(path, f);
        }

        public string AudioFolder {
            get {
                return Path.Combine(General.DataFolder, Audio.Folder);
            }
        }
    }

    public enum ContextMenuActions {
        RecordVoiceNote,
        MinMax,
    }

    public class IGeneral {
        public string DataFolder { get; set; }
        public string LogFile { get; set; }
        public ContextMenuActions ContextMenuAction { get; set; }
    }

    public class IAudio {
        public int DeviceIndex { get; set; }
        public int Channels { get; set; }
        public int SampleRate { get; set; }
        public string Folder { get; set; }
    }

    public class IApi {
        public string BaseUrl { get; set; }
        public string SocketPath { get; set; }
    }
}
