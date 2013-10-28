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
        Kernel.AudioDataObserver observer;


        public MicrophoneCaptor(Kernel.AudioDataObserver obs)
        {
            if(obs==null) 
            {
                log.Debug("observer argument is null");
            }
        }

        public void StartRecord(int index)
        {

            if (index < 0)
            {
                log.Debug("wrong index name");
                index = 0;
            }

            try
            {
                if(waveSource==null)
                     waveSource = new WaveIn();
                else
                     waveSource.StopRecording();
                waveSource.DeviceNumber = index;
                waveSource.WaveFormat = new NAudio.Wave.WaveFormat(44100, 16, NAudio.Wave.WaveIn.GetCapabilities(index).Channels);
                waveSource.DataAvailable += new EventHandler<NAudio.Wave.WaveInEventArgs>(AviableAudioData);
                waveSource.StartRecording();
            }
            catch(Exception ex)
            {
                log.Debug(ex.Message);
            }
        }
        

        private void AviableAudioData(object sender, NAudio.Wave.WaveInEventArgs e)
        {

            if (e == null)
            {
                waveSource.StopRecording();
                log.Debug("wave source return null data argument");
                return;
            }
            if (observer == null)
            {
                waveSource.StopRecording();
                log.Debug("no observer data");
                return;
            }

            observer.notify(e);

        }
    }
}
