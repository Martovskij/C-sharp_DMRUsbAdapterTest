using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.CompilerServices;
using NAudio;
using NAudio.WindowsMediaFormat;
using NAudio.Wave;


namespace DMRUsbAdapterTest.src.Sound
{
    class OutputLines : InterfaceLines<WaveOutCapabilities>
    {
        public List<Line<WaveOutCapabilities>> audioDeviceList = null;
        public static OutputLines instance = null;

        OutputLines()
        {
            audioDeviceList = new List<Line<WaveOutCapabilities>>();
        }

        [MethodImplAttribute(MethodImplOptions.Synchronized)]
        public static OutputLines getInstance()
        {
            if (instance == null)
            {
                instance = new OutputLines();
                return instance;
            }
            else return instance;
        }



        public void RefreshDeviceList()
        {
            int waveOutDevices = WaveOut.DeviceCount;
            audioDeviceList.Clear();
            for (int i = 0; i < waveOutDevices; i++ )
            {
                Line<WaveOutCapabilities> line = new Line<WaveOutCapabilities>();
                line.index = i;
                line.name = WaveOut.GetCapabilities(i).ProductName;
                line.line = WaveOut.GetCapabilities(i);
                audioDeviceList.Add(line);  
            }
        }

        public WaveOutCapabilities getLineByIndex(int index)
        {
            return audioDeviceList.ElementAt(index).line;
        }

        public int getDeviceNumCount()
        {
            return audioDeviceList.Count;
        }

    }


}
