using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using DMRUsbAdapterTest.src.Kernel;
using DMRUsbAdapterTest.src.UI;
using log4net;
using log4net.Config;

namespace DMRUsbAdapterTest
{
    static class DMRUsbAdapterTest
    {

        public static readonly ILog log = LogManager.GetLogger(typeof(DMRUsbAdapterTest)); 
        [STAThread]
        static void Main()
        {
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                // init kernel
                Kernel.getInstance().mainWindow = new MainWindow();
                log4net.Config.XmlConfigurator.Configure();
                log.Debug("start application");
               // Kernel.getInstance().radioService = new src.Radio.RadioService();
               // Kernel.getInstance().socketService = new src.Radio.SocketService(Kernel.getInstance().radioService);
                Application.Run(Kernel.getInstance().mainWindow);
            }
            catch(Exception ex)
            {
               log.Error(ex.Message);
                Console.WriteLine(ex);
                new WarningWindow(null,ex.ToString());
            }

        }
    }
}
