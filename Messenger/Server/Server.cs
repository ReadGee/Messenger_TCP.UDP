using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Server
{
    public class Server
    {
        public static async void StartServer()
        {
            IPAddress ipAddress = IPAddress.Any;        //Parse("127.0.0.1");
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 10000);

            // Создаем сокет для прослушивания
            Socket listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            ServerDataAccess.GetRows();

            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(10);

                Console.WriteLine("Ожидание подключений...");

                while (true)
                {
                    // Асинхронное ожидание подключения клиента
                    Socket clientSocket = await Task.Factory.FromAsync(
                        listener.BeginAccept, listener.EndAccept, null);

                    // Обработка клиента в отдельной таске
                    Task.Run(() => HandleClient(clientSocket));
                }
            

                /*Thread clientThread = new Thread(() => HandleClient(handler));
                clientThread.Start();
                Task.Run(() => BackgroundReceiveMessages(clientThread));

                while (true)
                {
                    string message = Console.ReadLine();

                    if (message == "exit")
                    {
                        break;
                    }

                    byte[] msg = Encoding.ASCII.GetBytes(message);
                    handler.Send(msg);
                }*/
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private static void BackgroundReceiveMessages(Thread task)
        {
            while (true)
            {
                if (!task.IsAlive)
                {
                    Task.Run(() => task.Start());
                }
            }
        }



        static void HandleClient(Socket clientSocket)
        {
            try
            {
                Console.WriteLine($"Клиент подключен: {clientSocket.RemoteEndPoint}");

                byte[] bytes = new byte[1024];

                int bytesRec = clientSocket.Receive(bytes);

                string data = Encoding.UTF8.GetString(bytes, 0, bytesRec).ToString();
                Console.WriteLine($"Cообщение: {data}");

               
                clientSocket.Shutdown(SocketShutdown.Both);
                clientSocket.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Ошибка при обработке клиента: {e.Message}");
            }
        }



        /*private static void HandleClient(Socket handler)
        {
            string data = null;
            byte[] bytes = new byte[1024];

            try
            {
                while (true)
                {
                    int bytesRec = handler.Receive(bytes);
                    data = Encoding.ASCII.GetString(bytes, 0, bytesRec);
                    Console.WriteLine("Получено от клиента: {0}", data);

                    if (data == "exit")
                    {
                        break;
                    }

                    //Console.WriteLine("Введите сообщение: ");
                    
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                handler.Shutdown(SocketShutdown.Both);
                handler.Close();
            }
        }*/







        /*public static void StartServer()
        {
            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 12345);

            Socket listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(10);

                Console.WriteLine("Ожидание подключений...");
                Socket handler = listener.Accept();

                Task.Run(() => SendOnClient(handler));

                string data = null;
                byte[] bytes = null;
                while (true)
                {
                    while (true)
                    {
                        bytes = new byte[1024];
                        int bytesRec = handler.Receive(bytes);
                        data += Encoding.ASCII.GetString(bytes, 0, bytesRec);
                        if (data.Length == bytesRec)
                        {
                            break;
                        }
                    }

                    Console.WriteLine("Получено: {0}", data);
                    if(Console.ReadLine() == "EXIT") break;
                }

                handler.Shutdown(SocketShutdown.Both);
                handler.Close();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            
        }

        private async static void SendOnClient(Socket handler)
        {
           *//* while (true)
            {*//*
                while (true)
                {
                    byte[] msg = Encoding.ASCII.GetBytes(Console.ReadLine());
                    handler.Send(msg);
                }
            //}
        }*/

    }

}
