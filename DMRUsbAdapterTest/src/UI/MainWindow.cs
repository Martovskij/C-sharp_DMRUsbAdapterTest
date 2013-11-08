using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using DMRUsbAdapterTest.src.Sound;
using DMRUsbAdapterTest.src.Radio;
using DMRUsbAdapterTest.src.Kernel;
using log4net;
using log4net.Config;

namespace DMRUsbAdapterTest
{
    public partial class MainWindow : Form, src.Kernel.AudioDataObserver
    {

        public static readonly ILog log = LogManager.GetLogger(typeof(MainWindow));
        Kernel kernel;


        public MainWindow(Kernel kernel)
        {
            InitializeComponent();
            ;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.kernel = kernel;
        }


/**------------------------------------------------------------------------**/
/**------------------------------ HANDLERS ------------------------------- **/
/**------------------------------------------------------------------------**/

        private void audioLineRefreshHandler(object sender, EventArgs e)
        {
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

            if (micList.Count <= SoundManager.getInstance().SelectedMicrophoneIndex)
            {
                micBox.SetSelected(SoundManager.getInstance().SelectedMicrophoneIndex,true);
                if (!SoundManager.getInstance().StartRecord())
                {
                    SoundManager.getInstance().SelectedMicrophoneIndex = 0;
                    micBox.SelectedIndex = 0;
                    new src.UI.WarningWindow("warning window");
                }
            }

            else SoundManager.getInstance().SelectedMicrophoneIndex = -1;

            if(speakList.Count <= SoundManager.getInstance().SelectedSpeakerIndex)
            {
                micBox.SetSelected(SoundManager.getInstance().SelectedSpeakerIndex,true);
            }
        

        }



        private void exitHandler(object sender, EventArgs e)
        {
            kernel.socketService.CloseSocket();
            SoundManager.getInstance().CloseAllDevices();
            kernel.radioService.CloseService();
            Application.Exit();
        }

        private void changeMicHandler(object sender, EventArgs e)
        {
            SoundManager.getInstance().StopRecord();
            SoundManager.getInstance().SelectedMicrophoneIndex = micBox.SelectedIndex;
            if (!SoundManager.getInstance().StartRecord(kernel.MicToSpeakOption))
            {
                new src.UI.WarningWindow("Линия захвата недоступна.\nВыберите другую линию захвата в меню.");
            };
        }

        private void changeSpeakHandler(object sender, EventArgs e)
        {
            SoundManager.getInstance().SelectedSpeakerIndex = speakBox.SelectedIndex;
        }


/**------------------------------------------------------------------------**/

        private void connect(object sender, EventArgs e)
        {
            if (textBox1.Equals(""))
            {
                textBox1.BackColor = Color.Red;
                return;
            }
            String[] splitted = textBox1.Text.Split('.');
            byte[] addr = new byte[4];
            try
            {
                addr[0] = Byte.Parse(splitted[0]);
                addr[1] = byte.Parse(splitted[1]);
                addr[2] = byte.Parse(splitted[2]);
                addr[3] = byte.Parse(splitted[3]);
                System.Net.IPAddress ip = new System.Net.IPAddress(addr);
                System.Net.IPEndPoint end = new System.Net.IPEndPoint(ip,3005);
            }
            catch(Exception ex)
            {
                textBox1.BackColor = Color.Red;
                log.Error("crush");
                return;
            }

            textBox1.BackColor = Color.White;
            radioStateIndicator.Value = 0;
            src.Kernel.Kernel.getInstance().ChangeRadioDevice(textBox1.Text);
            src.Kernel.Kernel.getInstance().radioService.GenerateConnect();
        }


        public void InitSoundPanel()
        {
            try
            {
                audioLineRefreshHandler(null, null);
                if (SoundManager.getInstance().getSpeakerList().Count > 0) speakBox.SetSelected(0, true);
                if (SoundManager.getInstance().getMicrophonesList().Count > 0) micBox.SetSelected(0, true);
            }
            catch(Exception ex)
            {
                log.Debug(ex.Message);
                log.Debug(ex.StackTrace);
            }

        }

        public void SetRadioOnline()
        {
            radioStateIndicator.Value = 1;
        }


        public void SetRadioOffline()
        {
            radioStateIndicator.Value = 0;
        }

        public void notify(object obj)
        {
            if (obj == null) return;
            if (obj.GetType() == typeof(byte[]))
            {
                byte[] data = (byte[])obj;
                short currentValue = 0;
                short maxValue = 0;
                for (int i = 0; i < data.Length; i+=2)
                {
                    if ((i + 2) > data.Length) break;
                    currentValue = (short)(data[i] & 0x00ff);
                    currentValue |= (short)(0xff00 & (data[i + 1] << 8));
                    if (maxValue < currentValue) maxValue = currentValue;
                    currentValue = 0;
                }
                micBar.Value = maxValue;
            }
        }


        public void AviableData(byte[] buffer)
        {
            if (buffer == null) return;
            if ((buffer.Length == 1) & (buffer[0] == 0))
            {
                button3.Enabled = true;
                if (kernel.radioDevice != null)
                   if (kernel.radioDevice.IsConnected)
                      kernel.radioService.GenerateReleasePtt();
                spkBar.Value = 0;
                return;
            }
            short currentValue = 0;
            short maxValue = 0;
            for (int i = 0; i < buffer.Length; i += 2)
            {
                if ((i + 2) > buffer.Length) break;
                currentValue = (short)(buffer[i] & 0x00ff);
                currentValue |= (short)(0xff00 & (buffer[i + 1] << 8));
                if (maxValue < currentValue) maxValue = currentValue;
                currentValue = 0;
            }
            spkBar.Value = maxValue;
        }



        private void StartCheckSound(object sender, EventArgs e)
        {
            button3.Enabled = false;
             if(kernel.radioDevice!=null)
              if (kernel.radioDevice.IsConnected)
                  kernel.radioService.GeneratePressPtt();

             if (!SoundManager.getInstance().StartPlaySound("audiotest.wav"))
             {
                 button3.Enabled = true;
                 new src.UI.WarningWindow("Линия вывода недоступна.\nЛибо отсутствует или некорректен\nтестовый аудиофайл\nВыберите другую линию вывода в меню.");
             }
        }

        private void setnewstate(object sender, EventArgs e)
        {
            kernel.MicToSpeakOption = checkBox1.Checked;
            src.Sound.SoundManager.getInstance().StopRecord();
            src.Sound.SoundManager.getInstance().StartRecord(kernel.MicToSpeakOption);
        }


        private void MainWindow_FormClosed(object sender, FormClosingEventArgs e)
        {
            kernel.socketService.CloseSocket();
            SoundManager.getInstance().CloseAllDevices();
            kernel.radioService.CloseService();
            Application.Exit();
        }

    }
}
