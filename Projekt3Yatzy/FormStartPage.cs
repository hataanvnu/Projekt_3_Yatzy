using Newtonsoft.Json;
using ProtocolUtils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Projekt3Yatzy
{
    public partial class FormStartPage : Form
    {
        public Client MyClient { get; set; }

        public FormStartPage()
        {
            InitializeComponent();
        }

        private void buttonStartGame_Click(object sender, EventArgs e)
        {
            try
            {
                MyClient = new Client(textBoxEnterYourName.Text, this);

                Thread clientThread = new Thread(MyClient.Start);
                clientThread.Start();

                GameBoardJsonObject newNameJsonObject = new GameBoardJsonObject();
                newNameJsonObject.Command = "Validate name";
                newNameJsonObject.NewName = textBoxEnterYourName.Text;

                string jsonString = JsonConvert.SerializeObject(newNameJsonObject);
                MyClient.Send(jsonString);

            }
            catch (Exception)
            {

                labelWaitingForPlayer.Text="No game running";
                labelWaitingForPlayer.Visible=true;

            }

        }

        //public void foo()
        //{
        //    if (textBoxEnterYourName.Text == "todo") // todo om username redan finns i current game
        //    {
        //        IndicateUserNameTaken();
        //    }
        //    else
        //    {
        //        IndicateWaitForPlayers();

        //    }
        //}

        public void IndicateWaitForPlayers()
        {
            textBoxEnterYourName.Enabled = false;
            labelWaitingForPlayer.Visible = true;
            buttonStartGame.Enabled = false;
            labelUserNameTaken.Visible = false;
        }

        public void IndicateUserNameTaken()
        {
            InitializeComponent();
            labelUserNameTaken.Visible = true;
        }
    }
}
