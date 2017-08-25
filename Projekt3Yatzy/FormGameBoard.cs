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
    public partial class FormGameBoard : Form
    {
        #region props and fields

        public Client MyClient { get; set; }

        PictureBox[] pictureBoxDiceList = new PictureBox[5];

        Dice[] diceArray = new Dice[5];

        public int PlayerId { get; set; }

        int CurrentPlayer = 1;

        int throwCounter = 0;

        int rowToCrossOut = 0;

        GameBoardJsonObject gameBoardProtocol;

        #endregion

        #region methods start automaticly starts when the form opens
        public FormGameBoard(Client myClient, int playerId, List<string> names)
        {
            InitializeComponent();

            PlayerId = playerId;
            gameBoardProtocol = new GameBoardJsonObject(names);

            MyClient = myClient;
            MyClient.MyGameBoard = this;

        }

        private void FormGameBoard_Load(object sender, EventArgs e)
        {
            // Initialize the list of dice
            InitializeDiceList();
            UpdateFormGameBoard(gameBoardProtocol);

        }

        private void InitializeDiceList()
        {
            pictureBoxDiceList[0] = pictureBoxDice0;
            pictureBoxDiceList[1] = pictureBoxDice1;
            pictureBoxDiceList[2] = pictureBoxDice2;
            pictureBoxDiceList[3] = pictureBoxDice3;
            pictureBoxDiceList[4] = pictureBoxDice4;

            diceArray[0] = new Dice();
            diceArray[1] = new Dice();
            diceArray[2] = new Dice();
            diceArray[3] = new Dice();
            diceArray[4] = new Dice();
        }
        #endregion

        #region Dice picture box logic

        private void buttonThrowDice_Click(object sender, EventArgs e)
        {
            throwCounter++;
            for (int i = 0; i < diceArray.Length; i++)
            {
                if (throwCounter == 1)
                {
                    diceArray[i] = new Dice(DiceUtils.NextThrow());
                }
                else if (!diceArray[i].IsChecked)
                {
                    diceArray[i].Value = DiceUtils.NextThrow();
                }

                pictureBoxDiceList[i].Image = Image.FromFile($@"..\..\images\dice{diceArray[i].Value}.png");
            }

            foreach (var diceImage in pictureBoxDiceList)
            {
                diceImage.Visible = true;
            }


            int throwsLeft = 3 - throwCounter;

            if (throwsLeft == 0)
            {
                buttonThrowDice.Enabled = false;
                textBoxStatus.Text = $"Guess you've used up all your chances. Choose dice and pointfield.";
            }
            else
            {
                textBoxStatus.Text = $"{throwsLeft} throws to go...";
            }
        }

        private void pictureBoxDice0_Click(object sender, EventArgs e)
        {
            ToggleEnabledDice(0);
        }

        private void pictureBoxDice1_Click(object sender, EventArgs e)
        {
            ToggleEnabledDice(1);
        }

        private void pictureBoxDice2_Click(object sender, EventArgs e)
        {
            ToggleEnabledDice(2);
        }

        private void pictureBoxDice3_Click(object sender, EventArgs e)
        {
            ToggleEnabledDice(3);
        }

        private void pictureBoxDice4_Click(object sender, EventArgs e)
        {
            ToggleEnabledDice(4);
        }
        #endregion

        private void ResetButtons()
        {
            throwCounter = 0;
            for (int i = 0; i < pictureBoxDiceList.Length; i++)
            {
                pictureBoxDiceList[i].Visible = false;
                pictureBoxDiceList[i].BackColor = Color.Transparent;
                diceArray[i].IsChecked = false;

            }
        }

        #region protocol and server stuff

        public void UpdateFormGameBoard(GameBoardJsonObject gameBoardProtocol)
        {
            ToggleGameBoardComponents(false);

            this.gameBoardProtocol = gameBoardProtocol;
            if (gameBoardProtocol.Command == "Final turn")
            {
                if (gameBoardProtocol.CurrentPlayer == PlayerId)
                {
                    MessageBox.Show($"You win!");
                }
                else
                {
                    MessageBox.Show("You lose! Are you from the Java class?!");
                }
            }
            else
            {
                CurrentPlayer = gameBoardProtocol.CurrentPlayer;
            }

            // Kollar om det är din tur eller inte.
            if (gameBoardProtocol.CurrentPlayer == PlayerId)
            {
                textBoxStatus.Text = "It's your turn. At least try your best.";
                ToggleGameBoardComponents(true);
                ResetButtons();
            }

            for (int col = 0; col < gameBoardProtocol.ListOfGameBoards.Count; col++)
            {

                //Sätter namn
                Label myLabel = (Label)tableScoreBoard.GetControlFromPosition(col + 1, 0);
                myLabel.Text = gameBoardProtocol.ListOfGameBoards[col].Name;

                // Sätter bold text på den spelaren som har turen nu
                if (col+1 == CurrentPlayer)
                {
                    myLabel.Font = new Font(Label.DefaultFont, FontStyle.Bold);
                }
                else
                {
                    myLabel.Font = new Font(Label.DefaultFont, FontStyle.Regular);
                }

                // Fyller i scoreboarden med poäng
                for (int row = 0; row < gameBoardProtocol.ListOfGameBoards[col].PointArray.Length; row++)
                {
                    myLabel = (Label)tableScoreBoard.GetControlFromPosition(col + 1, row + 1);
                    myLabel.Text = gameBoardProtocol.ListOfGameBoards[col].PointArray[row].Point;
                }

            }
        }

        private void UpdateProtocolGameBoard(int row, int points)
        {
            gameBoardProtocol.ListOfGameBoards[CurrentPlayer - 1].PointArray[row - 1].Point = points.ToString();
            gameBoardProtocol.ListOfGameBoards[CurrentPlayer - 1].PointArray[row - 1].IsUsed = true;

        }

        private void NextTurn()
        {
            gameBoardProtocol.Command = "Next turn";
            SendProtocolToServer();
        }

        private void SendProtocolToServer()
        {
            string protocol = JsonConvert.SerializeObject(gameBoardProtocol);

            MyClient.Send(protocol);
        }
        #endregion

        #region Point check methods

        /// <summary>
        /// All clicks on a pointfield goes here.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PointField_Click(object sender, EventArgs e)
        {
            if (sender is Control)
            {
                int row = tableScoreBoard.GetRow((Control)sender);

                Label myLabel = (Label)tableScoreBoard.GetControlFromPosition(CurrentPlayer, row);

                if (myLabel.Text != "-") // todo ändra om default-texten är ändrad
                {
                    return;
                }

                var chosenDice = DiceValidationUtils.GetChosenDice(diceArray);

                switch (row)
                {
                    case 1:
                    case 2:
                    case 3:
                    case 4:
                    case 5:
                    case 6:
                        UpperSectionPointFieldManager(chosenDice, row);
                        break;
                    case 9:
                        // Pair
                        CheckMatchingDice(2, chosenDice, row);
                        break;
                    case 10:
                        // Two pairs
                        CheckTwoPairs(chosenDice, row);
                        break;
                    case 11:
                        // 3 of a kind
                        CheckMatchingDice(3, chosenDice, row);
                        break;
                    case 12:
                        // 4 of a kind
                        CheckMatchingDice(4, chosenDice, row);
                        break;
                    case 13:
                        // small straight
                        CheckIfStraight(1, chosenDice, row);
                        break;
                    case 14:
                        // large straight
                        CheckIfStraight(2, chosenDice, row);
                        break;
                    case 15:
                        // Full house
                        CheckIfFullHouse(chosenDice, row);
                        break;
                    case 16:
                        // Chance
                        GivePointsForChance(diceArray, row);
                        break;
                    case 17:
                        // Yatzy - If 5, sätt 50 poäng
                        CheckIfYatzy(diceArray, row);
                        break;
                    default:
                        break;
                }
            }
        }


        private void CheckIfYatzy(Dice[] diceArray, int row)
        {
            bool isYatzy = DiceValidationUtils.YatzyValidation(diceArray);

            if (isYatzy)
            {
                Label myLabel = (Label)tableScoreBoard.GetControlFromPosition(CurrentPlayer, row);

                myLabel.Text = "50";

                UpdateProtocolGameBoard(row, 50);
                NextTurn();
                CalculateTotal();
            }
            else
            {
                CrossOutHandler(row);
            }
        }

        private void GivePointsForChance(Dice[] diceArray, int row)
        {
            Label myLabel = (Label)tableScoreBoard.GetControlFromPosition(CurrentPlayer, row);

            int points = DiceValidationUtils.CalculatePoints(diceArray);

            myLabel.Text = points.ToString();

            UpdateProtocolGameBoard(row, points);
            NextTurn();
            CalculateTotal();
        }

        private void CheckIfFullHouse(Dice[] chosenDice, int row)
        {
            var sortedDice = chosenDice.OrderBy(d => d.Value).ToArray();

            bool isFullHouse = DiceValidationUtils.FullHouseValidation(sortedDice);

            if (isFullHouse)
            {
                Label myLabel = (Label)tableScoreBoard.GetControlFromPosition(CurrentPlayer, row);

                int points = DiceValidationUtils.CalculatePoints(chosenDice);

                myLabel.Text = points.ToString();

                UpdateProtocolGameBoard(row, points);
                NextTurn();
                //myLabel.Text = DiceValidationUtils.CalculatePoints(chosenDice).ToString();

                CalculateTotal();
            }
            else
            {
                CrossOutHandler(row);
            }
        }

        private void CheckTwoPairs(Dice[] chosenDice, int row)
        {
            var sortedDice = chosenDice.OrderBy(d => d.Value).ToArray();

            bool isTwoPairs = DiceValidationUtils.TwoPairsValidation(sortedDice);

            if (isTwoPairs)
            {
                Label myLabel = (Label)tableScoreBoard.GetControlFromPosition(CurrentPlayer, row);


                int points = DiceValidationUtils.CalculatePoints(chosenDice);

                myLabel.Text = points.ToString();

                UpdateProtocolGameBoard(row, points);
                //myLabel.Text = DiceValidationUtils.CalculatePoints(chosenDice).ToString();
                NextTurn();
                CalculateTotal();
            }
            else
            {
                CrossOutHandler(row);
            }

        }

        private void CheckIfStraight(int size, Dice[] chosenDice, int row)
        {
            var sortedDice = chosenDice.OrderBy(d => d.Value).ToArray();

            bool isStraight = DiceValidationUtils.StraightValidation(size, sortedDice);

            if (isStraight)
            {
                Label myLabel = (Label)tableScoreBoard.GetControlFromPosition(CurrentPlayer, row);


                int points = DiceValidationUtils.CalculatePoints(sortedDice);

                myLabel.Text = points.ToString();

                UpdateProtocolGameBoard(row, points);
                NextTurn();
                //myLabel.Text = DiceValidationUtils.CalculatePoints(sortedDice).ToString();


                CalculateTotal();
            }
            else
            {
                CrossOutHandler(row);
            }
        }

        private void CheckMatchingDice(int numDice, Dice[] chosenDice, int row)
        {
            bool isMatching = DiceValidationUtils.CheckMatchingDiceValidation(numDice, chosenDice);

            if (isMatching)
            {
                int sum = DiceValidationUtils.CalculatePoints(chosenDice);

                Label myLabel = (Label)tableScoreBoard.GetControlFromPosition(CurrentPlayer, row);

                //int points = DiceValidationUtils.CalculatePoints(chosenDice);

                myLabel.Text = sum.ToString();

                UpdateProtocolGameBoard(row, sum);
                NextTurn();

                CalculateTotal();
            }
            else
            {
                CrossOutHandler(row);
            }

        }

        private void UpperSectionPointFieldManager(Dice[] chosenDice, int row)
        {
            if (chosenDice.Length != 0)
            {
                bool isOK = DiceValidationUtils.UpperSectionValidation(row, chosenDice);

                if (isOK)
                {
                    int points = DiceValidationUtils.CalculatePoints(row, chosenDice);
                    Label myLabel = (Label)tableScoreBoard.GetControlFromPosition(CurrentPlayer, row);
                    myLabel.Text = points.ToString();

                    // Kolla om bonusdags
                    CalculateSubtotalAndBonus();
                    CalculateTotal();
                    UpdateProtocolGameBoard(row, points);
                    NextTurn();
                    // Todo: resetta variabler
                    //InitNewTurn();

                }
                else
                {
                    CrossOutHandler(row);
                }
            }
            else
            {
                textBoxStatus.Text = "Normal people would choose at least one dice...";
            }
        }


        private void CalculateSubtotalAndBonus()
        {
            int sum = 0;
            for (int i = 1; i <= 6; i++)
            {
                Label myLabel = (Label)tableScoreBoard.GetControlFromPosition(CurrentPlayer, i);

                int points = 0;
                try
                {
                    points = Convert.ToInt32(myLabel.Text);
                    sum += points;

                }
                catch
                {
                    return;
                }
            }

            int subtotalLabelRow = 7;
            int bonusLabelRow = 8;

            Label subtotal = (Label)tableScoreBoard.GetControlFromPosition(CurrentPlayer, subtotalLabelRow);

            subtotal.Text = sum.ToString();
            gameBoardProtocol.ListOfGameBoards[CurrentPlayer - 1].PointArray[subtotalLabelRow - 1].Point = sum.ToString();
            gameBoardProtocol.ListOfGameBoards[CurrentPlayer - 1].PointArray[subtotalLabelRow - 1].IsUsed = true;

            Label bonus = (Label)tableScoreBoard.GetControlFromPosition(CurrentPlayer, bonusLabelRow);

            bonus.Text = sum >= 63 ? "50" : "0";

            gameBoardProtocol.ListOfGameBoards[CurrentPlayer - 1].PointArray[bonusLabelRow - 1].Point = bonus.Text;
            gameBoardProtocol.ListOfGameBoards[CurrentPlayer - 1].PointArray[bonusLabelRow - 1].IsUsed = true;
        }

        private void CalculateTotal()
        {
            int sum = 0;
            for (int i = 7; i <= 17; i++)
            {
                Label myLabel = (Label)tableScoreBoard.GetControlFromPosition(CurrentPlayer, i);

                int points = 0;
                try
                {
                    points = Convert.ToInt32(myLabel.Text);
                    sum += points;
                }
                catch
                {
                    return;
                }
            }
            Label total = (Label)tableScoreBoard.GetControlFromPosition(CurrentPlayer, 18);

            total.Text = sum.ToString();

            int arrayCount = gameBoardProtocol.ListOfGameBoards[CurrentPlayer - 1].PointArray.Length;
            gameBoardProtocol.ListOfGameBoards[CurrentPlayer - 1].PointArray[arrayCount - 1].Point = sum.ToString();
            gameBoardProtocol.ListOfGameBoards[CurrentPlayer - 1].PointArray[arrayCount - 1].IsUsed = true;

            gameBoardProtocol.Command = "Final turn";
            SendProtocolToServer();

        }
        #endregion

        #region Cross-out logic
        private void CrossOutHandler(int row)
        {
            rowToCrossOut = row;

            textBoxStatus.Text = "Do you want to cross out the current point field?";

            ToggleGameBoardComponents();

            buttonCrossOutNo.Visible = true;
            buttonCrossOutYes.Visible = true;
        }

        private void buttonCrossOutYes_Click(object sender, EventArgs e)
        {

            Label myLabel = (Label)tableScoreBoard.GetControlFromPosition(CurrentPlayer, rowToCrossOut);

            myLabel.Text = "0";

            //gameBoardProtocol.ListOfGameBoards[CurrentPlayer - 1].PointArray[rowToCrossOut].Point = myLabel.Text;
            //gameBoardProtocol.ListOfGameBoards[CurrentPlayer - 1].PointArray[rowToCrossOut].IsUsed = true;

            ToggleGameBoardComponents();
            ToggleCrossOutButtons();

            textBoxStatus.Text = "Well, someone has to lose...";

            UpdateProtocolGameBoard(rowToCrossOut, 0);
            NextTurn();
            CalculateSubtotalAndBonus();
            CalculateTotal();


        }

        private void buttonCrossOutNo_Click(object sender, EventArgs e)
        {
            ToggleGameBoardComponents();
            ToggleCrossOutButtons();

            textBoxStatus.Text = "Please, make a better move then...";
        }
        #endregion

        #region Toggle methods
        private void ToggleCrossOutButtons()
        {
            buttonCrossOutNo.Visible = !buttonCrossOutNo.Visible;
            buttonCrossOutYes.Visible = !buttonCrossOutYes.Visible;
        }

        private void ToggleGameBoardComponents()
        {
            tableScoreBoard.Enabled = !tableScoreBoard.Enabled;
            buttonThrowDice.Enabled = !buttonThrowDice.Enabled;
            foreach (var pictureBox in pictureBoxDiceList)
            {
                pictureBox.Enabled = !pictureBox.Enabled;
            }
        }

        private void ToggleGameBoardComponents(bool enabled)
        {
            tableScoreBoard.Enabled = enabled;
            buttonThrowDice.Enabled = enabled;
            foreach (var pictureBox in pictureBoxDiceList)
            {
                pictureBox.Enabled = enabled;
            }
        }

        private void ToggleEnabledDice(int index)
        {
            diceArray[index].IsChecked = !diceArray[index].IsChecked;

            if (diceArray[index].IsChecked)
            {
                pictureBoxDiceList[index].BackColor = Color.DarkGray;

            }
            else
            {
                pictureBoxDiceList[index].BackColor = Color.Transparent;

            }
        }

        #endregion

        private void label1_Click(object sender, EventArgs e)
        {
            this.BackColor = Color.Teal;
        }
    }
}
