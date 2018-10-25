using Common.Interfaces;
using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Models
{
    class BoardState : IBoardState
    {
        public Dictionary<int, int> BlackLocation { get; set; }
        public Dictionary<int, int> WhiteLocation { get; set; }
        public Cube Cube { get; set; }
        public string CurrentPlayer { get; set; }
        public int BarredBlackCheckers { get; set; }
        public int BarredWhiteCheckers { get; set; }
        public bool IsDouble { get; set; }

    }
}
