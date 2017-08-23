﻿using System;
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

        PictureBox[] pictureBoxDiceList = new PictureBox[5];

        Dice[] diceArray = new Dice[5];

        int throwCounter = 0;

        int CurrentPlayer = 1;



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
        }

        private void InitializeDiceList()
        {
            pictureBoxDiceList[0] = pictureBoxDice0;
            pictureBoxDiceList[1] = pictureBoxDice1;
            pictureBoxDiceList[2] = pictureBoxDice2;
            pictureBoxDiceList[3] = pictureBoxDice3;
            pictureBoxDiceList[4] = pictureBoxDice4;
        }

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


            int throwsLeft = 3 - throwCounter;

            if (throwsLeft == 0)
            {
                //buttonThrowDice.Enabled = false;
                textBoxStatus.Text = $"Välj de tärningar du vill använda för poänggivning";
            }
            else
            {
                textBoxStatus.Text = $"{throwsLeft} kast kvar...";
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

        //private void NextPlayer()
        //{
        //    CurrentPlayer
        //    throwCounter = 0;
        //    buttonThrowDice.Enabled = true;

        //}

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

                    // Todo: resetta variabler
                    //InitNewTurn();

                }
                else
                {
                    textBoxStatus.Text = "fu";
                }
            }
            else
            {
                textBoxStatus.Text = "Normal people would choose at least one dice...";
            }
        }


        private void PointField_Click(object sender, EventArgs e)
        {
            if (sender is Control)
            {
                int row = tableScoreBoard.GetRow((Control)sender);

                Label myLabel = (Label)tableScoreBoard.GetControlFromPosition(CurrentPlayer, row);

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
            }
        }

        private void GivePointsForChance(Dice[] diceArray, int row)
        {
            Label myLabel = (Label)tableScoreBoard.GetControlFromPosition(CurrentPlayer, row);

            myLabel.Text = DiceValidationUtils.CalculatePoints(diceArray).ToString();
        }

        private void CheckIfFullHouse(Dice[] chosenDice, int row)
        {
            var sortedDice = chosenDice.OrderBy(d => d.Value).ToArray();

            bool isFullHouse = DiceValidationUtils.FullHouseValidation(sortedDice);

            if (isFullHouse)
            {
                Label myLabel = (Label)tableScoreBoard.GetControlFromPosition(CurrentPlayer, row);

                myLabel.Text = DiceValidationUtils.CalculatePoints(chosenDice).ToString();
            }
        }

        private void CheckTwoPairs(Dice[] chosenDice, int row)
        {
            var sortedDice = chosenDice.OrderBy(d => d.Value).ToArray();

            bool isTwoPairs = DiceValidationUtils.TwoPairsValidation(sortedDice);

            if (isTwoPairs)
            {
                Label myLabel = (Label)tableScoreBoard.GetControlFromPosition(CurrentPlayer, row);

                myLabel.Text = DiceValidationUtils.CalculatePoints(chosenDice).ToString();
            }

        }

        private void CheckIfStraight(int size, Dice[] chosenDice, int row)
        {
            var sortedDice = chosenDice.OrderBy(d => d.Value).ToArray();

            bool isStraight = DiceValidationUtils.StraightValidation(size, sortedDice);

            if (isStraight)
            {
                Label myLabel = (Label)tableScoreBoard.GetControlFromPosition(CurrentPlayer, row);

                if (sortedDice.First().Value == 1)
                {
                    myLabel.Text = DiceValidationUtils.CalculatePoints(sortedDice).ToString();

                }
                else if (sortedDice.First().Value == 2)
                {
                    myLabel.Text = DiceValidationUtils.CalculatePoints(sortedDice).ToString();

                }
            }
        }

        private void CheckMatchingDice(int numDice, Dice[] chosenDice, int row)
        {
            bool isMatching = DiceValidationUtils.CheckMatchingDiceValidation(numDice, chosenDice);

            if (isMatching)
            {
                int sum = DiceValidationUtils.CalculatePoints(chosenDice);

                Label myLabel = (Label)tableScoreBoard.GetControlFromPosition(CurrentPlayer, row);

                myLabel.Text = sum.ToString();
            }
            else
            {
                textBoxStatus.Text = "staaph";
            }
        }
    }
}
