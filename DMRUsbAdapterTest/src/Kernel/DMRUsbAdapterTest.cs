using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using DMRUsbAdapterTest.src.Kernel;
using DMRUsbAdapterTest.src.UI;

namespace DMRUsbAdapterTest
{
    static class DMRUsbAdapterTest
    {
        

        [STAThread]
        static void Main()
        {



            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                // init kernel
                Kernel.getInstance().mainWindow = new MainWindow();
                Kernel.getInstance().radioService = new src.Radio.RadioService();
                Kernel.getInstance().socketService = new src.Radio.SocketService(Kernel.getInstance().radioService);

                Application.Run(Kernel.getInstance().mainWindow);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                new WarningWindow(null,ex.ToString());
            }

        }
    }
}
