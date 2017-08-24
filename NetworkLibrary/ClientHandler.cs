using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace NetworkLibrary
{
    public class ClientHandler
    {
        public TcpClient TcpClient { get; set; }
        public Server MyServer { get; set; }


        public ClientHandler(TcpClient c, Server server)
        {
            TcpClient = c;
            MyServer = server;
        }

        public void Run()
        {
            NetworkStream n;

            while (true) //Todo något condition
            {
                try
                {
                    n = TcpClient.GetStream();
                    string jsonString = new BinaryReader(n).ReadString();
                    

                    //Console.WriteLine(jsonString);
                    //Göra saker, Skicka data
                    MyServer.SendData(this, jsonString);

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            //MyServer.DisconnectClient(this);
            //TcpClient.Close();

        }
    }
}
