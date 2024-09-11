using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Client
{
    public class Client
    {
        public static IPAddress ipAddress = IPAddress.Parse("146.158.101.11");
        public static IPEndPoint remoteEP = new IPEndPoint(ipAddress, 10000);
        public static Socket sender = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

        public static void StartClient()
        {
            Console.Clear();
            byte[] bytes = new byte[1024];

               
                try
                {

                    for (int i = 0; i < 5; i++)
                    {
                        try 
                        {   
                            sender.Connect(remoteEP); 
                            break; 
                        }
                        catch
                        {
                            Console.WriteLine($"Подключение не удалось к {remoteEP.Address.ToString()}:{remoteEP.Port.ToString()}");
                            continue;
                        }
                    }

                    if (!sender.Connected)
                    {
                        sender.Close();
                        Console.WriteLine("\nНе удалось подключиться. \n y - Повторить подключение, n - Выйти");

                        if (Console.ReadLine().ToString() == "y")
                        {
                            StartClient();
                            return;
                        }
                        else 
                        { 
                            return; 
                        }
                    }

                    Console.Clear();

                    PrintLogo();

                    Console.WriteLine("Установлено соединение с " + sender.RemoteEndPoint.ToString());
                    
                    

                    Thread receiveThread = new Thread(() => ReceiveMessages(sender));
                    receiveThread.Start();
                    //.Run(() => BackgroundReceiveMessages(receiveThread)); 
                    
                    while (true)
                    {
                        string message = Console.ReadLine();

                        if (message.ToLower() == "_exit")
                        {
                            break;
                        }

                        byte[] msg = Encoding.UTF8.GetBytes(message);
                        sender.Send(msg);                        
                    }
                    
                    sender.Shutdown(SocketShutdown.Both);
                    sender.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            
        }
        private static void BackgroundReceiveMessages(Thread task)
        {
            while(true)
            {
                if(!task.IsAlive)
                {
                    Task.Run(() => task.Start());
                }
            }
        }
        private static void ReceiveMessages(Socket sender)
        {
            byte[] bytes = new byte[1024];
            int bytesRec;
            string data = "";
            try
            {
                while (true)
                {
                    bytesRec = sender.Receive(bytes);                    
                    data = Encoding.UTF8.GetString(bytes, 0, bytesRec);
                    if(bytesRec == 0 && sender.Connected)
                    {
                        if(!ReConnect()) return;                         
                    }
                    Console.WriteLine("Ответ от сервера: {0}", data);
                }
            }
            catch (Exception e)
            {
                sender.Disconnect(false);
                Console.WriteLine(e.ToString());
                ReceiveMessages(sender);
            }            
        }


        private static bool ReConnect()
        {
            for (int i = 0; i < 5; i++)
            {
                try
                {
                    sender.Connect(remoteEP);
                    return true;
                }
                catch
                {
                    continue;
                }
            }
            Console.WriteLine($"Сервер отключен");
            return false;
        }
        




        private static void PrintLogo()
        {
            Console.WriteLine("\r\n ███████████     █████████     ██████   █████ ██████████ ███████████\r" +
                                 "\n░░███░░░░░███   ███░░░░░███   ░░██████ ░░███ ░░███░░░░░█░█░░░███░░░█\r" +
                                 "\n ░███    ░███  ███     ░░░     ░███░███ ░███  ░███  █ ░ ░   ░███  ░ \r" +
                                 "\n ░██████████  ░███             ░███░░███░███  ░██████       ░███    \r" +
                                 "\n ░███░░░░░███ ░███    █████    ░███ ░░██████  ░███░░█       ░███    \r" +
                                 "\n ░███    ░███ ░░███  ░░███     ░███  ░░█████  ░███ ░   █    ░███    \r" +
                                 "\n █████   █████ ░░█████████     █████  ░░█████ ██████████    █████   \r" +
                                 "\n░░░░░   ░░░░░   ░░░░░░░░░     ░░░░░    ░░░░░ ░░░░░░░░░░    ░░░░░    \r" +
                                 "\n" +
                                 "\n" +
                                 "\n\t\t\tReaGee NET @ Make in 2024 @ Client \n\n\n\n");
        }




        /*public static void StartClient()
        {
            byte[] bytes = new byte[1024];

            try
            {
                IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, 12345);

                Socket sender = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                try
                {
                    sender.Connect(remoteEP);

                    Console.WriteLine("Сокет соединен с {0}", sender.RemoteEndPoint.ToString());
                    Task.Run(()  => ReceiveClient(sender));
                    Console.Write("Введите сообщение: ");
                    while (true)
                    {
                        string message = Console.ReadLine();
                        if (message == "EXIT") break;
                        byte[] msg = Encoding.ASCII.GetBytes(message);

                        int bytesSent = sender.Send(msg);
                    }
                    //int bytesRec = sender.Receive(bytes);
                    //Console.WriteLine("Ответ от сервера: {0}", Encoding.ASCII.GetString(bytes, 0, bytesRec));

                    sender.Shutdown(SocketShutdown.Both);
                    sender.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private async static void ReceiveClient(Socket sender)
        {
            string data = null;
            byte[] bytes = null;
            int bytesRec;

            while (true)
            {
                while (true)
                {
                    bytes = new byte[1024];
                    bytesRec = sender.Receive(bytes);
                    data += Encoding.ASCII.GetString(bytes, 0, bytesRec);
                    if (data.Length == bytesRec)
                    {
                        break;
                    }
                }
                Console.WriteLine("Ответ от сервера: {0}", Encoding.ASCII.GetString(bytes, 0, bytesRec));
            }
        }*/

    }

}
