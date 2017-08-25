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
using System.Windows.Forms;

namespace Projekt3Yatzy
{
    public class Client
    {
        public TcpClient TcpClient { get; set; }
        public FormGameBoard MyGameBoard { get; set; }
        public string Name { get; set; }
        FormStartPage myStartPage;

        public Client(string name, FormStartPage startPage)
        {
            Name = name;
            this.myStartPage = startPage;
        }

        public void Start()
        {
            try
            {
                TcpClient = new TcpClient("192.168.25.122", 5000);

                Thread senderThread = new Thread(Listen);
                senderThread.Start();

            }
            catch (Exception)
            {
                // startPage.get

            }
        }

        private void Listen()
        {
            bool quit = false;
            try
            {
                while (!quit)
                {
                    NetworkStream n = TcpClient.GetStream();
                    string message = new BinaryReader(n).ReadString();


                    //todo uppdatera spelplan efter json data kommer in
                    var gameBoard = JsonConvert.DeserializeObject<GameBoardJsonObject>(message);

                    if (gameBoard.Command == "Final turn")
                    {
                        MyGameBoard.UpdateFormGameBoard(gameBoard);

                    }
                    else if (gameBoard.Command == "Next turn")
                    {
                        MyGameBoard.UpdateFormGameBoard(gameBoard);

                    }
                    else if (gameBoard.Command == "Start game")
                    {
                        //Application.Run(new FormGameBoard(this,gameBoard.PlayerId));
                        var tmp = new FormGameBoard(this, gameBoard.PlayerId, gameBoard.Names);
                        myStartPage.Invoke(new Action(tmp.Show));
                    }
                    else if (gameBoard.Command == "Disconnected")
                    {
                        myStartPage.Invoke(new Action(myStartPage.IndicateUserNameTaken));
                        quit = true;
                    }
                    else if (gameBoard.Command == "Validate name")
                    {
                        myStartPage.Invoke(new Action(myStartPage.IndicateWaitForPlayers));

                    }
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


                BinaryWriter w = new BinaryWriter(n);
                w.Write(message);
                //w.Flush();



            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
    }
}
