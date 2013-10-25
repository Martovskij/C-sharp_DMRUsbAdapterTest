using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DMRUsbAdapterTest.src.Radio
{
    class RadioDevice : AbstractRadio
    {
        RadioDevice instance = null;


        RadioDevice() { }


        public RadioDevice getInstance()
        {
            if (instance == null)
            {
                instance = new RadioDevice();
                return instance;
            }
            else return instance;
        }

        public void connect()
        {

        }

    }
}
