using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;


namespace DMRUsbAdapterTest.src.Radio
{
    class SocketService
    {

        Socket udpSocket = null;
        Thread currThread;
        RadioService handlerService;
        IPEndPoint sender;
        EndPoint Remote;

        public SocketService(RadioService radio)
        {
            this.handlerService = radio;
            udpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            IPEndPoint ipep = new IPEndPoint(IPAddress.Any, 3005);
            sender =  new IPEndPoint(IPAddress.Any, 0);
            Remote = (EndPoint)(sender);
            udpSocket.Bind(ipep);
            currThread = new Thread(run);
            currThread.Start();
        }

        public void WriteToSocket(byte[] data)
        {
            try
            {
                udpSocket.Send(data);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public void run()
        {
            while(currThread.IsAlive)
            {
                try
                {
                    int recvLen = 0;
                    byte[] data = new byte[1024];
                    recvLen = udpSocket.ReceiveFrom(data, ref Remote);
                    if (recvLen > 1024) continue;
                    else
                        handlerService.AddToQueue(new RccPacket(data));
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }



    }
}
