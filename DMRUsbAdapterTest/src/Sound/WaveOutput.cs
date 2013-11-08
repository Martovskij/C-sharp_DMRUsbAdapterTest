using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using log4net.Config;

namespace DMRUsbAdapterTest.src.Sound
{
    class WaveOutput : src.Kernel.AudioDataObserver
    {
        public static readonly ILog log = LogManager.GetLogger(typeof(WaveOutput)); 
        NAudio.Wave.WaveOut waveOut = new NAudio.Wave.WaveOut();
        FileWaveProvider waveProvider;
        src.Kernel.AudioDataObserver observer = null;
        D_BufferedDataCallback callback = null;

        public WaveOutput()
        {

        }

        public bool Play(String path, int deviceIndex)
        {
            try
            {
                waveProvider = new FileWaveProvider(path);
                waveOut = new NAudio.Wave.WaveOut();
                waveOut.DeviceNumber = deviceIndex;
                waveProvider.SetBufferedDataCallback(callback);
                waveOut.Init(waveProvider);
                waveOut.Play();
                waveOut.PlaybackStopped += new EventHandler<NAudio.Wave.StoppedEventArgs>(PlayBackStopedHandler);
                return true;
            }
            catch(Exception ex)
            {
                
                log.Debug(ex.Message);
                return false;
            }
        }

        public void PlayBackStopedHandler(object sender, NAudio.Wave.StoppedEventArgs e)
        {
            if (callback != null)
            {
                callback(new byte[1]);
            }
            waveProvider.CloseFile();
        }

        public void notify(object obj)
        {
            if (observer != null)
            {
                observer.notify(obj);
            }
        }

        public void SetAviableDataHandler(D_BufferedDataCallback new_callback)
        {
            callback = new_callback;
        }


    }
}
