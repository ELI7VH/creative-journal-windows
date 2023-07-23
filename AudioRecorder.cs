using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace DaemonRecorder {
    public class AudioRecorder {
        public List<WaveInCapabilities> devices;
        public WaveInEvent waveIn;
        public string outputFolder = "audio-recorder-debug";
        public WaveInCapabilities device;
        public int channels = 1;
        public int sampleRate = 44100;
        public WaveFileWriter writer;
        public Action OnRecord;
        public Action OnStop;
        public Action OnPlay;
        public byte[] data;
        public string Folder => Path.Combine(App.Settings.General.DataFolder, "audio-recordings", outputFolder);

        public static List<WaveInCapabilities> GetDevices() {
            var deviceCount = NAudio.Wave.WaveIn.DeviceCount;
            var devices = new List<WaveInCapabilities>();

            for (int i = 0; i < deviceCount; i++) {
                var device = NAudio.Wave.WaveIn.GetCapabilities(i);
                devices.Add(device);
            }
            return devices;
        }

        public AudioRecorder() {
            waveIn = new WaveInEvent();
            devices = GetDevices();
            SetDevice(0);
        }

        public void SetDevice(int index) {
            waveIn.DeviceNumber = index;
            device = devices[index];

        }

        public void SetChannels(int _channels) {
            channels = _channels;
        }

        public void SetSampleRate(int _sampleRate) {
            sampleRate = _sampleRate;
            waveIn.WaveFormat = new WaveFormat(sampleRate, channels);
        }

        public string Record(string? filenameBase) {
            Directory.CreateDirectory(Folder);

            var filename = filenameBase.Length > 0 ? filenameBase : DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
            var outputPath = Path.Combine(Folder, $"{filename}.wav");

            writer = new WaveFileWriter(outputPath, waveIn.WaveFormat);

            waveIn.DataAvailable += (object sender, WaveInEventArgs e) => {
                Debug.WriteLine($"Writing {e.BytesRecorded} bytes");
                writer.Write(e.Buffer, 0, e.BytesRecorded);
            };

            waveIn.RecordingStopped += (object sender, StoppedEventArgs e) => {
                Debug.WriteLine("Recording Stopped");
                writer.Dispose();
                writer = null;
            };

            waveIn.StartRecording();

            Debug.WriteLine($"Recording to {outputPath}");

            OnRecord();
            return outputPath;
        }

        public void Play() {
            AppLog.Write("TODO: Play!");
            OnPlay();

        }
        public void Stop() {
            waveIn.StopRecording();
            OnStop();
        }

        public void Pause() {
            AppLog.Write("TODO: Pause!");
        }

    }
}
