using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace DMRUsbAdapterTest.src.Radio
{


    class RadioService
    {
        Socket udpSocket = null;
        Thread currThread;
        List<RccPacket> packetQueue = new List<RccPacket>(),
                        tempQueue = new List<RccPacket>();


        public RadioService()
        {

            udpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            IPEndPoint ipep = new IPEndPoint(IPAddress.Any, 9051);
            udpSocket.Bind(ipep);
            currThread = new Thread(run);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddToQueue(RccPacket packet)
        {
            packetQueue.Add(packet);
        }



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
                        if(packet.IsRccPacket())
                        {
                            if(packet.IsChkSumOk())
                            {
                                switch(packet.getOpcode())
                                {
                                    case Opcode.CONNECTION_ACCEPT:
                                        break;
                                    
                                    case Opcode.CONNECTION_CLOSE:
                                        break;

                                    case Opcode.CONNECTION_CLOSE_ACK:
                                        break;

                                    case Opcode.CONNECTION_REJECT:
                                        break;

                                    case Opcode.DATA:
                                        break;

                                    case Opcode.DATA_ACK:
                                        break;

                                }
                            }
                        }

                    }
                    tempQueue.Clear();

                }
                else
                {
                    Thread.Sleep(5);
                }


            }
        }






    }



}
