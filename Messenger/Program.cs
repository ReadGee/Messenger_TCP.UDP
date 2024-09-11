using Messenger;
using Messenger.TestingDev;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

internal class Program
{
    delegate void StartServer();
    delegate void StartClient();
    
    static void Initialization()
    {
        Settings.ReadFileSettings();
        Console.Title = "RG.NET";
    }
    static void Main(string[] args)
    {
        Initialization();
        StartServer startServer = Server.Server.StartServer;
        StartClient startClient = Client.Client.StartClient;
        


        Console.WriteLine("Запустить сервер - y? или клиент - n");
        string Selection = Console.ReadLine().ToLower();
        Console.WriteLine(Selection.StartsWith("dev"));
        while (Selection == null || Selection == "" || (Selection != "y" && Selection != "n" && !Selection.StartsWith("dev")))
        {
            Selection = GetSelection().ToLower();
        }

        SelectionType(Selection, startServer, startClient);

        Console.ReadKey();
    }

    private static void SelectionType(string Selection, StartServer startServer, StartClient startClient)
    {
        Console.Clear();
        switch (Selection)
        {
            case "y":
                startServer();
                break;

            case "n":
                startClient();
                break;

            case "devc":
                //var serverTask = Task.Run(() => Server.Server.StartServer());
                //var clientTask = Task.Run(() => Client.Client.StartClient());
                //Task.WhenAll(serverTask, clientTask);
                ClientUDP.udpClientStart(false);
                break;
            case "devs":
                //var serverTask = Task.Run(() => Server.Server.StartServer());
                //var clientTask = Task.Run(() => Client.Client.StartClient());
                //Task.WhenAll(serverTask, clientTask);
                ServerUDP.udpServerStart();
                break;
            case "devsc" or "devcs":
                ClientUDP.udpClientStart(true);
                break;
        }
    }

    private static string GetSelection()
    {
        return Console.ReadLine();
    }
}



