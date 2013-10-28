using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NAudio;
using NAudio.WindowsMediaFormat;
using NAudio.Wave;
using System.Runtime.CompilerServices;
using NUnit.Framework;


namespace DMRUsbAdapterTest.src.Sound
{
    class SoundManager
    {
        static SoundManager instance = null;
        MicrophoneCaptor microphoneCaptor = null;


        SoundManager() 
        { 
                
        }

        public void SetupCaptorHandler(Kernel.AudioDataObserver observer)
        {
            if (observer == null) throw new ArgumentNullException();
            microphoneCaptor = new MicrophoneCaptor(observer);
        }


        [MethodImplAttribute(MethodImplOptions.Synchronized)]
        public static SoundManager getInstance()
        {
            
            if (instance == null)
            {
                instance = new SoundManager();
                return instance;
            }
            else return instance;
        }


        public void ReloadAudioDeviceList()
        {
            InputLines.getInstance().RefreshDeviceList();
            OutputLines.getInstance().RefreshDeviceList();
        }

        public List<String> getMicrophonesList()
        {
            List<String> list = new List<String>();
            for (int i = 0; i < InputLines.getInstance().getDeviceNumCount(); i++)
            {
                list.Add(InputLines.getInstance().getLineByIndex(i).ProductName);
            }
            return list;
        }

        public List<String> getSpeakerList()
        {
            List<String> list = new List<String>();
            for (int i = 0; i < OutputLines.getInstance().getDeviceNumCount(); i++)
            {
                list.Add(OutputLines.getInstance().getLineByIndex(i).ProductName);
            }
            return list;
        }

        public void changeSelectedMic(int index)  // Warning: throws ArgumentIndexOutOfRangeException
        {
            InputLines.getInstance().setSelectedDeviceIndex(index);
        }

        public void changeSelectedSpeak(int index)  // Warning: throws ArgumentIndexOutOfRangeException
        {
            OutputLines.getInstance().setSelectedDeviceIndex(index);
        }

    }
}
