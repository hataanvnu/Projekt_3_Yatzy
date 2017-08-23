using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekt3Yatzy
{
    public static class DiceValidationUtils
    {
        internal static bool UpperSectionValidation(int v, Dice[] diceArray)
        {
            foreach (var dice in diceArray)
            {
                if (dice.Value != v)
                {
                    return false;
                }
            }

            return true;
        }

        internal static Dice[] GetChosenDice(Dice[] diceArray)
        {
            return diceArray
                             .Where(d => d.IsChecked)
                             .ToArray();
        }

        internal static int CalculatePoints(int v, Dice[] chosenDice)
        {
            return chosenDice.Length * v;
        }

        internal static bool CheckMatchingDiceValidation(int numDice, Dice[] chosenDice)
        {
            if (chosenDice.Length == numDice) // för att validera att par/ triss/ fyrtal-kriteriet matchas
            {
                int value = chosenDice.First().Value;

                foreach (var dice in chosenDice)
                {
                    if (dice.Value != value)
                    {
                        return false;
                    }
                }

                return true;
            }
            else
            {
                return false;
            }

        }

        internal static int CalculatePoints(Dice[] chosenDice)
        {
            return chosenDice.Sum(x => x.Value);
        }

        internal static bool StraightValidation(int size, Dice[] sortedDice)
        {
            if (sortedDice.Length == 5 && sortedDice.First().Value == size)
            {
                for (int i = 1; i < sortedDice.Length; i++)
                {
                    if (sortedDice[i].Value != sortedDice[i - 1].Value + 1)
                    {
                        return false;
                    }
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        internal static bool TwoPairsValidation(Dice[] sortedDice)
        {
            if (sortedDice.Length == 4)
            {
                if (sortedDice[0].Value == sortedDice[1].Value && // Första paret lika
                    sortedDice[2].Value == sortedDice[3].Value && // andra paret lika
                    sortedDice[1].Value != sortedDice[3].Value)   // Inte fyra lika
                {
                    return true;
                }
            }
            return false;
        }

        internal static bool FullHouseValidation(Dice[] sortedDice)
        {
            if (sortedDice.Length == 5)
            {
                int val1 = sortedDice.First().Value;
                int val2 = sortedDice.Last().Value;

                if (sortedDice[1].Value == val1 && sortedDice[3].Value == val2) // [0] == [1] och [3] == [4]
                {
                    if (sortedDice[2].Value == val1 || sortedDice[2].Value == val2) // [2] == [0] eller [2] == [4]
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        internal static bool YatzyValidation(Dice[] diceArray)
        {
            var number = diceArray.First().Value;

            if (diceArray.Where(d => d.Value == number).Count() == 5)
            {
                return true;
            }
            return false;
        }
    }
}
