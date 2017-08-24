using Newtonsoft.Json;
using ProtocolUtils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Projekt3Yatzy
{
    public class Client
    {
        public TcpClient TcpClient { get; set; }
        public FormGameBoard MyGameBoard { get; set; }

        public Client(FormGameBoard myGameBoard)
        {
            MyGameBoard = myGameBoard;
        }

        public void Start()
        {
            TcpClient = new TcpClient("192.168.25.94", 5000);

            //Thread listenerThread = new Thread(Send);
            //listenerThread.Start();

            Thread senderThread = new Thread(Listen);
            senderThread.Start();

            //senderThread.Join();
            //listenerThread.Join();
        }

        private void Listen()
        {
            try
            {
                while (true)
                {
                    NetworkStream n = TcpClient.GetStream();
                    string message = new BinaryReader(n).ReadString();


                    //todo uppdatera spelplan efter json data kommer in
                    var gameBoard = JsonConvert.DeserializeObject<GameBoardJsonObject>(message);

                    MyGameBoard.UpdateFormGameBoard(gameBoard);

                    //message = new BinaryReader(n).ReadString();
                    //Console.WriteLine("Other: " + message);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void Send(string message)
        {
            try
            {

                NetworkStream n = TcpClient.GetStream();


                //Skicka json efter din tur
                //message = Console.ReadLine();
                BinaryWriter w = new BinaryWriter(n);
                w.Write(message);
                //w.Flush();
                


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            //finally
            //{

            //    TcpClient.Close();
            //}
        }
    }
}
