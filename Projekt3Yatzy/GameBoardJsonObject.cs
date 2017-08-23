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

        public GameBoardJsonObject(List<string> names)
        {
            Command = "Next turn";
            ListOfGameBoards = new List<PlayerGameBoard>();
            for (int i = 0; i < names.Count; i++)
            {
                ListOfGameBoards.Add(new PlayerGameBoard(names[i], i+1));
            }
            CurrentPlayer = 1;
            Version = "1.0";
        }
    }
}
