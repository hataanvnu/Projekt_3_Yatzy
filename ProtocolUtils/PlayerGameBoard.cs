using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekt3Yatzy
{
    public class PlayerGameBoard
    {
        
        public string Name { get; set; }
        public int PlayerId { get; set; }
        public PointField[] PointArray { get; set; }

        public PlayerGameBoard(string name, int id)
        {
            Name = name;
            PlayerId = id;
            PointArray = new PointField[18];
            for (int i = 0; i < PointArray.Length; i++)
            {
                PointArray[i] = new PointField();
            }
        }
        
    }
}
