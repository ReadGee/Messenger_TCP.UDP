using Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Newtonsoft.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Messenger.TestingDev
{
    public class ClientUDP
    {
        static UdpClient udpClient = new UdpClient();

        private static void udpClientStart()
        {
            Console.WriteLine("Client");
            udpClient.Connect("127.0.0.1", 10000);
            

            while (true)
            {
                Byte[] sendBytes = Encoding.UTF8.GetBytes(Console.ReadLine());
                udpClient.Send(sendBytes, sendBytes.Length);
                if(sendBytes == Encoding.UTF8.GetBytes("Stop")) break;
                Thread.Sleep(1000);
            }
        }
        public static void udpClientStart(bool ActivateThead)
        {
            if (ActivateThead)
            {
                Thread DevUdpClientStart = new Thread(() => udpClientStart());
                Thread DevUdpServerStart = new Thread(() => ServerUDP.udpServerStart());
                DevUdpClientStart.Start();
                DevUdpServerStart.Start();
            }
            else
            {
                udpClientStart();
            }
        }
    }
}
