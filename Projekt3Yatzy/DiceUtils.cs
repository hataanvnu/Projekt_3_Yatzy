using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekt3Yatzy
{
    public static class DiceUtils
    {
        static Random random = new Random(); 

        public static int NextThrow()
        {
            return random.Next(1, 7);
        }
    }
}
