using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DMRUsbAdapterTest.src.Sound
{
    class Speaker 
    {

        NAudio.Wave.DirectSoundOut waveOut = null;
        public int DeviceIndex { get; set; }
        NAudio.Wave.WaveInProvider waveIn = null;


        public Speaker()
        {
            
        }


        public void PlayStream(NAudio.Wave.WaveIn sourceStream)
        {
            if (waveOut != null)
            {
                waveOut.Stop();
                waveOut.Dispose();
                waveOut = null;
            }
            if (sourceStream == null) return;
            waveOut = new NAudio.Wave.DirectSoundOut();
            waveIn = new NAudio.Wave.WaveInProvider(sourceStream);
            waveOut.Init(waveIn);
            waveOut.Play();
        }




    }
}
