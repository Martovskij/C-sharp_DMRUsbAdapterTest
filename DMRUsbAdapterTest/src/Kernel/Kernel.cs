using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DMRUsbAdapterTest.src.Radio;
using log4net;
using log4net.Config;
using DMRUsbAdapterTest.src.Sound;


namespace DMRUsbAdapterTest.src.Kernel 
{
      public class Kernel 
    {
        public MainWindow mainWindow;
        public RadioDevice radioDevice;
        public SocketService socketService;
        public RadioService radioService;
        static Kernel instance = null;




        Kernel()
        {
            instance = this;
            mainWindow = new MainWindow(Kernel.getInstance());
            radioService = new RadioService(Kernel.getInstance());
            socketService = new SocketService(Kernel.getInstance().radioService);
            SoundManager.getInstance().SetupCaptorHandler(mainWindow);
            SoundManager.getInstance().SetReadingDataHandler(mainWindow.AviableData);
            mainWindow.InitSoundPanel();
        }


        public static Kernel getInstance()
        {
            if (instance == null)
            {
                instance = new Kernel();
                return instance;
            }
            else return instance;
        }


        public void ChangeRadioDevice(String newIp)
        {
            radioDevice = new RadioDevice(newIp);
        }


    }
}
