using Newtonsoft.Json;
using Projekt3Yatzy;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NetworkLibrary
{
    public class Server
    {
        List<ClientHandler> clients = new List<ClientHandler>();

        public void Run()
        {
            TcpListener listener = new TcpListener(IPAddress.Any, 5000);
            Console.WriteLine("Server up and running, waiting for messages...");

            try
            {
                listener.Start();

                while (true)
                {
                    TcpClient c = listener.AcceptTcpClient();
                    ClientHandler newClient = new ClientHandler(c, this);
                    clients.Add(newClient);

                    Thread clientThread = new Thread(newClient.Run);
                    clientThread.Start();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (listener != null)
                    listener.Stop();
            }

        }

        internal void DisconnectClient(ClientHandler clientHandler)
        {
            clients.Remove(clientHandler);
            Console.WriteLine("Client X has left the building...");
            //todo Skriv i message box att någon lämnat spelet
        }

        public void SendData(ClientHandler client, string json)
        {
            //Packa upp json
            var jsonobject = JsonConvert.DeserializeObject<GameBoardJsonObject>(json);


            if (jsonobject.Command=="Next Turn")
            {
                jsonobject.CurrentPlayer++;
                
            }

            string jsonToSend = JsonConvert.SerializeObject(jsonobject);
           
            foreach (ClientHandler tmpClient in clients)
            {
                if (tmpClient!=client)
                {
                    NetworkStream n = tmpClient.TcpClient.GetStream();
                    BinaryWriter w = new BinaryWriter(n);
                    w.Write(jsonToSend);
                }


            }
        }
    }
}
