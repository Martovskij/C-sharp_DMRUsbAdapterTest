using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DMRUsbAdapterTest.src.Sound
{
    interface InterfaceLines<T>
    {
        void RefreshDeviceList();
       
        T getLineByIndex(int index);

        int getDeviceNumCount();
    }
}
