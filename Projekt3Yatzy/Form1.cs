using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Projekt3Yatzy
{
    public partial class FormStartPage : Form
    {
        public FormStartPage()
        {
            InitializeComponent();
        }

        private void buttonStartGame_Click(object sender, EventArgs e)
        {
            if (textBoxEnterYourName.Text == "todo") // todo om username redan finns i current game
            {
                labelUserNameTaken.Visible = true; //Sure, lets do this...
            }
            else
            {
                textBoxEnterYourName.Enabled = false;
                labelWaitingForPlayer.Visible = true;
                buttonStartGame.Enabled = false;
                labelUserNameTaken.Visible = false;

            }

        }
    }
}
