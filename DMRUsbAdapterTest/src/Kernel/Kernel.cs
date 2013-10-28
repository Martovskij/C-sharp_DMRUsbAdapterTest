using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DMRUsbAdapterTest.src.Radio;
using log4net;
using log4net.Config;

namespace DMRUsbAdapterTest.src.Kernel 
{
    class Kernel : AudioDataObserver
    {
        public MainWindow mainWindow;
        public RadioDevice radioDevice;
        public SocketService socketService;
        public RadioService radioService;
        static Kernel instance = null;


        public static Kernel getInstance()
        {
            if (instance == null)
            {
                instance = new Kernel();
                return instance;
            }
            else return instance;
        }

        public void notify(object data)
        {
            //mainWindo

        }


    }
}
