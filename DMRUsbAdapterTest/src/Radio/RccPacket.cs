using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DMRUsbAdapterTest.src.Radio
{

    enum GenerateMode {CONNECT_PACK,DATA_PACK};
    enum Opcode{DATA,DATA_ACK,CONNECTION_ACCEPT,CONNECTION_REJECT,CONNECTION_CLOSE,CONNECTION_CLOSE_ACK};

    class RccPacket
    {
        byte[] data = null;
        public RccPacket(byte[] data)
        {
            if (data == null)
                this.data = new byte[1024];
            else this.data = data;
        }

        public RccPacket(GenerateMode mode)
        {
            switch (mode)
            {
                case GenerateMode.CONNECT_PACK:
                    break;

                case GenerateMode.DATA_PACK:
                    break;
            }
        }

        public Opcode getOpcode()
        {
            return Opcode.CONNECTION_ACCEPT;
        }

        public byte[] getData()
        {
            return data;
        }

        public bool IsRccPacket()
        {
            return true;
        }

        public bool IsChkSumOk()
        {
            return true;
        }

    }



}
