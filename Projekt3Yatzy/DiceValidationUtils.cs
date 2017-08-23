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
            if (chosenDice.Length == numDice)
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
    }
}
