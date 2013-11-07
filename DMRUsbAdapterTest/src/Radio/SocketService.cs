using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using log4net;
using log4net.Config;
using System.Net.NetworkInformation;



namespace DMRUsbAdapterTest.src.Radio
{
     public class SocketService 
    {

        Socket udpSocket = null;
        Thread currThread;
        ISocketHandle handlerService;
        IPEndPoint sender;
        EndPoint Remote;
        public static readonly ILog log = LogManager.GetLogger(typeof(SocketService));
        Int16 PORT = 3005;
        Ping ping = new Ping();

        public SocketService(ISocketHandle handle)
        {
            if (handle == null) log.Debug("socket recv event handler is null");
            this.handlerService = handle;
            udpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            IPEndPoint ipep = new IPEndPoint(IPAddress.Any, PORT);
            sender =  new IPEndPoint(IPAddress.Any, 0);
            Remote = (EndPoint)(sender);
            udpSocket.Bind(ipep);
            currThread = new Thread(run);
            currThread.Start();
        }

        public bool Ping(String ip)
        {
            byte[] addr = new Byte[4];
            String[] splitted = ip.Split('.');
            addr[0] = Byte.Parse(splitted[0]);
            addr[1] = byte.Parse(splitted[1]);
            addr[2] = byte.Parse(splitted[2]);
            addr[3] = byte.Parse(splitted[3]);
            IPAddress ip_addr = new IPAddress(addr);
            if (ping.Send(ip_addr).Status != IPStatus.Success)
                return false;
            else return true;    
        }


        public void WriteToSocket(byte[] data, String ip)
        {
            try
            {
                if (ip == null)
                {
                    log.Debug("ip is null");
                    return;
                }
               byte[] addr = new Byte[4];
               String[] splitted = ip.Split('.'); 
               addr[0] = Byte.Parse(splitted[0]);
               addr[1] = byte.Parse(splitted[1]);
               addr[2] = byte.Parse(splitted[2]);
               addr[3] = byte.Parse(splitted[3]);
               udpSocket.SendTo(data, new IPEndPoint(new IPAddress(addr),PORT));
            }
            catch(Exception ex)
            {
                log.Debug(ex.Message);
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
                        if (handlerService != null)
                        handlerService.AddToHandle(new RccPacket(data));
                }
                catch(Exception ex)
                {
                    log.Debug(ex);
                    Console.WriteLine(ex);
                }
            }
        }



    }
}
