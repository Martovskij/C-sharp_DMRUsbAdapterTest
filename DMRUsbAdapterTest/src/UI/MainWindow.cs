using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using DMRUsbAdapterTest.src.Sound;
using log4net;
using log4net.Config;

namespace DMRUsbAdapterTest
{
    public partial class MainWindow : Form
    {

        public static readonly ILog log = LogManager.GetLogger(typeof(MainWindow)); 

        public MainWindow()
        {
            InitializeComponent();
            this.MaximizeBox = false;
            this.MinimizeBox = false;
          //  log.Debug("start application");
        }

        private void audioLineRefreshHandler(object sender, EventArgs e)
        {
            //clr audio list
            micBox.Items.Clear();
            speakBox.Items.Clear();

            SoundManager.getInstance().ReloadAudioDeviceList();
            List<String> micList = SoundManager.getInstance().getMicrophonesList();
            List<String> speakList = SoundManager.getInstance().getSpeakerList();

            for (int i = 0; i < micList.Count; i++ )
            {
                micBox.Items.Add(micList.ElementAt(i));
            }

            for (int i = 0; i < speakList.Count; i++)
            {
                speakBox.Items.Add(speakList.ElementAt(i));
            }
        }

        private void exitHandler(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void changeMicHandler(object sender, EventArgs e)
        {
            //
        }

        private void changeSpeakHandler(object sender, EventArgs e)
        {
            //
        }

    }
}
