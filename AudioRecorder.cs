using NAudio.Wave;
using System;
using System.Collections.Generic;

namespace DaemonRecorder {
    public class AudioRecorder {
        public List<WaveInCapabilities> devices;

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

        public void Start() {
            throw new NotImplementedException();
        }
        public void Stop() {
            throw new NotImplementedException();
        }

        public void Pause() {
            throw new NotImplementedException();
        }
    }
}
