using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DMRUsbAdapterTest.src.UI
{
    public partial class WarningWindow : Form
    {
        MainWindow mainWindow;
        
        public WarningWindow(MainWindow main, String message)
        {
            if (main != null) this.mainWindow = main;
            
            InitializeComponent();
            //this.messageLabel.Text = message;
            Application.Run(this);
           
        }

        private void okButtonClick(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
