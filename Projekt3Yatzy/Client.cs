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
        FormStartPage startPage;

        public Client(string name,FormStartPage startPage)
        {
            Name = name;
            this.startPage = startPage;
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
                    if (gameBoard.Command=="Next turn")
                    {
                    MyGameBoard.UpdateFormGameBoard(gameBoard);

                    }

                    else if (gameBoard.Command=="Start game")
                    {
                        //Application.Run(new FormGameBoard(this,gameBoard.PlayerId));
                        var tmp = new FormGameBoard(this, gameBoard.PlayerId);
                        startPage.Invoke(new Action(tmp.Show));
                        
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
