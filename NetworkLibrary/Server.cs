using Newtonsoft.Json;
using ProtocolUtils;
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
        int turnCounter = 0;
        public List<string> CurrentPlayerList { get; set; }

        public Server()
        {
            CurrentPlayerList = new List<string>();
        }

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
                    Console.WriteLine($"{clients.Count()} players have joined the server");

                    Thread clientThread = new Thread(newClient.Run);
                    clientThread.Start();

                    Thread.Sleep(1000);

                    if (clients.Count() >= 2 || CurrentPlayerList.Count == 2)
                    {
                        Thread.Sleep(500);
                        StartGame();
                        break;
                    }
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

        private void StartGame()
        {
            var startGameCommand = new GameBoardJsonObject();
            startGameCommand.Command = "Start game";
            startGameCommand.Names = CurrentPlayerList;

            for (int i = 0; i < clients.Count; i++)
            {
                startGameCommand.PlayerId = i + 1;

                string jsonToSend = JsonConvert.SerializeObject(startGameCommand);
                NetworkStream n = clients[i].TcpClient.GetStream();
                BinaryWriter w = new BinaryWriter(n);
                w.Write(jsonToSend);
            }

        }

        internal void DisconnectClient(ClientHandler clientHandler)
        {

            GameBoardJsonObject disconnectCommand = new GameBoardJsonObject();
            disconnectCommand.Command = "Disconnected";

            string jsonToSend = JsonConvert.SerializeObject(disconnectCommand);

            NetworkStream n = clientHandler.TcpClient.GetStream();
            BinaryWriter w = new BinaryWriter(n);
            w.Write(jsonToSend);



            clients.Remove(clientHandler);
            Console.WriteLine("Client X has left the building...");
            //todo Skriv i message box att någon lämnat spelet
        }

        public void SendData(ClientHandler client, string json)
        {
            //Packa upp json
            var jsonobject = JsonConvert.DeserializeObject<GameBoardJsonObject>(json);



            if (jsonobject.Command == "Next turn")
            {

                turnCounter++;
                jsonobject.CurrentPlayer = (turnCounter % clients.Count()) + 1; //todo modulus

            }
            else if (jsonobject.Command == "Validate name")
            {
                // Om namnet redan finns, disconnecta clienten igen.
                if (CurrentPlayerList.Where(p => p == jsonobject.NewName).Count() == 1)
                {
                    DisconnectClient(client);
                    return;
                }
                else
                {
                    CurrentPlayerList.Add(jsonobject.NewName);

                }
            }
            else if (jsonobject.Command == "Final turn" && jsonobject.CurrentPlayer == clients.Count)
            {
                Console.WriteLine("Someone won");
                jsonobject.PlayerId = jsonobject.ListOfGameBoards.Max(x => Convert.ToInt32(x.PointArray[17].Point));
            }
            else
            {
                jsonobject.Command = "Next turn";
            }

            string jsonToSend = JsonConvert.SerializeObject(jsonobject);

            foreach (ClientHandler tmpClient in clients)
            {
                //if (tmpClient!=client)
                //{
                NetworkStream n = tmpClient.TcpClient.GetStream();
                BinaryWriter w = new BinaryWriter(n);
                w.Write(jsonToSend);
                // }


            }
        }
    }
}
