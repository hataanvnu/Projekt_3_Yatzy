﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekt3Yatzy
{
    public class PlayerGameBoard
    {
        public string Command { get; set; }
        public string Name { get; set; }
        public int PlayerId { get; set; }
        public List<PointField> PointList { get; set; }
        
    }
}
