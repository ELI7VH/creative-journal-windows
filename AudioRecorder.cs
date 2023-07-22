using NAudio.Wave;
using System;
using System.Collections.Generic;

namespace DaemonRecorder {
    public class AudioRecorder {
        public List<WaveInCapabilities> devices;
        public WaveIn waveIn;
        public WaveInCapabilities device;
        public Action OnRecord;
        public Action OnStop;
        public Action OnPlay;

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
            devices = GetDevices();
        }

        public void SetDevice(WaveInCapabilities _device) {
            device = _device;
        }

        public void Record() {
            AppLog.Write("TODO: Record!");
            OnRecord();
        }

        public void Play() {
            AppLog.Write("TODO: Play!");
            OnPlay();

        }
        public void Stop() {
            AppLog.Write("TODO: Stop!");
            OnStop();
        }

        public void Pause() {
            AppLog.Write("TODO: Pause!");
        }

    }
}
