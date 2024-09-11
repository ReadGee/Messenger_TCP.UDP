using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Server.Models;

namespace Messenger.TestingDev
{
    public class ServerUDP
    {
        public static bool isSignal = false;

        public static void udpServerStart() 
        {
            Console.WriteLine("Server");

            IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Any, 10000); // Используем порт 11001 для сервера
            UdpClient udpServer = new UdpClient();

            // Разрешаем повторное использование адреса
            udpServer.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            udpServer.Client.Bind(localEndPoint);  // Привязываем сокет к порту

            IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);

            while (true)
            {
                Byte[] receiveBytes = udpServer.Receive(ref remoteEP);
                string receiveData = Encoding.UTF8.GetString(receiveBytes);
                Console.WriteLine($"{receiveData}");

                // Логика обработки сигнала
                if (receiveData == "Stop")
                {
                    isSignal = true;
                    break;
                }
            }

            udpServer.Close();
        }
    }
}
