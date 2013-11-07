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
        public static readonly ILog log = LogManager.GetLogger(typeof(DMRUsbAdapterTest).ToString()); 
        [STAThread]
        static void Main()
        {
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                // init kernel
                log4net.Config.XmlConfigurator.Configure();
                log.Debug("start application");
                Kernel.getInstance(); // init kernel
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
