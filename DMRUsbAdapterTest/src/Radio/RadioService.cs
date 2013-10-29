using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using log4net;
using log4net.Config;
using DMRUsbAdapterTest.src.Kernel;

namespace DMRUsbAdapterTest.src.Radio
{


    class RadioService : ISocketHandle
    {
        Socket udpSocket = null;
        Thread currThread;
        Thread reachableThread;

        List<RccPacket> packetQueue = new List<RccPacket>(),
                        tempQueue = new List<RccPacket>();

        static readonly ILog log = LogManager.GetLogger(typeof(RadioService));
        src.Kernel.Kernel kernel = null;


        public RadioService(src.Kernel.Kernel kernel)
        {
          
            if (kernel == null) throw new ArgumentNullException();
            this.kernel = kernel;
           
            IPEndPoint ipep = new IPEndPoint(IPAddress.Any, 9051);
            try
            {
                udpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                udpSocket.Bind(ipep);
            }
            catch (Exception ex)
            {
                log.Debug(ex);
                new src.UI.WarningWindow(null,"Ошибка открытия порта\nОсвободите порт 3005 и перезапустите программу");
            }
            currThread = new Thread(run);
            currThread.Name = "RadioMessageQueue";
            currThread.Start();
            reachableThread = new Thread(Ping);
            reachableThread.Start();
        }



/*=================================================================================*/
/*------------------------------------ API METHODS --------------------------------*/
/*=================================================================================*/

        public void GenerateACK()
        {
            byte[] Packet = new byte[12];
            Packet[0] = 0x7e;
            Packet[1] = 0x00;   //version
            Packet[2] = (byte)0x00; //block
            Packet[3] = (byte)0x10; //ack 
            Packet[4] = 0x20;   //src id
            Packet[5] = (byte)0x10; //dest id (10-master)
            Packet[6] = 0x00; //PN
            Packet[7] = 0x00; //PN
            Packet[8] = 0x00;   //length
            Packet[9] = 0x0C;   //length
            Packet[10] = 0x00;
            Packet[11] = 0x00;
            int checksum = GetChecksum(Packet, 10);
            Packet[10] = (byte)((checksum >> 8) & 0xFF);
            Packet[11] = (byte)(checksum & 0xFF);
            if (kernel.socketService != null)
                kernel.socketService.WriteToSocket(Packet, kernel.radioDevice.ip);
            else log.Debug("socket service not initialized");
        }



        public void GenerateConnect()
        {
            byte[] Packet = new byte[12];
            Packet[0] = 0x7e;
            Packet[1] = 0x00;   //version
            Packet[2] = (byte)0x00; //block
            Packet[3] = (byte)0xFE; //connect 
            Packet[4] = 0x20;   //src id
            Packet[5] = (byte)0x10; //dest id (10-master)
            Packet[6] = 0x00; //PN
            Packet[7] = 0x00; //PN
            Packet[8] = 0x00;   //length
            Packet[9] = 0x0C;   //length
            Packet[10] = 0x00;
            Packet[11] = 0x00;
            int checksum = GetChecksum(Packet, 10);
            Packet[10] = (byte)((checksum >> 8) & 0xFF);
            Packet[11] = (byte)(checksum & 0xFF);
            if (kernel.socketService != null)
                kernel.socketService.WriteToSocket(Packet, kernel.radioDevice.ip);
            else log.Debug("socket service not initialized");
        }



        public void GeneratePressPtt()
        {

            byte[] Packet = new byte[21];
            // HRNP HEADER
            Packet[0] = 0x7e;
            Packet[1] = 0x00;   //version
            Packet[2] = (byte)0x00; //block
            Packet[3] = (byte)0x00; //type data
            Packet[4] = 0x20;   //src id
            Packet[5] = (byte)0x10; //dest id (10-master)
            Packet[6] = 0x00; //PN
            Packet[7] = 0x00; //PN
            Packet[8] = 0x00;   //length
            Packet[9] = 0x15;  //// + rcc pack len
            Packet[10] = 0x5A;
            Packet[11] = 0xA9;

            // RCC HEADER
            Packet[12] = 0x02;   // rccheader 
            Packet[13] = 0x41;   // opcode
            Packet[14] = 0x00;   // opcode
            Packet[15] = 0x02;   // len
            Packet[16] = 0x00;   // len
            Packet[17] = 0x1E;   //ptt // target 1E
            Packet[18] = 0x01;   //press // action 01
            Packet[19] = 0xD0;   // check sum
            Packet[20] = 0x03;   // rcc end


            if (kernel.socketService != null)
                kernel.socketService.WriteToSocket(Packet, kernel.radioDevice.ip);
            else log.Debug("socket service not initialized");
        }


        public void GenerateReleasePtt()
        {
            byte[] Packet = new byte[21];
            // HRNP HEADER
            Packet[0] = 0x7e;
            Packet[1] = 0x00;   //version
            Packet[2] = (byte)0x00; //block
            Packet[3] = (byte)0x00; //type data
            Packet[4] = 0x20;   //src id
            Packet[5] = (byte)0x10; //dest id (10-master)
            Packet[6] = 0x00; //PN
            Packet[7] = 0x00; //PN
            Packet[8] = 0x00;   //length
            Packet[9] = 0x15;  //// + rcc pack len
            Packet[10] = 0x5A;
            Packet[11] = 0xA9;

            // RCC HEADER
            Packet[12] = 0x02;   // rccheader 
            Packet[13] = 0x41;   // opcode
            Packet[14] = 0x00;   // opcode
            Packet[15] = 0x02;   // len
            Packet[16] = 0x00;   // len
            Packet[17] = 0x1E;   //ptt // target 1E
            Packet[18] = 0x00;   //press // action 01
            Packet[19] = 0xD0;   // check sum
            Packet[20] = 0x03;   // rcc end


            if (kernel.socketService != null)
                kernel.socketService.WriteToSocket(Packet, kernel.radioDevice.ip);
            else log.Debug("socket service not initialized");
        }


/*=================================================================================*/
/*--------------------------------- UTIL METHODS ----------------------------------*/
/*=================================================================================*/

        public int GetChecksum(byte[] packet, int len)
        {
            int Checksum = 0;
            int i = 0;
            while (len > 1)
            {
                if (i != 10)
                {
                    int add = ((packet[i] << 8) | ((int)(packet[i + 1])) & 0xFF) & 0xFFFF;
                    Checksum += add;
                    if ((Checksum >> 16) != 0)
                        Checksum = (Checksum & 0xFFFF) + (Checksum >> 16);
                }
                i += 2;
                len -= 2;
            }
            if (len > 0) Checksum += (((int)packet[i]) & 0xFF) << 8;

            while ((Checksum >> 16) > 0) Checksum = (Checksum & 0xFFFF) + (Checksum >> 16);
            return (~Checksum) & 0xFFFF;
        }




        byte[] GenerateDataPacket(byte[] rcc_pack)
        {
            byte[] Packet = new byte[12 + rcc_pack.Length];
            Packet[0] = 0x7e;
            Packet[1] = 0x00;   //version
            Packet[2] = (byte)0x00; //block
            Packet[3] = (byte)0x00; //type data
            Packet[4] = 0x20;   //src id
            Packet[5] = (byte)0x10; //dest id (10-master)
            Packet[6] = 0x00; //PN
            Packet[7] = 0x00; //PN
            Packet[8] = 0x00;   //length
            Packet[9] = (byte)(0x0C + rcc_pack.Length);   //length  
            Packet[10] = 0;
            Packet[11] = 0;
            for (int i = 0; i < rcc_pack.Length; i++)
            {
                Packet[12 + i] = rcc_pack[i];
            }
            int checksum = GetChecksum(Packet, Packet.Length);
            Packet[10] = (byte)((checksum >> 8) & 0xFF);
            Packet[11] = (byte)(checksum & 0xFF);
            return Packet;
        }


/*=================================================================================*/
/*------------------------ Interface implemented method ---------------------------*/
/*=================================================================================*/


        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddToHandle(Object obj)
        {
            try
            {
                if (obj.GetType() == typeof(RccPacket))
                {
                    log.Debug("add pack to queue handle");
                    packetQueue.Add((RccPacket)obj);
                }
                else
                {
                    log.Debug("warn type");
                }
            }
            catch (Exception ex)
            {
                log.Debug(ex);
            }
        }

/*--------------------------------------------------------------------------------*/
/*--------------------------------- QUEUE HANDLER --------------------------------*/
/*--------------------------------------------------------------------------------*/

        public void run()
        {
            while(currThread.IsAlive)
            {
                if (packetQueue.Count > 0)
                {

                    tempQueue = packetQueue.ToList();
                    packetQueue.Clear();
                    for (int i = 0; i < tempQueue.Count; i++ )
                    {

                        RccPacket packet = tempQueue.ElementAt(i);
                        if (packet.IsHrnpPacket())
                        {

                            switch (packet.getOpcode())
                            {
                                case Opcode.CONNECTION_ACCEPT:
                                    log.Debug("connection ok");
                                    kernel.radioDevice.IsConnected = true;
                                    kernel.mainWindow.SetRadioOnline();
                                    new src.UI.WarningWindow(null, "подключение принято");
                                    break;

                                case Opcode.CONNECTION_REJECT:
                                    kernel.radioDevice.IsConnected = false;
                                    kernel.mainWindow.SetRadioOffline();
                                    log.Debug("connection reject");
                                    new src.UI.WarningWindow(null, "подключение отклонено");
                                    break;

                                case Opcode.DATA:
                                    break;

                                case Opcode.DATA_ACK:
                                    break;
                            }


                            if (packet.IsRccPacket())
                            {


                                
                            }
                        }

                    }
                    tempQueue.Clear();

                }
                else
                {
                    Thread.Sleep(50);
                }


            }
        }

/*--------------------------------------------------------------------------------*/
/*------------------------------ REACHEABLE HANDLER ------------------------------*/
/*--------------------------------------------------------------------------------*/

        public void Ping()
        {
            while(Thread.CurrentThread.IsAlive)
            {
                try
                {
                    if (kernel.radioDevice.IsConnected)
                        if (!kernel.socketService.Ping(kernel.radioDevice.ip))
                        {
                            kernel.radioDevice.IsConnected = false;
                            kernel.mainWindow.SetRadioOffline();
                        }
                    Thread.Sleep(3000);
                }
                catch(Exception ex)
                {
                    log.Debug(ex);
                }
            }
        }
    }



}
