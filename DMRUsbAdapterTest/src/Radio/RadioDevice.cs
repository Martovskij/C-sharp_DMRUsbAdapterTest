using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DMRUsbAdapterTest.src.Radio
{
    class RadioDevice : AbstractRadio
    {
 
        RadioService radioService = null;
        public String ip {get;set;}
        int id = 0;
        public bool IsConnected = false;

        public RadioDevice(RadioService radioserv, String ip)
        {
            if (radioserv == null) throw new ArgumentNullException();
            if (ip == null) throw new ArgumentNullException();
            this.ip = ip;
                 
            this.radioService = radioserv;
            String[] splitted = ip.Split('.');
            id = Int32.Parse(splitted[3]);
            id = id | (0xff00 & (byte.Parse(splitted[2]) << 8));
            id = id | (0xff0000 & (byte.Parse(splitted[1]) << 16));
        }

        public void Connect()
        {
           
        }

        public void PressPttRequest()
        {

        }

        public void ReleasePttRequest()
        {

        }


    }
}
