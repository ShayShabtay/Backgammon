using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Interfaces
{
    public interface IBoardState
    {
        Dictionary<int, int> BlackLocation { get; set; }
        Dictionary<int, int> WhiteLocation { get; set; }
        Cube Cube { get; set; }
        string CurrentPlayer { get; set; }
        int BarredBlackCheckers { get; set; }
        int BarredWhiteCheckers { get; set; }
        bool IsDouble { get; set; }
    }
}
