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
    class InputLines : InterfaceLines<WaveInCapabilities>
    {
        public List<Line<WaveInCapabilities>> audioDeviceList = null;
        static InputLines instance = null;
        int selectedDeviceIndex = -1;



        InputLines()
        {
            audioDeviceList = new List<Line<WaveInCapabilities>>();
        }

        [MethodImplAttribute(MethodImplOptions.Synchronized)]
        static public InputLines getInstance()
        {
            if (instance == null)
            {
                instance = new InputLines();
                return instance;
            }
            else return instance;
        }

        public void RefreshDeviceList()
        {
            int waveInDevices = WaveIn.DeviceCount;
            audioDeviceList.Clear();
            for (int i = 0; i < waveInDevices; i++ )
            {
                Line<WaveInCapabilities> line = new Line<WaveInCapabilities>();
                line.index = i;
                line.name = WaveIn.GetCapabilities(i).ProductName; 
                line.line = WaveIn.GetCapabilities(i);
                audioDeviceList.Add(line);  
            }
        }

        public int getSelectedDeviceIndex()
        {
            return selectedDeviceIndex;
        }

        public void setSelectedDeviceIndex(int index)
        {
            RefreshDeviceList();
            if (index > audioDeviceList.Count) throw new ArgumentOutOfRangeException();
            selectedDeviceIndex = index;
        }

        public WaveInCapabilities getLineByIndex(int index)
        {
            return audioDeviceList.ElementAt(index).line;
        }

        public int getDeviceNumCount()
        {
            return audioDeviceList.Count;
        }

    }
}
