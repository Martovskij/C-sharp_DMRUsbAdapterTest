using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using NAudio;
using NAudio.WindowsMediaFormat;
using NAudio.Wave;


namespace DMRUsbAdapterTest.src.Sound
{
    class Line<T>
    {
        public String name = "";
        public int index = 0;
        public T line;
    }
}
