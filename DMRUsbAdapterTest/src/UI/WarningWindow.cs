using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;


namespace DMRUsbAdapterTest.src.UI
{
    class WarningWindow 
    {

        public WarningWindow(String message)
        {
           
            new Thread(new ThreadStart(delegate
                {
                    MessageBox.Show(message,"Закрыть", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                })).Start();
        }





    }
}
