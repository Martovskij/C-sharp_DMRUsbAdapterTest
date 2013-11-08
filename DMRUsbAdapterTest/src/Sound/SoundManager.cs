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
        WaveOutput wavePlayer = null;
        Speaker speaker = null;


        public int SelectedMicrophoneIndex {set;get;}
        public int SelectedSpeakerIndex {set;get;}

        SoundManager() 
        {
            ReloadAudioDeviceList();
            SelectedMicrophoneIndex = 0;
            SelectedSpeakerIndex = 0;
            microphoneCaptor = new MicrophoneCaptor(null);
            speaker = new Speaker();
            wavePlayer = new WaveOutput();
        }

        public void SetupCaptorHandler(Kernel.AudioDataObserver observer)
        {
            if (observer == null) throw new ArgumentNullException();
            microphoneCaptor.SetupMicrophoneObserver(observer);
        }

        public bool StartRecord() 
        {
            if (microphoneCaptor != null)
            {
                if (!microphoneCaptor.StartRecord(SelectedMicrophoneIndex))
                {
                    return false;
                }
                speaker.PlayStream(microphoneCaptor.GetInputStream());
                return true;
            }
            return false;
        }

        public void StopRecord()
        {
            if (microphoneCaptor != null)
                microphoneCaptor.StopRecord();
        }

        public bool StartPlaySound(String pathToFile)
        {
            return wavePlayer.Play(pathToFile,SelectedSpeakerIndex);
        }

        public void SetReadingDataHandler(D_BufferedDataCallback callBack)
        {
            wavePlayer.SetAviableDataHandler(callBack);
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
            SelectedMicrophoneIndex = index;
            InputLines.getInstance().setSelectedDeviceIndex(index);
        }

        public void changeSelectedSpeak(int index)  // Warning: throws ArgumentIndexOutOfRangeException
        {
            SelectedSpeakerIndex = index;
            OutputLines.getInstance().setSelectedDeviceIndex(index);
        }

    }
}
