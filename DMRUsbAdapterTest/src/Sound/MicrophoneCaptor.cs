using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using NAudio;
using NAudio.WindowsMediaFormat;
using NAudio.Wave;

namespace DMRUsbAdapterTest.src.Sound
{
    class MicrophoneCaptor
    {

        //Thread currThread;

        public WaveIn waveSource = null;
        public WaveFileWriter waveFile = null;


        public MicrophoneCaptor()
        {

        }

        public void StartRecord(int index)
        {
            waveSource = new WaveIn(); 
            waveSource.DeviceNumber = index;
            waveSource.WaveFormat = new NAudio.Wave.WaveFormat(44100, NAudio.Wave.WaveIn.GetCapabilities(index).Channels);
            waveSource.DataAvailable += new EventHandler<NAudio.Wave.WaveInEventArgs>(AviableAudioData);
            waveSource.StartRecording();
        }
        

        private void AviableAudioData(object sender, NAudio.Wave.WaveInEventArgs e)
        {
            
        }
    }
}
