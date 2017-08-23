using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekt3Yatzy
{
    public class GameBoardJsonObject
    {
        public string Command { get; set; }
        public List<PlayerGameBoard> ListOfGameBoards { get; set; }
        public int CurrentPlayer { get; set; }
        public string Version { get; set; }


    }
}
