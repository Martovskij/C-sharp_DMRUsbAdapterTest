using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DMRUsbAdapterTest.src.Kernel
{
    interface AudioDataObserver
    {
        void notify(Object data);
    }
}
