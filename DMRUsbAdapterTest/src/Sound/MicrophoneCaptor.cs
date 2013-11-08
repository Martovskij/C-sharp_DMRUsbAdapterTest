using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using NAudio;
using NAudio.WindowsMediaFormat;
using NAudio.Wave;


using log4net;
using log4net.Config;



        
namespace DMRUsbAdapterTest.src.Sound
{
    class MicrophoneCaptor
    {

        //Thread currTread;
        
        WaveIn waveSource = null;
        static readonly ILog log = LogManager.GetLogger(typeof(MicrophoneCaptor));
        List<Kernel.AudioDataObserver> observers = new List<Kernel.AudioDataObserver>();


        public MicrophoneCaptor(Kernel.AudioDataObserver obs)
        {
           
           
            if (obs == null)
            {
                log.Debug("observer argument is null");
                return;
            }
            observers.Add(obs);
        }


        public void CloseCaptor()
        {
             if(waveSource!=null) waveSource.StopRecording();
        }

        public void SetupMicrophoneObserver(Kernel.AudioDataObserver obs)
        {
            if (obs != null)
                observers.Add(obs);
        }

        public bool StartRecord(int index)
        {
           
            if (index < 0)
            {
                log.Debug("wrong index name");
                index = 0;
            }
            try
            {   if(waveSource!=null)
                     waveSource.Dispose();
                waveSource = null;
                waveSource = new WaveIn();
                waveSource.DataAvailable += new EventHandler<NAudio.Wave.WaveInEventArgs>(AviableAudioData);
                log.Debug("start record from: " + NAudio.Wave.WaveIn.GetCapabilities(index).ProductName);
                waveSource.BufferMilliseconds = 100;
                waveSource.WaveFormat = new NAudio.Wave.WaveFormat(8000, 16, 1);
                waveSource.DeviceNumber = index;
                waveSource.StartRecording();
                return true;
            }
            catch(Exception ex)
            {
                log.Debug(ex.Message);
                return false;
            }
        }

        public void StopRecord()
        {
            log.Debug("stop record");
            if (waveSource != null)
            {
                waveSource.StopRecording();
            }
        }

        public NAudio.Wave.WaveIn GetInputStream()
        { 
            return waveSource; 
        }


        private void AviableAudioData(object sender, NAudio.Wave.WaveInEventArgs e)
        {
            if (e == null)
            {
                waveSource.StopRecording();
                log.Debug("wave source return null data argument");
                return;
            }
            for (int i = 0; i < observers.Count; i++)
            {
                if (observers.ElementAt(i) == null)
                    continue;
                observers.ElementAt(i).notify(e.Buffer);
            }
        }
    }
}
