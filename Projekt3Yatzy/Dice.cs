using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekt3Yatzy
{
    class Dice
    {
        public int Value { get; set; }

        public bool IsChecked { get; set; }

        public Dice(int value)
        {
            Value = value;
            IsChecked = false;
        }
    }
}
