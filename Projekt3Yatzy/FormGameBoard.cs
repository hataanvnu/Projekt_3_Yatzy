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
    public partial class FormGameBoard : Form
    {

        List<PictureBox> diceList;

        int throwCounter;



        public FormGameBoard()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            labelPlayers.BackColor = Color.Red;
        }



        private void FormGameBoard_Load(object sender, EventArgs e)
        {
            // Initialize the list of dice
            InitializeDiceList();

            throwCounter = 0;

        }

        private void InitializeDiceList()
        {
            diceList = new List<PictureBox>();
            diceList.Add(pictureBoxDice1);
            diceList.Add(pictureBoxDice2);
            diceList.Add(pictureBoxDice3);
            diceList.Add(pictureBoxDice4);
            diceList.Add(pictureBoxDice5);
        }

        private void buttonThrowDice_Click(object sender, EventArgs e)
        {
            throwCounter++;
            foreach (var diceImg in diceList)
            {
                int diceValue = DiceUtils.NextThrow();
                diceImg.Image = Image.FromFile($@"..\..\images\dice{diceValue}.png");
            }

            int throwsLeft = 3 - throwCounter;

            if (throwsLeft == 0)
            {
                buttonThrowDice.Enabled = false;
                textBoxStatus.Text = $"Välj de tärningar du vill använda för poänggivning";
            }
            else
            {
                textBoxStatus.Text = $"{throwsLeft} kast kvar...";
            }
        }
    }
}
