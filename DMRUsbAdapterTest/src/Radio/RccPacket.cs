using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DMRUsbAdapterTest.src.Radio
{

    enum GenerateMode {CONNECT_PACK,DATA_PACK};
    enum Opcode{DATA = 0x00,DATA_ACK=0x10,CONNECTION_ACCEPT=0xFD,CONNECTION_REJECT=0xFC, UNAVIABLE_CMD = -1};

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
            try
            {
                switch (data[3])
                {
                    case 0x00:
                        return Opcode.DATA;

                    case 0x10:
                        return Opcode.DATA_ACK;

                    case 0xFD:
                        return Opcode.CONNECTION_ACCEPT;

                    case 0xFC:
                        return Opcode.CONNECTION_REJECT;

                    default:
                        return Opcode.UNAVIABLE_CMD;
                }
            }
            catch(Exception ex)
            {
                  Console.WriteLine(ex);
                  return Opcode.UNAVIABLE_CMD;
            }
        }

        public byte[] getData()
        {
            return data;
        }

        public bool IsHrnpPacket()
        {
            if (data[0] == 0x7e) return true;
            else return false;
        }




        public bool IsRccPacket()
        {
            if (data[12] == 0x02)
            {
                return true;
            }
            else return false;
        }

        public bool IsChkSumOk()
        {
            return true;
        }

    }



}
